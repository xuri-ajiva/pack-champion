using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using unPack2;

namespace toolkit {
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

        public static readonly string TMP = Path.GetTempPath() + "#TOOLBOX#\\";

        [STAThread]
        static void Main() {
            ShowWindow(handle, SW_HIDE);
            Directory.CreateDirectory(TMP);
            File.WriteAllBytes(TMP + "7z.exe", Resource1._7z_exe);
            File.WriteAllBytes(TMP + "7z.dll", Resource1._7z_dll);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Build());

        }
        public static FileStream ne_file;
        public static Encoding encoding => Encoding.UTF8;

        public static string EXE;

        public static string[] SevenZip = new string[] { "-bd -bb2 a \"%\" \"&\"", "-y -bd -bb2 e \"%.7z\" -o\"&\"", "\"$\\7z.exe\"" };
        internal static Build build;

        public static void buildResource1(string path, Item[] items,string[] exe) {
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
