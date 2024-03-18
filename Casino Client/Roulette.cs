using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Casino_Client
{
    public partial class Roulette : Form
    {
        public Roulette()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

        private int angle = 0; // Field to store the current angle of rotation
        private void spinbutton_Click(object sender, EventArgs e)
        {

            // Array of numbers on the roulette wheel
            int[] numbers = Enumerable.Range(0, 37).ToArray(); // 0 to 36

            // Mapping numbers to colors (0: green, 1: red, 2: black)
            int[] colors = { 0, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            // Create a new Bitmap object based on the current image in PictureBox
            Bitmap bmp = new Bitmap(pictureBox2.Image);

            // Clear the PictureBox image
            pictureBox2.Image = null;

            // Rotate the image
            bmp = RotateImage(bmp, angle);

            // Display the rotated image in PictureBox
            pictureBox2.Image = bmp;

            // Increase the angle for the next rotation
            angle += 10;
            if (angle >= 360) angle = 0; // Reset the angle after a full rotation

            // Randomly determine where the wheel landed
            Random rand = new Random();
            int landedIndex = rand.Next(numbers.Length); // Get a random index for the numbers array
            int landedNumber = numbers[landedIndex];
            string color = colors[landedIndex] == 0 ? "Green" : colors[landedIndex] == 1 ? "Red" : "Black";

            // Display the result
            MessageBox.Show($"The wheel landed on {landedNumber} {color}", "Result");
        }

        // Method to rotate the image
        public Bitmap RotateImage(Bitmap bmp, float angle)
        {
            Bitmap rotatedImage = new Bitmap(bmp.Width, bmp.Height);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                // Set the rotation point to the center in the matrix
                g.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

                // Draw the image on the bitmap
                g.DrawImage(bmp, new Point(0, 0));
            }

            return rotatedImage;
        }


    }
}
