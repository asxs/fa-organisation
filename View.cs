using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace As
{
    public partial class ViewUI : Form
    {
        public ViewUI()
        {
            InitializeComponent();
        }

        public DataUnitPackage Package { get; set; }
        public StatementType TypeOfEditing { get; set; }

        public long Id { get; set; }

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

            if (TypeOfEditing != StatementType.Insert)
                btnAdd.Enabled = false;
            else
                btnSaveNoExit.Enabled = false;
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
