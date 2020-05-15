using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Enums;
using Xabe.FFmpeg.Model;

namespace ImageProcessor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Load MJpeg file
            //Set directory where app should look for FFmpeg 
            FFmpeg.ExecutablesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FFmpeg");
            //Get latest version of FFmpeg. It's great idea if you don't know if you had installed FFmpeg.
            await FFmpeg.GetLatestVersion();

            //System.Drawing.Image source = System.Drawing.Image.FromFile(@"Z:\Temp\temp.bmp");
            //System.Drawing.Image destination = new System.Drawing.Bitmap(128, 128);

            //using (var g = Graphics.FromImage(destination))
            //{
            //    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            //    g.DrawImage(source, new System.Drawing.Rectangle(0, 0, 128, 128), new System.Drawing.Rectangle(0, 0, source.Width, source.Height), GraphicsUnit.Pixel);
            //}

            //destination.Save(@"Z:\Temp\outpt.png", ImageFormat.Jpeg);
            int width = 852;
            int height = 480;
            var newSize = new VideoSize(width, height);
            string output = $"C:\\Users\\trist\\Videos\\Kinect\\20200313143123\\Kinect_{width}X{height}.mkv";
            string input = @"C:\Users\trist\Videos\Kinect\20200313143123\Kinect_1280X720.mkv";

            IConversionResult result = await Conversion.ChangeSize(input, output, newSize)
            .Start();


            // convert or compress

        }
    }
}
