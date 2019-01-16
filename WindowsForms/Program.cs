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
            
            ImageViewer viewer = new ImageViewer(); //create an image viewer

            
            Image<Rgb, Byte> littleMarioReferenceImage = new Image<Rgb, byte>("LittleMario.bmp");
            //Image<Rgb, Byte> littleMarioReferenceImage = new Image<Rgb, byte>("AnotherMario.bmp"); //4k size

            Image<Rgb, Byte> mushroomGuyReferenceImage = new Image<Rgb, byte>("MushroomGuy.bmp");

            Image<Rgb, Byte> brickReferenceImage = new Image<Rgb, byte>("BrickBlock.bmp");

            Image<Rgb, Byte> floorReferenceImage = new Image<Rgb, byte>("FloorBlock.bmp");

            Image<Rgb, Byte> mysteryReferenceImage = new Image<Rgb, byte>("MysteryBlock.bmp");

            Application.Idle += new EventHandler(delegate (object sender, EventArgs e)
            {  //run this until application closed (close button click on image viewer)

                //4k Monitor : 1164x1019
                //Normal is half that
                Rectangle emulatorScreen = new System.Drawing.Rectangle(100, 100, 1164/2+100, 1019/2+100);
                var output = Vision.Helpers.Capture(emulatorScreen);

                var source =  new Emgu.CV.Image<Rgb, Byte>(output); //draw the image obtained from camera

                var imageWithGrid = Vision.Helpers.DrawGrid(source);
                //Next lets find mario!

                var marioFoundImage = Vision.Helpers.MatchBestTemplate(imageWithGrid, littleMarioReferenceImage, Color.Red);

                //Let find enamies
                var charactersFound = Vision.Helpers.MatchAllTemplate(marioFoundImage, mushroomGuyReferenceImage, Color.Blue);

                //Let find bricks
                
                var bricksFound = Vision.Helpers.MatchAllTemplate(charactersFound, brickReferenceImage, Color.Gold);
                bricksFound = Vision.Helpers.MatchAllTemplate(bricksFound, mysteryReferenceImage, Color.Gold);

                //Let find the floor blocks
                viewer.Image = Vision.Helpers.MatchAllTemplate(bricksFound, floorReferenceImage, Color.Gold);

            });
            viewer.ShowDialog(); //show the image viewer
        }
    }
}
