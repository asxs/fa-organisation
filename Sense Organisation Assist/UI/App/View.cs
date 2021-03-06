﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

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

using System.Drawing.Imaging;
using System.Drawing.Text;

using System.Text;
using System.Text.RegularExpressions;

//using Google;
//using Google.Apis;
//using Google.Apis.Drive;
//using Google.Apis.Drive.v2;
//using Google.Apis.Drive.v2.Data;

namespace IxSApp
{
    using Data;
    using LvUnits;

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

    /*
     * Suchfenster mit vordefiniertem Text als eigenes Control?
     * Die Bewerbungswebseite mit ASP.NET
     * Eine Aktualisierungseinstellung mit in die ListView aufnehmen
     * Datenbank - Design überarbeiten
     * Anzeigeeinstellungen für die Tabs
     * Möglichkeit per Drag&Drop die Firmen zu ordnen. Währenddessen die Form / die ListView deaktivieren, damit das Neuzeichnen verhindert wird. Add+ Cursor / Wrong Cursor
     * ListView - Edit per HitTest
     * Look & Feel / User-Modes mit Abfragen kennzeichnen
     * Error Handling etablieren
     * Datenbank - Optionen verfügbar machen
     * Start der Datenbank / Status der Datenbank anzeigen. Im Datenbank - Formular
     */

    public partial class View : Form
    {
        private ListViewItem selectedItem = null;
        private ListViewItem.ListViewSubItem selectedSubItem = null;
        private Thread refreshVacancyViewThread = null;
        private VacancyView viewUi = null;
        private Units package;
        private int columnDisplayIndex = -1;
        private string sortColumn = "ORDER BY Absage";
        private bool reNewView = false;
        private bool caseSensitive4Filter = false;
        private List<CustomTabControl> customTabControls = null;

        public View()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

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
            lstFirm.Groups.Add(new ListViewGroup("Zusagen   "));
            lstFirm.Groups.Add(new ListViewGroup("Stellenangebote   "));
            //lstFirm.Groups.Add(new ListViewGroup("Absagen   "));

            //splitJobControl.Panel2Collapsed = true;
            Initialize();
            refreshVacancyViewThread.Start();
            try
            {
                VacancyView.StartLocation = new Point(Location.X + 23, Location.Y + 80);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            refreshVacancyViewThread 
                = new Thread(new ThreadStart(ReNewView));

            package = new Units()
            {
                Memo = new WorkUnitMemo(),
                Address = new WorkUnitAddress(),
                Bewerbung = new WorkUnitBewerbung(),
                Firm = new WorkUnitFirm(),
                Mandant = new WorkUnitMandant()
            };
            viewUi = new VacancyView();

            customTabControls = new List<CustomTabControl>();
            customTabControls.Add(new CustomTabControl() { Control = lblFirm, Active = true, InheritedControls = new List<Control>() { lstFirmlist } });
            customTabControls.Add(new CustomTabControl() { Control = lblVacancy, Active = false, InheritedControls = new List<Control>() { lstFirm } });
            customTabControls.Add(new CustomTabControl() { Control = lblUnterlagen, Active = false, InheritedControls = new List<Control>() });
            customTabControls.Add(new CustomTabControl() { Control = lblVerwaltung, Active = false, InheritedControls = new List<Control>() });
            customTabControls.Add(new CustomTabControl() { Control = lblSearch, Active = false, InheritedControls = new List<Control>() { lstJobSearch, lnkStellensuche, txtLnk, txtLocation, chkBewerbungen, chkLocation, txtKeywords, chkKeyword, groupBox2, groupBox3, txtRegion, label14, chkFirm, txtFirm } });
        }

        /// <summary>
        /// Res the new view.
        /// </summary>
        private void ReNewView()
        {
            try
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
                        lstFirmlist.Items.Clear();

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

                                        if (chkFilter.Checked && !string.IsNullOrEmpty(txtSearch.Text))
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
                                            new ListViewItemUnit(long.Parse(reader["ID"].ToString()).ToString().PadLeft(2, '0')) { Value = new UnitContentInfo() { Id = (package.Firm.Id = long.Parse(reader["ID"].ToString())), TableName = "ASXS_FIRM" } };

                                        lstFirmlist.Items.Add(long.Parse(reader["ID"].ToString()).ToString().PadLeft(2, '0'));
                                        lstFirmlist.Items[lstFirmlist.Items.Count - 1].SubItems.Add(firmName);

                                        package.Firm.Website = reader["Website"].ToString();
                                        package.Firm.Id_Memo = long.Parse(string.IsNullOrEmpty(reader["id_memo"].ToString()) ? "0" : reader["id_memo"].ToString());
                                        package.Firm.Id_Bew = long.Parse(reader["id_bew"].ToString());
                                        package.Firm.Id_Addr = long.Parse(reader["id_addr"].ToString());
                                        package.Firm.Id_Mandant = package.Mandant.Id = long.Parse(string.IsNullOrEmpty(reader["id_man"].ToString()) ? "0" : reader["id_man"].ToString());
                                        package.Firm.ReplyRequired = package.Mandant.ReplyRequired = string.IsNullOrEmpty(reader["reply_req"].ToString()) ? false : (bool)reader["reply_req"];
                                        package.Bewerbung.Id = long.Parse(reader["id_bew"].ToString());
                                        package.Bewerbung.Day = (DateTime)reader["Tag"];
                                        package.Bewerbung.Zusage = !string.IsNullOrEmpty(reader["Zusage"].ToString()) ? (bool)reader["Zusage"] : false;
                                        package.Address.Id = long.Parse(reader["id_addr"].ToString());
                                        package.Memo.Id = long.Parse(string.IsNullOrEmpty(reader["id_memo"].ToString()) ? "0" : reader["id_memo"].ToString());
                                        package.Memo.Content = reader["Memo"].ToString();
                                        package.Bewerbung.NegativeStateAtOwn = !string.IsNullOrEmpty(reader["Eigenverantwortlich"].ToString()) ? (bool)reader["Eigenverantwortlich"] : false;
                                        package.Bewerbung.FirmAnswer = !string.IsNullOrEmpty(reader["Antwort"].ToString()) ? (reader["Antwort"].ToString().ToUpper() == "1" ? true : false) : string.IsNullOrEmpty(reader["Antwort"].ToString()) ? new Nullable<bool>() : false;
                                        package.Bewerbung.AwaitingFirmReply = !string.IsNullOrEmpty(reader["Firmenrueckmeldung"].ToString()) ? ((reader["Firmenrueckmeldung"]).ToString().ToUpper() == "1" ? true : false) : false;

                                        dataItem.EntryControl = new TextBox();

                                        lstFirm.Items.Add(dataItem);
                                        lstFirm.Items[lstFirm.Items.Count - 1].UseItemStyleForSubItems = false;
                                        lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add((package.Firm.Name = reader["Firma"].ToString()));
                                        lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add("");
                                        lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add("");
                                        lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add("");
                                        lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add((package.Bewerbung.Reply = (bool)reader["Rueckmeldung"]).ToString().ToUpper() == "TRUE" ? "Ja" : "Nein");

                                        var sentInformationToFirm =
                                            (package.Bewerbung.Sent = (bool)reader["Abgeschickt"]).ToString().ToUpper() == "TRUE" ? "Ja" : "Nein";

                                        lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add(sentInformationToFirm);

                                        if (sentInformationToFirm == "Ja")
                                            lstFirm.Items[lstFirm.Items.Count - 1].SubItems[6].BackColor = Color.LightYellow;

                                        var today = DateTime.Today;
                                        //var idleTime =
                                        //    (today - (package.Bewerbung.Day = DateTime.Parse(reader["Tag"].ToString())).Value).Days.ToString();
                                        var idleTime =
                                            today.SubtractTimeSpanWithoutWeekends(DateTime.Parse(reader["Tag"].ToString()));

                                        lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add(idleTime + " Tage");

                                        var negativeReply
                                            = (package.Bewerbung.State = (bool)reader["Absage"]).ToString().ToUpper() == "TRUE" ? "Ja" : "Nein";

                                        //if (negativeReply == "Ja")
                                        //{
                                        //    lstFirm.Groups[1].Items.Add(lstFirm.Items[lstFirm.Items.Count - 1]);
                                        //}

                                        var negativeStateAtOwn
                                            = package.Bewerbung.NegativeStateAtOwn ? "Ja" : "Nein";

                                        var editButtonItem
                                            = lstFirm.Items[lstFirm.Items.Count - 1].SubItems.Add(negativeReply);

                                        lstFirm
                                            .Controls.Add(new Button()
                                                          {
                                                              Size = new System.Drawing.Size(25, 20),
                                                              Location = new System.Drawing.Point(editButtonItem.Bounds.X + 255, editButtonItem.Bounds.Y - 1),
                                                              Text = "...",
                                                              Visible = false
                                                          });

                                        //if (idleTime > 3)
                                        if (false)
                                        {
                                            //lstFirm.Items[jobNr - 1].BackColor = Color.FromArgb(112, 191, 250);
                                            //lstFirm.Items[jobNr - 1].ForeColor = Color.White;
                                        }
                                        else
                                        {
                                            if (idleTime > 2 && idleTime < 30)
                                            {
                                                lstFirm.Items[jobNr - 1].BackColor = Color.WhiteSmoke;
                                            }
                                            else if (idleTime > 30)
                                            {
                                                lstFirm.Items[jobNr - 1].BackColor = Color.Black;
                                                lstFirm.Items[jobNr - 1].ForeColor = Color.White;
                                                lstFirm.Items[jobNr - 1].UseItemStyleForSubItems = true;
                                            }
                                        }

                                        if (sentInformationToFirm == "Nein")
                                        {
                                            lstFirm.Items[jobNr - 1].BackColor = Color.AntiqueWhite;
                                            lstFirm.Items[jobNr - 1].UseItemStyleForSubItems = false;
                                        }

                                        if (negativeReply == "Ja" || negativeStateAtOwn == "Ja")
                                        {
                                            lstFirm.Items[jobNr - 1].BackColor = Color.IndianRed;
                                            lstFirm.Items[jobNr - 1].ForeColor = Color.White;
                                            lstFirm.Items[jobNr - 1].Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Strikeout, GraphicsUnit.Point);
                                            lstFirm.Items[jobNr - 1].UseItemStyleForSubItems = false;
                                        }

                                        //make a rule change set, with priority for color management of that information from user or for e.g. firm

                                        if (package.Firm.ReplyRequired)
                                        {
                                            lstFirm.Items[jobNr - 1].BackColor = Color.Chocolate;
                                            lstFirm.Items[jobNr - 1].ForeColor = Color.White;
                                        }

                                        if (package.Bewerbung.Zusage)
                                        {
                                            lstFirm.Items[jobNr - 1].ForeColor = Color.White;
                                            lstFirm.Items[jobNr - 1].BackColor = Color.ForestGreen;
                                            //lstFirm.Items[jobNr - 1].SubItems[1].Font = new Font("Arial", 10.25f, FontStyle.Bold);
                                        }

                                        if (package.Bewerbung.AwaitingFirmReply) 
                                        {
                                            lstFirm.Items[jobNr - 1].BackColor = Color.DarkGoldenrod;
                                            lstFirm.Items[jobNr - 1].ForeColor = Color.White;
                                        }

                                        if (package.Bewerbung.FirmAnswer != null)
                                        {
                                            if (!package.Bewerbung.FirmAnswer.Value)
                                            {
                                                lstFirm.Items[jobNr - 1].ForeColor = Color.White;
                                                lstFirm.Items[jobNr - 1].BackColor = Color.White;
                                            }
                                            else
                                            {
                                                if (package.Bewerbung.FirmAnswer.Value)
                                                {
                                                    lstFirm.Items[jobNr - 1].BackColor = Color.Green;
                                                    lstFirm.Items[jobNr - 1].ForeColor = Color.White;
                                                }
                                            }
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

                        foreach (ListViewItem item in lstFirm.Items)
                        {
                            if (item.GetItemUnit().GetItemContentInfo().Item.Bewerbung.Zusage)
                                continue;

                            lstFirm.Groups[1].Items.Add(item);
                        }

                        foreach (ListViewItem item in lstFirm.Items)
                        {
                            if (item.GetItemUnit().GetItemContentInfo().Item.Bewerbung.Zusage)
                                lstFirm.Groups[0].Items.Add(item);
                        }

                        //foreach (ListViewItem item in lstFirm.Items)
                        //{
                        //    if (item.GetItemUnit().GetItemContentInfo().Item.Bewerbung.State)
                        //        lstFirm.Groups[2].Items.Add(item);
                        //}

                        lstFirm.PerformLayout();

                        if (selectedItem != null)
                            selectedItem.Selected = true;

                        //Make a control, draw it in item directly

                        //var g = lstFirm.CreateGraphics();
                        //if (g != null)
                        //{
                        //    var item = lstFirm.Items[15 - 1].GetBounds(ItemBoundsPortion.Entire);
                        //    g.DrawRectangle(Pens.DarkGray, new System.Drawing.Rectangle(item.X, item.Y, 929, 17));
                        //}

                        if (toolStripFilter.SelectedIndex == -1)
                        {
                            toolStripFilter.SelectedIndex = 0;
                            lstFirm_ColumnClick(this, new ColumnClickEventArgs(1));
                        }
                    }));
                }

                reNewActivity++;
            }
            catch (Exception ex)
            {

            }
        }

        private void lstFirm_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                lstInfos.Items.Clear();
                selectedItem = e.Item;
                
                var value = selectedItem as ListViewItemUnit;
                if (value != null)
                {
                    if (!(value.Value.CanSelect))
                        selectedItem.Selected = false;

                    if (selectedItem.Selected)
                    {

                        lstInfos.Items.Add(value.Value.Item.Firm.Name);
                        lstInfos.Items[lstInfos.Items.Count - 1].Font = new Font("Segoe UI", 8.25f, FontStyle.Bold, GraphicsUnit.Point);

                        if (!value.Value.Item.Bewerbung.State && !value.Value.Item.Bewerbung.Zusage)
                        {
                            lstInfos.Items.Add("");
                            lstInfos.Items.Add("Sie haben noch keine");
                            lstInfos.Items.Add("endgültige Antwort zu Ihrer");
                            lstInfos.Items.Add("Bewerbung erhalten.");

                            foreach (var foreColor in new Color[] 
                        { 
                            Color.FromArgb(0, 250, 250, 250), 
                            Color.FromArgb(0, 200, 200, 200), 
                            Color.FromArgb(0, 180, 180, 180), 
                            Color.FromArgb(0, 150, 150, 150), 
                            Color.FromArgb(0, 125, 125, 125), 
                            Color.FromArgb(0, 110, 110, 110), 
                            Color.FromArgb(0, 80, 80, 80), 
                            Color.FromArgb(0, 60, 60, 60), 
                            Color.FromArgb(0, 40, 40, 40), 
                            Color.FromArgb(0, 20, 20, 20), 
                            Color.FromArgb(0, 0, 0, 0) 
                        })
                            {
                                lstInfos.Items[0].ForeColor = foreColor;
                                lstInfos.Items[2].ForeColor = foreColor;
                                lstInfos.Items[2].Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point);
                                lstInfos.Items[3].ForeColor = foreColor;
                                lstInfos.Items[3].Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point);
                                lstInfos.Items[4].ForeColor = foreColor;
                                lstInfos.Items[4].Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point);

                                Thread.Sleep(10);
                                Application.DoEvents();
                            }
                        }
                        else
                        {
                            lstInfos.Items.Add("");
                            if (!(value.Value.Item.Bewerbung.FirmAnswer.HasValue))
                                lstInfos.Items.Add(string.Concat(value.Value.Item.Bewerbung.State ? "Sie haben eine Absage bekommen" : (value.Value.Item.Bewerbung.Zusage ? "Sie haben eine Zusage bekommen" : "Sie haben noch keine endgültige Antwort zu Ihrer Bewerbung erhalten.")));
                            else
                            {
                                if (value.Value.Item.Bewerbung.FirmAnswer.HasValue)
                                    if (!(value.Value.Item.Bewerbung.FirmAnswer.Value))
                                    {
                                        lstInfos.Items.Add("Sie haben eine endgültige");
                                        lstInfos.Items[lstInfos.Items.Count - 1].Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point);
                                        lstInfos.Items[lstInfos.Items.Count - 1].ForeColor = Color.DarkRed;
                                        lstInfos.Items.Add("Absage bekommen.");
                                        lstInfos.Items[lstInfos.Items.Count - 1].Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point);
                                        lstInfos.Items[lstInfos.Items.Count - 1].ForeColor = Color.DarkRed;
                                    }
                            }

                            lstInfos.Items[lstInfos.Items.Count - 1].Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point);
                        }
                    }
                }
            }

            try
            {
                //webVacance.Url = new Uri(((ListViewItemUnit)selectedItem).Value.Item.Firm.Website);
                //webVacance.ScrollBarsEnabled = false;
            }
            catch { }
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

        private ListViewItemUnit vacancyItem = null;

        private void ShowDetailDialog(StatementType typeOfEditing)
        {
            if (typeOfEditing != StatementType.Insert && typeOfEditing != StatementType.Update)
                throw new InvalidOperationException("Detail dialog can only have the insertion or update value");

            viewUi = new VacancyView();
            viewUi.Id = vacancyItem.Value.Id;
            viewUi.Package = vacancyItem.Value.Item;
            viewUi.TypeOfEditing = typeOfEditing;

            var dialogResult
                = viewUi.ShowDialog() == System.Windows.Forms.DialogResult.OK;

            reNewView = true;
            ReNewView();
            reNewView = false;

            toolStripFilter_SelectedIndexChanged(null, new EventArgs());
        }

        private void toolBarAdd_Click(object sender, EventArgs e)
        {

        }

        private void toolButtonEdit_Click(object sender, EventArgs e)
        {
            ShowDetailDialog(typeOfEditing: StatementType.Update);
        }

        private FaOrganisationAbstract firmAppend = null;
        private FaOrganisationAbstract firmEditing = null;

        public static class FaOrganisationFactory
        {
            //public static FaOrganisationAppend CreateOrganisationAppend()
            //{
                
            //}

            //public static FaOrganisationEdit CreateOrganisationEditing()
            //{

            //}
        }

        private void ResetVacancyStateToken()
        {
            reNewView = false;
        }

        public override void Refresh()
        {
            base.Refresh();

            ResetVacancyStateToken();
            refreshVacancyViewThread.ReStart();
        }

        private void toolButtonDelete_Click(object sender, EventArgs e)
        {

            using (var edit = new FaOrganisationEdit())
            {
                var bewerbung =
                    selectedItem.GetItemContentInfo().Item.Bewerbung;
                
                bewerbung.State = true;

                edit.Edit(new Units()
                {
                    Bewerbung = bewerbung
                },

                new BewerbungDataUnit(),
                selectedItem.GetItemContentInfo().Id);
            }

            reNewView = true;
            ReNewView();
            reNewView = false;

            toolStripFilter_SelectedIndexChanged(sender, e);
        }

        private void toolButtonRefresh_Click(object sender, EventArgs e)
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
                vacancyItem = hitTestItem.Item.GetItemUnit();
                
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (selectedItem != null)
                    toolStripFilterFirm.Text = selectedItem.SubItems[1].Text;
            }

            //try
            //{
            //    if (selectedItem != null)
            //    {
            //        if (strikeOutWatcher != null)
            //        {
            //            strikeOutWatcher.Stop();
            //            if (strikeOutWatcher.Elapsed.Milliseconds > 500)
            //            {
            //                canStrikeOut = true;

            //                toolButtonDelete_Click(sender, e);
            //                toolStripFilter_SelectedIndexChanged(sender, e);
            //            }
            //            strikeOutWatcher.Reset();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        //private void tabFirm_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    switch (tabFirm.SelectedTab.Name.ToUpper())
        //    {
        //        case Constants.TabPage2:
        //            {
        //                lstDrive.Items.Clear();
        //                try
        //                {
        //                    using (var connection = new SAConnection
        //                                            (
        //                                                ConnectionStringManager.ConnectionStringNetworkServer
        //                                            ))
        //                    {
        //                        connection
        //                            .Open();

        //                        if (connection.State == System.Data.ConnectionState.Open)
        //                            using (var action = connection.CreateCommand())
        //                            {
        //                                action.CommandText = "SELECT ASXS_FIRM.NAME as 'Firma', ASXS_ANLAGE.NAME as 'Anlage' FROM ASXS_FIRM RIGHT OUTER JOIN ASXS_ANLAGE ON ASXS_ANLAGE.ID_FIRM = ASXS_FIRM.ID WHERE ASXS_ANLAGE.ID_FIRM = " + ((ListViewItemUnit)selectedItem).Value.Id;
        //                                action.Prepare();
        //                                var anlage
        //                                    = action.ExecuteReader();

        //                                while (anlage.Read())
        //                                {
        //                                    lstDrive.Items.Add(anlage["Anlage"].ToString());
        //                                    lstDrive.Items[lstDrive.Items.Count - 1].SubItems.Add(anlage["Firma"].ToString());
        //                                }
        //                            }

        //                        connection.Close();
        //                    }
        //                }
        //                catch { }
        //                finally
        //                {

        //                }
        //            }
        //            break;
        //    }
        //}

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

            reNewView = true;
            ReNewView();
            reNewView = false;

            toolStripFilter_SelectedIndexChanged(sender, e);
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
                case 8:
                    sortColumn = "Absage, Firma";
                    break;
                case 6:
                    sortColumn = "Abgeschickt";
                    break;
                case 7:
                    sortColumn = "Tag";
                    break;
                case 5:
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
            //lstFilter.Items.Clear();

            reNewView = true;
            ReNewView();
            reNewView = false;

            foreach (ListViewItem item in lstFirm.Items)
            {
                var value = item.GetItemUnit();
                if (value == null)
                    continue;

                switch (toolStripFilter.SelectedIndex) 
                {
                    case 0:
                        value.Value.CanSelect = true;

                        if (value.Value.Item.Bewerbung.FirmAnswer.HasValue)
                            if (!(value.Value.Item.Bewerbung.FirmAnswer.Value))
                            {
                                value.Value.CanSelect = false;
                            }
                        continue;
                    case 1:
                        if (value.Value.Item.Bewerbung.Sent)
                        {
                            //lstFilter.Columns[0].Text = "Bewerbungen";
                            //lstFilter.Items.Add(item.SubItems[1].Text);
                            //lstFilter.Items[lstFilter.Items.Count - 1].ImageIndex = 8;
                            value.Value.CanSelect = true;

                            if (value.Value.Item.Bewerbung.FirmAnswer.HasValue)
                                if (!(value.Value.Item.Bewerbung.FirmAnswer.Value))
                                {
                                    value.Value.CanSelect = false;
                                }
                            continue;
                        }
                        break;
                    case 2:
                        if (value.Value.Item.Bewerbung.Reply)
                        {
                            //lstFilter.Columns[0].Text = "Rückmeldungen";
                            //lstFilter.Items.Add(item.SubItems[1].Text);
                            //lstFilter.Items[lstFilter.Items.Count - 1].ImageIndex = 8;
                            value.Value.CanSelect = true;

                            if (value.Value.Item.Bewerbung.FirmAnswer.HasValue)
                                if (!(value.Value.Item.Bewerbung.FirmAnswer.Value))
                                {
                                    value.Value.CanSelect = false;
                                }
                            continue;
                        }
                        break;
                    case 3:
                        if (value.Value.Item.Bewerbung.State)
                        {
                            //lstFilter.Columns[0].Text = "Absagen";
                            //lstFilter.Items.Add(item.SubItems[1].Text);
                            //lstFilter.Items[lstFilter.Items.Count - 1].ImageIndex = 8;
                            value.Value.CanSelect = true;

                            if (value.Value.Item.Bewerbung.FirmAnswer.HasValue)
                                if (!(value.Value.Item.Bewerbung.FirmAnswer.Value))
                                {
                                    value.Value.CanSelect = false;
                                }
                            continue;
                        }
                        break;
                    case 4:
                        if (value.Value.Item.Bewerbung.Zusage)
                        {
                            //lstFilter.Columns[0].Text = "Zusagen";
                            //lstFilter.Items.Add(item.SubItems[1].Text);
                            //lstFilter.Items[lstFilter.Items.Count - 1].ImageIndex = 8;
                            value.Value.CanSelect = true;

                            if (value.Value.Item.Bewerbung.FirmAnswer.HasValue)
                                if (!(value.Value.Item.Bewerbung.FirmAnswer.Value))
                                {
                                    value.Value.CanSelect = false;
                                }
                            continue;
                        }
                        break;
                }

                item.ForeColor = Color.LightGray;
                item.BackColor = Color.White;

                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                {
                    subItem.ForeColor = Color.LightGray;
                }
            }

            //lstFilter.Columns[0].Text = toolStripFilter.SelectedIndex == 0 ? lstFilter.Columns[0].Text = string.Empty : lstFilter.Items.Count.ToString() + " " + lstFilter.Columns[0].Text;
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
                {
                    sortColumn = string.Empty;
                    lstFirm.BackColor = Color.White;
                }

            reNewView = true;
            ReNewView();
            reNewView = false;

            toolStripFilter_SelectedIndexChanged(sender, e);
            lstFirm.BackColor = Color.White;
        }

        private void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            caseSensitive4Filter = chkFilter.Checked;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            lstFirm.BackColor = Color.White;
        }

        private void lstFirm_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lstFirm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (selectedItem.Selected)
                toolButtonEdit_Click(sender, e);
        }

        private void toolStripFilterFirm_Click(object sender, EventArgs e)
        {
            if (selectedItem != null)
                txtSearch.Text = selectedItem.SubItems[1].Text;
        }

        private void toolStripFilterDelete_Click(object sender, EventArgs e)
        {
            btnDelete_Click(sender, e);
        }

        private void filterToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private int x = 0;
        private int y = 0;
        private Graphics graphics = null;

        private void lstFirm_ItemDrag(object sender, ItemDragEventArgs e)
        {
            graphics = lstFirm.CreateGraphics();
            //lstFirm.BeginUpdate();
        }

        private void lstFirm_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = false;
        }

        private bool canStrikeOut = false;
        private System.Diagnostics.Stopwatch strikeOutWatcher = null;

        private void lstFirm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (strikeOutWatcher == null)
                    strikeOutWatcher = new System.Diagnostics.Stopwatch();

                strikeOutWatcher.Start();
            }

            //if (e.Button == System.Windows.Forms.MouseButtons.Left)
            //{
            //    x = e.X;
            //    y = e.Y;

            //    //lstFirm.Refresh();
            //    //lstFirm.PerformLayout();
                
            //    lstFirm.Invalidate();

            //    if (graphics != null)
            //        graphics.DrawRectangle(Pens.Black, new Rectangle(x, y, 300, 20));

            //    //lstFirm.EndUpdate();
            //}

            //lstFirm.EndUpdate();
        }

        private void lstFirm_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void lstFirm_MouseDown(object sender, MouseEventArgs e)
        {
        
        }

        private void lstFirm_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            //lstFirm.BeginUpdate();
            //lstFirm.EndUpdate();

            //if (e.Item != null)
            //{
            //    selectedItem = e.Item;
                
            //    var value = selectedItem as ListViewItemUnit;
            //    if (value != null) 
            //    {
            //        var itemInfo = string.Empty;

            //        itemInfo = string.Concat(itemInfo, value.Value.Item.Firm.Name, "\r\n-\r\n");
            //        itemInfo = string.Concat(itemInfo, value.Value.Item.Mandant.ReplyRequired ? "Es ist eine Rückmeldung erforderlich!" : "Es ist keine Rückmeldung erforderlich!", "\r\n-\r\n");
            //        itemInfo = string.Concat(itemInfo, value.Value.Item.Firm.Website, "\r\n-\r\n");
            //        itemInfo = string.Concat(itemInfo, value.Value.Item.Bewerbung.Sent ? "Die Bewerbungsmappe liegt der ausgewählten Firma vor!" : "Es liegt noch keine Bewerbung vor!", "\r\n-\r\n");
            //        itemInfo = string.Concat(itemInfo, value.Value.Item.Memo.Content.Length > 0 && value.Value.Item.Memo.Content.Length > 10 ? value.Value.Item.Memo.Content.Substring(0, 10) + " ..." : string.Empty, "\r\n-\r\n");

            //        lblQuickInfo.Text = itemInfo;
            //    }

            //    var g = lstFirm.CreateGraphics();
            //    if (g != null)
            //    {
            //        g.DrawRectangle(Pens.Black, new Rectangle(e.Item.Bounds.X, e.Item.Bounds.Y, e.Item.Bounds.Size.Width, e.Item.Bounds.Size.Height));
            //        g.FillRectangle(Brushes.Black, new Rectangle(e.Item.Bounds.Size.Width + 2, e.Item.Bounds.Y, 2, e.Item.Bounds.Size.Height));
            //    }

            //    lstFirm.BeginUpdate();
            //}
        }

        private void lstFilter_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks != 0)
            {
                //lstFilter.SelectedIndices.Clear();
                //lstFilter.Refresh();
            }
        }

        private void lstFilter_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks != 0)
            {
                //lstFilter.SelectedIndices.Clear();
                //lstFilter.Refresh();
            }


        }

        private void lstFilter_MouseUp(object sender, MouseEventArgs e)
        {
            //if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Clicks == 1 && lstFilter.SelectedItems.Count > 0)
            //{
            //    txtSearch.Text = lstFilter.SelectedItems[0].Text;
            //}

            //foreach (var point in points)
            //    System.IO.File.AppendAllText(@"H:\absage.log", "new System.Drawing.Point(" + point.X + "," + point.Y + "),");
        }

        private void lstFirm_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            
        }

        private void View_Paint(object sender, PaintEventArgs e)
        {
            
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        private void lstFirm_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstFilter_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            e.Item.Selected = false;
        }

        private List<System.Drawing.Point> points = new List<Point>();

        private void lstFilter_MouseMove(object sender, MouseEventArgs e)
        {
            //if (e.Button == System.Windows.Forms.MouseButtons.Left)
            //{
            //    var g =
            //        lstFilter.CreateGraphics();

            //    if (g != null)
            //    {
            //        g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            //        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //        g.DrawLine(Pens.Brown, e.Location, new Point(e.Location.X + 2, e.Location.Y + 1));
            //        points.Add(e.Location);
                    
            //    }
            //}
        }

        private void lstFirm_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            return;
        }

        private void lstFilter_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
            //e.Graphics.DrawRectangle(Pens.Lavender, e.Bounds);
            //e.DrawText();
        }

        private void lstFilter_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            //e.DrawDefault = true;
            e.Graphics.FillRectangle(Brushes.Snow, e.Bounds);
            e.Graphics.DrawString(e.Header.Text, e.Font, SystemBrushes.ControlDarkDark, new PointF(3, 7));
        }

        private void lstFirm_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;

            //if (e.Item.Font.Strikeout)
            //    e.Graphics.DrawLine(Pens.White, new Point(0, e.Bounds.Y - 7), new Point(lstFirm.Width, e.Bounds.Y - 7));
        }

        private void lstFirm_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            new UI().ShowDialog();
        }

        private void View_Move(object sender, EventArgs e)
        {
            VacancyView.StartLocation = new Point(Location.X + 23, Location.Y + 80);
        }

        private unsafe void btnTestWrapper_Click(object sender, EventArgs e)
        {
            IXsApp.XSASQLiteConnection testConnection = new IXsApp.XSASQLiteConnection();
            var ptr = System.Runtime.InteropServices.Marshal.StringToBSTR(":memory");
            sbyte* connectionString = (sbyte*)ptr.ToPointer();
            testConnection.Open(connectionString);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblFirm_Click(object sender, EventArgs e)
        {
            SetActiveTabControl(lblFirm.Name);
        }

        private void SetActiveTabControl(string controlName)
        {
            var activateControl
                = true;

            foreach (var control in customTabControls)
            {
                if (control == null)
                    continue;

                if (control.Control.Name.ToUpper() != controlName.ToUpper())
                {
                    control.Active = true;

                    if (!(control.Control.BackColor == Color.SteelBlue) && !(control.Control.BackColor == Color.Brown))
                    {
                        control.Control.Size = new Size(control.Control.Size.Width, 25);
                        control.Control.BackColor = control.Control.Name.ToUpper() == "LBLSEARCH" ? Color.Brown : Color.SteelBlue;
                        control.Control.Font = new Font(control.Control.Font, control.Control.Name.ToUpper() == "LBLSEARCH" ? FontStyle.Regular | FontStyle.Underline : FontStyle.Regular);

                        foreach (var item in control.InheritedControls)
                            item.Visible = false;
                    }
                    else
                        activateControl = true;
                    continue;
                }

                if (activateControl)
                {
                    control.Active = false;
                    control.Control.Size = new Size(control.Control.Size.Width, 27);
                    if (!(control.Control.BackColor == Color.Black))
                        control.Control.Font = new Font(control.Control.Font, FontStyle.Bold);

                    control.Control.BackColor = Color.Black;
                    foreach (var item in control.InheritedControls)
                        item.Visible = true;
                }
            }
        }

        private void lblVacancy_Click(object sender, EventArgs e)
        {
            SetActiveTabControl(lblVacancy.Name);
        }

        private void lblFirm_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void lblVacancy_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void lblVacancy_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void lblFirm_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void lblUnterlagen_Click(object sender, EventArgs e)
        {
            SetActiveTabControl(lblUnterlagen.Name);
        }

        private void lblVerwaltung_Click(object sender, EventArgs e)
        {
            SetActiveTabControl(lblVerwaltung.Name);
        }

        private void lblUnterlagen_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void lblUnterlagen_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void lblVerwaltung_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void lblVerwaltung_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void btnGoogleSearchTest_Click(object sender, EventArgs e)
        {

        }

        private void lblSearch_Click(object sender, EventArgs e)
        {
            SetActiveTabControl(lblSearch.Name);
        }

        private void lblSearch_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void lblSearch_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void btnTest_Click_1(object sender, EventArgs e)
        {

        }

        public sealed class PlainHttpJobSearch 
            : IDisposable
        {
            private SAConnection jobConnection = null;
            private HttpWebRequest searchRequest = null;
            private StreamReader 
                            site = null;
            private Thread siteLoadThread = null;
            public PlainHttpJobSearch()
            {

            }

            #region PlainHttpJobSearch (IDisposable)

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void
                Dispose(bool disposing)
            {
                if (disposing)
                {

                }
            }

            #endregion

            public void OpenRequestAndSearch(string jobSearchSiteLink)
            {
                if (siteLoadThread != null)
                    try
                    {
                        siteLoadThread.Abort();
                    }
                    catch { }

                jobConnection = new SAConnection("uid=dba;pwd=sql;eng=asxs;dbf=asxs");
                jobConnection.Open();

                searchRequest
                    = WebRequest.CreateHttp(requestUriString: jobSearchSiteLink);

                Cursor.Current = Cursors.WaitCursor;

                Thread.Sleep(50);

                var response = OpenRequest();
                site =
                     new StreamReader(response.GetResponseStream());

                if (site == null)
                    throw new InvalidOperationException();

                Cursor.Current = Cursors.Default;

                siteLoadThread = new
                    Thread(new ThreadStart(SearchAsyncInternal));
                siteLoadThread.Start();
                
                Thread.Sleep(150);
            }

            internal WebResponse OpenRequest()
            {
                var result =
                    searchRequest.BeginGetResponse(new AsyncCallback((x) => { }), null);

                while (result.IsCompleted) Application.DoEvents();
                return
                    searchRequest.EndGetResponse(result);
            }

            public struct JobInformationPackage
            {
                public string Name 
                { 
                    get; 
                    set; 
                }

                public string Title 
                { 
                    get; 
                    set; 
                }

                public string Location
                {
                    get;
                    set;
                }

                public string Ref
                {
                    get;
                    set;
                }

                public bool IfItsNotAEmptyPackage()
                {
                    return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Location) && !string.IsNullOrEmpty(Ref);
                }
            }

            public ListView JobSite { get; set; }
            public ToolStripProgressBar JobActionState { get; set; }
            public ToolStripLabel JobActionStateText { get; set; }
            public View Ui { get; set; }
            private string ReplaceHttpStepStoneName(string http)
            {
                return http.Replace("</span>", string.Empty).Replace("<span itemprop=\"name\">", string.Empty).Replace("&amp;", "&").Trim();
            }

            private string ReplaceHttpStepStoneTitle(string http)
            {
                return http.Replace("</span>", string.Empty).Replace("<span itemprop=\"title\">", string.Empty).Replace("<span class='highlighted'>", string.Empty).Replace("&minus", "-").Replace("<br>", "\r\n").Replace("</br>", "\r\n").Trim();
            }

            private string ReplaceHttpStepStoneLocation(string http)
            {
                return http.Replace("</span>", string.Empty).Replace("<span itemprop=\"addressLocality\">", string.Empty).Trim();
            }

            internal void SearchAsyncInternal()
            {
                Application.DoEvents();

                JobSite.Invoke(new Action(() =>
                {
                    JobSite.Items.Clear();

                    var package =
                        new JobInformationPackage() { Name = string.Empty, Title = string.Empty, Location = string.Empty };

                    var id = 1;

                    while (site.Peek() > -1)
                    {
                        try
                        {
                            var content =
                                site.ReadLine();

                            if (content.Contains("<a href=\"/stellenangebote--"))
                            {
                                var start = content.IndexOf('"');
                                start =
                                      content.IndexOf('"', start + 1);
                                if (start != -1)
                                    package.Ref = content.Trim().Substring("<a href=\"".Length).Substring(0, content.Trim().Substring("<a href=\"".Length + 1).IndexOf('"'));
                            }
                            else
                            {
                                if (content.Contains("<span itemprop=\"title\">"))
                                    package.Title = ReplaceHttpStepStoneTitle(content);
                                else if (content.Contains("<span itemprop=\"name\">"))
                                    package.Name = ReplaceHttpStepStoneName(content);
                                else if (content.Contains("<span itemprop=\"addressLocality\">"))
                                    package
                                        .Location = ReplaceHttpStepStoneLocation(content);
                            }
                        }
                        catch { }

                        if (package.IfItsNotAEmptyPackage())
                        {
                            JobSite.Items.Add(id++.ToString());

                            var lastItemIndex = JobSite.Items.Count - 1;
                            try
                            {
                                JobSite.Items[lastItemIndex].SubItems.Add(package.Name);
                                JobActionStateText.Text
                                    = package.Name;

                                JobSite.Items[lastItemIndex].SubItems.Add(package.Title);
                                JobSite.Items[lastItemIndex].SubItems.Add(package.Location);
                                JobSite.Items[lastItemIndex].SubItems.Add(package.Ref);

                                if (Ui.chkKeyword.Checked && !string.IsNullOrEmpty(Ui.txtKeywords.Text))
                                    if (!(package.Title.ToUpper().Contains(Ui.txtKeywords.Text.ToUpper())))
                                    {
                                        JobSite.Items.RemoveAt(lastItemIndex);
                                        //JobSite.Items[lastItemIndex].BackColor = Color.Black;
                                        package = new JobInformationPackage();
                                        continue;
                                    }
                            
                                if (Ui.chkLocation.Checked && !string.IsNullOrEmpty(Ui.txtLocation.Text))
                                    if (!(package.Location.ToUpper().Contains(Ui.txtLocation.Text.ToUpper())))
                                    {
                                        JobSite.Items.RemoveAt(lastItemIndex);
                                        //JobSite.Items[lastItemIndex].BackColor = Color.Black;
                                        package = new JobInformationPackage();
                                        continue;
                                    }

                                if (Ui.chkFirm.Checked && !string.IsNullOrEmpty(Ui.txtFirm.Text))
                                    if (!(package.Name.ToUpper().Contains(Ui.txtFirm.Text.ToUpper())))
                                    {
                                        JobSite.Items.RemoveAt(lastItemIndex);
                                        //JobSite.Items[lastItemIndex].BackColor = Color.Black;
                                        package = new JobInformationPackage();
                                        continue;
                                    }

                                using (var action = jobConnection.CreateCommand())
                                {
                                    var name = package.Name;
                                    if (name.Length > 8)
                                        name = name.Substring(0, 8);

                                    action.CommandText = "select * from asxs_firm where Upper(name) like '%" + name.ToUpper() + "%' order by name desc";
                                    action.Prepare();

                                    using (var reader =
                                        action.ExecuteReader())
                                    {

                                        if (reader.HasRows)
                                        {
                                            JobSite.Items[lastItemIndex].BackColor = Color.Brown;
                                            JobSite.Items[lastItemIndex].ForeColor = Color.White;
                                            JobSite.Items[lastItemIndex].UseItemStyleForSubItems = false;

                                            if (Ui.chkBewerbungen.Checked)
                                                JobSite.Items.RemoveAt(lastItemIndex);
                                        }
                                    }
                                }
                            }
                            catch { }
                            
                            package 
                                = new JobInformationPackage();
                        }
                    }

                    try
                    {
                        site.Close();
                    }
                    finally { site.Dispose(); }

                }));
            }
        }

        private void lnkStellensuche_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var stepStoneJobSearch 
                = new PlainHttpJobSearch();

            stepStoneJobSearch.Ui = this;
            stepStoneJobSearch.JobSite = lstJobSearch;
            stepStoneJobSearch.JobActionState = toolStripProgressBar1;
            stepStoneJobSearch.JobActionStateText = toolStripStatusLabel1;
            stepStoneJobSearch.OpenRequestAndSearch(txtLnk.Text);
        }

        private void txtRegion_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLnk.Text))
            {
                var pos = txtLnk.Text.IndexOf("ws=");
                if (pos != -1)
                {
                    var end = txtLnk.Text.IndexOf('&', pos);
                    if (end != -1)
                    {
                        txtLnk.Text = txtLnk.Text.Remove(pos, end - pos);
                        txtLnk.Text = txtLnk.Text.Insert(pos, "ws=" + txtRegion.Text);
                    }
                }
            }
        }

        private void lstJobSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstJobSearch_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() { Arguments = "http://www.stepstone.de" + lstJobSearch.SelectedItems[0].SubItems[4].Text, UseShellExecute = true, FileName = "chrome.exe" });
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class CustomTabControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTabControl"/> class.
        /// </summary>
        public CustomTabControl()
        {
            InheritedControls = new List<Control>();
            Control = new Label();
            Active = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CustomTabControl"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get; set; }
        /// <summary>
        /// Gets or sets the control.
        /// </summary>
        /// <value>
        /// The control.
        /// </value>
        public Label Control { get; set; }

        /// <summary>
        /// Gets or sets the inherited controls.
        /// </summary>
        /// <value>
        /// The inherited controls.
        /// </value>
        public List<Control> InheritedControls { get; set; }
    }
}
