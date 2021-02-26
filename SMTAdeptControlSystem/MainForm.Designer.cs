
namespace SMTAdeptControlSystem
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.lbInfo = new System.Windows.Forms.Label();
            this.lbStep = new System.Windows.Forms.Label();
            this.btnRobotStatus = new System.Windows.Forms.Button();
            this.BtnReset = new System.Windows.Forms.Button();
            this.BtnQueryTask = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.richTextBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lbInfo);
            this.splitContainer1.Panel2.Controls.Add(this.lbStep);
            this.splitContainer1.Panel2.Controls.Add(this.btnRobotStatus);
            this.splitContainer1.Panel2.Controls.Add(this.BtnReset);
            this.splitContainer1.Panel2.Controls.Add(this.BtnQueryTask);
            this.splitContainer1.Size = new System.Drawing.Size(1178, 744);
            this.splitContainer1.SplitterDistance = 884;
            this.splitContainer1.TabIndex = 0;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.InfoText;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(884, 744);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbInfo.Location = new System.Drawing.Point(59, 417);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(76, 21);
            this.lbInfo.TabIndex = 1;
            this.lbInfo.Text = "label1";
            // 
            // lbStep
            // 
            this.lbStep.AutoSize = true;
            this.lbStep.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbStep.Location = new System.Drawing.Point(94, 352);
            this.lbStep.Name = "lbStep";
            this.lbStep.Size = new System.Drawing.Size(84, 18);
            this.lbStep.TabIndex = 1;
            this.lbStep.Text = "当前步骤";
            // 
            // btnRobotStatus
            // 
            this.btnRobotStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRobotStatus.Location = new System.Drawing.Point(52, 247);
            this.btnRobotStatus.Name = "btnRobotStatus";
            this.btnRobotStatus.Size = new System.Drawing.Size(181, 54);
            this.btnRobotStatus.TabIndex = 0;
            this.btnRobotStatus.Text = "查看机器人信息";
            this.btnRobotStatus.UseVisualStyleBackColor = true;
            this.btnRobotStatus.Click += new System.EventHandler(this.btnRobotStatus_Click);
            // 
            // BtnReset
            // 
            this.BtnReset.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnReset.Location = new System.Drawing.Point(52, 146);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.Size = new System.Drawing.Size(181, 54);
            this.BtnReset.TabIndex = 0;
            this.BtnReset.Text = "复位状态";
            this.BtnReset.UseVisualStyleBackColor = true;
            this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // BtnQueryTask
            // 
            this.BtnQueryTask.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnQueryTask.Location = new System.Drawing.Point(52, 43);
            this.BtnQueryTask.Name = "BtnQueryTask";
            this.BtnQueryTask.Size = new System.Drawing.Size(181, 54);
            this.BtnQueryTask.TabIndex = 0;
            this.BtnQueryTask.Text = "查询任务";
            this.BtnQueryTask.UseVisualStyleBackColor = true;
            this.BtnQueryTask.Click += new System.EventHandler(this.BtnQueryTask_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1178, 744);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "SMT任务调度软件";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.Label lbStep;
        private System.Windows.Forms.Button btnRobotStatus;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.Button BtnQueryTask;
    }
}

