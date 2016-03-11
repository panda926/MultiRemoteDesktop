using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

using GG2014;
using ESBasic.Security;
using System.Configuration;

namespace MultiRemotePC
{
    public partial class ExcelReg : Form
    {
        private bool bCheckError = false;
        public User currentUser = null;
        private IRemotingService ggService;

        public ExcelReg()
        {
            InitializeComponent();

            int registerPort = int.Parse(ConfigurationManager.AppSettings["RemotingPort"]);
            this.ggService = (IRemotingService)Activator.GetObject(typeof(IRemotingService), string.Format("tcp://{0}:{1}/RemotingService", ConfigurationManager.AppSettings["ServerIP"], registerPort));
        }

        private void ExcelReg_Load(object sender, EventArgs e)
        {
            this.dataGridView1.Columns[0].DataPropertyName = "check";
            this.dataGridView1.Columns[1].DataPropertyName = "id";
            this.dataGridView1.Columns[2].DataPropertyName = "password";
            this.dataGridView1.Columns[3].DataPropertyName = "name";
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                bCheckError = false;

                OpenFileDialog openfile1 = new OpenFileDialog();
                openfile1.Title = "엑셀파일선택";
                openfile1.Filter = "Excel Sheet(*.xls)|*.xls";
                openfile1.FilterIndex = 0;

                if (openfile1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.textBox1.Text = openfile1.FileName;

                    string pathconn = "Provider = Microsoft.jet.OLEDB.4.0; Data source=" + textBox1.Text + ";Extended Properties=\"Excel 8.0;HDR= yes;\";";
                    OleDbConnection conn = new OleDbConnection(pathconn);
                    OleDbDataAdapter MyDataAdapter = new OleDbDataAdapter("Select * from [" + "Sheet1" + "$]", conn);
                    DataTable dt = new DataTable();
                    MyDataAdapter.Fill(dt);

                    //dt.Columns.Add("check");
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    dr["check"] = "false";
                    //}

                    this.dataGridView1.DataSource = dt; 
                }
            }
            catch (Exception ex) { string strError = ex.ToString(); }

        }

        // 등록버튼 클릭사건.
        private void button2_Click(object sender, EventArgs e)
        {
            if (!bCheckError)
            {
                MessageBox.Show("오류검사를 해주세요.");
                return;
            }

            //DataTable dt = this.dataGridView1.DataSource as DataTable;
            //if (dt == null || dt.Rows.Count == 0)
            //    return;

            if (this.dataGridView1.Rows.Count == 0)
                return;

            string strUserID = string.Empty;
            string strUserPass = string.Empty;
            string strUserName = string.Empty;
            string strCreator = string.Empty;

            if (this.currentUser != null)
                strCreator = this.currentUser.UserID;

            List<User> listUser = new List<User>();
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value == null || dataGridView1.Rows[i].Cells[0].Value.ToString() == "false")
                    continue;

                if (this.dataGridView1.Rows[i].Cells[0].Value.ToString() == "True")
                {
                    strUserID = this.dataGridView1.Rows[i].Cells[2].Value.ToString();
                    strUserPass = this.dataGridView1.Rows[i].Cells[3].Value.ToString();
                    strUserName = this.dataGridView1.Rows[i].Cells[4].Value.ToString();

                    User user = new User(strUserID, SecurityHelper.MD5String(strUserPass), strUserName, string.Empty, string.Empty, 0, "", strCreator);
                    listUser.Add(user);
                }
            }

            if (listUser.Count > 0)
            {
                try
                {
                    RegisterResult result = ggService.Register(listUser);

                    if (result == RegisterResult.Error)
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.None;
                        MessageBox.Show("등록과정에 오류가 발생하였습니다.");
                        return;
                    }

                    this.DialogResult = DialogResult.OK;
                }
                catch(Exception ee)
                { MessageBox.Show("등록오류: " + ee.ToString()); }
            }
        }

        // 오류검사버튼 클릭사건.
        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dt = this.dataGridView1.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
                return;

            string strUserID = string.Empty;
            string strUserPass = string.Empty;
            string strUserName = string.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                strUserID = dr["id"].ToString();
                strUserPass = SecurityHelper.MD5String(dr["password"].ToString());
                strUserName = dr["name"].ToString();

                if (strUserID.Trim() == string.Empty || strUserPass.Trim() == string.Empty)
                {
                    MessageBox.Show("아이디나 패스워드가 비어있는 유저들이 있습니다. 체크해주세요.");
                    return;
                }
            }

            bCheckError = true;
            MessageBox.Show("오류가 없습니다.");
        }
    }
}
