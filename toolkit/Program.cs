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


        static readonly string IDENTY01 = string.Join("-", Enumerable.Range(0, 100).ToArray());
        static readonly string IDENTY02 = string.Join("#", Enumerable.Range(0, 100).ToArray());
        [STAThread]
        static void Main() {
            ShowWindow(handle, SW_HIDE);
            Directory.CreateDirectory(TMP);
            File.WriteAllBytes(TMP + "7z.exe", Resource1._7z_exe);
            File.WriteAllBytes(TMP + "7z.dll", Resource1._7z_dll);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
        public static FileStream ne_file;

        public static string custom = "";
        static byte[] unPack {
            get {
                if (string.IsNullOrEmpty(custom)) {
                    return Resource1.unPack;
                }

                if (!File.Exists(custom)) {
                    MessageBox.Show("Custom execFile Dose Not Exists!");
                    return Resource1.unPack;
                }
                return File.ReadAllBytes(custom);
            }
        }

        internal static void BUILDEXE() {
            if (MessageBox.Show("Build Custom Assembly?", "Custom?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            build = new Build();
            build.exitonbuild.Checked = true;

            build.ShowDialog();
        }

        internal static string pack(List<Item> _ls, string arch) {
            try {
                WRITE(ne_file, unPack);
                WRITE(ne_file, encoding.GetBytes(IDENTY01));

                Console.WriteLine("Copied DePacker");

                string name = Path.GetTempPath() + new Random().Next(0, 100).ToString();
                var fs = File.Open(name, FileMode.OpenOrCreate);

                var serializer = new XmlSerializer(typeof(List<Item>));
                serializer.Serialize(fs, _ls);
                fs.Flush();
                fs.Close();
                Thread.Sleep(100);
                fs = File.OpenRead(name);
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int) fs.Length);

                var xml_64 = Convert.ToBase64String(buffer);

                Console.WriteLine("Informations Generated");

                WRITE(ne_file, encoding.GetBytes(xml_64));

                WRITE(ne_file, encoding.GetBytes(IDENTY02));

                Console.WriteLine("Informations Copied");
                var a = File.OpenRead(arch);
                WRITE(ne_file, READ(a, (int) a.Length));
                Console.WriteLine("Data Writed");
                try {
                    a.Close();
                    ne_file.Close();
                    a.Dispose();
                    ne_file.Dispose();
                } catch { }
                Console.WriteLine("Files Colsed");
                File.Delete(arch);
                Console.WriteLine("TempFiles Deleated");
            } catch (Exception e) {
                return e.Message;
            }
            return "Erfolgreich erstellt!\n";
        }

        private static void ReOpen() {
            var pos = ne_file.Position;
            var name = ne_file.Name;
            ne_file.Close();
            ne_file = File.Open(name, FileMode.Append);
            if (ne_file.Position != pos) {
                ne_file.Position = pos;
            }
        }

        public static Encoding encoding => Encoding.Unicode;

        static void WRITE(FileStream fs, byte[] buffer) {
            fs.Write(buffer, 0, (int) buffer.Length);
        }

        static byte[] READ(FileStream fs, int count) {
            byte[] buffer = new byte[count];
            var b = fs.Read(buffer, 0, count);

            var tmp = new byte[b];
            Array.Copy(buffer, 0, tmp, 0, b);
            return tmp;
        }
        public static string[] SevenZip = new string[] { "-bd -bb2 a \"%\" \"&\"", "-y -bd -bb2 e \"%.7z\" -o\"&\"", "\"$\\7z.exe\"" };
        internal static Build build;
    }



    [Serializable]
    public class Item {
        public Item(Int32 id, string Name) {
            this.Id = id;
            this.Name = Name;
        }
        public Item() {

        }

        public Int32 Id { get; set; }
        public String Name { get; set; }
    }

}
