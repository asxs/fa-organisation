using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Net.Security;

using System.Threading;

using System.Windows.Forms;

namespace As
{
    public class Pop3Lite
        : IDisposable
    {
        private UdpClient
            client = null;

        private IPEndPoint address = null;
        private Thread receiverThread = null;

        public Pop3Lite()
        {

        }

        internal static int waitToken = -1;

        #region Pop3Lite (IDisposable)

        /// <summary>
        /// Disposing all save or unsaved components
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Disposing all save or unsaved components
        /// </summary>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                client = null;
                address = null;
            }
        }

        #endregion

        /// <summary>
        /// Open a connection with given information about User and Password (with Authentication)
        /// </summary>
        public void Open()
        {
            try
            {
                OpenClientInternal();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Open a connection with Authentication
        /// </summary>
        public void OpenWithAuthentication()
        {
            Open();

            IPopAuthParameter authentication =
                              new PopParameter(Constants.UserAuthenticationString);

            authentication.
                Type = PopTokenType.Authorization;

            SendAuthentication(authentication);
        }

        /// <summary>
        /// Waits for a receive event on POP-Client
        /// </summary>
        private void WaitOnToken()
        {
            receiverThread =
                new Thread(new ThreadStart(WaitOnClientTokenInternal));
            receiverThread.Start();

            while (true)
            {
                if (waitToken != -1)
                    break;

                Application.DoEvents();
                Thread.Sleep(new TimeSpan(500));
            }

            waitToken = -1;
        }

        /// <summary>
        /// Looks in the UDP-Client (POP-Client) if some data is available
        /// </summary>
        private void WaitOnClientTokenInternal()
        {
            while (true)
            {
                if (client.Available > 0)
                {
                    waitToken = 1;
                    return;
                }
            }
        }

        /// <summary>
        /// Closes the POP-Client
        /// </summary>
        public void Close()
        {
            if (client == null)
                return;

            try
            {
                client.Close();
            }
            catch { }
        }

        /// <summary>
        /// Sending information about a transaction (for e.g. LIST)
        /// </summary>
        /// <param name="parameter">Gets all transaction parameters</param>
        public void SendTransaction(IPopTransactionParameter parameter)
        {
            foreach (var command in parameter.GetTransactionParameter())
                SendToServerInternal
                    (
                        new PopParameter(command)
                    );
        }

        /// <summary>
        /// Sending information about authentications with USER and PASS
        /// </summary>
        /// <param name="parameter">Gets all authentication parameters USER and PASS</param>
        public void SendAuthentication(IPopAuthParameter parameter)
        {
            foreach (var command in parameter.GetAuthParameter())
            {
                SendToServerInternal
                    (
                        new PopParameter(command)
                    );

                WaitOnToken();
            }
        }

        /// <summary>
        /// Sending some custom data with the UDP-Client (POP-Client)
        /// </summary>
        /// <param name="parameter">Gets only one Token to send</param>
        internal void SendToServerInternal(IPopParameter parameter)
        {
            var byteCount
                = -1;
            var bytes = ConvertToByteSequence(parameter.Token, out byteCount);

            var asyncResult =
                client.BeginSend(bytes, byteCount, null, null);

            while (!asyncResult.IsCompleted)
            {
                Application.DoEvents();
                Thread.Sleep(new TimeSpan(500));
            }

            client.EndSend(asyncResult);
        }

        /// <summary>
        /// Opens a UDP-Client with given local IP-Address
        /// </summary>
        internal void OpenClientInternal()
        {
            try
            {
                client = new UdpClient
                ((
                       address = new IPEndPoint(IPAddress.Parse(GMailPop3.Client), 0)
                ));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Converts a Unicode-String in a byte-sequence and returns a out-value with the byte-count
        /// </summary>
        /// <param name="item">Unicode-String that should convert</param>
        /// <param name="byteCount">Unicode-String that returns with the conversion</param>
        /// <returns>The given byte-sequence from "item"</returns>
        private byte[] ConvertToByteSequence(string item, out int byteCount)
        {
            byteCount = -1;
            var bytes = Encoding.Default.GetBytes(item);
            byteCount = bytes.Length;

            return bytes;
        }
    }
}
