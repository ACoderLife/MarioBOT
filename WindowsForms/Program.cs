using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.Drawing;

namespace WindowsForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            ImageViewer viewer = new ImageViewer(); //create an image viewer

            
            Image<Rgb, Byte> littleMarioReferenceImage = new Image<Rgb, byte>("LittleMario.bmp");
            //Image<Rgb, Byte> littleMarioReferenceImage = new Image<Rgb, byte>("AnotherMario.bmp");

            Application.Idle += new EventHandler(delegate (object sender, EventArgs e)
            {  //run this until application closed (close button click on image viewer)

                //4k Monitor : 1164x1019
                //Normal is half that
                Rectangle emulatorScreen = new System.Drawing.Rectangle(100, 100, 1164/2+100, 1019/2+100);
                var output = OpenCIHelloWorld.Helpers.Capture(emulatorScreen);

                var source =  new Emgu.CV.Image<Rgb, Byte>(output); //draw the image obtained from camera

                //Next lets find mario!

                viewer.Image = OpenCIHelloWorld.Helpers.TemplateMatcher(source, littleMarioReferenceImage, Color.Red);


            });
            viewer.ShowDialog(); //show the image viewer
        }
    }
}
