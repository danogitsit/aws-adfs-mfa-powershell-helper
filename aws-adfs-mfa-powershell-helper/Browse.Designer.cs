namespace DO
{
    partial class Browser
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
        #region Windows Form Designer generated code

        private System.Windows.Forms.WebBrowser BrowserCtrl;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>


        private void InitializeComponent()
        {
            this.BrowserCtrl = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // BrowserCtrl
            // 
            this.BrowserCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowserCtrl.Location = new System.Drawing.Point(0, 0);
            this.BrowserCtrl.MinimumSize = new System.Drawing.Size(20, 20);
            this.BrowserCtrl.Name = "BrowserCtrl";
            this.BrowserCtrl.Size = new System.Drawing.Size(540, 750);
            this.BrowserCtrl.TabIndex = 0;
            this.BrowserCtrl.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.BrowserCtrl_DocumentCompleted);
            this.BrowserCtrl.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.BrowserCtrl_Navigating);
            // 
            // Browser
            // 
            this.ClientSize = new System.Drawing.Size(540, 750);
            this.Controls.Add(this.BrowserCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Browser";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Authenticate";
            this.TopMost = true;
            this.ResumeLayout(false);

        }



      

        #endregion
    }
}