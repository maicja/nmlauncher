using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NML_Updater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
        }
        WebClient client = new WebClient();
        public static string launcherdir = System.IO.Directory.GetCurrentDirectory();
        string url = "mksteam.ovh";
        private async void Form1_Load(object sender, EventArgs e)
        {
            await Task.Delay(500);
            try
            {

                if (await client.DownloadStringTaskAsync($"http://{url}/connect.txt") != "sex")
                {
                    statuslabel.Text = "Checking server connection (bypass)...";
                    url = "109.231.31.129.koba.pl";
                    if (await client.DownloadStringTaskAsync($"http://{url}/connect.txt") != "sex")
                    {
                        statuslabel.Text = "Checking server connection (bypass v2)...";
                        url = "109.231.31.129";
                        if (await client.DownloadStringTaskAsync($"http://{url}/connect.txt") != "sex")
                        {
                            statuslabel.Text = "Cannot connect to the server!";
                            await Task.Delay(2000);
                            Application.Exit();
                        }
                    }
                }
            }
            catch (Exception)
            {
                statuslabel.Text = "Checking server connection (bypass ex1)...";
                url = "109.231.31.129.koba.pl";
                try
                {
                    if (await client.DownloadStringTaskAsync($"http://{url}/connect.txt") != "sex")
                    {
                        statuslabel.Text = "Checking server connection (bypass v2ex1)...";
                        url = "109.231.31.129";
                        if (await client.DownloadStringTaskAsync($"http://{url}/connect.txt") != "sex")
                        {
                            statuslabel.Text = "Cannot connect to the server!";
                            await Task.Delay(2000);
                            Application.Exit();
                        }
                    }
                }
                catch (Exception)
                {
                    statuslabel.Text = "Checking server connection (bypass v2ex2)...";
                    url = "109.231.31.129";
                    try
                    {
                        if (await client.DownloadStringTaskAsync($"http://{url}/connect.txt") != "sex")
                        {
                            statuslabel.Text = "Cannot connect to the server!";
                            await Task.Delay(2000);
                            Application.Exit();
                        }
                    }
                    catch (Exception)
                    {
                        statuslabel.Text = "Cannot connect to the server!";
                        await Task.Delay(2000);
                        Application.Exit();
                    }
                }
            }




            
            statuslabel.Text = "Downloading update...";
            await client.DownloadFileTaskAsync($"http://{url}/nmlauncher/files/l/client.exe", launcherdir + "\\nmlauncher.exe");
            statuslabel.Text = "Launching...";
            Process.Start(launcherdir + "\\nmlauncher.exe");
            Application.Exit();
        }
    }
}
