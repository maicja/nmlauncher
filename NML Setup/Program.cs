using IWshRuntimeLibrary;
using NML_Setup.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace NML_Setup
{
    internal class Program
    {


        private static void AddProgramsShortcut(string exe)
        {
            string pathToExe = exe;
            string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            string appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", "NMLauncher");

            if (!Directory.Exists(appStartMenuPath))
                Directory.CreateDirectory(appStartMenuPath);

            string shortcutLocation = Path.Combine(appStartMenuPath, "Minecraft Launcher" + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "NMLauncher";
            //shortcut.IconLocation = @"C:\Program Files (x86)\TestApp\TestApp.ico"; //uncomment to set the icon of the shortcut
            shortcut.TargetPath = pathToExe;
            shortcut.WorkingDirectory = Path.GetDirectoryName(pathToExe);
            shortcut.Save();
        }

        public static void CreateShortcut(string name, string exename, string path)
        {
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + $"\\{name}.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = name;
            shortcut.TargetPath = path + "\\" + exename;
            shortcut.WorkingDirectory = path;
            shortcut.Save();
            string programs_path = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            string shortcutFolder = Path.Combine(programs_path, @"NMLauncher");
            if (!Directory.Exists(shortcutFolder))
            {
                Directory.CreateDirectory(shortcutFolder);
            }
            WshShell shellClass = new WshShell();
            string settingsLink = Path.Combine(shortcutFolder, $"{name}.lnk");
            IWshShortcut shortcutmenu = (IWshShortcut)shellClass.CreateShortcut(settingsLink);
            shortcutmenu.TargetPath = path + "\\" + exename;
            shortcutmenu.WorkingDirectory = path;
            shortcutmenu.Description = name;
            shortcutmenu.Save();
        }
        static void Main(string[] args)
        {
            if (!Directory.Exists("c:\\maicjadir\\nmlauncher"))
            {
                Directory.CreateDirectory("c:\\maicjadir\\nmlauncher");
            }
            if (!System.IO.File.Exists("c:\\maicjadir\\nmlauncher\\nmlauncher.exe"))
            {
                using (FileStream fs = System.IO.File.Create("c:\\maicjadir\\nmlauncher\\nmlauncher.exe"))
                {
                    byte[] info = Resource1.nml;
                    fs.Write(info, 0, info.Length);
                }
            }
            else
            {
                System.IO.File.WriteAllBytes("c:\\maicjadir\\nmlauncher\\nmlauncher.exe", Resource1.nml);
            }
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "c:\\maicjadir\\nmlauncher\\nmlauncher.exe";
            startInfo.WorkingDirectory = "c:\\maicjadir\\nmlauncher";
            AddProgramsShortcut("c:\\maicjadir\\nmlauncher\\nmlauncher.exe");
            CreateShortcut("Minecraft Launcher", "nmlauncher.exe", "c:\\maicjadir\\nmlauncher");
            Process.Start(startInfo);

        }
    }
}
