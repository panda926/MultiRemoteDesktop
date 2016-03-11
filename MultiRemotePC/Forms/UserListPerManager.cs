using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Configuration;
using GG2014;
using ESBasic.Security;

namespace MultiRemotePC
{
    public partial class UserListPerManager : Form
    {
        private IRemotingService ggService;
        public User currentUser;
        public List<string> listFriends;
        public string strCatalog;

        public UserListPerManager()
        {
            InitializeComponent();

            int registerPort = int.Parse(ConfigurationManager.AppSettings["RemotingPort"]);
            this.ggService = (IRemotingService)Activator.GetObject(typeof(IRemotingService), string.Format("tcp://{0}:{1}/RemotingService", ConfigurationManager.AppSettings["ServerIP"], registerPort));
        }

        // 추가버튼 클릭사건.
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("추가할 유저가 없습니다.");
                this.DialogResult = DialogResult.None;
                return;
            }

            List<string> listUserID = new List<string>();
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value == null || dataGridView1.Rows[i].Cells[0].Value.ToString() == "false")
                    continue;

                if (this.dataGridView1.Rows[i].Cells[0].Value.ToString() == "True")
                {
                    listUserID.Add(this.dataGridView1.Rows[i].Cells[1].Value.ToString());
                }
            }

            if (listUserID.Count > 0)
            {
                this.listFriends = listUserID;
                this.strCatalog = this.comboBox1.SelectedItem.ToString();
                this.ggService.AddFriends(currentUser.UserID, listUserID, this.comboBox1.SelectedItem.ToString());
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("선택된 유저가 없습니다.");
            }
        }

        private void UserList_Load(object sender, EventArgs e)
        {
            this.comboBox1.DataSource = currentUser.GetFriendCatalogList();
            this.comboBox1.SelectedIndex = 0;

            //List<User> listUser = this.ggService.GetAllUser();
            //foreach (User user in listUser)
            //{
            //    this.dataGridView1.Rows.Add();
            //    this.dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = user.UserID;
            //    this.dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value = user.Name;
            //}
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex == -1)
            {
                e.PaintBackground(e.ClipBounds, false);

                Point pt = e.CellBounds.Location;  // where you want the bitmap in the cell

                int nChkBoxWidth = 14;
                int nChkBoxHeight = 14;
                int offsetx = (e.CellBounds.Width - nChkBoxWidth) / 2 + 1;
                int offsety = (e.CellBounds.Height - nChkBoxHeight) / 2 + 1;

                pt.X += offsetx;
                pt.Y += offsety;

                CheckBox cb = new CheckBox();
                cb.Size = new Size(nChkBoxWidth, nChkBoxHeight);
                cb.Location = pt;
                cb.CheckedChanged += new EventHandler(gvSheetListCheckBox_CheckedChanged);

                ((DataGridView)sender).Controls.Add(cb);

                e.Handled = true;
            }
        }

        private void gvSheetListCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                r.Cells["colCheckBox"].Value = ((CheckBox)sender).Checked;
            }
        }

        // 유저검색버튼 클릭사건.
        private void button1_Click(object sender, EventArgs e)
        {
            string strMangerID = this.textBox1.Text;
            if (strMangerID.Trim() == string.Empty)
            {
                MessageBox.Show("관리자아이디가 비어있습니다.");
                return;
            }

            string strPassword = this.textBox2.Text;
            if (strPassword == string.Empty)
            {
                MessageBox.Show("패스워드를 입력하세요.");
                return;
            }

            List<User> listUser = this.ggService.GetAllUserPerManager(strMangerID, ESBasic.Security.SecurityHelper.MD5String(strPassword));
            foreach (User user in listUser)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = user.UserID;
                this.dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value = user.Name;
            }
        }
    }
}
