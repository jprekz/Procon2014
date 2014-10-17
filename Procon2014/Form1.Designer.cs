namespace Procon2014
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonStart = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.submit1 = new System.Windows.Forms.Button();
            this.submit2 = new System.Windows.Forms.Button();
            this.serverReturn = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.stop1 = new System.Windows.Forms.Button();
            this.stop2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1.Location = new System.Drawing.Point(3, 32);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(386, 490);
            this.textBox1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonStart, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.serverReturn, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 562);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(3, 3);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "解析開始";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textBox2
            // 
            this.textBox2.AcceptsReturn = true;
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2.Location = new System.Drawing.Point(395, 32);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(386, 490);
            this.textBox2.TabIndex = 2;
            // 
            // submit1
            // 
            this.submit1.AutoSize = true;
            this.submit1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.submit1.Enabled = false;
            this.submit1.Location = new System.Drawing.Point(3, 3);
            this.submit1.Name = "submit1";
            this.submit1.Size = new System.Drawing.Size(88, 25);
            this.submit1.TabIndex = 3;
            this.submit1.Text = "submit1(F1)";
            this.submit1.UseVisualStyleBackColor = true;
            this.submit1.Click += new System.EventHandler(this.submit1_Click);
            // 
            // submit2
            // 
            this.submit2.AutoSize = true;
            this.submit2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.submit2.Enabled = false;
            this.submit2.Location = new System.Drawing.Point(3, 3);
            this.submit2.Name = "submit2";
            this.submit2.Size = new System.Drawing.Size(88, 25);
            this.submit2.TabIndex = 4;
            this.submit2.Text = "submit2(F3)";
            this.submit2.UseVisualStyleBackColor = true;
            this.submit2.Click += new System.EventHandler(this.submit2_Click);
            // 
            // serverReturn
            // 
            this.serverReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverReturn.Location = new System.Drawing.Point(395, 3);
            this.serverReturn.Multiline = true;
            this.serverReturn.Name = "serverReturn";
            this.serverReturn.Size = new System.Drawing.Size(386, 23);
            this.serverReturn.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.submit1);
            this.flowLayoutPanel1.Controls.Add(this.stop1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 528);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(386, 31);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.submit2);
            this.flowLayoutPanel2.Controls.Add(this.stop2);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(395, 528);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(386, 31);
            this.flowLayoutPanel2.TabIndex = 7;
            // 
            // stop1
            // 
            this.stop1.AutoSize = true;
            this.stop1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.stop1.Enabled = false;
            this.stop1.Location = new System.Drawing.Point(97, 3);
            this.stop1.Name = "stop1";
            this.stop1.Size = new System.Drawing.Size(73, 25);
            this.stop1.TabIndex = 4;
            this.stop1.Text = "stop1(F2)";
            this.stop1.UseVisualStyleBackColor = true;
            this.stop1.Click += new System.EventHandler(this.stop1_Click);
            // 
            // stop2
            // 
            this.stop2.AutoSize = true;
            this.stop2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.stop2.Enabled = false;
            this.stop2.Location = new System.Drawing.Point(97, 3);
            this.stop2.Name = "stop2";
            this.stop2.Size = new System.Drawing.Size(73, 25);
            this.stop2.TabIndex = 5;
            this.stop2.Text = "stop2(F4)";
            this.stop2.UseVisualStyleBackColor = true;
            this.stop2.Click += new System.EventHandler(this.stop2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Procon2014";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button submit1;
        private System.Windows.Forms.Button submit2;
        private System.Windows.Forms.TextBox serverReturn;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button stop1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button stop2;
    }
}

