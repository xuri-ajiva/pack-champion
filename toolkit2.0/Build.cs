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

namespace TOOLBOX2 {
    public partial class Build : Form {
        const string ERROR = "## ERROR ##";
        string MSBUILD = ERROR;
        string PROJECT = ERROR;
        string ASSEMBLY = ERROR;

        public Build() {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) => COPYMANIFEST();
        private void button12Click(object sender, EventArgs e) => CLEARTEXTBOXESTEXT();
        private void button10Click(object sender, EventArgs e) => CLEARCHECKBOXLIST();
        private void button3_Click(object sender, EventArgs e) => BROUSEPROJEKT();
        private void button4_Click(object sender, EventArgs e) => EDITASSEMBLY();
        private void button1_Click(object sender, EventArgs e) => FINDMSBUILD();
        private void button13Click(object sender, EventArgs e) => NEWPROJECT();
        private void button5_Click(object sender, EventArgs e) => OPENICON();
        private void button0_Click(object sender, EventArgs e) => PREPACK();
        private void button2_Click(object sender, EventArgs e) => STARTVS();
        private void button7_Click(object sender, EventArgs e) => STARTVS();
        private void button9_Click(object sender, EventArgs e) => PANIC();
        private void button6_Click(object sender, EventArgs e) => BUILD();
        private void button8_Click(object sender, EventArgs e) => WIPE();

        #region menü

        object sel;
        private void clearListToolStripMenuItem_Click(object sender, EventArgs e) {
            checkedListBox1.Items.Clear();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e) {
            checkedListBox1.Items.Remove(sel);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            var o = new OpenFileDialog();
            o.Multiselect = true;
            if (o.ShowDialog() == DialogResult.OK) {
                try {
                    checkedListBox1.Items.AddRange(o.FileNames);
                } catch { }
            }
        }

        private void checkedListBox1_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right) return;
            sel = checkedListBox1.SelectedItem;
            contextMenuStrip1.Show((Control) sender, e.Location);
        }
        #endregion

        #region dragEnter

        private void checkedListBox1_DragDrop(object sender, DragEventArgs e) {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);


            foreach (string File in FileList)
                checkedListBox1.Items.Add(File);
        }

        private void checkedListBox1_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        #endregion

        void SortOutputHandler(object sender, DataReceivedEventArgs e) {
            if (string.IsNullOrEmpty(e.Data)) return;
            Trace.WriteLine(e.Data);
            this.BeginInvoke(new MethodInvoker(() => {
                var se = e.Data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var s in se) {
                    richTextBox1.AppendText((s + "\n") ?? string.Empty);
                }
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }));
        }
        private void Build_Load(object sender, EventArgs e) {
            LOCATEMSBUILD();
            EXTRACTPROJECT();
        }
        void setTextBoxes() {
            textBox3.Text = PROJECT + "\\unPack2.csproj";
            textBox2.Text = PROJECT;
            textBox1.Text = MSBUILD;
            textBox4.Text = ASSEMBLY;
            textBox5.Text = PROJECT + "\\5.ico";
        }

        void WIPE() {
            if (MessageBox.Show("This Will delete All files in The Checkbox List for Ever && " + Program.TMP, "Warning!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            File.WriteAllBytes(Path.GetTempPath() + "sdel.exe", Resource1.sdelete_exe);

            var execbase = " \"" + Path.GetTempPath() + "sdel.exe\" -p 5 ";
            var exec = "";
            foreach (var item in checkedListBox1.Items) {
                var s = (string)item;
                exec += execbase + "\"" + s + "\" && ";
            }
            var p = new Process();
            p.StartInfo = new ProcessStartInfo("cmd", "/k \"" + exec + " pause \"");
            p.StartInfo.RedirectStandardError = true;
            p.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);

            p.Start();
            p.BeginOutputReadLine();
        }
        void PANIC() {
            string Bash = "@echo off && echo Deleating Files! && timeout 5 && " + Path.GetTempPath() + "sdel.exe -p 8 -r -s \"" + Program.TMP + "\" && " + Path.GetTempPath() + "sdel.exe -p 8 -r \"" + Application.ExecutablePath + "\" && del \"" + Path.GetTempPath() + "sdel.exe\" && pause";

            if (MessageBox.Show("This Will delete The executable && " + Program.TMP, "Warning!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            File.WriteAllBytes(Path.GetTempPath() + "sdel.exe", Resource1.sdelete_exe);
            Process.Start("cmd", "/k \"" + Bash + "\"");
            Environment.Exit(0);
        }
        void BUILD() {
            PREPACK();
            Process pxs = new Process();
            var grgeses =  "/c @echo off && echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! start  build !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! && "+ MSBUILD + " \""+ PROJECT + "\\unPack2.csproj\"" + " && echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Finished !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!";

            //Console.WriteLine(grgeses);
            pxs.StartInfo = new ProcessStartInfo("cmd", grgeses);
            pxs.StartInfo.UseShellExecute = false;
            pxs.StartInfo.CreateNoWindow = true;
            pxs.StartInfo.RedirectStandardOutput = true;
            pxs.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);

            pxs.Start();
            pxs.BeginOutputReadLine();
            while (!pxs.HasExited) {
                Application.DoEvents(); // This keeps your form responsive by processing events
            }

            if (pxs.ExitCode == 0) {
                File.Copy(PROJECT + "\\bin\\Debug\\" + textBox6.Text, Program.TMP + "\\up.exe", true);
                Program.EXE = Program.TMP + "up.exe";

                Program.ShowWindow(Program.handle, Program.SW_HIDE);
                if (exitonbuild.Checked) {
                    try {
                        Directory.Delete(PROJECT, true);
                    } catch (Exception esx) {
                        MessageBox.Show(esx.Message);
                    }
                }

                var s = new SaveFileDialog();
                s.Filter = "Executable *.exe|*.exe|All Files *.*|*.*";
                if (s.ShowDialog() == DialogResult.OK) {
                    File.Copy(Program.EXE, s.FileName, true);
                } else {
                    richTextBox1.AppendText("BUILD canceled!");
                }
            } else {
                MessageBox.Show("MSBUILD FAIL\nSEE CONSOLE FOR MORE INFORMATIONS!");
                //Program.ShowWindow(Program.handle, Program.SW_SHOW);
            }
        }
        void STARTVS() {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(PROJECT + "\\unPack2.csproj");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);
        }
        void PREPACK() {
            var l = new List<Item>();
            foreach (var item in checkedListBox1.Items) {
                var i = (string) item;
                l.Add(new Item(Path.GetFileName(i), Path.GetFileName(i).Replace(".", "_").Replace("-", "_").Replace(" ", "")));
                File.Copy(i, PROJECT + "\\" + Path.GetFileName(i), true);
            }

            var l2 = new List<string>();
            foreach (var item in checkedListBox1.CheckedItems) {
                l2.Add(Path.GetFileName((string) item));
            }

            Program.buildResource1(PROJECT, l.ToArray(), l2.ToArray());
        }
        void OPENICON() {
            var of = new OpenFileDialog();
            of.Filter = "Icon *.ico|*.ico";
            if (of.ShowDialog() == DialogResult.OK) {
                File.Copy(of.FileName, PROJECT + "\\5.ico", true);
                MessageBox.Show("Copyed Icon!");
            }
        }
        void NEWPROJECT() {
            if (MessageBox.Show("Deleate old project?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                Directory.Delete(PROJECT, true);
            }
            EXTRACTPROJECT();
        }
        void FINDMSBUILD() {
            var o =new OpenFileDialog();
            MessageBox.Show("Pleas Locate MSBuild.exe\nHint:\nC:\\Windows\\WinSxS\\x86_msbuild_[***]4.0.15788.0_none_[***]\\MSBuild.exe\n" + MSBUILD, "Locate MSBuild.exe", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            o.Title = "Pleas Locate MSBuild.exe";
            o.Filter = "Executable *.exe|*.exe|All Files *.*|*.*";
            o.FileName = MSBUILD;
            o.InitialDirectory = Path.GetDirectoryName(MSBUILD);
            if (o.ShowDialog() != DialogResult.OK) return;
        }
        void COPYMANIFEST() {
            var f = PROJECT + "\\"+ (checkBox1.Checked ? "a" : "n") + ".manifest";
            File.Copy(f, PROJECT + "\\m.manifest", true);
            MessageBox.Show("Copyed " + (checkBox1.Checked ? "admin" : "normal") + " manifest");
        }
        void EDITASSEMBLY() {
            Process px = new Process();
            px.StartInfo = new ProcessStartInfo(@"C:\Windows\notepad.exe", ASSEMBLY);
            px.StartInfo.UseShellExecute = false;
            px.Start();
            while (!px.HasExited) {
                Application.DoEvents(); // This keeps your form responsive by processing events
            }
        }
        void BROUSEPROJEKT() {
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo = new ProcessStartInfo("explorer", PROJECT);
            p.StartInfo.UseShellExecute = true;
            p.Start();
            while (!p.HasExited) {
                Application.DoEvents(); // This keeps your form responsive by processing events
            }
        }
        void LOCATEMSBUILD() {
            string firstFileName = new   DirectoryInfo("C:\\Windows\\WinSxS").GetDirectories().Select(fi => fi.Name).FirstOrDefault(name => name .Contains("x86_msbuild") && name.Contains("4.0"));
            if (!string.IsNullOrEmpty(firstFileName))
                MSBUILD = "C:\\Windows\\WinSxS\\" + firstFileName + "\\MSBuild.exe";
            else MSBUILD = ERROR;
        }
        void EXTRACTPROJECT() {
            var arch = Program.TMP+ "project";
            File.WriteAllBytes(arch + ".7z", Resource1._project);

            var s7Module = Program.TMP;
            PROJECT = Program.TMP + "compilerTMP\\" + Path.GetRandomFileName();

            var arges = Program.SevenZip[1].Replace("%", arch).Replace("&",PROJECT );
            var exe = Program.SevenZip[2].Replace("$", s7Module);

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("cmd", "/c @echo off && echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! start extract !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! && " + exe + " " + arges + " && echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Finished !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
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
        void CLEARCHECKBOXLIST() => checkedListBox1.Items.Clear();
        private void CLEARTEXTBOXESTEXT() => richTextBox1.Text = "";

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            if (checkBox2.Checked)
                if (MessageBox.Show("Change output is usefull but you need to de this at your own!\n1. Chick EXPLORE\n2. open unPack2.csproj with your custom editor\n3. Chamge name to whatever you want (chek if filename is valed)\n4. Write The name in The Apearing Textbox !! wrong name -> Exception !!\n\nNote! this can crahsh this programm sow all you changes are gon (they are still in temp folder but with random name\nGood lock =]", "Your Choise", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
                    textBox6.Visible = true;
                } else {
                    checkBox2.Checked = false;
                    checkBox2.CheckState = CheckState.Unchecked;
                }
            else { textBox6.Visible = false; textBox6.Text = "assemblyname.exe"; }
        }
    }
}
