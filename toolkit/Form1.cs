using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace toolkit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void checkedListBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            string s = "";

            foreach (string File in FileList)
                checkedListBox1.Items.Add(File);
        }

        private void checkedListBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
        }

        string temp_arch;
        private void button1_Click(object sender, EventArgs e)
        {
            var of = new SaveFileDialog();
            of.Filter = "All Files|*.*|Executable|*.exe";
            if (of.ShowDialog() == DialogResult.OK)
            {
                temp_arch = of.FileName + ".7z";
                List<Item> _l = new List<Item>();
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    var row = (item as string);

                    _l.Add(new Item(1, Path.GetFileName(row)));
                }

                foreach (var item in checkedListBox1.Items)
                {
                    Process p = new Process();

                    var s7Module = Program.TMP;

                    var arges = Program.SevenZip[0].Replace("%", temp_arch).Replace("&", (string)item);
                    var exe = Program.SevenZip[2].Replace("$", s7Module);

                    Console.WriteLine("Creating archive...");

                    p.StartInfo = new ProcessStartInfo(exe, arges);
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.Start();

                    p.WaitForExit();
                }


                Program.ne_file = File.Open(of.FileName, FileMode.OpenOrCreate);
                MessageBox.Show(Program.pack(_l, temp_arch));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will delete The executable && " + Program.TMP, "Warning!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            File.WriteAllBytes(Path.GetTempPath() + "sdel.exe", Resource1.sdelete_exe);
            Process.Start("cmd", "/k \"" + Bash + "\"");
            Environment.Exit(0);
        }
        public static readonly string Bash = "@echo off && echo Deleating Files! && timeout 5 && " + Path.GetTempPath() + "sdel.exe -p 30 -r -s \"" + Program.TMP + "\" && " + Path.GetTempPath() + "sdel.exe -p 30 -r \"" + Application.ExecutablePath + "\" && del \"" + Path.GetTempPath() + "sdel.exe\" && pause";

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Will delete All files in The Checkbox List for Ever && " + Program.TMP, "Warning!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            File.WriteAllBytes(Path.GetTempPath() + "sdel.exe", Resource1.sdelete_exe);

            var execbase = " \"" + Path.GetTempPath() + "sdel.exe\" -p 5 ";
            var exec = "";
            foreach (var item in checkedListBox1.Items)
            {
                var s = (string)item;
                exec += execbase + "\"" + s + "\" && ";
            }

            Process.Start("cmd", "/k \"" + exec + " pause \"");
        }

        object sel;

        private void checkedListBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            sel = checkedListBox1.SelectedItem;
            contextMenuStrip1.Show((Control)sender, e.Location);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Remove(sel);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var o = new OpenFileDialog();
            o.Multiselect = true;
            if (o.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    checkedListBox1.Items.AddRange(o.FileNames);
                }
                catch { }
            }
        }
    }
}
