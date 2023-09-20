namespace GS25
{
    partial class Start
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_continue = new MetroFramework.Controls.MetroButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_new = new MetroFramework.Controls.MetroButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.radio_lite = new MetroFramework.Controls.MetroRadioButton();
            this.radio_sql = new MetroFramework.Controls.MetroRadioButton();
            this.SuspendLayout();
            // 
            // btn_continue
            // 
            this.btn_continue.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btn_continue.Highlight = true;
            this.btn_continue.Location = new System.Drawing.Point(313, 273);
            this.btn_continue.Name = "btn_continue";
            this.btn_continue.Size = new System.Drawing.Size(151, 52);
            this.btn_continue.Style = MetroFramework.MetroColorStyle.Blue;
            this.btn_continue.TabIndex = 0;
            this.btn_continue.Text = "이어하기";
            this.btn_continue.UseCustomBackColor = true;
            this.btn_continue.UseCustomForeColor = true;
            this.btn_continue.UseSelectable = true;
            this.btn_continue.UseStyleColors = true;
            this.btn_continue.Click += new System.EventHandler(this.btn_continue_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nirmala UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label1.Location = new System.Drawing.Point(187, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 65);
            this.label1.TabIndex = 2;
            this.label1.Text = "MS";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.LightSkyBlue;
            this.label2.Location = new System.Drawing.Point(278, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 65);
            this.label2.TabIndex = 3;
            this.label2.Text = "25";
            // 
            // btn_new
            // 
            this.btn_new.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btn_new.Highlight = true;
            this.btn_new.Location = new System.Drawing.Point(92, 273);
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(151, 52);
            this.btn_new.Style = MetroFramework.MetroColorStyle.Blue;
            this.btn_new.TabIndex = 4;
            this.btn_new.Text = "새로하기";
            this.btn_new.UseCustomBackColor = true;
            this.btn_new.UseCustomForeColor = true;
            this.btn_new.UseSelectable = true;
            this.btn_new.UseStyleColors = true;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panel1.Location = new System.Drawing.Point(24, 102);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(157, 15);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel2.Location = new System.Drawing.Point(24, 123);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(157, 15);
            this.panel2.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panel3.Location = new System.Drawing.Point(24, 144);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(157, 15);
            this.panel3.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panel4.Location = new System.Drawing.Point(357, 144);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(157, 15);
            this.panel4.TabIndex = 9;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel5.Location = new System.Drawing.Point(357, 123);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(157, 15);
            this.panel5.TabIndex = 8;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panel6.Location = new System.Drawing.Point(357, 102);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(157, 15);
            this.panel6.TabIndex = 7;
            // 
            // radio_lite
            // 
            this.radio_lite.AutoSize = true;
            this.radio_lite.Location = new System.Drawing.Point(134, 231);
            this.radio_lite.Name = "radio_lite";
            this.radio_lite.Size = new System.Drawing.Size(59, 15);
            this.radio_lite.TabIndex = 10;
            this.radio_lite.Text = "SQLITE";
            this.radio_lite.UseSelectable = true;
            // 
            // radio_sql
            // 
            this.radio_sql.AutoSize = true;
            this.radio_sql.Location = new System.Drawing.Point(134, 252);
            this.radio_sql.Name = "radio_sql";
            this.radio_sql.Size = new System.Drawing.Size(62, 15);
            this.radio_sql.TabIndex = 11;
            this.radio_sql.Text = "MYSQL";
            this.radio_sql.UseSelectable = true;
            // 
            // Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 409);
            this.Controls.Add(this.radio_sql);
            this.Controls.Add(this.radio_lite);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_new);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_continue);
            this.Name = "Start";
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Theme = MetroFramework.MetroThemeStyle.Default;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroButton btn_continue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private MetroFramework.Controls.MetroButton btn_new;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private MetroFramework.Controls.MetroRadioButton radio_lite;
        private MetroFramework.Controls.MetroRadioButton radio_sql;
    }
}

