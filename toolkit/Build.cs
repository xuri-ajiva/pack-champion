using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace toolkit {
    public partial class Build : Form {
        const string ERROR = "## ERROR ##";
        string MSBUILD = ERROR;
        string PROJECT = ERROR;
        string ASSEMBLY = ERROR;
        public Build() {
            InitializeComponent();
        }

        private void LOCATEMSBUILD() {
            string firstFileName = new   DirectoryInfo("C:\\Windows\\WinSxS").GetDirectories().Select(fi => fi.Name).FirstOrDefault(name => name .Contains("x86_msbuild") && name.Contains("4.0"));
            if (!string.IsNullOrEmpty(firstFileName))
                MSBUILD = "C:\\Windows\\WinSxS\\" + firstFileName + "\\MSBuild.exe";
            else MSBUILD = ERROR;
        }

        private void EXTRACTPROJECT() {

            var arch = Program.TMP+ "project";
            File.WriteAllBytes(arch + ".7z", Resource1._project);

            var s7Module = Program.TMP;
            PROJECT = Program.TMP + Path.GetRandomFileName();

            var arges = Program.SevenZip[1].Replace("%", arch).Replace("&",PROJECT );
            var exe = Program.SevenZip[2].Replace("$", s7Module);

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("cmd", "/c @echo off && echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! start extract !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! && " + exe + " " + arges + " && echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Finished !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);

            ASSEMBLY = PROJECT + "\\AssemblyInfo.cs";
            setTextBoxes();

            OPTIONS.Enabled = false;
            new Thread(() => {
                p.Start();
                p.BeginOutputReadLine();
                while (!p.HasExited) {
                    Application.DoEvents(); // This keeps your form responsive by processing events
                }
                this.BeginInvoke(new MethodInvoker(() => {
                    OPTIONS.Enabled = true;
                }));
            }).Start();
        }
        void setTextBoxes() {
            textBox3.Text = PROJECT + "\\unPack.csproj";
            textBox2.Text = PROJECT;
            textBox1.Text = MSBUILD;
            textBox4.Text = ASSEMBLY;
            textBox5.Text = PROJECT + "\\5.ico";
        }

        private void button1_Click(object sender, EventArgs e) {
            var o =new OpenFileDialog();
            MessageBox.Show("Pleas Locate MSBuild.exe\nHint:\nC:\\Windows\\WinSxS\\x86_msbuild_[***]4.0.15788.0_none_[***]\\MSBuild.exe\n" + MSBUILD, "Locate MSBuild.exe", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            o.Title = "Pleas Locate MSBuild.exe";
            o.Filter = "Executable *.exe|*.exe|All Files *.*|*.*";
            o.FileName = MSBUILD;
            o.InitialDirectory = Path.GetDirectoryName(MSBUILD);
            if (o.ShowDialog() != DialogResult.OK) return;

        }

        private void button3_Click(object sender, EventArgs e) {
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo = new ProcessStartInfo("explorer", PROJECT);
            p.StartInfo.UseShellExecute = true;
            p.Start();
            while (!p.HasExited) {
                Application.DoEvents(); // This keeps your form responsive by processing events
            }
        }

        private void button4_Click(object sender, EventArgs e) {
            Process px = new Process();
            px.StartInfo = new ProcessStartInfo(@"C:\Windows\notepad.exe", ASSEMBLY);
            px.StartInfo.UseShellExecute = false;
            px.Start();
            while (!px.HasExited) {
                Application.DoEvents(); // This keeps your form responsive by processing events
            }
        }

        private void button5_Click(object sender, EventArgs e) {
            var of = new OpenFileDialog();
            of.Filter = "Icon *.ico|*.ico";
            if (of.ShowDialog() == DialogResult.Yes) {
                File.Copy(of.FileName, PROJECT + "\\5.ico", true);
                MessageBox.Show("Copyed Icon!");
            }

        }

        private void button2_Click(object sender, EventArgs e) {

        }

        private void button6_Click(object sender, EventArgs e) {
            Process pxs = new Process();
            var grgeses =  "/c @echo off && echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! start  build !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! && "+ MSBUILD + " "+ PROJECT + "\\unPack.csproj" + " && echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Finished !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!";

            //Console.WriteLine(grgeses);
            pxs.StartInfo = new ProcessStartInfo("cmd", grgeses);
            pxs.StartInfo.UseShellExecute = false;
            pxs.StartInfo.RedirectStandardOutput = true;
            pxs.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);

            pxs.Start();
            pxs.BeginOutputReadLine();
            while (!pxs.HasExited) {
                Application.DoEvents(); // This keeps your form responsive by processing events
            }

            if (pxs.ExitCode == 0) {
                File.Copy(PROJECT + "\\bin\\Debug\\assemblyname.exe", Program.TMP + "\\up.exe", true);
                Program.custom = Program.TMP + "up.exe";

                MessageBox.Show("New Exe:\n" + Program.custom);
                Program.ShowWindow(Program.handle, Program.SW_HIDE);
                if (exitonbuild.Checked) {
                    try {
                        Directory.Delete(PROJECT, true);
                    } catch (Exception esx) {
                        MessageBox.Show(esx.Message);
                    }
                }
                this.Close();
            } else {
                MessageBox.Show("MSBUILD FAIL\nSEE CONSOLE FOR MORE INFORMATIONS!");
                Program.ShowWindow(Program.handle, Program.SW_SHOW);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            var f = PROJECT + "\\"+ (checkBox1.Checked ? "a" : "n") + ".manifest";
            File.Copy(f, PROJECT + "\\m.manifest", true);
            MessageBox.Show("Copyed " + (checkBox1.Checked ? "admin" : "normal") + " manifest");
        }

        private void button7_Click(object sender, EventArgs e) {
            Process pxs = new Process();
            var grgeses =  PROJECT + "\\unPack.csproj";
            pxs.StartInfo = new ProcessStartInfo(grgeses);
            pxs.StartInfo.UseShellExecute = true;
            pxs.Start();
        }

        void SortOutputHandler(object sender, DataReceivedEventArgs e) {
            if (string.IsNullOrEmpty(e.Data)) return;
            Trace.WriteLine(e.Data);
            this.BeginInvoke(new MethodInvoker(() => {
                var se = e.Data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var s in se) {
                    richTextBox1.AppendText((s + "\n") ?? string.Empty);
                }
            }));
        }

        private void Build_Load(object sender, EventArgs e) {
            LOCATEMSBUILD();
            EXTRACTPROJECT();
        }
    }
}
