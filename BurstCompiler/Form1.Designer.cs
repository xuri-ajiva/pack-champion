namespace BurstCompiler
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.wipe = new System.Windows.Forms.Button();
            this.panic = new System.Windows.Forms.Button();
            this.compile = new System.Windows.Forms.Button();
            this.build = new System.Windows.Forms.Button();
            this.files = new System.Windows.Forms.CheckedListBox();
            this.clear = new System.Windows.Forms.Button();
            this.menueip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menueip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // wipe
            // 
            this.wipe.Location = new System.Drawing.Point(162, 472);
            this.wipe.Name = "wipe";
            this.wipe.Size = new System.Drawing.Size(96, 23);
            this.wipe.TabIndex = 10;
            this.wipe.Text = "Wipe files in List";
            this.wipe.UseVisualStyleBackColor = true;
            this.wipe.Click += new System.EventHandler(this.wipe_Click);
            // 
            // panic
            // 
            this.panic.Location = new System.Drawing.Point(12, 472);
            this.panic.Name = "panic";
            this.panic.Size = new System.Drawing.Size(144, 23);
            this.panic.TabIndex = 9;
            this.panic.Text = "Clear Temp Files And Exit";
            this.panic.UseVisualStyleBackColor = true;
            this.panic.Click += new System.EventHandler(this.panic_Click);
            // 
            // compile
            // 
            this.compile.Location = new System.Drawing.Point(400, 472);
            this.compile.Name = "compile";
            this.compile.Size = new System.Drawing.Size(75, 23);
            this.compile.TabIndex = 8;
            this.compile.Text = "Compile";
            this.compile.UseVisualStyleBackColor = true;
            this.compile.Click += new System.EventHandler(this.compile_Click);
            // 
            // build
            // 
            this.build.Location = new System.Drawing.Point(481, 472);
            this.build.Name = "build";
            this.build.Size = new System.Drawing.Size(75, 23);
            this.build.TabIndex = 7;
            this.build.Text = "Build .cs";
            this.build.UseVisualStyleBackColor = true;
            this.build.Click += new System.EventHandler(this.build_Click);
            // 
            // files
            // 
            this.files.AllowDrop = true;
            this.files.FormattingEnabled = true;
            this.files.Location = new System.Drawing.Point(12, 12);
            this.files.Name = "files";
            this.files.Size = new System.Drawing.Size(546, 454);
            this.files.TabIndex = 6;
            this.files.DragDrop += new System.Windows.Forms.DragEventHandler(this.files_DragDrop);
            this.files.DragEnter += new System.Windows.Forms.DragEventHandler(this.files_DragEnter);
            this.files.MouseDown += new System.Windows.Forms.MouseEventHandler(this.files_MouseDown);
            // 
            // clear
            // 
            this.clear.Location = new System.Drawing.Point(319, 472);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(75, 23);
            this.clear.TabIndex = 11;
            this.clear.Text = "Clear";
            this.clear.UseVisualStyleBackColor = true;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // menueip1
            // 
            this.menueip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem,
            this.openToolStripMenuItem});
            this.menueip1.Name = "contextMenuStrip1";
            this.menueip1.Size = new System.Drawing.Size(115, 48);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeToolStripMenuItem.Text = "remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 516);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.wipe);
            this.Controls.Add(this.panic);
            this.Controls.Add(this.compile);
            this.Controls.Add(this.build);
            this.Controls.Add(this.files);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menueip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button wipe;
        private System.Windows.Forms.Button panic;
        private System.Windows.Forms.Button compile;
        private System.Windows.Forms.Button build;
        private System.Windows.Forms.CheckedListBox files;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.ContextMenuStrip menueip1;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    }
}

