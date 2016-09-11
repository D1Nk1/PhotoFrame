using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoFrame
{
    public partial class Form1 : Form
    {

        public Random r = new Random();
        public List<string> l = new List<string>();
        public int index;

        public Form1()
        {
            InitializeComponent();
            //Size height = new Size();
            //height.Height = pictureBox1.Height - 300;
            this.Size = pictureBox1.Size;
            this.button1.Click += Button1_Click;
            
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            var v = pictureBox1.ImageLocation.ToString();

            l.Add(v);

            if (l.Count >= 10)
            {
                l.RemoveAt(0);
            }

            index = l.Count;
            pictureBox1.ImageLocation = getFiles();
            //MessageBox.Show(l.Last());
            //MessageBox.Show(l.First());

        }

        private string getFiles()
        {
            var rand = new Random();
            var files = Directory.GetFiles(@"C:\Users\Igor\Documents\Visual Studio 2015\Projects\PhotoFrame\PhotoFrame\bin\Debug", "*.jpg");
            return files[rand.Next(files.Length)];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = getFiles();
            panel1.Visible = false;
            panel1.Parent = pictureBox1;
            panel1.BackColor = Color.FromArgb(70, 0, 0, 0);
            
            button1.BackColor = Color.FromArgb(70, 116, 146, 243);

            button1.TabStop = false;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 100, 120, 220);
            button1.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);


            button2.BackColor = Color.FromArgb(70, 116, 146, 243);

            button2.TabStop = false;
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 100, 120, 220);
            button2.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);


            button3.BackColor = Color.FromArgb(70, 116, 146, 243);

            button3.TabStop = false;
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 100, 120, 220);
            button3.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);


            button4.BackColor = Color.FromArgb(70, 116, 146, 243);

            button4.TabStop = false;
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 100, 120, 220);
            button4.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);

            checkBox1.ForeColor = Color.FromArgb(70, 116, 146, 243);

            timer2.Enabled = true;
            timer2.Interval = 30 * 60 * 1000;

            this.Location = new Point(1100, 150);
            //this.TopMost = true;

            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (rk.GetValue("PhotoFrame.exe") == null)
            {
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Checked = true;
            }

        }


        private bool draging = false;
        private Point pointClicked;

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            draging = false;
        }

        private void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (checkBox1.Checked)
                rk.SetValue("PhotoFrame.exe", Application.ExecutablePath.ToString());
            else
                rk.DeleteValue("PhotoFrame.exe", false);
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;
        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                draging = true;
                pointClicked = new Point(e.X, e.Y);
            }
            else
            {
                draging = false;
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (draging)
            {
                Point pointMoveTo;
                pointMoveTo = this.PointToScreen(new Point(e.X, e.Y));
                pointMoveTo.Offset(-pointClicked.X, -pointClicked.Y);
                this.Location = pointMoveTo;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }
        

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 2000;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.ImageLocation = l[index - 1];
                index--;
            }
            catch (Exception ex)
            {
                AutoClosingMessageBox.Show("No previous images available.", "Error", 1500);
            }
            //MessageBox.Show(l.ElementAt(l.Count - 1));
        }

        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                MessageBox.Show(text, caption);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow(null, _caption);
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            var v = pictureBox1.ImageLocation.ToString();

            l.Add(v);

            if (l.Count >= 10)
            {
                l.RemoveAt(0);
            }

            index = l.Count;

            pictureBox1.ImageLocation = getFiles();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.TopMost == false)
            {
                this.TopMost = true;
                button4.BackColor = Color.FromArgb(70, 255, 255, 255);
            }
            else
            {
                this.TopMost = false;
                button4.BackColor = Color.FromArgb(70, 116, 146, 243);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetStartup();
        }
    }
}
