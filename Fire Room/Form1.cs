using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Fire_Room
{
    public partial class Form1 : Form
    {
        private string permanante;
        private string username = "Anonymous";
        private int original;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            original = 0;
        }

        public void sendMessage(object sender)
        {
            DateTime date = DateTime.Now;
            string sdate = date.ToString("yyyy:MM:dd");
            string stime = date.ToString("HH:mm:ss");
            string completevalue = username + ": " + textBox1.Text.ToString();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Name = stime,
                Value = completevalue,
                
            });
            var request = WebRequest.CreateHttp("https://testing-firebase-60b9b.firebaseio.com/.json");

            request.Method = "PATCH";
            request.ContentType = "application/json";
            var buffer = Encoding.UTF8.GetBytes(json);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            var response = request.GetResponse();
            json = (new StreamReader(response.GetResponseStream())).ReadToEnd();

            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendMessage(sender);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string URL = "https://testing-firebase-60b9b.firebaseio.com/.json";
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(URL);
            request1.ContentType = "application/json: charset-utf-8";
            HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
            Stream responsestream = response1.GetResponseStream();
            StreamReader Read = new StreamReader(responsestream, Encoding.UTF8);
            var output = Read.ReadToEnd();
            string outphrase = output;
            string[] outie = outphrase.Split('"');

            if (permanante == outie[7])
            {
            }
            else
            {
                permanante = outie[7];
                listBox1.Items.Add(permanante);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            username = textBox2.Text;
            label1.Text = "Currently known as: " + username;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                sendMessage(sender);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                sendMessage(sender);
            }
        }
    }
}