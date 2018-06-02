using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EffectSome
{
    public partial class CustomObjectEditor : Form
    {
        public static bool IsOpen = false;

        Bitmap customObjectPreview;
        Rectangle pictureBoxRectangle;
        int tempXOffset, tempYOffset, customObjectPreviewImageXOffset, customObjectPreviewImageYOffset;
        int tempMouseXOffset, tempMouseYOffset, startingMouseX, startingMouseY;
        bool isMouseClickHolded;
        
        public CustomObjectEditor()
        {
            IsOpen = true;
            InitializeComponent();
            customObjectPreview = GenerateGrid(1000, 1000, 25);
            pictureBox1.Image = customObjectPreview;
            pictureBoxRectangle = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
        }

        #region PictureBox
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            startingMouseX = e.X;
            startingMouseY = e.Y;
            isMouseClickHolded = true;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseClickHolded)
            {
                tempMouseXOffset = (startingMouseX - e.X).WithinBounds(-customObjectPreviewImageXOffset, customObjectPreview.Width - pictureBox1.Size.Width - customObjectPreviewImageXOffset);
                tempMouseYOffset = (startingMouseY - e.Y).WithinBounds(-customObjectPreviewImageYOffset, customObjectPreview.Height - pictureBox1.Size.Height - customObjectPreviewImageYOffset);
                tempXOffset = (customObjectPreviewImageXOffset + tempMouseXOffset).WithinBounds(0, customObjectPreview.Width - pictureBox1.Size.Width);
                tempYOffset = (customObjectPreviewImageYOffset + tempMouseYOffset).WithinBounds(0, customObjectPreview.Height - pictureBox1.Size.Height);
                Bitmap newImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g = Graphics.FromImage(newImage);
                g.DrawImage(customObjectPreview, pictureBoxRectangle, tempXOffset, tempYOffset, pictureBox1.Size.Width, pictureBox1.Size.Height, GraphicsUnit.Pixel);
                g.Dispose();
                pictureBox1.Image = null;
                pictureBox1.Image = newImage;
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseClickHolded = false;
            customObjectPreviewImageXOffset += tempMouseXOffset;
            customObjectPreviewImageYOffset += tempMouseYOffset;
        }

        private void CustomObjectEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsOpen = false;
        }
        #endregion

        #region Buttons
        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {

        }
        #endregion

        Bitmap RenderCustomObject(List<LevelObject> customObj)
        {
            List<double> xLocations = new List<double>();
            List<double> yLocations = new List<double>();
            for (int i = 0; i < customObj.Count; i++)
            {
                xLocations.Add((double)customObj[i].Parameters[(int)LevelObject.ObjectParameter.X]);
                yLocations.Add((double)customObj[i].Parameters[(int)LevelObject.ObjectParameter.Y]);
            }
            double minX = xLocations.Min();
            double maxX = xLocations.Max();
            double minY = yLocations.Min();
            double maxY = yLocations.Max();
            Bitmap result = GenerateGrid((int)(maxX - minX) + 500, (int)(maxY - minY) + 500, 10); // Create a new image which is big enough to fit all objects in the custom object and have extra 500 pixels of space for the remaining
            return result;
        }
        Bitmap GenerateGrid(int width, int height, int gridSpan)
        {
            Bitmap result = new Bitmap(width, height);
            for (int i = 0; i < width; i += gridSpan)
                for (int j = 0; j < height; j++)
                    result.SetPixel(i, j, Color.Black);
            for (int i = 0; i < height; i += gridSpan)
                for (int j = 0; j < width; j++)
                    result.SetPixel(j, i, Color.Black);
            return result;
        }
        Bitmap GetRectangle(int startX, int startY, int endX, int endY, Bitmap image)
        {
            Bitmap result = new Bitmap(endX - startX, endY - startY);
            for (int i = startX; i < endX; i++)
                for (int j = startY; j < endY; j++)
                    result.SetPixel(i - startX, j - startY, image.GetPixel(i, j));
            return result;
        }
    }
}