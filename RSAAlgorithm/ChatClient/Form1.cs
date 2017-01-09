using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Form1 : Form
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            if (connect_box.Text.Length > 0 && name_box.Text.Length > 0)
            {
                readData = "Połączono z serwerem ...";
                msg();
                clientSocket.Connect(connect_box.Text, 8888);
                serverStream = clientSocket.GetStream();

                byte[] outStream = System.Text.Encoding.Unicode.GetBytes(name_box.Text + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                Thread ctThread = new Thread(getMessage);
                ctThread.Start();
                btn_connect.Enabled = false;
            }
            else MessageBox.Show("Wypełnij dane");
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (message_box.Text.Length > 0)
            {
                byte[] outStream = System.Text.Encoding.Unicode.GetBytes(message_box.Text + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                message_box.Clear();
            }
        }
        

        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                byte[] inStream = new byte[clientSocket.ReceiveBufferSize];
                int bytesRead = serverStream.Read(inStream, 0, inStream.Length);
                string returndata = Encoding.Unicode.GetString(inStream, 0, bytesRead);
                readData = "" + returndata;
                msg();
            }
        }

        private void msg()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(msg));
            else
                chat_box.Text = chat_box.Text + Environment.NewLine + " >> " + readData;
        } 
    }
}