using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace ImageChopPro
{
    public partial class frmMainWindow : Form
    {
        public frmMainWindow()
        {
            InitializeComponent();
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            DialogResult result = dialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                foreach (String file in dialog.FileNames)
                    convertImage(file);
            }
        }

        public void convertImage(string file)
        {
            Bitmap img = new Bitmap(file);
            Double factor = 0.0;
            if (img.Width >= img.Height)
                factor = (double)numHeight.Value / (double)img.Height;
            else
                factor = (double)numWidth.Value / (double)img.Width;

            Size s = new Size((int)(img.Width * factor * 1.02), (int)(img.Height * factor * 1.02));

            Console.WriteLine(s.ToString());
            Bitmap resized = resizeImage(img, s);

            pictureBox.Image = resized;
            int offset = 0;
            Rectangle rect;

            if (img.Width >= img.Height)
            {
                offset = (s.Width - (int)numWidth.Value) / 2;
                rect = new Rectangle(offset, 0, (int)numWidth.Value, (int)numHeight.Value);
            }
            else
            {
                offset = (s.Height - (int)numHeight.Value) / 2;
                rect = new Rectangle(0, offset, (int)numWidth.Value, (int)numHeight.Value);
            }
            Console.WriteLine(rect.ToString());
            Bitmap finished = cropImage(resized, rect);
            pictureBox.Image = finished;

            string extension = Path.GetExtension(file);
            string filename = Path.GetFileName(file);
            string filenameNoExtension = Path.GetFileNameWithoutExtension(file);
            string root = Path.GetDirectoryName(file);

            string finalPath = root + "\\" + filenameNoExtension + numWidth.Value.ToString() + "x" + numHeight.Value.ToString() + ".jpg";
            Console.WriteLine(finalPath);
            finished.Save(finalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public Bitmap resizeImage(Image imgToResize, Size size)
        {
           return new Bitmap(imgToResize, size);
        }

        private Bitmap cropImage(Bitmap img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }
    }
}
