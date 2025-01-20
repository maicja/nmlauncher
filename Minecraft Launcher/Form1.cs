using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Minecraft_Launcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MaximizeBox = false;
        }
        public static string url = "mksteam.ovh";
        public static string launcherdir = System.IO.Directory.GetCurrentDirectory();
        public static string configsdir = $"{System.IO.Directory.GetCurrentDirectory()}\\configs";
        public static string javapath = $"{System.IO.Directory.GetCurrentDirectory()}\\jre\\";
        string[] config = { "ram", "lastver", "acc" };
        void savecfg(string cfg)
        {
            File.WriteAllText(configsdir + "\\settings.mks", cfg);
        }
        string gencfg(bool isnew)
        {
            string retval = "";
            if (isnew)
            {
                foreach (string s in config)
                {
                    switch (s)
                    {
                        case "ram":
                            retval += s + ";2048\r\n";
                            break;
                        case "lastver":
                            retval += s + ";Release 1.20.6\r\n";
                            break;
                        case "acc":
                            retval += s + ";n|cracked\r\n";
                            break;
                    }

                }
            }
            else
            {
                foreach (string s in config)
                {
                    switch (s)
                    {
                        case "ram":
                            retval += s + $";{ram}\r\n";
                            break;
                        case "lastver":
                            retval += s + $";{selver}\r\n";
                            break;
                        case "acc":
                            retval += s + $";{account[0]}|{account[1]}\r\n";
                            break;
                    }

                }
            }
            return retval;
        }

        string[] account = { "n", "cracked", "cracked" };
        string ram = "2048";
        string selver = "Release 1.20.6";
        int version = 15;
        async void init()
        {
            base.Size = new Size(902, 578);
            panelmain.Location = new Point(0, 0);

            panelloading.Location = new Point(0, 0);
            panelpostload.Location = new Point(151, 177);
            panellogin.Location = new Point(151, 177);
            panelnews.Location = new Point(0, 0);
            panelaccount.Location = new Point(0, 0);
            panelprofiles.Location = new Point(0, 0);
            panellogin.BackColor = Color.FromArgb(130, 50, 50, 50);
            panelpostload.BackColor = Color.FromArgb(130, 50, 50, 50);
            panel2.BackColor = Color.FromArgb(130, 50, 50, 50);
            panel5.BackColor = Color.FromArgb(130, 50, 50, 50);
            panelmain.Visible = false;
            progressBarmain.Visible = false;
            panellogin.Visible = false;
            panelaccount.Visible = false;
            panelprofiles.Visible = false;
            panelpostload.Visible = true;
            panelnews.Visible = true;
    


            labelloading.Text = "Checking server connection...";
            try
            {

                if (await webClient.DownloadStringTaskAsync($"http://{url}/connect.txt") != "yup")
                {
                    labelloading.Text = "Checking server connection (bypass)...";
                    url = "109.231.31.129.koba.pl";
                    if (await webClient.DownloadStringTaskAsync($"http://{url}/connect.txt") != "yup")
                    {
                        labelloading.Text = "Checking server connection (bypass v2)...";
                        url = "109.231.31.129";
                        if (await webClient.DownloadStringTaskAsync($"http://{url}/connect.txt") != "yup")
                        {
                            labelloading.Text = "Trying offline mode...";
                            await Task.Delay(100);
                            url = "offline";
                        }
                    }
                }
            }
            catch (Exception)
            {
                labelloading.Text = "Checking server connection (bypass ex1)...";
                url = "109.231.31.129.koba.pl";
                try
                {
                    if (await webClient.DownloadStringTaskAsync($"http://{url}/connect.txt") != "yup")
                    {
                        labelloading.Text = "Checking server connection (bypass v2ex1)...";
                        url = "109.231.31.129";
                        if (await webClient.DownloadStringTaskAsync($"http://{url}/connect.txt") != "yup")
                        {
                            labelloading.Text = "Trying offline mode...";
                            await Task.Delay(100);
                            url = "offline";
                        }
                    }
                }
                catch (Exception)
                {
                    labelloading.Text = "Checking server connection (bypass v2ex2)...";
                    url = "109.231.31.129";
                    try
                    {
                        if (await webClient.DownloadStringTaskAsync($"http://{url}/connect.txt") != "yup")
                        {
                            labelloading.Text = "Trying offline mode...";
                            await Task.Delay(100);
                            url = "offline";
                        }
                    }
                    catch (Exception)
                    {
                        labelloading.Text = "Trying offline mode...";
                        await Task.Delay(100);
                        url = "offline";
                    }
                }
            }





            if (url != "offline")
            {
                if (int.Parse(await webClient.DownloadStringTaskAsync($"http://{url}/nmlauncher/files/l/last.ver")) > version)
                {
                    labelloading.Text = "Update aviable!";
                    await webClient.DownloadFileTaskAsync($"http://{url}/nmlauncher/files/l/updater.exe", launcherdir + "\\nmupdater.exe");
                    Process.Start(launcherdir + "\\nmupdater.exe");
                    Application.Exit();
                }
            }



            labelloading.Text = "Checking configs...";
            if (!Directory.Exists(configsdir))
            {
                Directory.CreateDirectory(configsdir);
            }
            if (!Directory.Exists(launcherdir + "\\profiles"))
            {
                Directory.CreateDirectory(launcherdir + "\\profiles");
            }
            if (!Directory.Exists(launcherdir + "\\bedrock\\stockappx"))
            {
                Directory.CreateDirectory(launcherdir + "\\bedrock\\stockappx");
            }
            if (!Directory.Exists(launcherdir + "\\bedrock\\importedappx"))
            {
                Directory.CreateDirectory(launcherdir + "\\bedrock\\importedappx");
            }
            if (!File.Exists(configsdir + "\\settings.mks"))
            {
                using (FileStream fs = File.Create(configsdir + "\\settings.mks"))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(gencfg(true));
                    fs.Write(info, 0, info.Length);
                }
            }
            if (!File.Exists(configsdir + "\\profiles.mks"))
            {
                using (FileStream fs = File.Create(configsdir + "\\profiles.mks"))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("");
                    fs.Write(info, 0, info.Length);
                }
            }
            if (!File.Exists(configsdir + "\\accounts.mks"))
            {
                using (FileStream fs = File.Create(configsdir + "\\accounts.mks"))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("n|cracked|cracked");
                    fs.Write(info, 0, info.Length);
                }
            }

            foreach (string cfg in File.ReadAllLines(configsdir + "\\settings.mks"))
            {
                if (cfg.Contains("ram;"))
                {
                    ram = cfg.Split(';')[1];
                }
                else if (cfg.Contains("lastver;"))
                {
                    selver = cfg.Split(';')[1];
                }
                else if (cfg.Contains("acc;"))
                {
                    account[0] = cfg.Split(';')[1].Split('|')[0];
                    account[1] = cfg.Split(';')[1].Split('|')[1];
                }
            }



            labelloading.Text = "Checking accounts...";
            await Task.Delay(10);
            if (account[0] == "n")
            {
                labelloading.Text = "Account not found!";
                await Task.Delay(1000);
                panellogin.Visible = true;
                panelpostload.Visible = false;
                //tu bedzie switch do login
                return;
            }

            if (account[1] != "cracked")
            {
                labelloading.Text = "Refreshing microsoft account...";
                await Task.Delay(10);
                string tosave = "";
                foreach (string s in File.ReadAllLines(configsdir + "\\accounts.mks"))
                {
                    if (s.Length > 2)
                    {
                        if (s.Contains($"{account[0]}|{account[1]}"))
                        {
                            string[] creds = mslogin.RefreshAccountData(s.Split('|')[2]).Split('|');
                            if (creds[0] == "error")
                            {
                                MessageBox.Show($"Failed to refresh account {account[0]}. You will be logged in as cracked user", "Microsoft login");

                                tosave += s + "\r\n";
                            }
                            else
                            {
                                account[0] = creds[0];
                                account[1] = creds[1];
                                account[2] = creds[2];
                                accesstoken = creds[3];
                                tosave += $"{account[0]}|{account[1]}|{account[2]}\r\n";
                            }
                        }
                        else
                        {
                            tosave += s + "\r\n";
                        }
                    }
                }
                File.WriteAllText(configsdir + "\\accounts.mks", tosave);
            }
            labelwelcome.Text = $"Welcome, {account[0]}\r\nReady to launch {selver}";

            labelloading.Text = "Checking versions...";
            if (url != "offline")
            {
                await webClient.DownloadFileTaskAsync($"http://{url}/nmlauncher/files/v/index.txt", launcherdir + "\\vers.txt");
            }
            if (!File.Exists(launcherdir + "\\vers.txt"))
            {
                MessageBox.Show("You can't use launcher in offline mode because you doesn't have any downloaded versions", "Offline error");
                Application.Exit();
            }
            refreshversions();
            try
            {
                comboBoxprofiles.SelectedItem = selver;
            }
            catch (Exception) { }
            panelloading.Visible = false;
            panelmain.Visible = true;
        }
        void refreshversions()
        {
            comboBoxprofiles.Items.Clear();
            foreach (string ver in File.ReadAllLines(launcherdir + "\\vers.txt"))
            {
                comboBoxprofiles.Items.Add(ver.Split('|')[0]);
            }
            foreach (string modver in File.ReadAllLines(configsdir + "\\profiles.mks"))
            {
                comboBoxprofiles.Items.Add("Custom " + modver.Split('|')[0]);
            }
        }
        string accesstoken = "cracked";
        WebClient webClient = new WebClient();

        private void Form1_Load(object sender, EventArgs e)
        {

            init();
        }
        bool skipcheck = true;
        private async void buttonplay_Click(object sender, EventArgs e)
        {
            try
            {
                if (!skipcheck)
                {
                    skipcheck = true;
                    return;
                }
                if (!deatach)
                {
                    java.Kill();

                    return;
                }
                deatach = false;
                progressBarmain.Visible = true;
                skipcheck = true;

                if (comboBoxprofiles.SelectedItem.ToString().StartsWith("Custom"))
                {

                    string profilename = comboBoxprofiles.SelectedItem.ToString().Substring(7);
                    string gamever = "";

                    foreach (string modver in File.ReadAllLines(configsdir + "\\profiles.mks"))
                    {
                        if (modver.StartsWith(profilename + "|"))
                        {
                            gamever = modver.Split('|')[1];
                        }
                    }
                    if (gamever == "")
                    {
                        MessageBox.Show("Cannot find version");
                        return;
                    }

                    string[] verindex = { };
                    foreach (string ver in File.ReadAllLines(launcherdir + "\\vers.txt"))
                    {
                        if (ver.Split('|')[0] == gamever)
                        {
                            verindex = ver.Split('|');
                        }
                    }
                    if (verindex == new string[] { })
                    {
                        MessageBox.Show("Cannot find version");
                        return;
                    }

                    if (url != "offline")
                    {
                        skipcheck = false;
                        buttonplay.Text = $"Checking files...\r\nClick to skip";
                        List<string[]> files = new List<string[]>();
                        float maxsizef = 0;
                        foreach (string filen in webClient.DownloadString($"http://{url}/nmlauncher/files/jre/{verindex[2]}/list.txt").Split('\n'))
                        {
                            if (filen.Length > 2)
                            {
                                string[] file = (filen.Replace("\r", "") + "|jre").Split('|');
                                files.Add(file);
                                maxsizef += float.Parse(file[2]);
                            }
                        }
                        foreach (string filen in webClient.DownloadString($"http://{url}/nmlauncher/files/v/{verindex[1]}/list.txt").Split('\n'))
                        {
                            if (filen.Length > 2)
                            {
                                string[] file = (filen.Replace("\r", "") + "|mc").Split('|');
                                files.Add(file);
                                maxsizef += float.Parse(file[2]);
                            }
                        }
                        maxsizef = maxsizef / 1000000;
                        float dwnlsizef = 0;
                        progressBarmain.Minimum = 0;
                        progressBarmain.Value = 0;
                        progressBarmain.Maximum = (int)maxsizef;
                        webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                        foreach (string[] file in files)
                        {
                            if (skipcheck)
                            {
                                continue;
                            }
                            if (!deatach)
                            {
                                bool amicool = false;
                                if (file[3] == "jre")
                                {
                                    int wl = Path.GetFileName($"{javapath}{verindex[2]}\\{file[0]}").Length + 1;
                                    string directory = $"{javapath}{verindex[2]}\\{file[0]}".Remove($"{javapath}{verindex[2]}\\{file[0]}".Length - wl, wl);

                                    if (!Directory.Exists(directory))
                                    {
                                        Directory.CreateDirectory(directory);
                                    }

                                    if (File.Exists($"{javapath}{verindex[2]}\\{file[0]}"))
                                    {
                                        if (await CalculateMD5($"{javapath}{verindex[2]}\\{file[0]}") != file[1])
                                        {
                                            await webClient.DownloadFileTaskAsync($"http://{url}/nmlauncher/files/jre/{verindex[2]}/{file[0]}", $"{javapath}{verindex[2]}\\{file[0]}");
                                        }
                                        else
                                        {
                                            amicool = true;
                                        }
                                    }
                                    else
                                    {
                                        await webClient.DownloadFileTaskAsync($"http://{url}/nmlauncher/files/jre/{verindex[2]}/{file[0]}", $"{javapath}{verindex[2]}\\{file[0]}");
                                    }

                                }
                                else
                                {
                                    int wl = Path.GetFileName($"{launcherdir}\\profiles\\{profilename}\\{file[0]}").Length + 1;
                                    string directory = $"{launcherdir}\\profiles\\{profilename}\\{file[0]}".Remove($"{launcherdir}\\profiles\\{profilename}\\{file[0]}".Length - wl, wl);
                                    if (!Directory.Exists(directory))
                                    {
                                        Directory.CreateDirectory(directory);
                                    }
                                    if (File.Exists($"{launcherdir}\\profiles\\{profilename}\\{file[0]}"))
                                    {
                                        if (await CalculateMD5($"{launcherdir}\\profiles\\{profilename}\\{file[0]}") != file[1])
                                        {
                                            await webClient.DownloadFileTaskAsync($"http://{url}/nmlauncher/files/v/{verindex[1]}/{file[0]}", $"{launcherdir}\\profiles\\{profilename}\\{file[0]}");
                                        }
                                        else
                                        {
                                            amicool = true;
                                        }
                                    }
                                    else
                                    {
                                        bool skip = false;
                                        if (File.Exists($"{launcherdir}\\game\\{file[0]}"))
                                        {
                                            if (await CalculateMD5($"{launcherdir}\\game\\{file[0]}") == file[1])
                                            {
                                                File.Copy($"{launcherdir}\\game\\{file[0]}", $"{launcherdir}\\profiles\\{profilename}\\{file[0]}");
                                                skip = true;
                                            }
                                        }
                                        if (!skip)
                                        {
                                            await webClient.DownloadFileTaskAsync($"http://{url}/nmlauncher/files/v/{verindex[1]}/{file[0]}", $"{launcherdir}\\profiles\\{profilename}\\{file[0]}");
                                        }
                                    }
                                }
                                //MessageBox.Show((float.Parse(file[2]) / 1000000f).ToString());
                                dwnlsizef += float.Parse(file[2]) / 1000000f;
                                progressBarmain.Value = (int)dwnlsizef;
                                if (amicool)
                                {
                                    buttonplay.Text = $"Checked {progressBarmain.Value}/{progressBarmain.Maximum}MB {progressBarmain.Value * 100 / progressBarmain.Maximum}%\r\nClick to skip";
                                }
                                else
                                {
                                    buttonplay.Text = $"Downloading {progressBarmain.Value}/{progressBarmain.Maximum}MB {progressBarmain.Value * 100 / progressBarmain.Maximum}%\r\nClick to skip";
                                }
                            }
                        }
                        buttonplay.Text = $"Checked files";
                        await webClient.DownloadFileTaskAsync($"http://{url}/nmlauncher/files/v/{verindex[1]}/args.txt", $"{launcherdir}\\{verindex[1]}.args");
                    }
                    else
                    {
                        buttonplay.Text = $"Can't check files because of offline mode";
                    }
                    if (!deatach)
                    {
                        progressBarmain.Visible = false;
                        string args = File.ReadAllText($"{launcherdir}\\{verindex[1]}.args").Replace("LAUNCHERPATH", $"{launcherdir}\\profiles\\{profilename}").Replace("RAMMB", ram).Replace("USERNAMEMOD", account[0]).Replace("TOKEN32", accesstoken);
                        if (account[1] != "cracked")
                        {
                            args = args.Replace("--userType legacy", "--userType msa").Replace("UUID32", account[1]);
                        }
                        else
                        {
                            args = args.Replace("UUID32", "21376942021376942021376942012345");
                            if (verindex[1].Contains("1.16.5"))
                            {
                                args = "-Dminecraft.api.auth.host=https://nope.invalid -Dminecraft.api.account.host=https://nope.invalid -Dminecraft.api.session.host=https://nope.invalid -Dminecraft.api.services.host=https://nope.invalid " + args;
                            }
                        }

                        System.Diagnostics.ProcessStartInfo startinfo = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = $"{javapath}{verindex[2]}\\bin\\javaw.exe",
                            Arguments = "-noverify " + args,
                            ErrorDialog = true,
                            RedirectStandardError = true,
                            RedirectStandardOutput = true,
                            UseShellExecute = false,

                            WorkingDirectory = $"{launcherdir}\\profiles\\{profilename}"
                        };
                        java = Process.Start(startinfo);
                        stream = java.StandardOutput;
                        buttonplay.Text = "Kill game";
                        Task.Run(() =>
                        {
                            readstream();
                        });
                        while (!deatach)
                        {
                            if (!java.HasExited)
                            {
                                java.Refresh();
                            }
                            else
                            {
                                deatach = true;
                            }
                            await Task.Delay(1000);
                        }
                    }
                    buttonplay.Text = "Play";
                    progressBarmain.Visible = false;
                }
                else
                {
                    string[] verindex = { };
                    foreach (string ver in File.ReadAllLines(launcherdir + "\\vers.txt"))
                    {
                        if (ver.Split('|')[0] == comboBoxprofiles.SelectedItem.ToString())
                        {
                            verindex = ver.Split('|');
                        }
                    }
                    if (verindex == new string[] { })
                    {
                        MessageBox.Show("Cannot find version");
                        return;
                    }
                    if (url != "offline")
                    {
                        skipcheck = false;
                        buttonplay.Text = $"Checking files...\r\nClick to skip";
                        List<string[]> files = new List<string[]>();
                        float maxsizef = 0;
                        foreach (string filen in webClient.DownloadString($"http://{url}/nmlauncher/files/jre/{verindex[2]}/list.txt").Split('\n'))
                        {
                            if (filen.Length > 2)
                            {
                                string[] file = (filen.Replace("\r", "") + "|jre").Split('|');
                                files.Add(file);
                                maxsizef += float.Parse(file[2]);
                            }
                        }

                        foreach (string filen in webClient.DownloadString($"http://{url}/nmlauncher/files/v/{verindex[1]}/list.txt").Split('\n'))
                        {
                            if (filen.Length > 2)
                            {
                                string[] file = (filen.Replace("\r", "") + "|mc").Split('|');
                                files.Add(file);
                                maxsizef += float.Parse(file[2]);
                            }
                        }

                        maxsizef = maxsizef / 1000000; //konwersja do MB
                        float dwnlsizef = 0;
                        progressBarmain.Minimum = 0;
                        progressBarmain.Value = 0;
                        progressBarmain.Maximum = (int)maxsizef;

                        webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                        foreach (string[] file in files)
                        {
                            if (!skipcheck)
                            {
                                if (!deatach)
                                {
                                    bool amicool = false;
                                    if (file[3] == "jre")
                                    {
                                        int wl = Path.GetFileName($"{javapath}{verindex[2]}\\{file[0]}").Length + 1;
                                        string directory = $"{javapath}{verindex[2]}\\{file[0]}".Remove($"{javapath}{verindex[2]}\\{file[0]}".Length - wl, wl);

                                        if (!Directory.Exists(directory))
                                        {
                                            Directory.CreateDirectory(directory);
                                        }

                                        if (File.Exists($"{javapath}{verindex[2]}\\{file[0]}"))
                                        {
                                            if (await CalculateMD5($"{javapath}{verindex[2]}\\{file[0]}") != file[1])
                                            {

                                                await webClient.DownloadFileTaskAsync($"http://{url}/nmlauncher/files/jre/{verindex[2]}/{file[0]}", $"{javapath}{verindex[2]}\\{file[0]}");
                                            }
                                            else
                                            {
                                                amicool = true;
                                            }
                                        }
                                        else
                                        {
                                            await webClient.DownloadFileTaskAsync($"http://{url}/nmlauncher/files/jre/{verindex[2]}/{file[0]}", $"{javapath}{verindex[2]}\\{file[0]}");
                                        }
                                    }
                                    else
                                    {
                                        int wl = Path.GetFileName($"{launcherdir}\\game\\{file[0]}").Length + 1;
                                        string directory = $"{launcherdir}\\game\\{file[0]}".Remove($"{launcherdir}\\game\\{file[0]}".Length - wl, wl);
                                        if (!Directory.Exists(directory))
                                        {
                                            Directory.CreateDirectory(directory);
                                        }
                                        if (File.Exists($"{launcherdir}\\game\\{file[0]}"))
                                        {
                                            if (await CalculateMD5($"{launcherdir}\\game\\{file[0]}") != file[1])
                                            {
                                                await webClient.DownloadFileTaskAsync($"http://{url}/nmlauncher/files/v/{verindex[1]}/{file[0]}", $"{launcherdir}\\game\\{file[0]}");
                                            }
                                            else
                                            {
                                                amicool = true;
                                            }
                                        }
                                        else
                                        {
                                            await webClient.DownloadFileTaskAsync($"http://{url}/nmlauncher/files/v/{verindex[1]}/{file[0]}", $"{launcherdir}\\game\\{file[0]}");
                                        }


                                    }
                                    dwnlsizef += float.Parse(file[2]) / 1000000f;
                                    progressBarmain.Value = (int)dwnlsizef;
                                    buttonplay.Text = $"Downloading {progressBarmain.Value}/{progressBarmain.Maximum}MB {progressBarmain.Value * 100 / progressBarmain.Maximum}%\r\nClick to skip";
                                    if (amicool)
                                    {
                                        buttonplay.Text = $"Checked {progressBarmain.Value}/{progressBarmain.Maximum}MB {progressBarmain.Value * 100 / progressBarmain.Maximum}%\r\nClick to skip";
                                    }
                                    else
                                    {
                                        buttonplay.Text = $"Downloading {progressBarmain.Value}/{progressBarmain.Maximum}MB {progressBarmain.Value * 100 / progressBarmain.Maximum}%\r\nClick to skip";
                                    }
                                }
                            }
                        }
                        buttonplay.Text = $"Checked files";
                        await webClient.DownloadFileTaskAsync($"http://{url}/nmlauncher/files/v/{verindex[1]}/args.txt", $"{launcherdir}\\{verindex[1]}.args");
                    }
                    else
                    {
                        buttonplay.Text = $"Can't check files because of offline mode";
                    }
                    if (!deatach)
                    {
                        progressBarmain.Visible = false;
                        string args = File.ReadAllText($"{launcherdir}\\{verindex[1]}.args").Replace("LAUNCHERPATH", $"{launcherdir}\\game").Replace("RAMMB", ram).Replace("USERNAMEMOD", account[0]).Replace("TOKEN32", accesstoken);
                        if (account[1] != "cracked")
                        {
                            args = args.Replace("--userType legacy", "--userType msa").Replace("UUID32", account[1]);
                        }
                        else
                        {
                            args = args.Replace("UUID32", "21376942021376942021376942012345");
                            if (verindex[1].Contains("1.16.5"))
                            {
                                args = "-Dminecraft.api.auth.host=https://nope.invalid -Dminecraft.api.account.host=https://nope.invalid -Dminecraft.api.session.host=https://nope.invalid -Dminecraft.api.services.host=https://nope.invalid " + args;
                            }
                        }

                        System.Diagnostics.ProcessStartInfo startinfo = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = $"{javapath}{verindex[2]}\\bin\\javaw.exe",
                            Arguments = "-noverify " + args,
                            ErrorDialog = true,
                            RedirectStandardError = true,
                            RedirectStandardOutput = true,
                            UseShellExecute = false,

                            WorkingDirectory = $"{launcherdir}\\game"
                        };
                        java = Process.Start(startinfo);
                        stream = java.StandardOutput;
                        buttonplay.Text = "Kill game";
                        Task.Run(() =>
                        {
                            readstream();
                        });
                        while (!deatach)
                        {
                            if (!java.HasExited)
                            {
                                java.Refresh();
                            }
                            else
                            {
                                deatach = true;
                            }
                            await Task.Delay(1000);
                        }
                    }

                    buttonplay.Text = "Play";

                    progressBarmain.Visible = false;


                }
            }
            catch (Exception ex)
            {
                if (url == "offline")
                {
                    MessageBox.Show($"{ex.Message}\r\n\r\nThis error may be caused because of launching incomplete version in offline mode");
                }
                else
                {
                    MessageBox.Show($"{ex.Message}\r\n\r\nUnknown error");
                }
                buttonplay.Text = "Play";
                progressBarmain.Visible = false;
                deatach = true;
            }
            skipcheck = true;
        }
        Process java;
        float oldbytes = 0;
        public static bool deatach = true;

        public static StreamReader stream;
        public static async void readstream()
        {
            while (!deatach)
            {
                try
                {
                    while (!stream.EndOfStream)
                    {
                        stream2 = stream2 + "\r\n" + await stream.ReadLineAsync();
                    }
                }
                catch (Exception) { }
            }
            stream2 = "Logs:";
        }
        public static string stream2 = "Logs:";

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

            progressBarmain.Value += (int)((e.BytesReceived / 1000000) - oldbytes);
            buttonplay.Text = $"Downloading {progressBarmain.Value}/{progressBarmain.Maximum}MB {progressBarmain.Value * 100 / progressBarmain.Maximum}%\r\nClick to skip";
            oldbytes = e.BytesReceived / 1000000;


        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {

        }
        Regex rg = new Regex("^[a-zA-Z0-9_]{2,16}$");
        Regex rgm = new Regex("^[a-zA-Z0-9_ -.]{1,20}$");
        public static async Task<string> CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            {
                var buffer = new byte[8192];
                int bytesRead;
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    md5.TransformBlock(buffer, 0, bytesRead, null, 0);
                }

                // Finalize the hash computation
                md5.TransformFinalBlock(Array.Empty<byte>(), 0, 0);

                return BitConverter.ToString(md5.Hash).Replace("-", "").ToLowerInvariant();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxlogincrack.Text.Length < 3 || textBoxlogincrack.Text.Length > 16)
            {
                MessageBox.Show("Username must be between 3-16 characters", "Login error");
                return;
            }
            if (!rg.IsMatch(textBoxlogincrack.Text))
            {
                MessageBox.Show("You can use only a-Z 0-9 _ characters in username", "Login error");
                return;
            }
            account[0] = textBoxlogincrack.Text;
            savecfg(gencfg(false));
            File.WriteAllText(configsdir + "\\accounts.mks", $"{account[0]}|cracked|cracked");
            init();
        }
        void refreshaccswitch()
        {
            labelwelcome.Text = $"Welcome, {account[0]}\r\nReady to launch {selver}";
            labellogasw.Text = $"You are already logged in as {account[0]}. You can login with different account below.";
            comboBoxcracked.Items.Clear();
            comboBoxmicrosoft.Items.Clear();
            foreach (string line in File.ReadAllLines(configsdir + "\\accounts.mks"))
            {
                string[] lac = line.Split('|');
                if (lac[1] == "cracked")
                {
                    comboBoxcracked.Items.Add(lac[0]);
                }
                else
                {
                    comboBoxmicrosoft.Items.Add($"{lac[0]}                                        {lac[1]}");
                }
            }
            if (account[1] == "cracked")
            {
                comboBoxcracked.SelectedItem = account[0];
            }
            else
            {
                comboBoxmicrosoft.SelectedItem = $"{account[0]}                                        {account[1]}";
            }
            textBoxram.Text = ram;
        }


        private void buttoncrkdel_Click(object sender, EventArgs e)
        {
            try
            {
                if (account[1] == "cracked")
                {
                    if (account[0] == comboBoxcracked.SelectedItem.ToString())
                    {
                        MessageBox.Show("You cannot delete the account you are logged in to", "Account deletion");
                        return;
                    }
                }
                string tosave = "";
                foreach (string s in File.ReadAllLines(configsdir + "\\accounts.mks"))
                {
                    if (!s.Contains($"{comboBoxcracked.SelectedItem.ToString()}|cracked"))
                    {
                        tosave += s + "\r\n";
                    }
                }
                File.WriteAllText(configsdir + "\\accounts.mks", tosave);
                refreshaccswitch();
            }
            catch (Exception) { }
        }

        private void buttonmsdel_Click(object sender, EventArgs e)
        {
            try
            {
                if (account[1] != "cracked")
                {
                    if ($"{account[0]}                                        {account[1]}" == comboBoxmicrosoft.SelectedItem.ToString())
                    {
                        MessageBox.Show("You cannot delete the account you are logged in to", "Account deletion");
                        return;
                    }
                }
                string tosave = "";
                foreach (string s in File.ReadAllLines(configsdir + "\\accounts.mks"))
                {
                    if (!s.Contains(comboBoxmicrosoft.SelectedItem.ToString().Replace("                                        ", "|")))
                    {
                        tosave += s + "\r\n";
                    }
                }
                File.WriteAllText(configsdir + "\\accounts.mks", tosave);
                refreshaccswitch();
            }
            catch (Exception) { }
        }

        private void buttoncrklogin_Click(object sender, EventArgs e)
        {
            try
            {
                account[2] = "cracked";
                account[1] = "cracked";
                account[0] = comboBoxcracked.SelectedItem.ToString();
                savecfg(gencfg(false));
                refreshaccswitch();
            }
            catch (Exception) { }
        }

        private void buttoncrkadd_Click(object sender, EventArgs e)
        {
            if (textBoxcrkadd.Text.Length < 3 || textBoxcrkadd.Text.Length > 16)
            {
                MessageBox.Show("Username must be between 3-16 characters", "Login error");
                return;
            }
            if (!rg.IsMatch(textBoxcrkadd.Text))
            {
                MessageBox.Show("You can use only a-Z 0-9 _ characters in username", "Login error");
                return;
            }
            account[0] = textBoxcrkadd.Text;
            account[2] = "cracked";
            account[1] = "cracked";
            savecfg(gencfg(false));
            string tosave = "";
            foreach (string s in File.ReadAllLines(configsdir + "\\accounts.mks"))
            {
                tosave += s + "\r\n";
            }
            tosave += $"{account[0]}|{account[1]}|{account[2]}";
            File.WriteAllText(configsdir + "\\accounts.mks", tosave);
            refreshaccswitch();
        }

        private void buttonmsadd_Click(object sender, EventArgs e)
        {
            string[] creds = mslogin.addmsacc();
            if (creds[1] == "error")
            {
                MessageBox.Show("Unknown error");
                return;
            }

            account[0] = creds[0];
            account[1] = creds[1];
            account[2] = creds[2];
            accesstoken = creds[3];
            savecfg(gencfg(false));
            string tosave = "";
            foreach (string s in File.ReadAllLines(configsdir + "\\accounts.mks"))
            {
                tosave += s + "\r\n";
            }
            tosave += $"{account[0]}|{account[1]}|{account[2]}";
            File.WriteAllText(configsdir + "\\accounts.mks", tosave);
            refreshaccswitch();
        }

        private void buttonmslogin_Click(object sender, EventArgs e)
        {
            try
            {
                string refresh = "none";
                foreach (string s in File.ReadAllLines(configsdir + "\\accounts.mks"))
                {
                    if (s.Contains(comboBoxmicrosoft.SelectedItem.ToString().Replace("                                        ", "|")))
                    {
                        refresh = s.Split('|')[2];
                    }
                }

                string[] creds = mslogin.RefreshAccountData(refresh).Split('|');
                if (creds[1] == "error")
                {
                    MessageBox.Show("Unknown error");
                    return;
                }

                account[0] = creds[0];
                account[1] = creds[1];
                account[2] = creds[2];
                accesstoken = creds[3];
                savecfg(gencfg(false));
                string tosave = "";
                foreach (string s in File.ReadAllLines(configsdir + "\\accounts.mks"))
                {
                    if (s.Length > 2)
                    {
                        if (!s.Contains($"|{account[1]}|"))
                        {

                            tosave += s + "\r\n";
                            //MessageBox.Show(s, "cool");
                        }
                        else
                        {
                            //MessageBox.Show(s, "skipped");
                        }
                    }
                }
                tosave += $"{account[0]}|{account[1]}|{account[2]}";
                File.WriteAllText(configsdir + "\\accounts.mks", tosave);
                refreshaccswitch();
            }
            catch (Exception) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string[] creds = mslogin.addmsacc();
            if (creds[1] == "error")
            {
                MessageBox.Show("Unknown error");
                return;
            }

            account[0] = creds[0];
            account[1] = creds[1];
            account[2] = creds[2];
            accesstoken = creds[3];
            savecfg(gencfg(false));

            File.WriteAllText(configsdir + "\\accounts.mks", $"{account[0]}|{account[1]}|{account[2]}");
            init();
        }



        private void buttonswitchuser_Click(object sender, EventArgs e)
        {
            refreshaccswitch();
            if (panelaccount.Visible)
            {
                panelaccount.Visible = false;
                panelnews.Visible = true;
                return;
            }
            panelprofiles.Visible = false;
            panelnews.Visible = false;
            panelaccount.Visible = true;

        }
        Regex num = new Regex(@"^\d+$");


        private void buttonsaveset_Click_1(object sender, EventArgs e)
        {
            if (!num.IsMatch(textBoxram.Text))
            {
                MessageBox.Show("Can't save settings because of bad ram config");
                return;
            }
            ram = textBoxram.Text;
            savecfg(gencfg(false));
        }

        void profpagecfg(bool isnew)
        {
            comboBox1.Items.Clear();
            foreach (string ver in File.ReadAllLines(launcherdir + "\\vers.txt"))
            {
                comboBox1.Items.Add(ver.Split('|')[0]);
            }
            if (isnew)
            {
                button4.Visible = false;
                buttondelprof.Visible = false;
                textBox1.Text = "";

                labelprof.Text = "Create new profile";
                button3.Text = "Create profile";
                textBoxprofilefolder.Text = "Not created";

            }
            else
            {
                button4.Visible = true;
                buttondelprof.Visible = true;
                textBox1.Text = comboBoxprofiles.SelectedItem.ToString().Substring(7);
                labelprof.Text = "Profile editor";
                button3.Text = "Save profile";
                textBoxprofilefolder.Text = $"{launcherdir}\\profiles\\{comboBoxprofiles.SelectedItem.ToString()}".Replace("profiles\\Custom ", "profiles\\");
                foreach (string modver in File.ReadAllLines(configsdir + "\\profiles.mks"))
                {
                    if (modver.StartsWith(oldprofname + "|"))
                    {
                        comboBox1.SelectedItem = modver.Split('|')[1];
                        oldprofver = modver.Split('|')[1];
                    }
                }
            }

        }

        private void buttonnewprof_Click(object sender, EventArgs e)
        {
            profpagecfg(true);
            if (panelprofiles.Visible)
            {
                panelprofiles.Visible = false;
                panelnews.Visible = true;
                return;
            }
            panelaccount.Visible = false;
            panelnews.Visible = false;
            panelprofiles.Visible = true;
        }
        string oldprofname = "";
        string oldprofver = "";
        private void buttoneditprof_Click(object sender, EventArgs e)
        {
            if (!comboBoxprofiles.SelectedItem.ToString().StartsWith("Custom"))
            {
                MessageBox.Show("Select profile before clicking edit!");
                return;
            }
            oldprofname = comboBoxprofiles.SelectedItem.ToString().Substring(7);
            profpagecfg(false);






            if (panelprofiles.Visible)
            {
                panelprofiles.Visible = false;
                panelnews.Visible = true;
                return;
            }
            panelaccount.Visible = false;
            panelnews.Visible = false;
            panelprofiles.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem.ToString().Length < 3)
                {
                    MessageBox.Show("Select version");
                    return;
                }



                textBox1.Text = textBox1.Text.Replace("  ", " ");
                //save/create
                if (!rgm.IsMatch(textBox1.Text))
                {
                    MessageBox.Show("Profile can only contain a-Z 0-9 characters and must be 3-16 characters long");
                    return;
                }

                if (button3.Text == "Create profile")
                {
                    if (Directory.Exists($"{launcherdir}\\profiles\\{textBox1.Text}"))
                    {
                        MessageBox.Show("Profile with this name already exists");
                        return;
                    }


                    string tosave = "";
                    foreach (string modver in File.ReadAllLines(configsdir + "\\profiles.mks"))
                    {
                        if (modver.Contains("|"))
                        {
                            tosave += modver + "\r\n";
                        }
                    }
                    File.WriteAllText(configsdir + "\\profiles.mks", tosave + $"{textBox1.Text}|{comboBox1.Text}");
                    Directory.CreateDirectory($"{launcherdir}\\profiles\\{textBox1.Text}");
                    refreshversions();
                    comboBoxprofiles.SelectedItem = "Custom " + textBox1.Text;
                    selver = comboBoxprofiles.SelectedItem.ToString();
                    savecfg(gencfg(false));
                    labelwelcome.Text = "Welcome, " + account[0] + "\r\nReady to launch " + selver;
                }
                else
                {
                    //edycja
                    if (textBox1.Text != oldprofname)
                    {
                        if (Directory.Exists($"{launcherdir}\\profiles\\{textBox1.Text}"))
                        {
                            MessageBox.Show("Profile with this name already exists");
                            return;
                        }
                        Directory.Move($"{launcherdir}\\profiles\\{oldprofname}", $"{launcherdir}\\profiles\\{textBox1.Text}");


                    }

                    string tosave = "";
                    foreach (string modver in File.ReadAllLines(configsdir + "\\profiles.mks"))
                    {
                        if (modver.StartsWith(oldprofname + "|"))
                        {
                            tosave += $"{textBox1.Text}|{comboBox1.Text}\r\n";
                        }
                        else if (modver.Contains("|"))
                        {
                            tosave += modver + "\r\n";
                        }
                    }
                    File.WriteAllText(configsdir + "\\profiles.mks", tosave);
                    refreshversions();
                    comboBoxprofiles.SelectedItem = "Custom " + textBox1.Text;
                    selver = comboBoxprofiles.SelectedItem.ToString();
                    savecfg(gencfg(false));
                    labelwelcome.Text = "Welcome, " + account[0] + "\r\nReady to launch " + selver;
                }
                if (panelprofiles.Visible)
                {
                    panelprofiles.Visible = false;
                    panelnews.Visible = true;
                    return;
                }
                panelaccount.Visible = false;
                panelnews.Visible = false;
                panelprofiles.Visible = true;
            }
            catch (Exception) { MessageBox.Show("Select version"); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", textBoxprofilefolder.Text);
        }

        private void comboBoxprofiles_SelectionChangeCommitted(object sender, EventArgs e)
        {
            selver = comboBoxprofiles.SelectedItem.ToString();
            savecfg(gencfg(false));
            labelwelcome.Text = "Welcome, " + account[0] + "\r\nReady to launch " + selver;
        }

        private void buttondelprof_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.Delete($"{launcherdir}\\profiles\\{oldprofname}", true);

                string tosave = "";
                foreach (string modver in File.ReadAllLines(configsdir + "\\profiles.mks"))
                {
                    if (!modver.StartsWith(oldprofname + "|"))
                    {
                        if (modver.Contains("|"))
                        {
                            tosave += modver + "\r\n";
                        }
                    }
                }
                File.WriteAllText(configsdir + "\\profiles.mks", tosave);

                selver = "Release 1.20.6";
                refreshversions();
                comboBoxprofiles.SelectedItem = selver;
                savecfg(gencfg(false));
                labelwelcome.Text = "Welcome, " + account[0] + "\r\nReady to launch " + selver;
                if (panelprofiles.Visible)
                {
                    panelprofiles.Visible = false;
                    panelnews.Visible = true;
                    return;
                }
                panelaccount.Visible = false;
                panelnews.Visible = false;
                panelprofiles.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unknown error: " + ex.Message, "Delete profile error");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "This will delete all your versions and mc settings (excluding profiles). After deleting launcher will automatically restart. Do you want to proceed?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    Directory.Delete($"{launcherdir}\\game", true);
                    Directory.Delete($"{launcherdir}\\jre", true);
                    init();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unknown error: " + ex.Message, "Delete profile error");
            }
        }

       
    }
}