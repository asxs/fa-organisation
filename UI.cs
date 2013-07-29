using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Net.Mail;

using System.Threading;

namespace As
{
    public partial class UI : Form
    {
        public UI()
        {
            InitializeComponent();
        }

        //Portnumber for POP3 SSL is 995
        //Can connect with UDP, because it is not security question and here we need only the packages, without checking them in total way
        //Local ip address 192.168.83.100
        //SendAuthorization
        //SendTransaction

        private void btnPopTest_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public static class GMailPop3
    {
        public const string Server = "pop.gmail.com";
        public const uint SslPort = 995;
        public static string Client { get; set; }
    }

    public enum PopTokenType : int
    {
        Authorization = 0,
        Transaction = 1,
        None
    }

    public interface IPopParameter
    {
        string Token { get; set; }
    }

    public interface IPopAuthParameter : IPopParameter
    {
        PopTokenType Type 
        { 
            get; 
            set; 
        }

        IEnumerable<string> GetAuthParameter();
    }

    public interface IPopTransactionParameter : IPopParameter
    {
        PopTokenType Type 
        { 
            get; 
            set; 
        }

        IEnumerable<string> GetTransactionParameter();
    }

    public sealed class PopParameter 
        : 
        IPopAuthParameter, 
        IPopTransactionParameter
    {
        public PopParameter(string token)
        {
            Token = token;
        }

        public string Token 
        { 
            get; 
            set; 
        }

        #region PopParameter (Authentication)

        PopTokenType IPopAuthParameter.Type
        {
            get;
            set;
        }

        IEnumerable<string> IPopAuthParameter.GetAuthParameter()
        {
            foreach (var parameter in new string[] { Constants.UserAuthenticationString, Constants.PasswordAuthenticationString })
                yield return parameter;

            yield break;
        }

        #endregion

        #region PopParameter (Transaction)

        PopTokenType IPopTransactionParameter.Type
        {
            get;
            set;
        }

        IEnumerable<string> IPopTransactionParameter.GetAuthParameter()
        {
            foreach (var parameter in new string[] { Constants.ListTokenString })
                yield return parameter;

            yield break;
        }

        #endregion
    }

    public class GMailClient 
        : IDisposable
    {
        public GMailClient()
        {

        }

        #region GMailClient (IDisposable)

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }

        #endregion
    }

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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                client = null;
                address = null;
            }
        }

        #endregion

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

        public void OpenWithAuthentication()
        {
            Open();

            IPopAuthParameter authentication =
                              new PopParameter(Constants.UserAuthenticationString);

            authentication.
                Type = PopTokenType.Authorization;
            
            SendAuthentication(authentication);
        }

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

        public void SendTransaction(IPopTransactionParameter parameter)
        {
            foreach (var command in parameter.GetTransactionParameter())
                SendToServerInternal
                    (
                        new PopParameter(command)
                    );
        }

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

        private byte[] ConvertToByteSequence(string item, out int byteCount)
        {
            byteCount = -1;
            var bytes = Encoding.Default.GetBytes(item);
            byteCount = bytes.Length;

            return bytes;
        }
    }

    public static class Constants
    {
        public const string UserAuthenticationString = "USER";
        public const string PasswordAuthenticationString = "PASS";
        public const string ListTokenString = "LIST";
    }

    public static class Extensions
    {

    }
}
