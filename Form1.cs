using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace ChatClient
{
    public partial class Form1 : MetroForm
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gui_state(false);
            button1.Enabled = false;
            label4.Text = "Not Connected";
            label4.ForeColor = Color.Red;
        }

        private void gui_state(bool state)
        {
            richTextBox1.Enabled = state;
            textBox3.Enabled = state;
            button2.Enabled = state;
        }

        private void gui_state2(bool state)
        {
            textBox1.Enabled = state;
            textBox2.Enabled = state;
            button1.Enabled = state;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            readData = "Connected to Chat Server ...";
            msg();
            clientSocket.Connect(textBox1.Text, 8888);
            if (clientSocket.Connected)
            {
                gui_state(true);
                gui_state2(false);
                label4.Text = "Connected";
                label4.ForeColor = Color.Green;
                serverStream = clientSocket.GetStream();

                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(textBox2.Text + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                Thread ctThread = new Thread(getMessage);
                ctThread.Start();
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(textBox3.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
            textBox3.Text = "";
        } 

        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[65536];
                buffSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                readData = "" + returndata;
                msg();
            }
        }

        private void msg()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(msg));
            else
                richTextBox1.Text = richTextBox1.Text + Environment.NewLine + readData;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //clientSocket.Close();
        }
    }
}
