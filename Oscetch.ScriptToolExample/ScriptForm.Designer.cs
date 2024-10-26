
namespace Oscetch.ScriptToolExample
{
    partial class ScriptForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.vadKallasDetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.directoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recursiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regularToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addReferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syntaxHighlightingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptControl1 = new Oscetch.ScriptToolExample.Controls.ScriptControl();
            this.setBuildOutputPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vadKallasDetToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(955, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // vadKallasDetToolStripMenuItem
            // 
            this.vadKallasDetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.compileToolStripMenuItem});
            this.vadKallasDetToolStripMenuItem.Name = "vadKallasDetToolStripMenuItem";
            this.vadKallasDetToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.vadKallasDetToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.LoadToolStripMenuItem_Click);
            // 
            // compileToolStripMenuItem
            // 
            this.compileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentCodeToolStripMenuItem,
            this.directoryToolStripMenuItem});
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            this.compileToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.compileToolStripMenuItem.Text = "Compile";
            // 
            // currentCodeToolStripMenuItem
            // 
            this.currentCodeToolStripMenuItem.Name = "currentCodeToolStripMenuItem";
            this.currentCodeToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.currentCodeToolStripMenuItem.Text = "Current code";
            this.currentCodeToolStripMenuItem.Click += new System.EventHandler(this.CurrentCodeToolStripMenuItem_Click);
            // 
            // directoryToolStripMenuItem
            // 
            this.directoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recursiveToolStripMenuItem,
            this.regularToolStripMenuItem});
            this.directoryToolStripMenuItem.Name = "directoryToolStripMenuItem";
            this.directoryToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.directoryToolStripMenuItem.Text = "Directory";
            // 
            // recursiveToolStripMenuItem
            // 
            this.recursiveToolStripMenuItem.Name = "recursiveToolStripMenuItem";
            this.recursiveToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.recursiveToolStripMenuItem.Text = "Recursive";
            this.recursiveToolStripMenuItem.Click += new System.EventHandler(this.RecursiveToolStripMenuItem_Click);
            // 
            // regularToolStripMenuItem
            // 
            this.regularToolStripMenuItem.Name = "regularToolStripMenuItem";
            this.regularToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.regularToolStripMenuItem.Text = "Regular";
            this.regularToolStripMenuItem.Click += new System.EventHandler(this.RegularToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addReferenceToolStripMenuItem,
            this.syntaxHighlightingToolStripMenuItem,
            this.setBuildOutputPathToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // addReferenceToolStripMenuItem
            // 
            this.addReferenceToolStripMenuItem.Name = "addReferenceToolStripMenuItem";
            this.addReferenceToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.addReferenceToolStripMenuItem.Text = "Add reference";
            this.addReferenceToolStripMenuItem.Click += new System.EventHandler(this.AddReferenceToolStripMenuItem_Click);
            // 
            // syntaxHighlightingToolStripMenuItem
            // 
            this.syntaxHighlightingToolStripMenuItem.Name = "syntaxHighlightingToolStripMenuItem";
            this.syntaxHighlightingToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.syntaxHighlightingToolStripMenuItem.Text = "Syntax Highlighting";
            this.syntaxHighlightingToolStripMenuItem.Click += new System.EventHandler(this.SyntaxHighlightingToolStripMenuItem_Click);
            // 
            // scriptControl1
            // 
            this.scriptControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptControl1.Location = new System.Drawing.Point(0, 24);
            this.scriptControl1.Name = "scriptControl1";
            this.scriptControl1.Size = new System.Drawing.Size(955, 478);
            this.scriptControl1.SpacesInTabs = 4;
            this.scriptControl1.TabIndex = 1;
            this.scriptControl1.UseTabs = false;
            // 
            // setBuildOutputPathToolStripMenuItem
            // 
            this.setBuildOutputPathToolStripMenuItem.Name = "setBuildOutputPathToolStripMenuItem";
            this.setBuildOutputPathToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.setBuildOutputPathToolStripMenuItem.Text = "Set Build Output Path";
            this.setBuildOutputPathToolStripMenuItem.Click += new System.EventHandler(this.SetBuildOutputPathToolStripMenuItem_Click);
            // 
            // ScriptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 502);
            this.Controls.Add(this.scriptControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ScriptForm";
            this.Text = "Script Form";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem vadKallasDetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addReferenceToolStripMenuItem;
        private Controls.ScriptControl scriptControl1;
        private System.Windows.Forms.ToolStripMenuItem currentCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem directoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recursiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regularToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syntaxHighlightingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setBuildOutputPathToolStripMenuItem;
    }
}

