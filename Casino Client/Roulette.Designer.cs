namespace Casino_Client
{
    partial class Roulette
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Roulette));
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            spinbutton = new Button();
            button1 = new Button();
            button2 = new Button();
            red = new Button();
            black = new Button();
            nineteento = new Button();
            oneto = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            Money = new Label();
            label2 = new Label();
            button7 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(404, -2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(580, 239);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImageLayout = ImageLayout.None;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(-7, 2);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(405, 258);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // spinbutton
            // 
            spinbutton.BackColor = SystemColors.ActiveCaptionText;
            spinbutton.Font = new Font("Showcard Gothic", 15.75F);
            spinbutton.ForeColor = SystemColors.ButtonHighlight;
            spinbutton.Location = new Point(853, 470);
            spinbutton.Name = "spinbutton";
            spinbutton.Size = new Size(119, 75);
            spinbutton.TabIndex = 19;
            spinbutton.Text = "SPIN";
            spinbutton.UseVisualStyleBackColor = false;
            spinbutton.Click += spinbutton_Click;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ActiveCaptionText;
            button1.Font = new Font("Showcard Gothic", 15.75F);
            button1.ForeColor = SystemColors.ButtonHighlight;
            button1.Location = new Point(654, 243);
            button1.Name = "button1";
            button1.Size = new Size(119, 75);
            button1.TabIndex = 20;
            button1.Text = "Even";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.ActiveCaptionText;
            button2.Font = new Font("Showcard Gothic", 15.75F);
            button2.ForeColor = SystemColors.ButtonHighlight;
            button2.Location = new Point(654, 324);
            button2.Name = "button2";
            button2.Size = new Size(119, 75);
            button2.TabIndex = 21;
            button2.Text = "ODD";
            button2.UseVisualStyleBackColor = false;
            // 
            // red
            // 
            red.BackColor = SystemColors.ActiveCaptionText;
            red.Font = new Font("Showcard Gothic", 15.75F);
            red.ForeColor = SystemColors.ButtonHighlight;
            red.Location = new Point(529, 243);
            red.Name = "red";
            red.Size = new Size(119, 75);
            red.TabIndex = 22;
            red.Text = "red";
            red.UseVisualStyleBackColor = false;
            // 
            // black
            // 
            black.BackColor = SystemColors.ActiveCaptionText;
            black.Font = new Font("Showcard Gothic", 15.75F);
            black.ForeColor = SystemColors.ButtonHighlight;
            black.Location = new Point(529, 324);
            black.Name = "black";
            black.Size = new Size(119, 75);
            black.TabIndex = 23;
            black.Text = "BLACK";
            black.UseVisualStyleBackColor = false;
            // 
            // nineteento
            // 
            nineteento.BackColor = SystemColors.ActiveCaptionText;
            nineteento.Font = new Font("Showcard Gothic", 15.75F);
            nineteento.ForeColor = SystemColors.ButtonHighlight;
            nineteento.Location = new Point(404, 324);
            nineteento.Name = "nineteento";
            nineteento.Size = new Size(119, 75);
            nineteento.TabIndex = 24;
            nineteento.Text = "19to36";
            nineteento.UseVisualStyleBackColor = false;
            // 
            // oneto
            // 
            oneto.BackColor = SystemColors.ActiveCaptionText;
            oneto.Font = new Font("Showcard Gothic", 15.75F);
            oneto.ForeColor = SystemColors.ButtonHighlight;
            oneto.Location = new Point(404, 243);
            oneto.Name = "oneto";
            oneto.Size = new Size(119, 75);
            oneto.TabIndex = 25;
            oneto.Text = "1to18";
            oneto.UseVisualStyleBackColor = false;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(797, 324);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(175, 70);
            textBox1.TabIndex = 26;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(23, 292);
            label1.Name = "label1";
            label1.Size = new Size(92, 23);
            label1.TabIndex = 27;
            label1.Text = "Currency:";
            // 
            // Money
            // 
            Money.AutoSize = true;
            Money.BackColor = Color.Transparent;
            Money.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Money.ForeColor = Color.White;
            Money.Location = new Point(112, 292);
            Money.Name = "Money";
            Money.Size = new Size(30, 23);
            Money.TabIndex = 28;
            Money.Text = "$0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(797, 271);
            label2.Name = "label2";
            label2.Size = new Size(139, 23);
            label2.TabIndex = 29;
            label2.Text = "Enter Amount :";
            // 
            // button7
            // 
            button7.BackColor = SystemColors.ActiveCaptionText;
            button7.Font = new Font("Showcard Gothic", 15.75F);
            button7.ForeColor = SystemColors.ButtonHighlight;
            button7.Location = new Point(12, 470);
            button7.Name = "button7";
            button7.Size = new Size(119, 75);
            button7.TabIndex = 30;
            button7.Text = "BACK";
            button7.UseVisualStyleBackColor = false;
            // 
            // Roulette
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Green;
            ClientSize = new Size(984, 557);
            Controls.Add(button7);
            Controls.Add(label2);
            Controls.Add(Money);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(oneto);
            Controls.Add(nineteento);
            Controls.Add(black);
            Controls.Add(red);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(spinbutton);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            ForeColor = SystemColors.ControlLight;
            Name = "Roulette";
            Text = "Roulette";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Button spinbutton;
        private Button button1;
        private Button button2;
        private Button red;
        private Button black;
        private Button nineteento;
        private Button oneto;
        private TextBox textBox1;
        private Label label1;
        private Label Money;
        private Label label2;
        private Button button7;
    }
}