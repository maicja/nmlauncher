using Minecraft_Launcher.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minecraft_Launcher
{
    internal static class Program
    {
        public static bool IsFontInstalled(string fontName)
        {
            using (var testFont = new System.Drawing.Font(fontName, 8))
                return 0 == string.Compare(fontName, testFont.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public static void InstallFont(string fontSourcePath)
        {
            var shellAppType = Type.GetTypeFromProgID("Shell.Application");
            var shell = Activator.CreateInstance(shellAppType);
            var fontFolder = (Shell32.Folder)shellAppType.InvokeMember("NameSpace", System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { Environment.GetFolderPath(Environment.SpecialFolder.Fonts) });
            if (File.Exists(fontSourcePath))
                fontFolder.CopyHere(fontSourcePath);
        }




        [STAThread]
        static void Main()
        {
            if (!Directory.Exists(Form1.launcherdir))
            {
                Directory.CreateDirectory(Form1.launcherdir);
            }
            if (!File.Exists(Form1.launcherdir + "\\minecraftzyczu.ttf"))
            {
                using (FileStream fs = File.Create(Form1.launcherdir + "\\minecraftzyczu.ttf"))
                {
                    byte[] info = Resources.minecraftzyczu;
                    fs.Write(info, 0, info.Length);
                }
            }
            else
            {
                File.WriteAllBytes(Form1.launcherdir + "\\minecraftzyczu.ttf", Resources.minecraftzyczu);
            }
            Task.Delay(100);
            if (!IsFontInstalled("MinecraftZyczu"))
            {
                InstallFont(Form1.launcherdir + "\\minecraftzyczu.ttf");
                MessageBox.Show("Font installed! Restart launcher to load it");
                Application.Exit();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
