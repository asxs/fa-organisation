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
    /*

    create table asxs_firm (id bigint primary key, id_bew bigint, id_addr bigint, name varchar(255))
    create table asxs_bewerbung (id bigint primary key, state bit)
    create table asxs_address (id bigint primary key, city varchar(255), plz int, street varchar(255), hnr int)
    alter table asxs_firm add foreign key id_addr references asxs_address (id)
    alter table asxs_firm add foreign key id_bew references asxs_bewerbung (id)
    
    create view v_firm
    as
    select asxs_firm.name       as 'Firma',
           asxs_address.city    as 'Stadt',
           asxs_address.plz     as 'PLZ',
           asxs_address.street  as 'Straße',
           asxs_address.hnr     as 'Hausnummer',
           asxs_bewerbung.state as 'Absage'
           from asxs_firm
           left outer join asxs_bewerbung on asxs_bewerbung.id = asxs_firm.id_bew
           left outer join asxs_address on asxs_address.id = asxs_firm.id_addr
     
    */

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

        private void UI_Load(object sender, EventArgs e)
        {
            
        }

        private void hinzufügenToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void lstFirm_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void lstFirm_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstFirm_DoubleClick(object sender, EventArgs e)
        {

        }
    }

    /// <summary>
    /// Google GMail Property Package
    /// </summary>
    public static class GMailPop3
    {
        /// <summary>
        /// Gets the POP3 Server from GMail
        /// </summary>
        public const string Server = "pop.gmail.com";

        /// <summary>
        /// POP3 (SSL)
        /// </summary>
        public const uint SslPort = 995;

        /// <summary>
        /// Gets or sets the client IP Address (for e.g. 192.168.xxx.xxx)
        /// </summary>
        public static string Client { get; set; }
    }

    /// <summary>
    /// Gets the Token-Type for type of sending messages to a POP3 - Account
    /// </summary>
    public enum PopTokenType : int
    {
        /// <summary>
        /// Authorization
        /// </summary>
        Authorization = 0,

        /// <summary>
        /// Transaction
        /// </summary>
        Transaction = 1,

        /// <summary>
        /// Default
        /// </summary>
        None
    }

    /// <summary>
    /// POP3 Parameter with Tokens for Authentication or Transaction (for e.g. USER or PASS or LIST)
    /// </summary>
    public interface IPopParameter
    {
        /// <summary>
        /// Gets or sets the POP3 Token
        /// </summary>
        string Token { get; set; }
    }

    /// <summary>
    /// POP3 Authentication Parameter
    /// </summary>
    public interface IPopAuthParameter 
        : IPopParameter
    {
        /// <summary>
        /// Gets the Token-Type for type of sending messages to a POP3 - Account
        /// </summary>
        PopTokenType Type 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets all necessary authentication parameter
        /// </summary>
        /// <returns>Sequence with waiter</returns>
        IEnumerable<string> GetAuthParameter();
    }

    /// <summary>
    /// POP3 Transaction Parameter
    /// </summary>
    public interface IPopTransactionParameter 
        : IPopParameter
    {
        /// <summary>
        /// Gets the Token-Type for type of sending messages to a POP3 - Account
        /// </summary>
        PopTokenType Type 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets all necessary transaction parameter
        /// </summary>
        /// <returns>Sequence with waiter</returns>
        IEnumerable<string> GetTransactionParameter();
    }

    /// <summary>
    /// Explicit use of Authentication or Transaction parameters for interfaces of IPopAuthParameter or IPopTransactionParameter
    /// </summary>
    public sealed class PopParameter 
        : 
        IPopAuthParameter, 
        IPopTransactionParameter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Can be a custom token from the protocol type like USER or PASS or LIST</param>
        public PopParameter(string token)
        {
            Token = token;
        }

        /// <summary>
        /// Gets or sets the token for communication with the POP3 Server
        /// </summary>
        public string Token 
        { 
            get; 
            set; 
        }

        #region PopParameter (Authentication)

        /// <summary>
        /// Gets the Token-Type for type of sending messages to a POP3 - Account
        /// </summary>
        PopTokenType IPopAuthParameter.Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets all necessary authentication parameter
        /// </summary>
        /// <returns>Sequence with waiter</returns>
        IEnumerable<string> IPopAuthParameter.GetAuthParameter()
        {
            foreach (var parameter in new string[] { Constants.UserAuthenticationString, Constants.PasswordAuthenticationString })
                yield return parameter;

            yield break;
        }

        #endregion

        #region PopParameter (Transaction)

        /// <summary>
        /// Gets the Token-Type for type of sending messages to a POP3 - Account
        /// </summary>
        PopTokenType IPopTransactionParameter.Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets all necessary transaction parameter
        /// </summary>
        /// <returns>Sequence with waiter</returns>
        IEnumerable<string> IPopTransactionParameter.GetTransactionParameter()
        {
            foreach (var parameter in new string[] { Constants.ListTokenString })
                yield return parameter;

            yield break;
        }

        #endregion
    }

    /// <summary>
    /// Little GMail Client that receives only a List of E-Mails
    /// </summary>
    public class GMailClient 
        : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GMailClient()
        {

        }

        #region GMailClient (IDisposable)

        /// <summary>
        /// Disposing all save or unsaved components
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing all save or unsaved components
        /// </summary>
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

    /// <summary>
    /// Constants for the Process
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// USER
        /// </summary>
        public const string UserAuthenticationString = "USER";

        /// <summary>
        /// PASS
        /// </summary>
        public const string PasswordAuthenticationString = "PASS";

        /// <summary>
        /// LIST
        /// </summary>
        public const string ListTokenString = "LIST";
    }

    public static class Extensions
    {

    }
}
