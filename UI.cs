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

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

//using Google;
//using Google.Apis;
//using Google.Apis.Drive;
//using Google.Apis.Drive.v2;
//using Google.Apis.Drive.v2.Data;

namespace IxSApp
{
    using Data;

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
     
    create table asxs_memo (id bigint identity primary key, id_firm bigint references asxs_firm (id), memo text)
    create table asxs_anlage (id bigint primary key, id_firm bigint, document binary)
    create table asxs_mandant (id bigint primary key, id_firm bigint, reply_req bit, foreign key id_firm references asxs_firm (id))
     
    */

    public partial class UI : Form
    {
        private ListViewItem selectedItem = null;
        private ListViewItem.ListViewSubItem selectedSubItem = null;
        private Thread reloadFirmThread = null;
        private ViewUI viewUi = null;
        private Units package;
        private int columnDisplayIndex = -1;
        private string sortColumn = "ORDER BY Absage";
        private bool reNewView = false;
        private bool caseSensitive4Filter = false;

        public UI()
        {
            InitializeComponent();
        }

        //Portnumber for POP3 SSL is 995
        //Can connect with UDP, because it is not security question and here we need only the packages, without checking them in total way
        //Local ip address 192.168.83.100
        //SendAuthorization
        //SendTransaction

        protected override void DefWndProc(ref Message m)
        {
            base.DefWndProc(ref m);
        }

        private void UI_Load(object sender, EventArgs e)
        {
            splitJobControl.Panel2Collapsed = true;
            Initialize();
            reloadFirmThread.Start();
        }

        private void Initialize()
        {
            reloadFirmThread = new Thread(new ThreadStart(ReNewView));
            package = new Units()
            {
                Memo = new WorkUnitMemo(),
                Address = new WorkUnitAddress(),
                Bewerbung = new WorkUnitBewerbung(),
                Firm = new WorkUnitFirm(),
                Mandant = new WorkUnitMandant()
            };
            viewUi = new ViewUI();
        }

        private void ReNewView()
        {
            var reNewActivity
                = 1;

            Thread.Sleep(reNewActivity == 1 ? 1 : 5000);

            if (lstFirm.InvokeRequired || reNewView)
            {
                lstFirm.Invoke(new Action(() =>
                {
                    lstFirm.SuspendLayout();
                    lstFirm.Items.Clear();

                    Application.DoEvents();

                    using (var connection = new SAConnection(ConnectionStringManager.ConnectionStringNetworkServer))
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            var command =
                                connection.CreateCommand();

                            command.CommandText = "SELECT * FROM V_FIRM " + sortColumn + (sortColumn.StartsWith("ORDER BY") ? " ASC" : string.Empty);
                            command.Prepare();

                            using (var reader = command.ExecuteReader())
                            {
                                var jobNr = 1;

                                while (reader.Read())
                                {
                                    var id = reader.GetInt64(0);
                                    if (id == 999)
                                        continue;

                                    var firmName =
                                        reader["Firma"].ToString();

                                    if (chkFilter.Checked)
                                        if (!(firmName.CompareWithCase(txtSearch.Text)))
                                            continue;

                                    package = new Units() 
                                    { 
                                        Firm = new WorkUnitFirm(),
                                        Bewerbung = new WorkUnitBewerbung(), 
                                        Address = new WorkUnitAddress(), 
                                        Memo = new WorkUnitMemo(),
                                        Anlage = new WorkUnitAnlage(),
                                        Mandant = new WorkUnitMandant()
                                    };

                                    var dataItem = 
                                        new ListViewItemUnit(jobNr.ToString().PadLeft(2, '0')) { Value = new UnitContentInfo() { Id = (package.Firm.Id = long.Parse(reader["ID"].ToString())), TableName = "ASXS_FIRM" } };

                                    package.Firm.Website = reader["Website"].ToString();
                                    package.Firm.Id_Memo = long.Parse(string.IsNullOrEmpty(reader["id_memo"].ToString()) ? "0" : reader["id_memo"].ToString());
                                    package.Firm.Id_Bew = long.Parse(reader["id_bew"].ToString());
                                    package.Firm.Id_Addr = long.Parse(reader["id_addr"].ToString());
                                    package.Firm.Id_Mandant = package.Mandant.Id = long.Parse(string.IsNullOrEmpty(reader["id_man"].ToString()) ? "0" : reader["id_man"].ToString());
                                    package.Firm.ReplyRequired = package.Mandant.ReplyRequired = string.IsNullOrEmpty(reader["reply_req"].ToString()) ? false : (bool)reader["reply_req"];
                                    package.Bewerbung.Id = long.Parse(reader["id_bew"].ToString());
                                    package.Bewerbung.Day = (DateTime)reader["Tag"];
                                    package.Address.Id = long.Parse(reader["id_addr"].ToString());
                                    package.Memo.Id = long.Parse(string.IsNullOrEmpty(reader["id_memo"].ToString()) ? "0" : reader["id_memo"].ToString());
                                    package.Memo.Content = reader["Memo"].ToString();

                                    lstFirm.Items.Add(dataItem);
                                    lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add((package.Firm.Name = reader["Firma"].ToString()));
                                    lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add((package.Bewerbung.Reply = (bool)reader["Rueckmeldung"]).ToString().ToUpper() == "TRUE" ? "Ja" : "Nein");

                                    var sentInformationToFirm =
                                        (package.Bewerbung.Sent = (bool)reader["Abgeschickt"]).ToString().ToUpper() == "TRUE" ? "Ja" : "Nein";

                                    lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add(sentInformationToFirm);

                                    var today = DateTime.Today;
                                    //var idleTime =
                                    //    (today - (package.Bewerbung.Day = DateTime.Parse(reader["Tag"].ToString())).Value).Days.ToString();
                                    var idleTime =
                                        today.SubtractTimeSpanWithoutWeekends(DateTime.Parse(reader["Tag"].ToString()));

                                    lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add(idleTime + " Tage");

                                    var negativeReply
                                        = (package.Bewerbung.State = (bool)reader["Absage"]).ToString().ToUpper() == "TRUE" ? "Ja" : "Nein";

                                    var editButtonItem 
                                        = lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add(negativeReply);

                                    lstFirm
                                        .Controls.Add(new Button() 
                                                      { 
                                                          Size = new System.Drawing.Size(25, 20), 
                                                          Location = new System.Drawing.Point(editButtonItem.Bounds.X+255, editButtonItem.Bounds.Y-1),
                                                          Text = "...",
                                                          Visible = false
                                                      });

                                    if (idleTime > 3)
                                    {
                                        //lstFirm.Items[jobNr - 1].BackColor = Color.FromArgb(112, 191, 250);
                                        //lstFirm.Items[jobNr - 1].ForeColor = Color.White;
                                    }
                                    else
                                    {
                                        if (idleTime > 2)
                                        {
                                            lstFirm.Items[jobNr - 1].BackColor = Color.WhiteSmoke;
                                        }
                                    }

                                    if (sentInformationToFirm == "Nein")
                                        lstFirm.Items[jobNr - 1].BackColor = Color.AntiqueWhite;

                                    if (negativeReply == "Ja")
                                    {
                                        lstFirm.Items[jobNr - 1].BackColor = Color.IndianRed;
                                        lstFirm.Items[jobNr - 1].ForeColor = Color.White;
                                        lstFirm.Items[jobNr - 1].Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Strikeout, GraphicsUnit.Point);
                                    }

                                    //make a rule change set, with priority for color management of that information from user or for e.g. firm

                                    if (package.Firm.ReplyRequired)
                                    {
                                        lstFirm.Items[jobNr - 1].BackColor = Color.Chocolate;
                                        lstFirm.Items[jobNr - 1].ForeColor = Color.White;

                                    }

                                    dataItem.Value.Item = package;

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

                    //Make a control, draw it in item directly

                    //var g = lstFirm.CreateGraphics();
                    //if (g != null)
                    //{
                    //    var item = lstFirm.Items[15 - 1].GetBounds(ItemBoundsPortion.Entire);
                    //    g.DrawRectangle(Pens.DarkGray, new System.Drawing.Rectangle(item.X, item.Y, 929, 17));
                    //}

                    if (toolStripFilter.SelectedIndex == -1)
                        toolStripFilter.SelectedIndex = 0;
                }));
            }

            reNewActivity++;
        }

        private void lstFirm_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                selectedItem = e.Item;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void toolStripCrm_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var item = e.ClickedItem;
            if (item != null)
            {
                switch (item.Name)
                {
                    case "toolBarAdd":
                        break;
                    case "toolBarEdit":
                        break;
                    case "toolBarDelete":
                        break;
                    case "toolBarRefresh":
                        break;
                }

                if (item.Name == "toolBarEdit")
                {

                }
            }
        }

        private void toolBarAdd_Click(object sender, EventArgs e)
        {
            viewUi.Id = ((ListViewItemUnit)selectedItem).Value.Id;
            viewUi.Package = ((ListViewItemUnit)selectedItem).Value.Item;
            viewUi.TypeOfEditing = StatementType.Insert;

            if (viewUi.IsDisposed)
            {
                viewUi = new ViewUI();
                viewUi.Id = ((ListViewItemUnit)selectedItem).Value.Id;
                viewUi.Package = ((ListViewItemUnit)selectedItem).Value.Item;
                viewUi.TypeOfEditing = StatementType.Insert;
            }

            if (viewUi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        private void toolButtonEdit_Click(object sender, EventArgs e)
        {
            viewUi.Id = ((ListViewItemUnit)selectedItem).Value.Id;
            viewUi.Package = ((ListViewItemUnit)selectedItem).Value.Item;
            viewUi.TypeOfEditing = StatementType.Update;

            if (viewUi.IsDisposed)
            {
                viewUi = new ViewUI();
                viewUi.Id = ((ListViewItemUnit)selectedItem).Value.Id;
                viewUi.Package = ((ListViewItemUnit)selectedItem).Value.Item;
                viewUi.TypeOfEditing = StatementType.Update;
            }

            if (viewUi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        private void toolButtonDelete_Click(object sender, EventArgs e)
        {
            using (var edit = new FaOrganisationEdit())
            {
                var bewerbung =
                    ((ListViewItemUnit)selectedItem).Value.Item.Bewerbung;
                
                bewerbung.State = true;

                edit.Edit(new Units()
                {
                    Bewerbung = bewerbung
                },

                new BewerbungDataUnit(),
                ((ListViewItemUnit)selectedItem).Value.Id);
            }
        }

        private void toolButtonRefresh_Click(object sender, EventArgs e)
        {
            reNewView = false;
            try
            {
                reloadFirmThread.Abort();
                reloadFirmThread = null;
            }
            catch { }

            reloadFirmThread = new Thread(new ThreadStart(ReNewView));
            reloadFirmThread.Start();
        }

        private void lstFirm_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }

        private void lstFirm_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked)
            {
                e.Item.BackColor = Color.NavajoWhite;
            }
            else
                e.Item.BackColor = Color.White;
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void lstFirm_MouseUp(object sender, MouseEventArgs e)
        {
            var hitTestItem 
                = lstFirm.HitTest(e.Location);
            
            if (hitTestItem != null) 
            {
                selectedItem = hitTestItem.Item;
                selectedSubItem = hitTestItem.SubItem;
            }
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void druckenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void datenbankToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tabFirm_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabFirm.SelectedTab.Name.ToUpper())
            {
                case "TABPAGE2":
                    {
                        lstDrive.Items.Clear();
                        try
                        {
                            using (var connection = new SAConnection
                                                    (
                                                        ConnectionStringManager.ConnectionStringNetworkServer
                                                    ))
                            {
                                connection
                                    .Open();

                                if (connection.State == System.Data.ConnectionState.Open)
                                    using (var action = connection.CreateCommand())
                                    {
                                        action.CommandText = "SELECT ASXS_FIRM.NAME as 'Firma', ASXS_ANLAGE.NAME as 'Anlage' FROM ASXS_FIRM RIGHT OUTER JOIN ASXS_ANLAGE ON ASXS_ANLAGE.ID_FIRM = ASXS_FIRM.ID WHERE ASXS_ANLAGE.ID_FIRM = " + ((ListViewItemUnit)selectedItem).Value.Id;
                                        action.Prepare();
                                        var anlage
                                            = action.ExecuteReader();

                                        while (anlage.Read())
                                        {
                                            lstDrive.Items.Add(anlage["Anlage"].ToString());
                                            lstDrive.Items[lstDrive.Items.Count - 1].SubItems.Add(anlage["Firma"].ToString());
                                        }
                                    }

                                connection.Close();
                            }
                        }
                        catch { }
                        finally
                        {

                        }
                    }
                    break;
            }
        }

        private void tabFirm_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolNew_Click(object sender, EventArgs e)
        {
            var id =
                0L;

            using (var add = new FaOrganisationAppend())
            {
                //factory workunits
                //primary keys, foreign keys constraint
                //check
                //add,edit,remove,create
                //sql tables

                var bewerbung = new WorkUnitBewerbung()
                {
                     Day = DateTime.Now
                };

                id = add.Append
                (
                    new Units()
                    {
                        Bewerbung = bewerbung
                    },

                    new BewerbungDataUnit()
                );

                bewerbung.Id = id;

                var address = new WorkUnitAddress()
                {
                    City = "Musterstadt",
                    Street = string.Empty,
                    Plz = 0,
                    Hnr = 0
                };

                id = add.Append
                (
                    new Units()
                    {
                        Address = address
                    },

                    new AddressDataUnit()
                );

                address.Id = id;

                var memo = new WorkUnitMemo()
                {
                    Content = string.Empty
                };

                id = add.Append
                (
                    new Units()
                    {
                        Memo = memo
                    },

                    new MemoDataUnit()
                );

                memo.Id = id;

                var firm = new WorkUnitFirm()
                {
                    Id_Bew = bewerbung.Id,
                    Id_Addr = address.Id,
                    Id_Memo = memo.Id
                };

                id = add.Append
                (
                    new Units()
                    {
                        Memo = memo,
                        Address = address,
                        Bewerbung = bewerbung,
                        Firm = firm
                    },

                    new FirmDataUnit()
                );
            }
        }

        private void toolState_Click(object sender, EventArgs e)
        {
            try
            {
                using (var edit = new FaOrganisationEdit())
                {
                    var bewerbung =
                        ((ListViewItemUnit)selectedItem).Value.Item.Bewerbung;

                    bewerbung.Sent = true;

                    edit.Edit(new Units()
                    {
                        Bewerbung = bewerbung
                    },

                    new BewerbungDataUnit(),
                    ((ListViewItemUnit)selectedItem).Value.Id);
                }
            }
            catch { }
        }

        private void lstFirm_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (columnDisplayIndex != -1)
                lstFirm.Columns[columnDisplayIndex].TextAlign = HorizontalAlignment.Left;

            //lstFirm.Columns[e.Column].TextAlign = HorizontalAlignment.Right;
            //lstFirm.Columns[e.Column].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

            switch (lstFirm.Columns[e.Column].DisplayIndex)
            {
                case 1:
                    sortColumn = "Firma";
                    break;
                case 5:
                    sortColumn = "Absage";
                    break;
                case 3:
                    sortColumn = "Abgeschickt";
                    break;
                case 4:
                    sortColumn = "Tag";
                    break;
                case 2:
                    sortColumn = "Rueckmeldung";
                    break;
            } sortColumn = !string.IsNullOrEmpty(sortColumn) ? "ORDER BY " + sortColumn : string.Empty;

            //toolButtonRefresh_Click(sender, e);

            if (columnDisplayIndex == -1 || columnDisplayIndex != e.Column)
                columnDisplayIndex = e.Column;

            toolStripFilter_SelectedIndexChanged(sender, e);
        }

        private void toolStripFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            reNewView = true;
            ReNewView();
            reNewView = false;

            foreach (ListViewItem item in lstFirm.Items)
            {
                var value = (ListViewItemUnit)item;
                if (value == null)
                    continue;

                switch (toolStripFilter.SelectedIndex) 
                {
                    case 0:
                        continue;
                    case 1:
                        if (value.Value.Item.Bewerbung.Sent) 
                            continue;
                        break;
                    case 2:
                        if (value.Value.Item.Bewerbung.Reply)
                            continue;
                        break;
                    case 3:
                        if (value.Value.Item.Bewerbung.State)
                            continue;
                        break;
                    case 4:
                        continue;
                }

                item.ForeColor = Color.LightGray;
                item.BackColor = Color.White;

                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                    subItem.ForeColor = Color.LightGray;
            }
        }

        private void toolStripFilter_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSearch_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length > 0)
            {
                //sense applications: die funktionen stehen immer fuer mehr funktion als der text oder die anscheinende ansicht aussagt.
                //das ist der sinn von sense applications. better to read as view

                switch (caseSensitive4Filter)
                {
                    case true:
                        sortColumn = "WHERE Firma LIKE '" + txtSearch.Text + "%'"; ;
                        break;
                    case false:
                        sortColumn = "WHERE Upper(Firma) LIKE '%" + txtSearch.Text.ToUpper() + "%'"; ;
                        break;
                }
            }
            else
                if (string.IsNullOrEmpty(txtSearch.Text))
                    sortColumn = string.Empty;

            reNewView = true;
            ReNewView();
            reNewView = false;

            toolStripFilter_SelectedIndexChanged(sender, e);
        }

        private void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            caseSensitive4Filter = chkFilter.Checked;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
        }

        private void lstFirm_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            
        }
    }
}
