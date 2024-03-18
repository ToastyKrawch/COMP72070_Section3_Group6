using CasinoLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Casino_Client
{
    public partial class BlackJack : Form
    {
        private PlayerInfo player;
        TCPClient client;

        private int numPlayerCards;
        private int numDealerCards;

        public BlackJack(PlayerInfo p, TCPClient c)
        {
            player = p;
            client = c;

            numPlayerCards = 0;
            numDealerCards = 0;

            InitializeComponent();
            InitializeStartupScreen();
        }

        private void InitializeStartupScreen()
        {
            // Create the panel
            startupPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Green // Use an appropriate color or background image
            };
            this.Controls.Add(startupPanel);

            // Create the Play button
            btnPlay = new Button()
            {
                Text = "Play",
                Size = new Size(100, 50),
                Location = new Point((this.ClientSize.Width - 100) / 2, (this.ClientSize.Height / 2) - 60)
            };
            btnPlay.Click += new EventHandler(btnPlay_Click);
            startupPanel.Controls.Add(btnPlay);

            // Create the Go Back button
            btnGoBack = new Button()
            {
                Text = "Go Back",
                Size = new Size(100, 50),
                Location = new Point((this.ClientSize.Width - 100) / 2, (this.ClientSize.Height / 2) + 10)
            };
            btnGoBack.Click += new EventHandler(btnGoBack_Click);
            startupPanel.Controls.Add(btnGoBack);

            // Hide game elements
            HideGameElements();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            // Hide the startup panel
            startupPanel.Visible = false;

            // Show bet elements
            ShowBetElements();
        }

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            player.bet = 0;

            this.Close();
            var newForm = new MainMenu(player, client);
            newForm.Show();
        }

        private void HideGameElements()
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label7.Visible = false;
            button3.Visible = false;
            button6.Visible = false;
            button8.Visible = false;
            button10.Visible = false;
            pictureBox7.Visible = false;
            pictureBox8.Visible = false;
            pictureBox9.Visible = false;
            pictureBox10.Visible = false;

            HideBetElements();
        }

        private void ShowGameElements()
        {
            button3.Show();
            label2.Show();
            label3.Show();
        }

        private void ShowGameOptions()
        {
            button6.Show();
            button8.Show();
            label1.Show();
            label7.Show();
        }

        private void HideGameOptions()
        {
            button6.Hide();
            button8.Hide();
        }

        private void waitForBet(object sender, EventArgs e)
        {
            HideGameElements();
            ShowBetElements();

            player.bet = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            player.bet = 0;

            this.Close();
            var newForm = new MainMenu(player, client);
            newForm.Show();
        }

        private void HideBetElements()
        {
            label4.Hide();
            label5.Hide();
            label6.Hide();
            pictureBox1.Hide();
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            pictureBox5.Hide();
            pictureBox6.Hide();
            button10.Hide();
            button11.Hide();
        }

        private void ShowBetElements()
        {
            label4.Show();
            label5.Show();
            label6.Show();
            pictureBox1.Show();
            pictureBox2.Show();
            pictureBox3.Show();
            pictureBox4.Show();
            pictureBox5.Show();
            pictureBox6.Show();
            button3.Show();
            button10.Show();
            button11.Show();
        }

        private void updateBet()
        {
            label5.Text = player.bet.ToString();

            if (player.bet > 0)
            {
                button10.Enabled = true;
                button10.ForeColor = Color.White;
            }

            else
            {
                button10.Enabled = false;
                button10.ForeColor = Color.DarkRed;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            player.bet += 1;
            updateBet();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            player.bet += 10;
            updateBet();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            player.bet += 25;
            updateBet();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            player.bet += 50;
            updateBet();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            player.bet += 100;
            updateBet();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            player.bet += 500;
            updateBet();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            player.bet = 0;
            updateBet();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (player.bet > 0)
            {
                HideBetElements();
                ShowGameElements();
                UpdateBalance();

                //Send a request to start a game of blackjack
                client.packet = client.sendPacket(1, "");

                //Send a request for the player's first card
                client.packet = client.sendPacket(0, "Player,1");
                client.packet = client.receivePacket();
                pictureBox9.Image = determineCard(0);

                //Send a request for the player's second card
                client.packet = client.sendPacket(0, "Player,2");
                client.packet = client.receivePacket();
                pictureBox10.Image = determineCard(0);

                //Send a request for the player's hand total
                client.packet = client.sendPacket(1, "Player,2");
                client.packet = client.receivePacket();
                label7.Text = client.dataPayloadString;

                //Send a request for the dealer's second card
                client.packet = client.sendPacket(0, "Dealer,2");
                client.packet = client.receivePacket();
                pictureBox8.Image = determineCard(0);

                //Send a request for the dealer's hand total
                client.packet = client.sendPacket(1, "Dealer,1");
                client.packet = client.receivePacket();
                label1.Text = client.dataPayloadString;

                numPlayerCards += 2;
                numDealerCards += 2;

                dealCards();

                //Notify server that client is ready, and receive the determiner wheather blackjack is the case or not
                client.packet = client.sendPacket(2, "");
                client.packet = client.receivePacket();

                if (client.dataPayloadString == "Y")
                {
                    label7.Text = "BlackJack!";
                    HideGameOptions();
                }
            }
        }

        private void UpdateBalance()
        {
            player.balance -= player.bet;
            label6.Text = "Balance: " + player.balance;
        }

        private async void dealCards()
        {
            await Task.Delay(1000);
            pictureBox9.Show();
            await Task.Delay(1000);
            pictureBox7.Show();
            await Task.Delay(1000);
            pictureBox10.Show();
            await Task.Delay(1000);
            pictureBox8.Show();
            await Task.Delay(1000);
            ShowGameOptions();
        }

        private Image determineCard(int index)
        {
            string[] cardInfo = client.dataPayloadString.Split(',');

            string rank = cardInfo[index];
            string suit = cardInfo[index + 1];

            Image determinedImage = Properties.Resources.ReverseSide;

            if (suit == "Clubs")
            {
                if (rank == "2")
                {
                    determinedImage = Properties.Resources.TwoClubs;
                }
                else if (rank == "3")
                {
                    determinedImage = Properties.Resources.ThreeClubs;
                }
                else if (rank == "4")
                {
                    determinedImage = Properties.Resources.FourClubs;
                }
                else if (rank == "5")
                {
                    determinedImage = Properties.Resources.FiveClubs;
                }
                else if (rank == "6")
                {
                    determinedImage = Properties.Resources.SixClubs;
                }
                else if (rank == "7")
                {
                    determinedImage = Properties.Resources.SevenClubs;
                }
                else if (rank == "8")
                {
                    determinedImage = Properties.Resources.EightClubs;
                }
                else if (rank == "9")
                {
                    determinedImage = Properties.Resources.NineClubs;
                }
                else if (rank == "10")
                {
                    determinedImage = Properties.Resources.TenClubs;
                }
                else if (rank == "Jack")
                {
                    determinedImage = Properties.Resources.JackClubs;
                }
                else if (rank == "Queen")
                {
                    determinedImage = Properties.Resources.QueenClubs;
                }
                else if (rank == "King")
                {
                    determinedImage = Properties.Resources.KingClubs;
                }
                else if (rank == "Ace")
                {
                    determinedImage = Properties.Resources.AceClubs;
                }
            }
            else if (suit == "Diamonds")
            {
                if (rank == "2")
                {
                    determinedImage = Properties.Resources.TwoDiamonds;
                }
                else if (rank == "3")
                {
                    determinedImage = Properties.Resources.ThreeDiamonds;
                }
                else if (rank == "4")
                {
                    determinedImage = Properties.Resources.FourDiamonds;
                }
                else if (rank == "5")
                {
                    determinedImage = Properties.Resources.FiveDiamonds;
                }
                else if (rank == "6")
                {
                    determinedImage = Properties.Resources.SixDiamonds;
                }
                else if (rank == "7")
                {
                    determinedImage = Properties.Resources.SevenDiamonds;
                }
                else if (rank == "8")
                {
                    determinedImage = Properties.Resources.EightDiamonds;
                }
                else if (rank == "9")
                {
                    determinedImage = Properties.Resources.NineDiamonds;
                }
                else if (rank == "10")
                {
                    determinedImage = Properties.Resources.TenDiamonds;
                }
                else if (rank == "Jack")
                {
                    determinedImage = Properties.Resources.JackDiamonds;
                }
                else if (rank == "Queen")
                {
                    determinedImage = Properties.Resources.QueenDiamonds;
                }
                else if (rank == "King")
                {
                    determinedImage = Properties.Resources.KingDiamonds;
                }
                else if (rank == "Ace")
                {
                    determinedImage = Properties.Resources.AceDiamonds;
                }
            }
            else if (suit == "Hearts")
            {
                if (rank == "2")
                {
                    determinedImage = Properties.Resources.TwoHearts;
                }
                else if (rank == "3")
                {
                    determinedImage = Properties.Resources.ThreeHearts;
                }
                else if (rank == "4")
                {
                    determinedImage = Properties.Resources.FourHearts;
                }
                else if (rank == "5")
                {
                    determinedImage = Properties.Resources.FiveHearts;
                }
                else if (rank == "6")
                {
                    determinedImage = Properties.Resources.SixHearts;
                }
                else if (rank == "7")
                {
                    determinedImage = Properties.Resources.SevenHearts;
                }
                else if (rank == "8")
                {
                    determinedImage = Properties.Resources.EightHearts;
                }
                else if (rank == "9")
                {
                    determinedImage = Properties.Resources.NineHearts;
                }
                else if (rank == "10")
                {
                    determinedImage = Properties.Resources.TenHearts;
                }
                else if (rank == "Jack")
                {
                    determinedImage = Properties.Resources.JackHearts;
                }
                else if (rank == "Queen")
                {
                    determinedImage = Properties.Resources.QueenHearts;
                }
                else if (rank == "King")
                {
                    determinedImage = Properties.Resources.KingHearts;
                }
                else if (rank == "Ace")
                {
                    determinedImage = Properties.Resources.AceHearts;
                }
            }
            else if (suit == "Spades")
            {
                if (rank == "2")
                {
                    determinedImage = Properties.Resources.TwoSpades;
                }
                else if (rank == "3")
                {
                    determinedImage = Properties.Resources.ThreeSpades;
                }
                else if (rank == "4")
                {
                    determinedImage = Properties.Resources.FourSpades;
                }
                else if (rank == "5")
                {
                    determinedImage = Properties.Resources.FiveSpades;
                }
                else if (rank == "6")
                {
                    determinedImage = Properties.Resources.SixSpades;
                }
                else if (rank == "7")
                {
                    determinedImage = Properties.Resources.SevenSpades;
                }
                else if (rank == "8")
                {
                    determinedImage = Properties.Resources.EightSpades;
                }
                else if (rank == "9")
                {
                    determinedImage = Properties.Resources.NineSpades;
                }
                else if (rank == "10")
                {
                    determinedImage = Properties.Resources.TenSpades;
                }
                else if (rank == "Jack")
                {
                    determinedImage = Properties.Resources.JackSpades;
                }
                else if (rank == "Queen")
                {
                    determinedImage = Properties.Resources.QueenSpades;
                }
                else if (rank == "King")
                {
                    determinedImage = Properties.Resources.KingSpades;
                }
                else if (rank == "Ace")
                {
                    determinedImage = Properties.Resources.AceSpades;
                }
            }

            return determinedImage;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            client.packet = client.sendPacket(2, "H"); //Send decision
            
            client.packet = client.receivePacket(); //Receive next card
            addPlayerCard();
            
            client.packet = client.receivePacket(); //Check for bust (type 2)
            if (client.packet.PacketType == 2) 
            {
                label7.Text = "Bust!";
                HideGameOptions();
            }

            client.packet = client.receivePacket(); //Check for blackjack (type 3)
            if (client.packet.PacketType == 3)
            {
                label7.Text = "BlackJack!";
                HideGameOptions();
            }
        }

        private void addPlayerCard()
        {
            PictureBox pictureBox = new PictureBox();

            if (numPlayerCards == 2) 
            {
                pictureBox.Location = new Point(375, 274);
            }
            else 
            {
                pictureBox.Location = new Point(499, 274);
            }

            pictureBox.BackColor = Color.Transparent;
            pictureBox.Size = new Size(136, 168);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabStop = false;
            pictureBox.Image = determineCard(0);
            this.Controls.Add(pictureBox);
            pictureBox.BringToFront();
            pictureBox.Show();

            numPlayerCards++;

            string[] data = client.dataPayloadString.Split(',');
            label7.Text = data[data.Length - 1];
        }

        private async void addDealerCard()
        {
            PictureBox pictureBox = new PictureBox();

            if (numDealerCards == 2)
            {
                pictureBox.Location = new Point(375, 31);
            }
            else
            {
                pictureBox.Location = new Point(499, 31);
            }

            pictureBox.BackColor = Color.Transparent;
            pictureBox.Size = new Size(136, 168);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabStop = false;
            pictureBox.Image = determineCard(0);
            this.Controls.Add(pictureBox);
            pictureBox.BringToFront();
            pictureBox.Show();

            numDealerCards++;

            string[] data = client.dataPayloadString.Split(',');
            label1.Text = data[data.Length - 1];
        }

        private void button8_Click(object sender, EventArgs e)
        {
            client.packet = client.sendPacket(3, "S"); //Send decision

            //Send a request for the dealer's first card
            client.packet = client.sendPacket(0, "Dealer,1");
            client.packet = client.receivePacket();
            flipDealerCard();

            //Send a request for the dealer's card count
            client.packet = client.sendPacket(4, "Dealer");
            client.packet = client.receivePacket();

            //Dealer loop for how many cards are left
            for (int i = 3; i <= Int32.Parse(client.dataPayloadString); i++) 
            {
                client.packet = client.sendPacket(0, "Dealer," + i);
                client.packet = client.receivePacket();
                addDealerCard();
            }

            //Send a request for the outcome
            client.packet = client.sendPacket(5, "");
            client.packet = client.receivePacket();

            //switch (client.dataPayloadString)
            //{
            //    case "Push":
            //        showPushScreen();
            //        break;

            //    case "Win":
            //        showWinScreen();
            //        break;

            //    case "Lose"():
            //        showLoseScreen();
            //        break;
            //}
        }

        //private void showLoseScreen()
        //{
        //    BlackJackOutcome endScreen = new BlackJackOutcome();
            

        //}

        //private void showWinScreen()
        //{
            
        //}

        //private void showPushScreen()
        //{

        //}

        private async void flipDealerCard()
        {
            pictureBox7.Image = determineCard(0);

            client.packet = client.sendPacket(1, "Dealer,2");
            client.packet = client.receivePacket();
            label1.Text = client.dataPayloadString;
        }
    }
}
