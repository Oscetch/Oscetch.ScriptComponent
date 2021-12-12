
namespace Osctech.ScriptToolExample.Controls
{
    partial class ScriptControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.codeTextBox = new System.Windows.Forms.RichTextBox();
            this.suggestionsMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // codeTextBox
            // 
            this.codeTextBox.AcceptsTab = true;
            this.codeTextBox.ContextMenuStrip = this.suggestionsMenuStrip;
            this.codeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeTextBox.Location = new System.Drawing.Point(0, 0);
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.Size = new System.Drawing.Size(611, 452);
            this.codeTextBox.TabIndex = 0;
            this.codeTextBox.Text = "";
            this.codeTextBox.TextChanged += new System.EventHandler(this.CodeTextBox_TextChanged);
            this.codeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CodeTextBox_KeyDown);
            // 
            // suggestionsMenuStrip
            // 
            this.suggestionsMenuStrip.Name = "suggestionsMenuStrip";
            this.suggestionsMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // ScriptControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.codeTextBox);
            this.Name = "ScriptControl";
            this.Size = new System.Drawing.Size(611, 452);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox codeTextBox;
        private System.Windows.Forms.ContextMenuStrip suggestionsMenuStrip;
    }
}
