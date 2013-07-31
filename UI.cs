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

using iAnywhere;
using iAnywhere.Data;
using iAnywhere.Data.SQLAnywhere;

using System.Data.Common;
using System.Data.SqlTypes;
using System.Data.SqlClient;

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
     
    create table asxs_ids (table_name varchar(255), table_id bigint)
    
    delete asxs_firm
    delete asxs_bewerbung
    delete asxs_address
    
    alter table asxs_bewerbung add sent bit
    alter table asxs_bewerbung add day timestamp
    
    update asxs_bewerbung 
        set reply = 0
     
    update asxs_bewerbung 
        set messages = ''
     
    */

    public partial class UI : Form
    {
        private ListViewItem selectedItem = null;

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
            splitJobControl.Panel2Collapsed = true;
            new Thread(new ThreadStart(ReNew)).Start();
        }

        private void ReNew()
        {
            var reNewActivity 
                = 1;

            while (true)
            {
                Thread.Sleep(reNewActivity == 1 ? 1 : 5000);

                if (lstFirm.InvokeRequired)
                {
                    lstFirm.Invoke(new Action(() =>
                    {
                        lstFirm.Items.Clear();
                        lstFirm.SuspendLayout();

                        using (var connection = new SAConnection("uid=dba;pwd=sql;dbf=asxs;eng=asxs;astart=yes"))
                        {
                            connection.Open();
                            if (connection.State == ConnectionState.Open)
                            {
                                var command =
                                    connection.CreateCommand();

                                command.CommandText = "SELECT * FROM V_FIRM";
                                command.Prepare();

                                using (var reader = command.ExecuteReader())
                                {
                                    var jobNr = 1;

                                    while (reader.Read())
                                    {
                                        lstFirm.Items.Add(new DataListItem(jobNr.ToString().PadLeft(2, '0')) { DataItem = new DataPackage() { Id = int.Parse(reader["ID"].ToString()), TableName = "ASXS_FIRM" } });
                                        lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add(reader["Rueckmeldung"].ToString().ToUpper() == "TRUE" ? "Ja" : "Nein");
                                        lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add(reader["Korrespondenz"].ToString());
                                        lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add(reader["Firma"].ToString());

                                        var sentInformationToFirm =
                                            reader["Abgeschickt"].ToString().ToUpper() == "TRUE" ? "Ja" : "Nein";

                                        lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add(sentInformationToFirm);

                                        var today = DateTime.Today;
                                        var idleTime =
                                            (today - DateTime.Parse(reader["Tag"].ToString())).Days.ToString();

                                        lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add(idleTime + " Tage");

                                        var negativeReply
                                            = reader["Absage"].ToString().ToUpper() == "TRUE" ? "Ja" : "Nein";

                                        lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add(negativeReply);

                                        if (int.Parse(idleTime) > 3)
                                        {
                                            lstFirm.Items[jobNr - 1].BackColor = Color.DarkGray;
                                            lstFirm.Items[jobNr - 1].ForeColor = Color.White;
                                        }
                                        else
                                        {
                                            if (int.Parse(idleTime) > 2)
                                            {
                                                lstFirm.Items[jobNr - 1].BackColor = Color.WhiteSmoke;
                                            }
                                        }

                                        if (negativeReply == "Ja")
                                            lstFirm.Items[jobNr - 1].BackColor = Color.IndianRed;

                                        if (sentInformationToFirm == "Nein")
                                            lstFirm.Items[jobNr - 1].BackColor = Color.Khaki;

                                        jobNr++;
                                    }

                                    try
                                    {
                                        reader.Close();
                                    }
                                    catch { }
                                }
                            }
                        }

                        lstFirm.PerformLayout();

                        if (selectedItem != null)
                        {
                            selectedItem.Selected = true;
                        }
                    }));
                }

                reNewActivity++;
            }
        }

        private void hinzufügenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            splitJobControl.Panel2Collapsed = !splitJobControl.Panel2Collapsed;
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

        private void btnContinue_Click(object sender, EventArgs e)
        {
            using (var faOrganisation =
                       new FaOrganisationAppend())
            {
                faOrganisation.Append
                (
                    new AsFirm() 
                    { 
                        Name = txtOrganisation.Text 
                    }, 
                    new AsAddress() 
                    { 
                        City = "Musterstadt" 
                    }, 
                    new AsBewerbung() 
                    { 
                        State = chkAbsage.Checked,
                        Sent = chkBewerbung.Checked,
                        Day = dateTimeDay.Value
                    }
                );

                chkAbsage.Checked = false;
                chkBewerbung.Checked = false;
                txtOrganisation.Text = string.Empty;
            }
        }

        private void lstFirm_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                using (var connection = new SAConnection("uid=dba;pwd=sql;dbf=asxs;eng=asxs;astart=yes"))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        var command =
                            connection.CreateCommand();

                        command.CommandText = "SELECT * FROM V_FIRM WHERE ID = " + ((DataListItem)e.Item).DataItem.Id.ToString();
                        command.Prepare();

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            txtOrganisation.Text = reader["Firma"].ToString();
                            chkAbsage.Checked = reader["Absage"].ToString().ToUpper() == "TRUE" ? true : false;
                            chkBewerbung.Checked = reader["Abgeschickt"].ToString().ToUpper() == "TRUE" ? true : false;
                            chkReply.Checked = reader["Rueckmeldung"].ToString().ToUpper() == "TRUE" ? true : false;
                            dateTimeDay.Value = reader.GetDateTime(8);
                        }
                    }
                }

                selectedItem = e.Item;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var connection = new SAConnection("uid=dba;pwd=sql;dbf=asxs;eng=asxs;astart=yes"))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    var command =
                        connection.CreateCommand();

                    command.CommandText = 
                        string.Concat
                        (
                            "UPDATE V_FIRM SET Rueckmeldung = ", chkReply.Checked ? 1 : 0, " WHERE ID = ", ((DataListItem)selectedItem).DataItem.Id
                        );

                    command.Prepare();
                    command.ExecuteNonQuery();

                    command.CommandText =
                        string.Concat
                        (
                            "UPDATE V_FIRM SET Absage = ", chkAbsage.Checked ? 1 : 0, " WHERE ID = ", ((DataListItem)selectedItem).DataItem.Id
                        );

                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void bearbeitenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            splitJobControl.Panel2Collapsed = !splitJobControl.Panel2Collapsed;
        }

        private void anzeigenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void lstFirm_DrawItem(object sender, DrawListViewItemEventArgs e)
        {

        }

        private void lstFirm_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {

        }

        private void lstFirm_Click(object sender, EventArgs e)
        {

        }

        private void lstFirm_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void lstFirm_MouseUp(object sender, MouseEventArgs e)
        {
            var item = lstFirm.SelectedItems[0];

            lstFirm.Controls.Add(new TextBox() { Location = new Point(lstFirm.SelectedItems[0].Bounds.X, lstFirm.SelectedItems[0].Bounds.Y) });
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }
    }

    public struct DataPackage
    {
        public int Id { get; set; }
        public string TableName { get; set; }
    }

    public class DataListItem 
        : ListViewItem
    {
        public DataListItem()
            : base()
        {

        }

        public DataListItem(string text)
            : base(text)
        {

        }

        public DataPackage DataItem { get; set; }
        public Control EntryControl { get; set; }
    }

    public interface IFaOrganisation
    {

    }

    public interface IFaOrganisationAppend : IFaOrganisation
    {

    }

    public interface IFaOrganisationEdit : IFaOrganisation
    {

    }

    public interface IFaOrganisationRemove : IFaOrganisation
    {

    }

    public interface IFaOrganisationDisplay : IFaOrganisation
    {

    }

    public sealed class AsFirm
    {
        public AsFirm()
        {

        }

        public long Id { get; set; }
        public long Id_Bew { get; set; }
        public long Id_Addr { get; set; }
        public string Name { get; set; }
    }

    public sealed class AsAddress
    {
        public AsAddress()
        {

        }

        public long Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public short Plz { get; set; }
        public short Hnr { get; set; }
    }

    public sealed class AsBewerbung
    {
        public AsBewerbung()
        {

        }

        public long Id { get; set; }
        public bool State { get; set; }
        public bool Sent { get; set; }
        public SqlDateTime Day { get; set; }
    }

    public enum PackageType : int
    {
        Firm = 0,
        Bewerbung,
        Address,
        None
    }

    //public class FaOrganisationEdit : IFaOrganisationEdit, IDisposable
    //{

    //}

    //public class FaOrganisationRemove : IFaOrganisationRemove, IDisposable
    //{

    //}

    //public class FaOrganisationDisplay : IFaOrganisationDisplay, IDisposable
    //{

    //}

    //public abstract class FaOrganisationAbstract
    //    : IFaOrganisation, IDisposable
    //{
        

    //    public FaOrganisationAbstract()
    //    {

    //    }


    //}

    public class FaOrganisationAppend : IFaOrganisationAppend, IDisposable
    {
        protected SAConnection connection = null;

        public FaOrganisationAppend()
        {

        }

        #region FaOrganisationAppend (IDisposable)

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }

        #endregion

        #region FaOrganisationAppend (IFaOrganisationAppend)

        public void Append(AsFirm firm, AsAddress address, AsBewerbung bewerbung, string connectionString = "uid=dba;pwd=sql;dbf=asxs;eng=asxs")
        {
            connection = new SAConnection(connectionString: connectionString);
            connection.Open();

            if (connection.State == ConnectionState.Open)
            {
                using (var command = connection.CreateCommand())
                {
                    PrepareTableIds(command, ref firm);
                    PrepareAddressPackageId(command, ref address);

                    Insert(command, string.Format
                    (
                        "INSERT INTO ASXS_ADDRESS VALUES ({0}, '{1}', {2}, '{3}', {4})", address.Id, address.City, address.Plz, address.Street, address.Hnr
                    ));

                    PrepareBewerbungPackageId(command, ref bewerbung);

                    Insert(command, string.Format
                    (
                        "INSERT INTO ASXS_BEWERBUNG (id,state,sent,day) VALUES ({0}, {1}, {2}, '{3}')", bewerbung.Id, bewerbung.State ? 1 : 0, bewerbung.Sent ? 1 : 0, bewerbung.Day.ToSaTimeStamp()
                    ));

                    Insert(command, string.Format
                    (
                        "INSERT INTO ASXS_FIRM VALUES ({0},{1},{2},'{3}')", firm.Id, bewerbung.Id, address.Id, firm.Name
                    ));
                }
            }
        }

        #endregion

        protected virtual void PrepareTableIds(SACommand command, ref AsFirm firm)
        {
            firm.Id = GetAndIncrementTableId(command, "ASXS_FIRM", true);
            firm.Id_Bew = GetAndIncrementTableId(command, "ASXS_BEWERBUNG", true);
            firm.Id_Addr = GetAndIncrementTableId(command, "ASXS_ADDRESS", true);
        }

        protected virtual void PrepareAddressPackageId(SACommand command, ref AsAddress address)
        {
            address.Id = GetAndIncrementTableId(command, "ASXS_ADDRESS");
        }

        protected virtual void PrepareBewerbungPackageId(SACommand command, ref AsBewerbung bewerbung)
        {
            bewerbung.Id = GetAndIncrementTableId(command, "ASXS_BEWERBUNG");
        }

        protected virtual void Insert(SACommand command, string commandText)
        {
            try
            {
                try
                {
                    if (command != null)
                        command.Cancel();
                }
                catch { }

                command.CommandText = commandText;
                command.Prepare();

                var result
                    = command.BeginExecuteNonQuery();

                while (!result.IsCompleted)
                    Thread.Sleep(new TimeSpan(1));
                command.EndExecuteNonQuery(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected virtual void InsertTableId(SACommand command, string tableName)
        {
            Insert(command, string.Concat("INSERT INTO ASXS_IDS VALUES ('", tableName, "',", 1, ")"));
        }

        protected virtual bool TableExistsInIds(SACommand command, string tableName)
        {
            command.CommandText = string.Concat("SELECT * FROM ASXS_IDS WHERE TABLE_NAME = '", tableName, "'");
            command.Prepare();
            var reader
                = command.ExecuteReader();
            var tableIsAlive = reader.HasRows;

            try
            {
                reader.Close();
            }
            catch { }
            finally
            {
                reader.Dispose();
                reader = null;
            }

            return tableIsAlive;
        }

        protected virtual long GetAndIncrementTableId(SACommand command, string tableName, bool increment = false)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (tableName == string.Empty || tableName == null)
                throw new ArgumentNullException("tableName");

            var id
                 = 0L;

            if (!(TableExistsInIds(command, tableName)))
                InsertTableId(command, tableName);

            command.CommandText = string.Concat("SELECT TABLE_ID FROM ASXS_IDS WHERE TABLE_NAME = '", tableName, "'");
            command.Prepare();
            id = (long)command.ExecuteScalar();

            if (increment)
            {
                id++;
                Insert(command, string.Concat("UPDATE ASXS_IDS SET TABLE_ID = ", id, " WHERE TABLE_NAME = '", tableName, "'"));
            }

            return id;
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
        protected virtual void Dispose(bool disposing)
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
        public static string ToSaTimeStamp(this SqlDateTime value)
        {
            var dateTimeValue = value.ToSqlString();
            var dateTimeValues
                = dateTimeValue.Value.Split(new char[] { '.' });
            var year = string.Empty;

            if (dateTimeValues.Length > 0)
            {
                var dateTimeYear
                    = dateTimeValues[2].Split(new char[] { ' ' });
                
                if (dateTimeYear.Length > 0)
                    year = dateTimeYear[0];
            }

            return year + "/" + dateTimeValues[1] + "/" + dateTimeValues[0];
        }
    }
}
