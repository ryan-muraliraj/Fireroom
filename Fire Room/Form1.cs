using System;
using System.IO;
using System.Media;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Fire_Room
{
    public partial class Form1 : Form
    {
        private string permanante;
        private string username = "Anonymous";
        private SoundPlayer SP;
        private string completevalue;

        public Form1()
        {
            InitializeComponent();
            SP = new SoundPlayer("ding.wav");
        }

        private void Form1_Load(object sender, EventArgs e)
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

            permanante = outie[7];
            label2.Text = "Connected to Firebase Database";
        }

        public void sendMessage(object sender)
        {
            DateTime date = DateTime.Now;
            string sdate = date.ToString("yyyy:MM:dd");
            string stime = date.ToString("HH:mm:ss");
            completevalue = username + ": " + textBox1.Text.ToString();

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
                if (permanante == completevalue)
                {
                    Console.Write("Same value");
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        SP.Play();
                    }
                    else
                    {
                        Console.Write("No Sound");
                    }
                }
            }
        }
        private void setNickname(object sender)
        {
            if (textBox2.Text != "")
            {
                username = textBox2.Text;
                label1.Text = "Currently known as: " + username;
            }
            else
            {
                MessageBox.Show("Please put in a valid username");
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            setNickname(sender);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                sendMessage(sender);
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                setNickname(sender);
            }
        }
    }
}