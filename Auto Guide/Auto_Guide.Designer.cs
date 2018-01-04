namespace MissionPlanner.Auto_Guide
{
    partial class Auto_Guide
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Button_start = new MissionPlanner.Controls.MyButton();
            this.Armed_All = new MissionPlanner.Controls.MyButton();
            this.Takeoff_All = new MissionPlanner.Controls.MyButton();
            this.Reset = new MissionPlanner.Controls.MyButton();
            this.Connection_Select = new System.Windows.Forms.ComboBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.SetA = new MissionPlanner.Controls.MyButton();
            this.SetB = new MissionPlanner.Controls.MyButton();
            this.SetC = new MissionPlanner.Controls.MyButton();
            this.Armed_and_Takeoff_All = new MissionPlanner.Controls.MyButton();
            this.RTL_All = new MissionPlanner.Controls.MyButton();
            this.SetD = new MissionPlanner.Controls.MyButton();
            this.SetE = new MissionPlanner.Controls.MyButton();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // Button_start
            // 
            this.Button_start.Font = new System.Drawing.Font("Arial", 9F);
            this.Button_start.Location = new System.Drawing.Point(12, 70);
            this.Button_start.Name = "Button_start";
            this.Button_start.Size = new System.Drawing.Size(75, 23);
            this.Button_start.TabIndex = 0;
            this.Button_start.Text = "Start";
            this.Button_start.UseVisualStyleBackColor = true;
            this.Button_start.Click += new System.EventHandler(this.Button_start_Click);
            // 
            // Armed_All
            // 
            this.Armed_All.Font = new System.Drawing.Font("Arial", 9F);
            this.Armed_All.Location = new System.Drawing.Point(12, 12);
            this.Armed_All.Name = "Armed_All";
            this.Armed_All.Size = new System.Drawing.Size(75, 23);
            this.Armed_All.TabIndex = 1;
            this.Armed_All.Text = "Armed All";
            this.Armed_All.UseVisualStyleBackColor = true;
            this.Armed_All.Click += new System.EventHandler(this.Armed_All_Click);
            // 
            // Takeoff_All
            // 
            this.Takeoff_All.Font = new System.Drawing.Font("Arial", 9F);
            this.Takeoff_All.Location = new System.Drawing.Point(12, 41);
            this.Takeoff_All.Name = "Takeoff_All";
            this.Takeoff_All.Size = new System.Drawing.Size(75, 23);
            this.Takeoff_All.TabIndex = 2;
            this.Takeoff_All.Text = "Takeoff All";
            this.Takeoff_All.UseVisualStyleBackColor = true;
            this.Takeoff_All.Click += new System.EventHandler(this.Takeoff_All_Click);
            // 
            // Reset
            // 
            this.Reset.Font = new System.Drawing.Font("Arial", 9F);
            this.Reset.Location = new System.Drawing.Point(12, 99);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(75, 23);
            this.Reset.TabIndex = 3;
            this.Reset.Text = "Reset";
            this.Reset.UseVisualStyleBackColor = true;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // Connection_Select
            // 
            this.Connection_Select.DataSource = this.bindingSource1;
            this.Connection_Select.FormattingEnabled = true;
            this.Connection_Select.Location = new System.Drawing.Point(155, 12);
            this.Connection_Select.Name = "Connection_Select";
            this.Connection_Select.Size = new System.Drawing.Size(121, 20);
            this.Connection_Select.TabIndex = 4;
            this.Connection_Select.SelectedIndexChanged += new System.EventHandler(this.Connection_Select_SelectedIndexChanged);
            // 
            // SetA
            // 
            this.SetA.Font = new System.Drawing.Font("Arial", 9F);
            this.SetA.Location = new System.Drawing.Point(178, 38);
            this.SetA.Name = "SetA";
            this.SetA.Size = new System.Drawing.Size(75, 23);
            this.SetA.TabIndex = 5;
            this.SetA.Text = "Set to A";
            this.SetA.UseVisualStyleBackColor = true;
            this.SetA.Click += new System.EventHandler(this.SetA_Click);
            // 
            // SetB
            // 
            this.SetB.Font = new System.Drawing.Font("Arial", 9F);
            this.SetB.Location = new System.Drawing.Point(178, 67);
            this.SetB.Name = "SetB";
            this.SetB.Size = new System.Drawing.Size(75, 23);
            this.SetB.TabIndex = 6;
            this.SetB.Text = "Set to B";
            this.SetB.UseVisualStyleBackColor = true;
            this.SetB.Click += new System.EventHandler(this.SetB_Click);
            // 
            // SetC
            // 
            this.SetC.Font = new System.Drawing.Font("Arial", 9F);
            this.SetC.Location = new System.Drawing.Point(178, 96);
            this.SetC.Name = "SetC";
            this.SetC.Size = new System.Drawing.Size(75, 23);
            this.SetC.TabIndex = 7;
            this.SetC.Text = "Set to C";
            this.SetC.UseVisualStyleBackColor = true;
            this.SetC.Click += new System.EventHandler(this.SetC_Click);
            // 
            // Armed_and_Takeoff_All
            // 
            this.Armed_and_Takeoff_All.Font = new System.Drawing.Font("Arial", 9F);
            this.Armed_and_Takeoff_All.Location = new System.Drawing.Point(12, 161);
            this.Armed_and_Takeoff_All.Name = "Armed_and_Takeoff_All";
            this.Armed_and_Takeoff_All.Size = new System.Drawing.Size(75, 29);
            this.Armed_and_Takeoff_All.TabIndex = 8;
            this.Armed_and_Takeoff_All.Text = "Armed and Takeoff All";
            this.Armed_and_Takeoff_All.UseVisualStyleBackColor = true;
            this.Armed_and_Takeoff_All.Click += new System.EventHandler(this.Armed_and_Takeoff_All_Click);
            // 
            // RTL_All
            // 
            this.RTL_All.Location = new System.Drawing.Point(12, 128);
            this.RTL_All.Name = "RTL_All";
            this.RTL_All.Size = new System.Drawing.Size(75, 23);
            this.RTL_All.TabIndex = 9;
            this.RTL_All.Text = "RTL All";
            this.RTL_All.UseVisualStyleBackColor = true;
            this.RTL_All.Click += new System.EventHandler(this.RTL_All_Click);
            // 
            // SetD
            // 
            this.SetD.Font = new System.Drawing.Font("Arial", 9F);
            this.SetD.Location = new System.Drawing.Point(178, 128);
            this.SetD.Name = "SetD";
            this.SetD.Size = new System.Drawing.Size(75, 23);
            this.SetD.TabIndex = 10;
            this.SetD.Text = "Set to D";
            this.SetD.UseVisualStyleBackColor = true;
            this.SetD.Click += new System.EventHandler(this.SetD_Click);
            // 
            // SetE
            // 
            this.SetE.Font = new System.Drawing.Font("Arial", 9F);
            this.SetE.Location = new System.Drawing.Point(178, 157);
            this.SetE.Name = "SetE";
            this.SetE.Size = new System.Drawing.Size(75, 23);
            this.SetE.TabIndex = 11;
            this.SetE.Text = "Set to E";
            this.SetE.UseVisualStyleBackColor = true;
            this.SetE.Click += new System.EventHandler(this.SetE_Click);
            // 
            // Auto_Guide
            // 
            this.BackgroundImage = global::MissionPlanner.Properties.Resources.bgdark;
            this.ClientSize = new System.Drawing.Size(384, 302);
            this.Controls.Add(this.SetE);
            this.Controls.Add(this.SetD);
            this.Controls.Add(this.RTL_All);
            this.Controls.Add(this.Armed_and_Takeoff_All);
            this.Controls.Add(this.SetC);
            this.Controls.Add(this.SetB);
            this.Controls.Add(this.SetA);
            this.Controls.Add(this.Connection_Select);
            this.Controls.Add(this.Reset);
            this.Controls.Add(this.Takeoff_All);
            this.Controls.Add(this.Armed_All);
            this.Controls.Add(this.Button_start);
            this.Name = "Auto_Guide";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.MyButton Button_start;
        private Controls.MyButton Armed_All;
        private Controls.MyButton Takeoff_All;
        private Controls.MyButton Reset;
        private System.Windows.Forms.ComboBox Connection_Select;
        private System.Windows.Forms.BindingSource bindingSource1;
        private Controls.MyButton SetA;
        private Controls.MyButton SetB;
        private Controls.MyButton SetC;
        private Controls.MyButton Armed_and_Takeoff_All;
        private Controls.MyButton RTL_All;
        private Controls.MyButton SetD;
        private Controls.MyButton SetE;
    }
}
