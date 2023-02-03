namespace LuYao.Toolkit.Tabs.Rdp.Controls
{
    partial class RdpSession
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ErrorTextBox = new System.Windows.Forms.TextBox();
            this.tConnect = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ErrorTextBox
            // 
            this.ErrorTextBox.AcceptsReturn = true;
            this.ErrorTextBox.AcceptsTab = true;
            this.ErrorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrorTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(227)))), ((int)(((byte)(227)))));
            this.ErrorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ErrorTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(0)))), ((int)(((byte)(51)))));
            this.ErrorTextBox.Location = new System.Drawing.Point(243, 243);
            this.ErrorTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.ErrorTextBox.Multiline = true;
            this.ErrorTextBox.Name = "ErrorTextBox";
            this.ErrorTextBox.ReadOnly = true;
            this.ErrorTextBox.Size = new System.Drawing.Size(403, 113);
            this.ErrorTextBox.TabIndex = 1;
            this.ErrorTextBox.Visible = false;
            // 
            // RdoSession
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ErrorTextBox);
            this.Name = "RdoSession";
            this.Size = new System.Drawing.Size(700, 400);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ErrorTextBox;
        private System.Windows.Forms.Timer tConnect;
    }
}
