using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;

namespace CameraDevice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;

        int flag = 1;


        private void Form1_Load(object sender, EventArgs e)
        {

            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo videoCaptureDevice in VideoCaptureDevices)
            {
                combDeviceList.Items.Add(videoCaptureDevice.Name);
            }
            combDeviceList.SelectedIndex = 0;
            FinalVideo = new VideoCaptureDevice();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (FinalVideo.IsRunning)
            {
                FinalVideo.Stop();
               // FinalVideo = null;
                btnStart.Text = "&Start";
                return;

            }
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[combDeviceList.SelectedIndex].MonikerString);
            FinalVideo.NewFrame += FinalVideo_NewFrame;
            FinalVideo.Start();
            btnStart.Text = "&Stop";
        }

        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // throw new NotImplementedException();
            Bitmap video = (Bitmap)eventArgs.Frame.Clone();
            picDevice.Image = video;
            //if (flag ==0)
            //{
            //    string file = Application.StartupPath + @"\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            //    video.Save(file);
            //    flag = 1;
            //    //FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);

            //    FinalVideo.SignalToStop();
            //    FinalVideo.WaitForStop();
            //    this.Close();

            //}
            

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FinalVideo.IsRunning) FinalVideo.Stop();
        }

        private void btnSaveImg_Click(object sender, EventArgs e)
        {
            //flag = 0;
            //FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);


            if (picDevice.Image != null)
            {
                Bitmap img = new Bitmap(picDevice.Image);
                img.Save(Application.StartupPath + @"\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg", ImageFormat.Jpeg);
                img.Dispose();
                img = null;
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            FinalVideo.SignalToStop();
            FinalVideo.WaitForStop();
            picDevice.Image = null;
            picDevice.BackColor = Color.Black;
        }
    }
}
