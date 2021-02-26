using DBHelper;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMTAdeptControlSystem
{
    public partial class TaskView : Form
    {
        public TaskView()
        {
            InitializeComponent();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            List<RobotTask> allTasks = OracleHelper.GetAllTasks(MainForm.areaName);
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = allTasks;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string text = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                DialogResult dialogResult = MessageBox.Show("您确定要删除当前任务吗，任务id=" + text + "？", "提示", MessageBoxButtons.OKCancel);
                bool flag = dialogResult == DialogResult.OK;
                if (flag)
                {
                    OracleHelper.DeleteTask(text);
                    List<RobotTask> allTasks = OracleHelper.GetAllTasks(MainForm.areaName);
                    this.dataGridView1.DataSource = null;
                    this.dataGridView1.DataSource = allTasks;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请选择一行!");
            }
        }

        private void TaskView_Load(object sender, EventArgs e)
        {
            List<RobotTask> allTasks = OracleHelper.GetAllTasks(MainForm.areaName);
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = allTasks;
        }
    }
}
