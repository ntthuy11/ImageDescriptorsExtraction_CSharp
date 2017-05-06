using IM.Imaging;
using IM.Lib.Mathematics;
using System;

namespace IM.Lib.Descriptor.HistogramOfOrientedGradientsDescriptor
{
    // By Thuy Tuong Nguyen, April 16, 2013

    /* -------------------- [1] this code is implemented based on this paper --------------------
     * http://www.mathworks.com/matlabcentral/fileexchange/28689-hog-descriptor-for-matlab/content/HOG.m
     * Image descriptor based on Histogram of Orientated Gradients for gray-level images. 
     * This code was developed for the work: 
     *          O. Ludwig, D. Delgado, V. Goncalves, and U. Nunes, 'Trainable Classifier-Fusion Schemes: An Application To Pedestrian Detection,' 
     *          In: 12th International IEEE Conference On Intelligent Transportation Systems, 2009, St. Louis, 2009. V. 1. P. 432-437. 
     *
     * -------------------- [2] original paper to refer --------------------
     * Navneet Dalal , Bill Triggs, “Histograms of Oriented Gradients for Human Detection,” Int Conference on Computer Vision and Pattern Recognition (CVPR'05)
     *           Volume 1, p.886-893, June 20-26, 2005
     * http://lear.inrialpes.fr/people/triggs/pubs/Dalal-cvpr05.pdf
     *
     * -------------------- [3] another document --------------------
     * http://cs.bilkent.edu.tr/~cansin/projects/cs554-vision/pedestrian-detection/pedestrian-detection-paper.pdf
     */

    public class RectangularHOG
    {

        public static float[] HOG(Image3D img, int imgBand)
        {
            return HOG(img, imgBand, 3, 3, 9);
        }


        public static float[] HOG(Image3D img, int imgBand, int nHOGwinX, int nHOGwinY, int nHistBins)
        {
            int imgW = img.Width;
            int imgH = img.Height;

            // calculate gradients in X and Y directions
            Image3D imgGradientX    = CalcImgGradientX(img, imgBand);
            Image3D imgGradientY    = CalcImgGradientY(img, imgBand);
            
            // calculate angles and magnitudes of X and Y gradients
            Image3D angles          = new Image3D(imgW, imgH, 1, 1);
            Image3D magnitudes      = new Image3D(imgW, imgH, 1, 1);
            CalcAnglesAndMagnitudes(imgGradientX, imgGradientY, angles, magnitudes);

            // init for HOG
            float[] result  = new float[nHOGwinX * nHOGwinY * nHistBins];
            int stepX       = (int)Math.Floor(imgW * 1.0 / (nHOGwinX + 1));
            int stepY       = (int)Math.Floor(imgH * 1.0 / (nHOGwinY + 1));

            Image3D subAngles     = new Image3D(stepX * 2, stepY * 2, 1, 1);
            Image3D subMagnitudes = new Image3D(stepX * 2, stepY * 2, 1, 1);
            float[] subHist       = new float[nHistBins]; 

            float e2  = 0.01f;
            int count = 0;

            // run HOG
            for (int i = 0; i < nHOGwinY; i++)
            {
                for (int j = 0; j < nHOGwinX; j++)
                {   
                    // consider each pair of subimages (considering vertical+horizontal pairs is significantly better than considering vertical/horizontal pairs only)
                    // (i.e., we defined nHOGwinX = nHOGwinY = 3, that means we define the image into 4 x 4
                    //        we have 3 pairs on each direction X and Y (0 and 1, 1 and 2, 2 and 3). Totally, we have 3 x 3 = 9 pairs in 4 x 4 subimages)
                    angles.CopySubImageInto(j * stepX, i * stepY, 0, subAngles);
                    magnitudes.CopySubImageInto(j * stepX, i * stepY, 0, subMagnitudes);                   

                    // calculate the sub-histogram
                    for (int bin = 1; bin <= nHistBins; bin++)
                    {
                        subHist[bin-1] = 0;
                        float angleLimit = (float)(-Math.PI + (2*Math.PI/nHistBins)*bin);

                        for (int k = 0; k < subAngles.ImageSize; k++)
                        {
                            if (subAngles.Data[0][k] < angleLimit)
                            {
                                subHist[bin-1] += subMagnitudes.Data[0][k];
                                subAngles.Data[0][k] = float.MaxValue; // we already visited this angle and used it for the smaller angle
                            }
                        }
                    }

                    // normalize subHist & store subHist to the result
                    float normOfSubHist = NormOfHistogram(subHist);
                    for (int bin = 0; bin < nHistBins; bin++)
                        result[count * nHistBins + bin] = subHist[bin] / (normOfSubHist + e2);                                            

                    count++;
                }
            }
            return result;
        }


        // ================================== PRIVATE ==================================


        private static Image3D CalcImgGradientX(Image3D img, int imgBand)
        {
            // define the kernel for X
            float[] data = new float[] { -1f, 0f, 1f }; // <----
            //Image3D kernel = new Image3D(3, 1, 1, data);

            // calculate the gradient
            Image3D imgGradient = new Image3D(img.Width, img.Height, 1, 1);
            //new Convolution().Convolve1D_X(img, imgBand, imgGradient, 0, kernel, BoundaryConditions.Mirror);
            Convolve._1DX(img.Data[imgBand], imgGradient.Data[0], img.Width, img.Height, img.Depth,
                data, BoundaryCondition.Mirror);


            return imgGradient;
        }


        private static Image3D CalcImgGradientY(Image3D img, int imgBand)
        {
            // define the kernel for X
            float[] data = new float[] { 1f, 0f, -1f } ; // <----
            //Image3D kernel = new Image3D(3, 1, 1, data);

            // calculate the gradient
            Image3D imgGradient = new Image3D(img.Width, img.Height, 1, 1);
            //new Convolution().Convolve1D_Y(img, imgBand, imgGradient, 0, kernel, BoundaryConditions.Mirror);
            Convolve._1DY(img.Data[imgBand], imgGradient.Data[0], img.Width, img.Height, img.Depth,
                data, BoundaryCondition.Mirror);

            return imgGradient;
        }


        private static void CalcAnglesAndMagnitudes(Image3D imgGradientX, Image3D imgGradientY, Image3D angles, Image3D magnitudes)
        {
            for (int i = 0; i < imgGradientX.ImageSize; i++)
            {
                float xData           = imgGradientX.Data[0][i];
                float yData           = imgGradientY.Data[0][i];
                angles.Data[0][i]     = (float)Math.Atan2(xData, yData);
                magnitudes.Data[0][i] = (float)Math.Sqrt(xData * xData + yData * yData);
            }
        }


        private static float NormOfHistogram(float[] hist)
        {
            float sum = 0;
            for (int i = 0; i < hist.Length; i++)
                sum += hist[i] * hist[i];
            return (float) Math.Sqrt(sum);
        }
    }
}
