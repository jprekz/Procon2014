namespace ProconSortUI
{
    partial class InputDialog
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
            this.submit = new System.Windows.Forms.Button();
            this.problemnum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // submit
            // 
            this.submit.Location = new System.Drawing.Point(120, 216);
            this.submit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.submit.Name = "submit";
            this.submit.Size = new System.Drawing.Size(87, 29);
            this.submit.TabIndex = 1;
            this.submit.Text = "確定";
            this.submit.UseVisualStyleBackColor = true;
            this.submit.Click += new System.EventHandler(this.submit_Click);
            // 
            // problemnum
            // 
            this.problemnum.Location = new System.Drawing.Point(105, 164);
            this.problemnum.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.problemnum.Name = "problemnum";
            this.problemnum.Size = new System.Drawing.Size(116, 19);
            this.problemnum.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(85, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "問題番号を入力して下さい";
            // 
            // InputDialog
            // 
            this.AcceptButton = this.submit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 328);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.problemnum);
            this.Controls.Add(this.submit);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "InputDialog";
            this.Text = "inputDialog";
            this.Load += new System.EventHandler(this.InputDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button submit;
        private System.Windows.Forms.TextBox problemnum;
        private System.Windows.Forms.Label label1;
    }
}