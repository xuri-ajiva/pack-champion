using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace TOOLBOX2 {
    static class Program {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        /// 

        public static IntPtr handle = GetConsoleWindow();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;

        public static string TMP { get; private set; }

        public const string copy = "-copy";
        public const string GLOBAL = @"C:\Program Files\Windows Mail\";

        [STAThread]
        static void Main(string[] args) {
            if (args.Length > 0) {
                if (args[0] == copy) {
                    if (args.Length > 1) {
                        var n =  String.Join(" ",args.Skip(1)) + Path.GetFileName(Application.ExecutablePath);
                        Console.WriteLine("new path: "+n);
                        File.Copy(Application.ExecutablePath,n, true);
                        MessageBox.Show("Migrated to: " + GLOBAL);
                        if (MessageBox.Show("Create StartMenüe","shortcut",MessageBoxButtons.YesNo) == DialogResult.Yes) {
                            CreateShortcut("TOOLBOX", Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)+ "\\Programs\\", n);
                        }
                        ExecuteAsAdmin(n,"");
                        Environment.Exit(0);
                    } else {
                        MessageBox.Show("ERROR");
                    }
                }
            }
            Console.WriteLine(Path.GetDirectoryName(Application.ExecutablePath) + "\\");
            Console.WriteLine(GLOBAL);
            if (Path.GetDirectoryName(Application.ExecutablePath)+ "\\" == GLOBAL) {
                using (WindowsIdentity identity = WindowsIdentity.GetCurrent()) {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    if (!principal.IsInRole(WindowsBuiltInRole.Administrator)) {
                        ExecuteAsAdmin(Application.ExecutablePath, "");
                    }
                }
                TMP = GLOBAL + "#TOOLBOX#\\";
            } else {
                if (MessageBox.Show("It is Recomendet to Execute this in a Folder that is on every System!\nBecause the compield exe has the absulut path insite means your user name!\nThis is your risk!\n\nDo you want co Copy to a windows folder that is on any system?", "Atention", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes) {
                    
                    ExecuteAsAdmin(Application.ExecutablePath, copy + " " + GLOBAL);
                    Environment.Exit(0);
                }
                TMP = Path.GetTempPath() + "#TOOLBOX#\\";
            }
            ShowWindow(handle, SW_HIDE);
            Directory.CreateDirectory(TMP);
            File.WriteAllBytes(TMP + "7z.exe", Resource1._7z_exe);
            File.WriteAllBytes(TMP + "7z.dll", Resource1._7z_dll);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            

            Application.Run(new Build());

        }
        public static Encoding encoding => Encoding.UTF8;

        public static string EXE;

        public static string[] SevenZip = new string[] { "-bd -bb2 a \"%\" \"&\"", "-y -bd -bb2 e \"%.7z\" -o\"&\"", "\"$\\7z.exe\"" };

        public static void buildResource1(string path, Item[] items, string[] exe) {
            File.Delete(path + "\\Program.cs");
            File.Delete(path + "\\Resource1.resx");
            File.Delete(path + "\\Resource1.Designer.cs");

            var p = File.Open(path + "\\Program.cs",FileMode.OpenOrCreate);
            var r = File.Open(path + "\\Resource1.resx",FileMode.OpenOrCreate);
            var c = File.Open(path + "\\Resource1.Designer.cs",FileMode.OpenOrCreate);
            write(p, strings.program_begin);
            write(r, strings.resource_begin);
            write(c, strings.resourcs_begin);
            foreach (var i in items) {
                write(p, strings.program_mid.Replace("&", i.Name).Replace("%", i.Path));
                write(r, strings.resource_mid.Replace("&", i.Name).Replace("%", i.Path));
                write(c, strings.resourcs_mid.Replace("&", i.Name));
            }
            foreach (var i in exe) {
                write(p, strings.program_exe.Replace("&", i));
            }

            write(p, strings.program_end);
            write(r, strings.resource_end);
            write(c, strings.resourcs_end);

            p.Close();
            r.Close();
            c.Close();
        }

        public static void write(FileStream f, string s) {
            var b = encoding.GetBytes(s);
            f.Write(b, 0, b.Length);
        }

        public static void ExecuteAsAdmin(string fileName, string args) {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.Arguments = args;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }

        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation) {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "My shortcut description";   // The description of the shortcut
            shortcut.IconLocation = targetFileLocation;           // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }
    }



    [Serializable]
    public class Item {
        public Item(string path, string name) {
            this.Path = path;
            this.Name = name;
        }
        public Item() {

        }

        public string Path { get; set; }
        public String Name { get; set; }
    }

}
