using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.XFeatures2D;
using Emgu.CV.Util;

namespace Vision
{
    public class Helpers
    {
        public static Bitmap Capture(Rectangle Region)
        {
            var bmp = new Bitmap(Region.Width, Region.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(Region.Location, Point.Empty, Region.Size, CopyPixelOperation.SourceCopy);
                g.Flush();
            }

            return bmp;
        }

        public static Emgu.CV.Image<Rgb, byte> MatchAllTemplate(Emgu.CV.Image<Rgb, byte> source, Emgu.CV.Image<Rgb, byte> template, Color matchHighlight)
        {   Image<Rgb, byte> imageToShow = source.Copy();

            using (Image<Gray, float> result = source.MatchTemplate(template, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed))
            {
                for (int i = 0; i < result.Height; i++)
                {
                    for (int j = 0; j < result.Width; j++)
                    {
                        var a = result.Data.GetValue(i, j, 0);
                        if ((float)a > 0.4)
                        {

                            // This is a match. Do something with it, for example draw a rectangle around it.
                            Rectangle match = new Rectangle(new Point(j,i), template.Size);
                            imageToShow.Draw(match, new Rgb(matchHighlight), 3);

                        }
                    }
                }
            }

            // Show imageToShow in an ImageBox (here assumed to be called imageBox1)
            return imageToShow;
        }

        public static Emgu.CV.Image<Rgb, byte> MatchBestTemplate(Emgu.CV.Image<Rgb, byte> source, Emgu.CV.Image<Rgb, byte> template, Color matchHighlight)
        {
            Image<Rgb, byte> imageToShow = source.Copy();

            using (Image<Gray, float> result = source.MatchTemplate(template, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed))
            {
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;


                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                if (maxValues[0] > 0.4)
                {

                    // This is a match. Do something with it, for example draw a rectangle around it.
                    Rectangle match = new Rectangle(maxLocations[0], template.Size);
                    imageToShow.Draw(match, new Rgb(matchHighlight), 3);

                }
            }

            // Show imageToShow in an ImageBox (here assumed to be called imageBox1)
            return imageToShow;
        }

        public static Emgu.CV.Image<Rgb, byte> DrawGrid(Emgu.CV.Image<Rgb, byte> source)
        {
            Image<Rgb, byte> imageToShow = source.Copy();

            for (int i = 0; i < source.Width; i+=32)
            {
                for (int j = 0; j < source.Height; j += 32)
                {
                    Rectangle match = new Rectangle(i,j, 32,32);
                    imageToShow.Draw(match, new Rgb(Color.White), 1);

                }
            }
            return imageToShow;
        }

            //public static Image<Bgr, Byte> Draw(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage)
            //{
            //    Mat homography = null;

            //    FastDetector fastCPU = new FastDetector(10, true);
            //    VectorOfKeyPoint modelKeyPoints;
            //    VectorOfKeyPoint observedKeyPoints;
            //    Matrix<int> indices;

            //    BriefDescriptorExtractor descriptor = new BriefDescriptorExtractor();

            //    Matrix<byte> mask;
            //    int k = 2;
            //    double uniquenessThreshold = 0.8;

            //    //extract features from the object image
            //    modelKeyPoints = fastCPU.DetectKeyPointsRaw(modelImage, null);
            //    Matrix<Byte> modelDescriptors = descriptor.ComputeDescriptorsRaw(modelImage, null, modelKeyPoints);

            //    // extract features from the observed image
            //    observedKeyPoints = fastCPU.DetectKeyPointsRaw(observedImage, null);
            //    Matrix<Byte> observedDescriptors = descriptor.ComputeDescriptorsRaw(observedImage, null, observedKeyPoints);
            //    BruteForceMatcher<Byte> matcher = new BruteForceMatcher<Byte>(DistanceType.L2);
            //    matcher.Add(modelDescriptors);

            //    indices = new Matrix<int>(observedDescriptors.Rows, k);
            //    using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Rows, k))
            //    {
            //        matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
            //        mask = new Matrix<byte>(dist.Rows, 1);
            //        mask.SetValue(255);
            //        Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
            //    }

            //    int nonZeroCount = CvInvoke.cvCountNonZero(mask);
            //    if (nonZeroCount >= 4)
            //    {
            //        nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
            //        if (nonZeroCount >= 4)
            //            homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(
            //            modelKeyPoints, observedKeyPoints, indices, mask, 2);
            //    }

            //    //Draw the matched keypoints
            //    Image<Bgr, Byte> result = Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
            //       indices, new Bgr(255, 255, 255), new Bgr(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.Default);

            //    #region draw the projected region on the image
            //    if (homography != null)
            //    {  //draw a rectangle along the projected model
            //        Rectangle rect = modelImage.ROI;
            //        PointF[] pts = new PointF[] {
            // new PointF(rect.Left, rect.Bottom),
            // new PointF(rect.Right, rect.Bottom),
            // new PointF(rect.Right, rect.Top),
            // new PointF(rect.Left, rect.Top)};
            //        homography.ProjectPoints(pts);

            //        result.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Bgr(Color.Red), 5);
            //    }
            //    #endregion

            //    return result;
            //}

        }
}
