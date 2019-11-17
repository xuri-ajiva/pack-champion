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
            build.ShowDialog();
            /*var o =new OpenFileDialog();

            string firstFileName = new   DirectoryInfo("C:\\Windows\\WinSxS").GetDirectories().Select(fi => fi.Name).FirstOrDefault(name => name .Contains("x86_msbuild") && name.Contains("4.0"));
            var msbuild = "C:\\Windows\\WinSxS\\" + firstFileName + "\\MSBuild.exe";
            o.FileName = msbuild;
            o.InitialDirectory = Path.GetDirectoryName(msbuild);
            Console.WriteLine(msbuild);
            if (!File.Exists(msbuild)) {
                MessageBox.Show("Pleas Locate MSBuild.exe\nHint:\nC:\\Windows\\WinSxS\\x86_msbuild_[***]4.0.15788.0_none_[***]\\MSBuild.exe\n" + firstFileName, "Locate MSBuild.exe", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                o.Title = "Pleas Locate MSBuild.exe";
                o.Filter = "Executable *.exe|*.exe|All Files *.*|*.*";
                o.InitialDirectory = "C:\\Windows\\WinSxS\\" + firstFileName;
                if (o.ShowDialog() != DialogResult.OK) return;
            }

            var arch = TMP+ "project";
            File.WriteAllBytes(arch + ".7z", Resource1._project);

            var s7Module = TMP;
            var s7Out = TMP + "o";

            var arges = SevenZip[1].Replace("%", arch).Replace("&",s7Out );
            var exe = SevenZip[2].Replace("$", s7Module);

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("cmd", "/c @echo off && echo !!!!!!!! start extract !!!!!!!! && timeout 2 && " + exe + " " + arges + " && echo !!!!!!!! Finished !!!!!!!! && pause");
            p.StartInfo.UseShellExecute = true;
            p.Start();
            p.WaitForExit();

            if (MessageBox.Show("You Can Custom The Assembly!", "Custom ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {

                Process px = new Process();
                px.StartInfo = new ProcessStartInfo(@"C:\Windows\notepad.exe", s7Out + "\\AssemblyInfo.cs");
                px.StartInfo.UseShellExecute = false;
                px.Start();
                px.WaitForExit();
            }

            if (MessageBox.Show("You Can Custom The Icon!", "Custom ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                var of = new OpenFileDialog();
                of.Filter = "Icon *.ico|*.ico";
                if (of.ShowDialog() == DialogResult.Yes) {
                    File.Copy(of.FileName, s7Out + "\\5.ico", true);
                    MessageBox.Show("Copyed Icon");
                }
            }

            Process pxs = new Process();
            var grgeses =  "/c @echo off && echo !!!!!!!! start  build !!!!!!!! && timeout 2 && "+ o.FileName+ " "+ s7Out + "\\unPack.csproj" + " && echo !!!!!!!! Finished !!!!!!!! && pause";

            //Console.WriteLine(grgeses);
            pxs.StartInfo = new ProcessStartInfo("cmd", grgeses);
            pxs.StartInfo.UseShellExecute = false;
            pxs.Start();
            pxs.WaitForExit();

            if (MessageBox.Show("Surcsess ?", "Surcsess", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                custom = s7Out + "\\bin\\Debug\\assemblyname.exe";
                MessageBox.Show("Custom Exe:\n" + custom);
            }*/
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
        internal static Form build;
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
