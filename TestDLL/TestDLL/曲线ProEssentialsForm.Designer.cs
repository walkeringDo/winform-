namespace TestDLL
{
    partial class 曲线ProEssentialsForm
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(曲线ProEssentialsForm));
            this.axPego1 = new AxPego32eLib.AxPego();
            ((System.ComponentModel.ISupportInitialize)(this.axPego1)).BeginInit();
            this.SuspendLayout();
            // 
            // axPego1
            // 
            this.axPego1.Location = new System.Drawing.Point(0, 0);
            this.axPego1.Name = "axPego1";
            this.axPego1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPego1.OcxState")));
            this.axPego1.Size = new System.Drawing.Size(200, 200);
            this.axPego1.TabIndex = 0;
            // 
            // 曲线ProEssentialsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.axPego1);
            this.Name = "曲线ProEssentialsForm";
            this.Text = "曲线ProEssentialsForm";
            ((System.ComponentModel.ISupportInitialize)(this.axPego1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        //private AxPego32eLib.AxPego axPego1;
    }
}