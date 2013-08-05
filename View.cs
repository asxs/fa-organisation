using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;

#region iAnywhere.Data.SQLAnywhere.v3.5

using iAnywhere;
using iAnywhere.Data;
using iAnywhere.Data.SQLAnywhere;

#endregion

namespace As
{
    public partial class ViewUI : Form
    {
        private SAConnection connection = null;

        public ViewUI()
        {
            InitializeComponent();
        }

        public DataUnitPackage Package { get; set; }
        public StatementType TypeOfEditing { get; set; }
        public long Id { get; set; }
        public SAConnection Connection { get { return connection; } }

        private void ViewUI_Load(object sender, EventArgs e)
        {
            txtOrganisation.Text = Package.Firm.Name;
            txtAnzeige.Text = Package.Firm.Website;
            txtMemo.Text = Package.Memo.Content;
            radioAbsage.Checked = Package.Bewerbung.State;
            dateTimeDay.Value = Package.Bewerbung.Day.Value;
            chkBewerbung.Checked = Package.Bewerbung.Sent;
            chkReply.Checked = Package.Bewerbung.Reply;
            txtMemo.Enabled = (Package.Firm.Id_Memo != 0);
            toolAdd.Enabled = (Package.Firm.Id_Memo == 0);

            lstAnlagen.Items.Clear();
            try
            {
                using (connection = new SAConnection(ConnectionStringManager.TinyOrganisationCrmAnyConnectionString))
                {
                    connection
                        .Open();

                    if (connection.State == System.Data.ConnectionState.Open)
                        using (var action = connection.CreateCommand())
                        {
                            action.CommandText = "SELECT * FROM ASXS_ANLAGE WHERE ID_FIRM = " + Package.Firm.Id;
                            action.Prepare();
                            var anlage 
                                = action.ExecuteReader();

                            while (anlage.Read())
                            {
                                lstAnlagen.Items.Add(anlage["name"].ToString());
                                lstAnlagen.Items[lstAnlagen.Items.Count - 1].SubItems.Add(anlage["descr"].ToString());
                            }
                        }
                }
            }
            catch { }
            finally
            {
                connection.Close();
            }

            if (TypeOfEditing != StatementType.Insert)
                btnAdd.Enabled = false;
            else
                btnSaveNoExit.Enabled = false;

            if (!radioAbsage.Checked && !radioAbsageUser.Checked)
                chkAbsageBack.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
                
            }
            finally
            {
                Dispose();
            }
        }

        private void btnSaveNoExit_Click(object sender, EventArgs e)
        {
            var id =
                0L;

            using (var save = new FaOrganisationEdit())
            {
                var bewerbung = new AsBewerbung()
                {
                    Day = dateTimeDay.Value,
                    NegativeStateAtOwn = radioAbsageUser.Checked,
                    Reply = chkReply.Checked,
                    Sent = chkBewerbung.Checked,
                    State = radioAbsage.Checked
                };

                save.Edit
                (
                    new DataUnitPackage()
                    {
                        Bewerbung = bewerbung
                    },

                    new BewerbungDataUnit(),
                    Id = this.Id
                );

                bewerbung.Id = Id;

                var address = new AsAddress()
                {
                    City = "Musterstadt"
                };

                save.Edit
                (
                    new DataUnitPackage()
                    {
                        Address = address
                    },

                    new AddressDataUnit(),
                    Id = this.Id
                );

                address.Id = Id;

                var memo = new AsMemoPackage()
                {
                    Content = txtMemo.Text
                };

                save.Edit
                (
                    new DataUnitPackage()
                    {
                        Memo = new AsMemoPackage() { Content = txtMemo.Text, Id = Package.Memo.Id }
                    },

                    new MemoDataUnit(),
                    Id = this.Id
                );

                memo.Id = Package.Memo.Id;

                var firm = new AsFirm()
                {
                    Id = this.Id,
                    Id_Bew = bewerbung.Id,
                    Id_Addr = address.Id,
                    Id_Memo = Package.Memo.Id,
                    Name = txtOrganisation.Text,
                    Website = txtAnzeige.Text
                };

                save.Edit
                (
                    new DataUnitPackage()
                    {
                        Memo = memo,
                        Address = address,
                        Bewerbung = bewerbung,
                        Firm = firm
                    },

                    new FirmDataUnit(),
                    Id = this.Id
                );

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var id =
                0L;

            using (var add = new FaOrganisationAppend())
            {
                var bewerbung = new AsBewerbung()
                {
                    Day = dateTimeDay.Value,
                    NegativeStateAtOwn = radioAbsageUser.Checked,
                    Reply = chkReply.Checked,
                    Sent = chkBewerbung.Checked,
                    State = radioAbsage.Checked
                };

                id = add.Append
                (
                    new DataUnitPackage()
                    {
                        Bewerbung = bewerbung
                    },

                    new BewerbungDataUnit()
                );

                bewerbung.Id = id;

                var address = new AsAddress()
                {
                    City = "Musterstadt"
                };

                id = add.Append
                (
                    new DataUnitPackage()
                    {
                        Address = address
                    },

                    new AddressDataUnit()
                );

                address.Id = id;

                var memo = new AsMemoPackage()
                {
                    Content = txtMemo.Text
                };

                id = add.Append
                (
                    new DataUnitPackage()
                    {
                        Memo = memo
                    },

                    new MemoDataUnit()
                );

                memo.Id = id;

                var firm = new AsFirm()
                {
                    Id_Bew = bewerbung.Id,
                    Id_Addr = address.Id,
                    Id_Memo = memo.Id,
                    Name = txtOrganisation.Text,
                    Website = txtAnzeige.Text
                };

                id = add.Append
                (
                    new DataUnitPackage()
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch { }
            finally
            {
                Dispose();
            }
        }

        private void toolAdd_Click(object sender, EventArgs e)
        {
            using (var add = new FaOrganisationAppend())
            {
                var content
                    = new AsMemoPackage()
                    {
                        Content = txtMemo.Text
                    };

                var id = add.Append
                (
                    new DataUnitPackage()
                    {
                        Memo = content
                    },

                    new MemoDataUnit()
                );

                var edit =
                    new FaOrganisationEdit();

                Package.Memo.Id = id;
                Package.Firm.Id_Memo = id;

                edit.Edit
                (
                    new DataUnitPackage() 
                    {
                        Address = Package.Address,
                        Bewerbung = Package.Bewerbung,
                        Firm = Package.Firm,
                        Memo = Package.Memo
                    },

                    new FirmDataUnit(), 
                    Id
                );
            }

            toolAdd.Enabled = false;
            txtMemo.Enabled = true;
        }

        private void toolNext_Click(object sender, EventArgs e)
        {

        }

        private void toolBack_Click(object sender, EventArgs e)
        {

        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            using (var stream = new FileStream(txtAnlage.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                try
                {
                    var bytes =
                        new byte[stream.Length];

                    stream.Read(bytes, 0, bytes.Length);
                    try
                    {
                        stream.Close();
                    }
                    finally { stream.Dispose(); }

                    if (bytes.Length == 0)
                        throw new InvalidOperationException("the document should have a value");

                    if (connection == null || connection.State != System.Data.ConnectionState.Open)
                    {
                        connection =
                            new SAConnection(ConnectionStringManager.TinyOrganisationCrmAnyConnectionString);

                        connection.Open();
                    }

                    try
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            var action = connection.CreateCommand();
                            if (action != null)
                            {
                                var id
                                    = TableIdCommand.CreateInstance(action).Get("NEW", "ASXS_ANLAGE");

                                action.CommandText = string.Format("INSERT INTO ASXS_ANLAGE VALUES ({0}, {1}, csconvert('{2}', 'os_charset'), '{3}', '{4}')", id, Package.Firm.Id, txtAnlage.Text, "Anlage", Path.GetFileNameWithoutExtension(txtAnlage.Text));
                                action.Prepare();
                                action.ExecuteNonQuery();
                            }
                        }
                    }
                    catch { }
                }
                catch { }
            }

            lstAnlagen.Items.Clear();
            try
            {
                using (var action = connection.CreateCommand())
                {
                    action.CommandText = "SELECT * FROM ASXS_ANLAGE WHERE ID_FIRM = " + Package.Firm.Id;
                    action.Prepare();
                    var anlage
                        = action.ExecuteReader();

                    while (anlage.Read())
                    {
                        lstAnlagen.Items.Add(anlage["name"].ToString());
                        lstAnlagen.Items[lstAnlagen.Items.Count - 1].SubItems.Add(anlage["descr"].ToString());
                    }
                }
            }
            catch { }
            finally
            {

            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var documentDialog = new OpenFileDialog())
            {
                documentDialog.AddExtension = true;
                documentDialog.AutoUpgradeEnabled = true;
                documentDialog.CheckFileExists = true;
                documentDialog.CheckPathExists = true;
                documentDialog.Filter = "Alle Dateien (*.*)|*.*";

                if (documentDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtAnlage.Text = documentDialog.FileName;
                }
            }
        }

        private void radioAbsage_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioAbsageUser_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkAbsageBack_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAbsageBack.Checked)
            {
                try
                {
                    using (var edit = new FaOrganisationEdit())
                    {
                        var bewerbung =
                            Package.Bewerbung;

                        bewerbung.State = false;

                        edit.Edit(new DataUnitPackage()
                        {
                            Bewerbung = bewerbung
                        },

                        new BewerbungDataUnit(),
                        Package.Firm.Id);
                    }
                }
                catch { }

                chkAbsageBack.Enabled = false;
                radioAbsage.Checked = false;
                radioAbsageUser.Checked = false;
            }
        }
    }

    public static class ASxFactory
    {
        public static IFaOrganisation CreateFaOrganisation(ViewEditState state)
        {
            IFaOrganisation unit = null;

            switch (state)
            {
                case ViewEditState.Add:
                    unit = new FaOrganisationAppend();
                    break;
                case ViewEditState.Edit:
                    break;
                case ViewEditState.Remove:
                    break;
                case ViewEditState.Update:
                    break;
            }

            return unit;
        }
    }

    public enum ViewEditState : int
    {
        Add = 0,
        Edit = 1,
        Remove = 2,
        Update = 3,
        None
    }
}
