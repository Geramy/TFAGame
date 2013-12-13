namespace TFAGame
{
    partial class Form1
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
            this.host_game = new System.Windows.Forms.Button();
            this.start_game = new System.Windows.Forms.Button();
            this.end_game = new System.Windows.Forms.Button();
            this.username = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.glControl = new OpenTK.GLControl();
            this.SuspendLayout();
            // 
            // host_game
            // 
            this.host_game.Location = new System.Drawing.Point(12, 576);
            this.host_game.Name = "host_game";
            this.host_game.Size = new System.Drawing.Size(75, 23);
            this.host_game.TabIndex = 1;
            this.host_game.Text = "Host Game";
            this.host_game.UseVisualStyleBackColor = true;
            this.host_game.Click += new System.EventHandler(this.host_game_Click);
            // 
            // start_game
            // 
            this.start_game.Location = new System.Drawing.Point(510, 573);
            this.start_game.Name = "start_game";
            this.start_game.Size = new System.Drawing.Size(75, 23);
            this.start_game.TabIndex = 2;
            this.start_game.Text = "Start Game";
            this.start_game.UseVisualStyleBackColor = true;
            this.start_game.Click += new System.EventHandler(this.start_game_Click);
            // 
            // end_game
            // 
            this.end_game.Location = new System.Drawing.Point(697, 576);
            this.end_game.Name = "end_game";
            this.end_game.Size = new System.Drawing.Size(75, 23);
            this.end_game.TabIndex = 3;
            this.end_game.Text = "End Game";
            this.end_game.UseVisualStyleBackColor = true;
            this.end_game.Click += new System.EventHandler(this.end_game_Click);
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(591, 576);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(100, 20);
            this.username.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(591, 557);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Username";
            // 
            // glControl
            // 
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Location = new System.Drawing.Point(12, 12);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(760, 542);
            this.glControl.TabIndex = 6;
            this.glControl.VSync = false;
            this.glControl.Load += new System.EventHandler(this.GLScreen_Load);
            this.glControl.Paint += new System.Windows.Forms.PaintEventHandler(this.GLScreen_Paint);
            this.glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl_KeyDown);
            this.glControl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.glControl_KeyUp);
            this.glControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseDown);
            this.glControl.Resize += new System.EventHandler(this.GLScreen_Resize);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 608);
            this.Controls.Add(this.glControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.username);
            this.Controls.Add(this.end_game);
            this.Controls.Add(this.start_game);
            this.Controls.Add(this.host_game);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button host_game;
        private System.Windows.Forms.Button start_game;
        private System.Windows.Forms.Button end_game;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label label1;
        private OpenTK.GLControl glControl;
    }
}

