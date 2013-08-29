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

namespace IxSApp
{
    using Data;

    public partial class VacancyView : Form
    {
        private SAConnection connection = null;

        public VacancyView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the package.
        /// </summary>
        /// <value>
        /// The package.
        /// </value>
        public Units Package { get; set; }
        /// <summary>
        /// Gets or sets the type of editing.
        /// </summary>
        /// <value>
        /// The type of editing.
        /// </value>
        public StatementType TypeOfEditing { get; set; }
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public long Id { get; set; }
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
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
            chkUserReply.Checked = Package.Mandant.ReplyRequired;
            chkPositiveReply.Checked = Package.Bewerbung.Zusage;
            radioAbsageUser.Checked = Package.Bewerbung.NegativeStateAtOwn;
            chkFirmReply.Checked = Package.Bewerbung.AwaitingFirmReply;

            if (Package.Bewerbung.FirmAnswer.HasValue)
            {
                radioDeath.Checked = !Package.Bewerbung.FirmAnswer.Value;
                radioWork.Checked = Package.Bewerbung.FirmAnswer.Value;
            }

            lstAnlagen.Items.Clear();
            try
            {
                using (connection = new SAConnection(ConnectionStringManager.ConnectionStringNetworkServer))
                {
                    connection
                        .Open();

                    if (connection.State == System.Data.ConnectionState.Open)
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

                            if (!anlage.IsClosed)
                                anlage.Close();

                            action.CommandText = "SELECT * FROM ASXS_MANDANT WHERE ID = " + Package.Firm.Id_Mandant;
                            action.Prepare();

                            var haveToInsertOneMandant = true;
                            var reader 
                                = default(SADataReader);
                            if ((reader = action.ExecuteReader()).HasRows)
                            {
                                haveToInsertOneMandant = false;
                            }

                            if (!reader.IsClosed)
                                reader.Close();

                            if (haveToInsertOneMandant)
                            {
                                action.CommandText = "INSERT INTO ASXS_MANDANT VALUES (" + (Package.Mandant.Id = TableIdCommand.CreateInstance(action).Get("NEW", "ASXS_MANDANT")) + ", 0)";
                                action.Prepare();
                                action.ExecuteNonQuery();
                            }
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
                var bewerbung = new WorkUnitBewerbung()
                {
                    Day = dateTimeDay.Value,
                    NegativeStateAtOwn = radioAbsageUser.Checked,
                    Reply = chkReply.Checked,
                    Sent = chkBewerbung.Checked,
                    State = radioAbsage.Checked,
                    Zusage = chkPositiveReply.Checked,
                    AwaitingFirmReply = chkFirmReply.Checked,
                    FirmAnswer = radioWork.Checked ? true : radioDeath.Checked ? false : new Nullable<bool>()
                };

                save.Edit
                (
                    new Units()
                    {
                        Bewerbung = bewerbung
                    },

                    new BewerbungDataUnit(),
                    Id = this.Id
                );

                bewerbung.Id = Id;

                var address = new WorkUnitAddress()
                {
                    City = "Musterstadt"
                };

                save.Edit
                (
                    new Units()
                    {
                        Address = address
                    },

                    new AddressDataUnit(),
                    Id = this.Id
                );

                address.Id = Id;

                var memo = new WorkUnitMemo()
                {
                    Content = txtMemo.Text
                };

                save.Edit
                (
                    new Units()
                    {
                        Memo = new WorkUnitMemo() { Content = txtMemo.Text, Id = Package.Memo.Id }
                    },

                    new MemoDataUnit(),
                    Id = this.Id
                );

                memo.Id = Package.Memo.Id;

                var mandant = new WorkUnitMandant()
                {
                    ReplyRequired = chkUserReply.Checked
                };

                save.Edit
                (
                    new Units()
                    {
                        Mandant = mandant
                    },

                    new MandantDataUnit(),
                    Id = this.Id
                );

                mandant.Id = Package.Mandant.Id;

                var firm = new WorkUnitFirm()
                {
                    Id = this.Id,
                    Id_Bew = bewerbung.Id,
                    Id_Addr = address.Id,
                    Id_Memo = Package.Memo.Id,
                    Id_Mandant = mandant.Id,
                    Name = txtOrganisation.Text,
                    Website = txtAnzeige.Text,
                    ReplyRequired = chkUserReply.Checked
                };

                save.Edit
                (
                    new Units()
                    {
                        Memo = memo,
                        Address = address,
                        Bewerbung = bewerbung,
                        Firm = firm,
                        Mandant = mandant
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
                var bewerbung = new WorkUnitBewerbung()
                {
                    Day = dateTimeDay.Value,
                    NegativeStateAtOwn = radioAbsageUser.Checked,
                    Reply = chkReply.Checked,
                    Sent = chkBewerbung.Checked,
                    State = radioAbsage.Checked
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
                    City = "Musterstadt"
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
                    Content = txtMemo.Text
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

                var mandant = new WorkUnitMandant()
                {
                    ReplyRequired = chkUserReply.Checked
                };

                id = add.Append
                (
                    new Units()
                    {
                        Mandant = mandant
                    },

                    new MandantDataUnit()
                );

                mandant.Id = id;

                var firm = new WorkUnitFirm()
                {
                    Id_Bew = bewerbung.Id,
                    Id_Addr = address.Id,
                    Id_Memo = memo.Id,
                    Id_Mandant = mandant.Id,
                    Name = txtOrganisation.Text,
                    Website = txtAnzeige.Text
                };

                id = add.Append
                (
                    new Units()
                    {
                        Memo = memo,
                        Address = address,
                        Bewerbung = bewerbung,
                        Firm = firm,
                        Mandant = mandant
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
                    = new WorkUnitMemo()
                    {
                        Content = txtMemo.Text
                    };

                var id = add.Append
                (
                    new Units()
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
                    new Units() 
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
                            new SAConnection(ConnectionStringManager.ConnectionStringNetworkServer);

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

                                action.CommandText = string.Format("INSERT INTO ASXS_ANLAGE VALUES ({0}, {1}, '{2}', '{3}', '{4}')", id, Package.Firm.Id, txtAnlage.Text, "Anlage", Path.GetFileNameWithoutExtension(txtAnlage.Text));
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

                        edit.Edit(new Units()
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

        private void toolAdd_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void toolAdd_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolAdd_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void ViewUI_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
