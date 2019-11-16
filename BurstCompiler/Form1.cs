using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BurstCompiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void build_Click(object sender, EventArgs e)
        {
            var sf = new SaveFileDialog();
            sf.Filter = "Code Files*.cs|*.cs|All Files|*.*";
            if (sf.ShowDialog() != DialogResult.OK) return;

            var fs = File.CreateText(sf.FileName);
            var _c = new List<string>();

            fs.Write(Program.begin);
            foreach (var i in files.CheckedItems)
            {
                _c.Add((string)i);
            }
            foreach (var i in files.Items)
            {
                fs.Write(Program.mid.Replace("§", Path.GetFileName((string)i)).Replace("$", Convert.ToBase64String(File.ReadAllBytes((string)i))).Replace("%", _c.Contains((string)i) ? "true" : "false"));
            }
            fs.Write(Program.end);
            fs.Flush();
            fs.Close();
        }

        private void panic_Click(object sender, EventArgs e)
        {

        }

        private void wipe_Click(object sender, EventArgs e)
        {

        }

        private void clear_Click(object sender, EventArgs e)
        {
            files.Items.Clear();
        }

        private void compile_Click(object sender, EventArgs e)
        {
            var sf = new SaveFileDialog();
            var of = new OpenFileDialog(); 
            sf.Filter = "All Files|*.*|Executable|*.exe";
            of.Filter = "Code Files*.cs|*.cs|All Files|*.*";

            if (of.ShowDialog() != DialogResult.OK) return;
            if (sf.ShowDialog() != DialogResult.OK) return;



        }

        private void files_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);


            foreach (string File in FileList)
                files.Items.Add(File);
        }

        private void files_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        object sel;

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            files.Items.Remove(sel);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var o = new OpenFileDialog();
            o.Multiselect = true;
            if (o.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    files.Items.AddRange(o.FileNames);
                }
                catch { }
            }
        }

        private void files_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button != MouseButtons.Right) return;
            sel = files.SelectedItem;
            menueip1.Show((Control)sender, e.Location);
        }


    }
    public struct exec
    {
        public exec(string _filename, string _base64Bynary, bool _execute, string _args = "")
        {
            this.filename = _filename;
            this.base64Bynary = _base64Bynary;
            execute = _execute;
            args = _args;
        }
        public string filename;
        public string base64Bynary;
        public bool execute;
        public string args;
    }
}
