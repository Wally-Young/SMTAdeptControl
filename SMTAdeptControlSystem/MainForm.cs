using AsyncTcp;
using DBHelper;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using tools;

namespace SMTAdeptControlSystem
{
    public partial class MainForm : Form
    {
        #region  Field
        private string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

		private static readonly object tasklock = new object();

		private Dictionary<string, string> cfgDictIps = new Dictionary<string, string>();

		private Dictionary<string, int> cfgDictPorts = new Dictionary<string, int>();

		private List<RobotTask> taskList = new List<RobotTask>();

		private static AdapterRobot AdapterRobot;

		public string oracleDB_str;

		private static RobotTask currentTask;

		private static RobotTask nextTask;

		public static string areaName;
	
		private TaskServer taskServer = new TaskServer();

		private static int waitSeconds = 0;

		private static int flagio = 0;

		private bool iscomplete;

		private Thread workthread;

		private AsyncTcpServer server = null;

		private Thread updateThread;
		private Task checkTask = null;

		private static readonly object tasklockobj = new object();
        #endregion 
        
		public MainForm()
        {
            InitializeComponent();
			Control.CheckForIllegalCrossThreadCalls = false;
		}

     

        #region TcpServer 后来放弃未使用
        private void Server_PlaintextReceived(object sender, TcpDatagramReceivedEventArgs<byte[]> e)
		{
			try
			{
				string text = Encoding.ASCII.GetString(e.Datagram);
				text = text.Substring(0, 2);
				Logger.Append(this.richTextBox1, $"client [ { e.TcpClient.Client.RemoteEndPoint.ToString() }] received is :{ text}");
				bool flag = e.TcpClient.Client.RemoteEndPoint.ToString().Contains(this.cfgDictIps["RobotIp"]);
				if (flag)
				{
					bool flag2 = e.Datagram.Equals("1E");
					if (flag2)
					{
						currentTask = null;
						Logger.Append(this.richTextBox1, $"robot-status:{AdapterRobot.GetRobotStatus("StatusForHumans")} " +
							$"is going to {currentTask.TARGET_1}E");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Append(this.richTextBox1, "socket  [" + e.TcpClient.Client.RemoteEndPoint.ToString() + "] error is :" + ex.Message);
			}
		}

		private void Server_ClientDisconnected(object sender, TcpClientDisconnectedEventArgs e)
		{
			Logger.Append(this.richTextBox1, "client [" + e.TcpClient.Client.RemoteEndPoint.ToString() + "] disconnected");
		}

		private void Server_ClientConnected(object sender, TcpClientConnectedEventArgs e)
		{
			Logger.Append(this.richTextBox1, "client [" + e.TcpClient.Client.RemoteEndPoint.ToString() + "] connected");
		}
		#endregion


		private void mainWork()
		{
			while (true)
			{
				this.UpdateTaskList(ref this.taskList);
				
				///机器人连接成功
				bool flag = !AdapterRobot.GetConnectStatus() || AdapterRobot.GetRobotStatus("StatusForHumans").Contains("Unknown");
				if (flag)
				{
					
					AdapterRobot.Close();
					Thread.Sleep(3000);
					AdapterRobot = new AdapterRobot(this.cfgDictIps["RobotIp"], this.cfgDictPorts["RobotPort"], 1);
					AdapterRobot.Name = "AdeptRobot";
				    AdapterRobot.Isconnected = false;
					AdapterRobot.Connect();
					Logger.Append(this.richTextBox1, "attempting to reconnect to robot...");
				}
				else
				{
					bool flag2 = this.taskList.Count > 0 && currentTask == null;
					if (flag2)
					{
						waitSeconds = 0;
						this.iscomplete = false;
						currentTask = this.taskList.Last<RobotTask>();
						AdapterRobot.Write($"qm 2 2 {currentTask.TARGET_1} " +
							$"pickup default {currentTask.TARGET_2} dropoff default " +
							$"{currentTask.REQUEST_ID}");
						Logger.Append(this.richTextBox1, string.Concat(new string[]
						{
							"the task ",
							currentTask.TARGET_1,
							"-",
							currentTask.TARGET_2,
							"-","E is send to robotTask,waiting to excute"
						}));
						currentTask.TASK_STATUS = "Process";
						currentTask.PATH_STAGE = "0-1";
						currentTask.ASSIGNED_ROBOT_NAME = AdapterRobot.Name;
						OracleHelper.UpdateFirst(currentTask);
					}
					else
					{
						bool flag3 = this.taskList.Count == 0 &&currentTask == null;
						if (flag3)
						{
							waitSeconds++;
							bool flag4 = waitSeconds >= 90 && !this.iscomplete;
							if (flag4)
							{
								this.iscomplete = true;
								waitSeconds = 0;
								AdapterRobot.Goto(this.taskServer.lastgoal);
							}
						}
						else
						{
							bool flag5 = currentTask != null;
							if (flag5)
							{
								this.DealWithTask(AdapterRobot,currentTask, this.richTextBox1);
							}
						}
					}
				}
				lbStep.Text = "当前步骤为:" + TaskServer.flag;
				this.lbInfo.Text = string.Format("机器人信息:\n\nIsConnected:{0}\n+{1}", AdapterRobot.GetConnectStatus(), AdapterRobot.GetRobotStatus());
				Thread.Sleep(2000);
			}
		}




        private void AssignTask()
        {
            bool flag = false;
            bool flag2 = AdapterRobot.GetConnectStatus() && !AdapterRobot.GetRobotStatus("StatusForHumans").Contains("Unknown");
            if (flag2)
            {
                string text = AdapterRobot.GetRobotStatus("StatusForHumans").Trim();
                flag = (AdapterRobot.GetRobotStatus("StatusForHumans").Trim().StartsWith("Docked") || AdapterRobot.GetRobotStatus("StatusForHumans").Trim().StartsWith("Arrived at"));
            }
            else
            {
                AdapterRobot.Close();
                AdapterRobot = new AdapterRobot(this.cfgDictIps["RobotIp"], this.cfgDictPorts["RobotPort"], 1);
                AdapterRobot.Name = "AdeptRobot";
                AdapterRobot.Isconnected = false;
                AdapterRobot.Connect();
                Logger.Append(this.richTextBox1, "attempting to reconnect to robot...");
            }
            bool flag3 = currentTask == null & flag;
            if (flag3)
            {
                bool flag4 = this.taskList.Count > 0 && currentTask == null;
                if (flag4)
                {
                    object obj = tasklockobj;
                    lock (obj)
                    {
                        waitSeconds = 0;
                        currentTask = this.taskList[this.taskList.Count - 1];
                        bool flag6 = false;
                        while (!flag6)
                        {
                            flag6 = AdapterRobot.Write(string.Concat(new string[]
                            {
                                "qm 2 2 ",
                                 currentTask.TARGET_1,
                                " pickup default ",
                                 currentTask.TARGET_2,
                                " dropoff default "
                            }));
                        }
                    }
                }
            }
        }

        private void UpdateTaskList(ref List<RobotTask> taskList)
		{
			try
			{
				taskList = OracleHelper.GetRobotTasks( areaName);
			}
			catch (Exception ex)
			{
				Logger.Append(this.richTextBox1, "Failed to acquire Tasks from DB the reason is:" + ex.Message);
			}
		}

	
		public void DealWithTask(AdapterRobot adapterRobot, RobotTask robotTask, RichTextBox richTextBox)
		{
			string name = adapterRobot.Name;
			robotTask.ASSIGNED_ROBOT_NAME = name;
			Logger.Append(richTextBox, "robot is delivering.........");
			bool flag = adapterRobot.CheckedRobotStatus(robotTask.TARGET_1);
			if (flag)
			{
				robotTask.TASK_STATUS = "Process";
				robotTask.PATH_STAGE = "1-2";
				OracleHelper.UpdateState(robotTask);
				Logger.Append(richTextBox, "robot-status:" + adapterRobot.GetRobotStatus("StatusForHumans") + " Arrived at  " + robotTask.TARGET_1);
			}
			else 
			{
				bool flag2 = adapterRobot.CheckedRobotStatus(robotTask.TARGET_2);
				if (flag2)
				{
					Thread.Sleep(2000);
					robotTask.PATH_STAGE = "final";
					robotTask.TASK_STATUS = "Complete";
					OracleHelper.UpdateState(robotTask);
					Logger.Append(this.richTextBox1, $"robot arrived at {robotTask.TARGET_2}");
					
				}
				else
                {
					bool flag3 = adapterRobot.CheckIO("o4");
					if(flag3)
                    {
						currentTask = null;
						Logger.Append(this.richTextBox1, $"robot current task completed!");
					}
				}
			}
		}

		#region 窗体事件

		/// <summary>
		/// 窗体加载事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_Load(object sender, EventArgs e)
		{
			#region 构建AdeptClient
			this.cfgDictIps.Clear();
			this.cfgDictPorts.Clear();
			try
			{
				ConfigLoad configLoad = new ConfigLoad(this.configPath);
				for (int i = 0; i < 2; i++)
				{
					bool flag = i == 0;
					if (flag)
					{
						this.cfgDictIps.Add("ServerIp", configLoad.GetStringValue("ServerIp"));
					}
					else
					{
						this.cfgDictIps.Add("RobotIp", configLoad.GetStringValue("RobotIp"));
					}
				}
				for (int j = 0; j < 2; j++)
				{
					bool flag2 = j == 0;
					if (flag2)
					{
						this.cfgDictPorts.Add("ServerPort", configLoad.GetIntValue("ServerPort"));
					}
					else
					{
						this.cfgDictPorts.Add("RobotPort", configLoad.GetIntValue("RobotPort"));
					}
				}
				this.oracleDB_str = string.Concat(new string[]
				{
					"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=",
					configLoad.GetStringValue("DatabaseIP"),
					")(PORT=",
					configLoad.GetStringValue("DatabasePort"),
					"))(CONNECT_DATA=(SERVICE_NAME=",
					configLoad.GetStringValue("DataServer"),
					")));Persist Security Info=True;User ID=",
					configLoad.GetStringValue("DatabaseUserName"),
					"; Password=",
					configLoad.GetStringValue("DatabaseUserPassword"),
					";"
				});
				areaName = configLoad.GetStringValue("AreaName");
				OracleHelper.connString = this.oracleDB_str;
				this.taskServer.lastgoal = configLoad.GetStringValue("LastGoal");
			}
			catch (Exception ex)
			{
				Logger.Append(this.richTextBox1, "Load config.ini failed the reason is:" + ex.Message);
			}
			AdapterRobot = new AdapterRobot(this.cfgDictIps["RobotIp"], this.cfgDictPorts["RobotPort"], 1);
			AdapterRobot.Name = "SMTAdept";
			Task task = AdapterRobot.ConnectAsync();
			#endregion

			#region 刷新任务
			this.UpdateTaskList(ref this.taskList);
			bool flag3 = this.taskList.Count == 0;
			if (flag3)
			{
				Logger.Append(this.richTextBox1, "There is no Task in DB");
			}
			this.workthread = new Thread(new ThreadStart(this.mainWork));
			this.workthread.IsBackground = true;
			this.workthread.Start();
			#endregion

			#region 构建服务端
			try
			{
				this.server = new AsyncTcpServer(new IPEndPoint(IPAddress.Parse(this.cfgDictIps["ServerIp"]), this.cfgDictPorts["ServerPort"]));
				this.server.ClientConnected += new EventHandler<TcpClientConnectedEventArgs>(this.Server_ClientConnected);
				this.server.ClientDisconnected += new EventHandler<TcpClientDisconnectedEventArgs>(this.Server_ClientDisconnected);
				this.server.DatagramReceived += new EventHandler<TcpDatagramReceivedEventArgs<byte[]>>(this.Server_PlaintextReceived);
				this.server.Start();
				Logger.Append(this.richTextBox1, string.Format("started server @ {0} : {1}", this.cfgDictIps["ServerIp"], this.cfgDictPorts["ServerPort"]));
			}
			catch (Exception ex2)
			{
				MessageBox.Show("failed to start server!\n" + ex2.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Logger.Append(this.richTextBox1, string.Format("start server failed with error:{0}", ex2));
			}
			#endregion
		}

		/// <summary>
		/// 窗体关闭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.workthread.Abort();
			Environment.Exit(0);
		}

		#endregion

		#region  ButtonEvent

		/// <summary>
		/// 任务列表弹出按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnQueryTask_Click(object sender, EventArgs e)
        {
			TaskView taskView = new TaskView();
			taskView.ShowDialog();
		}

		/// <summary>
		/// 机器人状态复位按钮
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void BtnReset_Click(object sender, EventArgs e)
        {
			TaskServer.flag = 1;
			bool flag = currentTask != null;
			if (flag)
			{
				currentTask.PATH_STAGE = "final";
				currentTask.ASSIGNED_ROBOT_NAME = AdapterRobot.Name;
				currentTask.TASK_STATUS = "Complete";
				OracleHelper.UpdateFirst(currentTask);
				currentTask = null;
			}

			Logger.Append(this.richTextBox1, "Human set the reset the robotStatus");
		}

		/// <summary>
		/// 查看机器人状态
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void btnRobotStatus_Click(object sender, EventArgs e)
        {
			bool flag = currentTask != null;
			if (flag)
			{
				Logger.Append(this.richTextBox1, string.Concat(new string[]
				{
					"current Task:Area-",
					currentTask.AREA_NAME,
					",Target1-",
					currentTask.TARGET_1,
					",Target2-",
					currentTask.TARGET_2
				}));
			}
			else
			{
				Logger.Append(this.richTextBox1, "There is no task");
			}
			Logger.Append(this.richTextBox1, string.Format("robot status is-IsConnected:{0}\n+{1}", AdapterRobot.GetConnectStatus(), AdapterRobot.GetRobotStatus()));
		}

        #endregion 

       
    }
}
