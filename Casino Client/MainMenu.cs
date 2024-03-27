using CasinoLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Casino_Client
{
    public partial class MainMenu : Form
    {
        private Rectangle button1OriginalRectangle;
        private Rectangle button2OriginalRectangle;
        private Rectangle button3OriginalRectangle;

        private Rectangle label1OriginalRectangle;
        private Rectangle label2OriginalRectangle;

        private Rectangle originalFormSize;

        private PlayerInfo player;
        TCPClient client;

        public MainMenu(PlayerInfo p, TCPClient c)
        {
            InitializeComponent();
            player = p;
            client = c;
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            originalFormSize = new Rectangle(this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height);

            button1OriginalRectangle = new Rectangle(button1.Location.X, button1.Location.Y, button1.Width, button1.Height);
            button2OriginalRectangle = new Rectangle(button2.Location.X, button2.Location.Y, button2.Width, button2.Height);
            button3OriginalRectangle = new Rectangle(button3.Location.X, button3.Location.Y, button3.Width, button3.Height);

            label1OriginalRectangle = new Rectangle(label1.Location.X, label1.Location.Y, label1.Width, label1.Height);
            label2OriginalRectangle = new Rectangle(label2.Location.X, label2.Location.Y, label2.Width, label2.Height);
        }

        private void resizeControl(Rectangle r, Control c)
        {
            float xRatio = (float)(this.Width) / (float)(originalFormSize.Width);
            float yRatio = (float)(this.Height) / (float)(originalFormSize.Height);

            int newX = (int)(r.Location.X * xRatio);
            int newY = (int)(r.Location.Y * yRatio);

            int newWidth = (int)(r.Width * xRatio);
            int newHeight = (int)(r.Height * yRatio);

            c.Location = new Point(newX, newY);
            c.Size = new Size(newWidth, newHeight);
        }

        private void MainMenu_Resize(object sender, EventArgs e)
        {
            resizeControl(button1OriginalRectangle, button1);
            resizeControl(button2OriginalRectangle, button2);
            resizeControl(button3OriginalRectangle, button3);
            resizeControl(label1OriginalRectangle, label1);
            resizeControl(label2OriginalRectangle, label2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var newform = new BlackJack(player, client);
            this.Hide();
            newform.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var newform = new Roulette(player, client);
            this.Hide();
            newform.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
