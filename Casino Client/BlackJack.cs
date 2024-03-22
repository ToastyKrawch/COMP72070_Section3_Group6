using CasinoLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private int numDealerTotalCards = 2;

        List<PictureBox> dealerCards;

        public BlackJack(PlayerInfo p, TCPClient c)
        {
            player = p;
            client = c;

            numPlayerCards = 0;
            numDealerCards = 0;

            dealerCards = new List<PictureBox>();

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

        private void btnPlay_Click(object? sender, EventArgs e)
        {
            // Hide the startup panel
            startupPanel.Visible = false;

            UpdateBalance();

            // Show bet elements
            ShowBetElements();
        }

        private void btnGoBack_Click(object? sender, EventArgs e)
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

            foreach (Control c in this.Controls)
            {
                if (c is PictureBox)
                {
                    c.Visible = false; // This will hide the PictureBoxes that are dynamically added
                }
            }

            HideBetElements();
            hideGameEndElements();
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

            if (player.bet > 0 && player.balance > 0)
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

        private async void button10_Click(object sender, EventArgs e)
        {
            if (player.bet > 0)
            {
                HideBetElements();
                ShowGameElements();
                UpdateBalance();

                //Send a request to start a game of blackjack
                client.packet = client.sendPacket(1, player.bet.ToString() + "," + player.balance.ToString());

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

                //Check to see if the player got blackjack
                client.packet = client.sendPacket(5, "Player,Blackjack");   //Request game outcome
                client.packet = client.receivePacket();                     //Receive game outcome

                await dealCards();

                if (client.dataPayloadString == "True")
                {
                    label7.Text = "BlackJack!";
                    label8.Text = "BlackJack!";
                    HideGameOptions();

                    dealerTurn();
                    await revealDealerCards();
                }

                else
                {
                    ShowGameOptions();
                }

                //Notify server that client is finished
                client.packet = client.sendPacket(2, "");
            }
        }

        private async Task revealDealerCards()
        {
            await Task.Delay(2000);
            pictureBox11.Hide();
            pictureBox7.Show();

            //Send a request for the dealer's hand total
            client.packet = client.sendPacket(1, "Dealer,2");
            client.packet = client.receivePacket();
            label1.Text = client.dataPayloadString;

            for (int i = 0; i < numDealerCards - 2; i++) 
            {
                await Task.Delay(1000);
                dealerCards[i].Show();

                //Send a request for the dealer's new hand total
                client.packet = client.sendPacket(1, "Dealer," + (i + 3));
                client.packet = client.receivePacket();
                label1.Text = client.dataPayloadString;
            }

            if (label1.Text == "21")
            {
                label1.Text = "Blackjack!";
            }

            await endScreen();
        }

        private void UpdateBalance()
        {
            player.balance = player.balance - player.bet;
            label6.Text = "Balance: " + player.balance;
        }

        private async Task dealCards()
        {
            await Task.Delay(1000);
            pictureBox9.Show();
            await Task.Delay(1000);
            pictureBox11.Show();
            await Task.Delay(1000);
            pictureBox10.Show();
            await Task.Delay(1000);
            pictureBox8.Show();
            await Task.Delay(1000);
            label1.Show();
            label7.Show();
            await Task.Delay(1000);
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

        //Hit button
        private async void button6_Click(object sender, EventArgs e)
        {
            //Run initial hit communications
            client.packet = client.sendPacket(3, "H");                                      //Send decision
            numPlayerCards++;
            client.packet = client.sendPacket(0, "Player," + numPlayerCards.ToString());    //Request next card
            client.packet = client.receivePacket();                                         //Receive next card
            addPlayerCard();

            //Check to see if the player has bust
            client.packet = client.sendPacket(5, "Player,Bust");    //Request game outcome
            client.packet = client.receivePacket();                 //Receive game outcome
            if (client.dataPayloadString == "True")
            {
                label8.Text = "You Have Bust";
                HideGameOptions();

                dealerTurn();
                await revealDealerCards();
            }

            //Check to see if the player got blackjack
            client.packet = client.sendPacket(5, "Player,Blackjack");   //Request game outcome
            client.packet = client.receivePacket();                     //Receive game outcome
            if (client.dataPayloadString == "True")
            {
                label7.Text = "Blackjack!";
                label8.Text = "BlackJack!";
                HideGameOptions();

                dealerTurn();
                await revealDealerCards();
            }

            client.packet = client.sendPacket(2, "Information received successfully, continue"); //Tell server to continue
        }

        private async Task endScreen()
        {
            await Task.Delay(1000);

            label10.Text = "Hand Total: " + label7.Text;
            label13.Text = "Hand Total: " + label1.Text;
            label6.Text = "Balance: " + player.balance;
            player.bet = 0;

            //Display information
            HideGameElements();
            showGameEndElements();
        }

        private void showGameEndElements()
        {
            button1.Show();
            button2.Show();
            panel1.Show();
            label8.Show();
        }

        private void hideGameEndElements()
        {
            button1.Hide();
            button2.Hide();
            panel1.Hide();
            label8.Hide();
        }

        private void addPlayerCard()
        {
            PictureBox pictureBox = new PictureBox();

            if (numPlayerCards == 3)
            {
                pictureBox.Location = new Point(375, 274);
            }
            else if (numPlayerCards == 4)
            {
                pictureBox.Location = new Point(499, 274);
            }
            else
            {
                pictureBox.Location = new Point(623, 274);
            }

            pictureBox.BackColor = Color.Transparent;
            pictureBox.Size = new Size(136, 168);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabStop = false;
            pictureBox.Image = determineCard(0);
            this.Controls.Add(pictureBox);
            pictureBox.BringToFront();
            pictureBox.Show();

            client.packet = client.sendPacket(1, "Player"); //Request hand value

            client.packet = client.receivePacket(); //Receive the hand total for the player

            string[] data = client.dataPayloadString.Split(',');
            label7.Text = data[0];
        }

        private void addDealerCard()
        {
            PictureBox pictureBox = new PictureBox();

            if (numDealerCards == 3)
            {
                pictureBox.Location = new Point(375, 31);
            }
            else if ( numDealerCards == 4)
            {
                pictureBox.Location = new Point(499, 31);
            }
            else
            {
                pictureBox.Location = new Point(623, 31);
            }
            pictureBox.BackColor = Color.Transparent;
            pictureBox.Size = new Size(136, 168);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabStop = false;
            pictureBox.Image = determineCard(0);
            this.Controls.Add(pictureBox);
            pictureBox.BringToFront();
            pictureBox.Hide();

            dealerCards.Add(pictureBox);

            string[] data = client.dataPayloadString.Split(',');
        }

        //Stand button
        private async void button8_Click(object sender, EventArgs e)
        {
            client.packet = client.sendPacket(3, "S"); //Send decision

            HideGameOptions();

            dealerTurn();
            await revealDealerCards();
        }

        private void dealerTurn()
        {
            //Wait for the server to be ready
            client.packet = client.sendPacket(2, "You can now run the rest of the game");
            client.packet = client.receivePacket();

            //Send a request for the dealer's first card
            client.packet = client.sendPacket(0, "Dealer,1");
            client.packet = client.receivePacket();
            pictureBox7.Image = determineCard(0);

            //Send a request for the dealer's card count
            client.packet = client.sendPacket(4, "Dealer");
            client.packet = client.receivePacket();
            numDealerTotalCards = Int32.Parse(client.dataPayloadString);

            //Dealer loop for how many cards are left
            if (Int32.Parse(client.dataPayloadString) > 2)
            {
                for (int i = 3; i <= numDealerTotalCards; i++)
                {
                    numDealerCards++;
                    client.packet = client.sendPacket(0, "Dealer," + i);
                    client.packet = client.receivePacket();
                    addDealerCard();
                }
            }

            client.packet = client.sendPacket(5, "Dealer,0");       //Request for game end payout
            client.packet = client.receivePacket();                 //Receive player's payout
            label11.Text = "Payout: " + client.dataPayloadString;

            client.packet = client.sendPacket(5, "Dealer,1");       //Request for game end results
            client.packet = client.receivePacket();                 //Receive the end results
            int result = Int32.Parse(client.dataPayloadString);
            switch (result) 
            { 
                case 0:
                    label8.Text = "Blackjack!";
                    break;

                case 1:
                    label8.Text = "You Win!";
                    break;

                case 2:
                    label8.Text = "You Lose!";
                    break;

                case 3:
                    label8.Text = "Push!";
                    break;

                case 4:
                    label8.Text = "You Have Bust";
                    break;
            }

            client.packet = client.sendPacket(5, "Dealer,2");       //Request for game end balance
            client.packet = client.receivePacket();                 //Receive the new balance
            player.balance = Int32.Parse(client.dataPayloadString); 
        }

        //Restarts the game
        private void button1_Click(object sender, EventArgs e)
        {
            client.packet = client.sendPacket(2, "Information received successfully, continue");

            numPlayerCards = 0;
            numDealerCards = 0;
            dealerCards = new List<PictureBox>();

            updateBet();

            HideGameElements();
            ShowBetElements();
        }
    }
}
