﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoLiveRecorder
{
    public partial class Form1 : Form
    {
        #region Private 字段

        private B_Live aa;
        private bool IsRecording = false;

        #endregion Private 字段

        #region Public 构造函数

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        #endregion Public 构造函数

        #region Private 方法

        private void button1_Click(object sender, EventArgs e)
        {
            aa = new B_Live(int.Parse(textBox2.Text));
            aa.RecordStatusChanged += RecordStatus_Changed;
            aa.StartRecord(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            aa.StopRecord();
        }
        /// <summary>
        /// 检查更新
        /// </summary>
        /// <param name="MustGUI">是否必须有界面提示</param>
        private void CheckUpdate(bool MustGUI = false)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://api.zhangbudademao.com/112/update.php");
            HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
            System.IO.StreamReader Reader = new System.IO.StreamReader(rep.GetResponseStream());
            string str = Reader.ReadToEnd();
            string[] infos = str.Split('|');
            if (infos[0] != Application.ProductVersion)
            {
                if (MessageBox.Show("有新版本！\r\n" + Application.ProductVersion + "→" + infos[0] + "\r\n更新内容：" + infos[2] + "\r\n更新时间：" + infos[3], "有新版本！", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(infos[1]);
                }
            }
            else if (MustGUI) MessageBox.Show("版本已是最新","录播姬");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsRecording)
            {
                MessageBox.Show("正在录制中，请结束录制后重试");
                e.Cancel = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckUpdate();
            label3.Text = "当前版本：" + Application.ProductVersion;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CheckUpdate(true);
        }

        private void RecordStatus_Changed(bool RecordStatus)
        {
            IsRecording = RecordStatus;
            if (RecordStatus)
            {
                button1.Enabled = false;
                button2.Enabled = true;
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = false;
                textBox1.Text = "";
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\\*\\.flv");
            if (!reg.Match(saveFileDialog1.FileName).Success)
            {
                textBox1.Text = saveFileDialog1.FileName;
            }
            else
            {
                textBox1.Text = saveFileDialog1.FileName.Replace("*", "defult");
            }
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            saveFileDialog1.ShowDialog();
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("\\*\\.flv");
            if (!reg.Match(saveFileDialog1.FileName).Success)
            {
                textBox1.Text = saveFileDialog1.FileName;
            }
            else
            {
                textBox1.Text = saveFileDialog1.FileName.Replace("*", "defult");
            }
        }

        #endregion Private 方法
    }
}