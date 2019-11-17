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


namespace toolkit {
    public partial class Build : Form {
        const string ERROR = "## ERROR ##";
        string MSBUILD = ERROR;
        string PROJECT = ERROR;
        string ASSEMBLY = ERROR;
        public Build() {
            InitializeComponent();
            LOCATEMSBUILD();
            EXTRACTPROJECT();
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
            PROJECT = Program.TMP + "o";

            var arges = Program.SevenZip[1].Replace("%", arch).Replace("&",PROJECT );
            var exe = Program.SevenZip[2].Replace("$", s7Module);

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("cmd", "/c @echo off && echo !!!!!!!! start extract !!!!!!!! && " + exe + " " + arges + " && echo !!!!!!!! Finished !!!!!!!!");
            p.StartInfo.UseShellExecute = false;
            p.Start();
            p.WaitForExit();

            ASSEMBLY = PROJECT + "\\AssemblyInfo.cs";
            setTextBoxes();
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
            p.WaitForExit();
        }

        private void button4_Click(object sender, EventArgs e) {
            Process px = new Process();
            px.StartInfo = new ProcessStartInfo(@"C:\Windows\notepad.exe", ASSEMBLY);
            px.StartInfo.UseShellExecute = false;
            px.Start();
            px.WaitForExit();
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
            Program.ShowWindow(Program.handle, Program.SW_SHOW);
            Process pxs = new Process();
            var grgeses =  "/c @echo off && echo !!!!!!!! start  build !!!!!!!! && timeout 3 && "+ MSBUILD + " "+ PROJECT + "\\unPack.csproj" + " && echo !!!!!!!! Finished !!!!!!!!";

            //Console.WriteLine(grgeses);
            pxs.StartInfo = new ProcessStartInfo("cmd", grgeses);
            pxs.StartInfo.UseShellExecute = false;
            pxs.Start();
            pxs.WaitForExit();

            if (pxs.ExitCode == 0) {

                Program.custom = PROJECT + "\\bin\\Debug\\assemblyname.exe";
                MessageBox.Show("New Exe:\n" + Program.custom);
                Program.ShowWindow(Program.handle, Program.SW_HIDE);
                this.Close();
            } else {
                MessageBox.Show("MSBUILD FAIL\nSEE CONSOLE FOR MORE INFORMATIONS!");
            }
        }

    }
}
