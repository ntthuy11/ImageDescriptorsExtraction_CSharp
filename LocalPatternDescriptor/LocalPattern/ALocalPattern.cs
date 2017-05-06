using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;
using IM.Imaging;
using System;


namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern
{
    public abstract class ALocalPattern
    {        
        protected ANeighborTopo _neighborTopo;
        protected int _lengthOfEncoding;

        protected ALocalPattern(ANeighborTopo neighborTopo)
        {
            this._neighborTopo = neighborTopo;
        }


        /// <summary>
        /// Abstract function for TransformWindow
        /// </summary>
        /// <param name="_srcImg">Source image</param>
        /// <param name="x">x of the current position</param>
        /// <param name="y">y of the current position</param>
        /// <returns>The _value calculated based on its neighborhood</returns>
        public abstract void TransformWindow(Image3D srcImg, int srcBand, int x, int y, LPImage lpImg);


        // ---------------------------------------------------------


        /// <summary>
        /// Run Local Pattern on all image bands
        /// </summary>
        /// <param name="_srcImg">Source image</param>        
        /// <returns>LocalPattern-ed image</returns>
        public Image3D ApplyOnImg(Image3D srcImg)
        {
            Image3D resultImg = new Image3D(srcImg.Width, srcImg.Height, srcImg.Depth, srcImg.NumBands);
            //Image3D4 resultImg = new Image3D4(srcImg.Width, srcImg.Height, srcImg.MaxDepth, srcImg.NumBands);
            for (int i = 0; i < srcImg.NumBands; i++)
                ApplyOnImg(srcImg, i, resultImg, i);
            return resultImg;
        }


        public void ApplyOnImg(Image3D srcImg, Image3D resultImg)
        {
            //for (int i = 0; i < srcImg.NumBands; i++)
            //    ApplyOnImg(srcImg, i, resultImg, i);
            ApplyOnImg(srcImg, 0, resultImg, 0);
        }


        /// <summary>
        /// Run Local Pattern on one image band (in detail)
        /// </summary>
        /// <param name="srcImg">Source image</param>
        /// <param name="srcBand">Source image's band</param>
        /// <param name="resultImg">Result image</param>
        /// <param name="resultBand">Result image's band</param>
        public void ApplyOnImg(Image3D srcImg, int srcBand, Image3D resultImg, int resultBand)
        {
            int imgH = resultImg.Height;
            int imgW = resultImg.Width;
            LPImage lpImg = new LPImage(imgW, imgH, this._lengthOfEncoding);

            int halfWinSize = this._neighborTopo.GetWinSize() / 2;

            int yFrom = halfWinSize;
            int yTo = imgH - 1 - halfWinSize;

            int xFrom = halfWinSize;
            int xTo = imgW - 1 - halfWinSize;

            for (int y = yFrom; y <= yTo; y++)
            {
                for (int x = xFrom; x <= xTo; x++)
                {
                    TransformWindow(srcImg, srcBand, x, y, lpImg);
                    resultImg.Set(x, y, 0, resultBand, lpImg.GetSumValue(x, y)); // for Image3D
                    //resultImg.SetPixelValue(new Point(x, y), resultBand, lpImg.GetSumValue(x, y)); // store the Sum _value to resultImg to the current position
                }
            }
        }


        // ---------------------------------------------------------


        public IntHistogram CalcHistogram(Image3D srcImg)
        {
            //
            return CalcHistogram(srcImg, 0);
        }


        public IntHistogram CalcHistogram(Image3D srcImg, int srcBand)
        {
            int imgH = srcImg.Height;
            int imgW = srcImg.Width;
            LPImage lpImg = new LPImage(imgW, imgH, this._lengthOfEncoding);

            int halfWinSize = this._neighborTopo.GetWinSize() / 2;

            int yFrom = halfWinSize;
            int yTo = imgH - 1 - halfWinSize;

            int xFrom = halfWinSize;
            int xTo = imgW - 1 - halfWinSize;

            // --- Run
            TransformWindow(srcImg, srcBand, xFrom, yFrom, lpImg);
            IntHistogram globalHist = new IntHistogram(lpImg.GetEncodedArray(xFrom, yFrom));

            for (int y = yFrom; y <= yTo; y++)
            {
                for (int x = xFrom; x <= xTo; x++)
                {
                    if (y == yFrom && x == xFrom) continue;

                    TransformWindow(srcImg, srcBand, x, y, lpImg);
                    IntHistogram hist = new IntHistogram(lpImg.GetEncodedArray(x, y));
                    globalHist.AddHistogram(hist);
                }
            }

            return globalHist;
        }
    }


    // ========================================================================


    public class IntHistogram
    {
        private Image3D _srcImg;
        private int _srcBand;

        private int[] _histogram;
        private int _sizeOfHistogramBin;


        /*
         * srcImg: must be scaled to [0..255]
         */
        public IntHistogram(Image3D srcImg, int srcBand, int histogramSize)
        {
            this._srcImg = srcImg;
            this._srcBand = srcBand;

            //const int max16bitsVal = 65536;
            const int max8bitsVal = 256;
            _sizeOfHistogramBin = max8bitsVal / histogramSize;
            this._histogram = Create(histogramSize);
        }


        public IntHistogram(bool[] encodedArray)
        {
            this._histogram = new int[encodedArray.Length];
            for (int i = 0; i < encodedArray.Length; i++)
            {
                if (encodedArray[i] == true)
                    _histogram[i] = 1;
                else
                    _histogram[i] = 0;
            }
        }


        // --------------------------------------------------------------------


        public int Size
        {
            get { return this._histogram.Length; }
        }


        public int GetValueAtBin(int idx)
        {
            return this._histogram[idx];
        }


        public int Sum()
        {
            int result = 0;
            for (int i = 0; i < this._histogram.Length; i++)
                result += this._histogram[i];
            return result;
        }


        public void AddHistogram(IntHistogram hist)
        {
            for (int i = 0; i < this._histogram.Length; i++)
                this._histogram[i] += hist.GetValueAtBin(i);
        }


        public void Equalize(Image3D resultImg, int resultBand)
        {
            int imgWidth = resultImg.Width;
            int imgHeight = resultImg.Height;

            // initialize Sum of _histogram
            int histogramSize = this._histogram.Length;
            int[] sumOfHistogram = new int[histogramSize];
            for (int i = 0; i < histogramSize; i++)
                sumOfHistogram[i] = 0;

            // calculate sumOfHistogram
            int dummySum = 0;
            for (int i = 0; i < histogramSize; i++)
            {
                dummySum += this._histogram[i];
                sumOfHistogram[i] = dummySum;
            }

            // histEq
            int areaOfImg = imgHeight * imgWidth;
            for (int y = 0; y < imgHeight; y++)
            {
                for (int x = 0; x < imgWidth; x++)
                {
                    // for Image3D
                    int binIdx = (int)(this._srcImg.Get(x, y, 0, this._srcBand) / _sizeOfHistogramBin);
                    resultImg.Set(x, y, 0, resultBand, (histogramSize * 1.0f / areaOfImg) * sumOfHistogram[binIdx]);

                    // for Image3D4
                    //int binIdx = (int)(this._srcImg.GetPixelValue(new Point(x, y), this._srcBand) / _sizeOfHistogramBin);
                    //resultImg.SetPixelValue(new Point(x, y), resultBand, (histogramSize * 1.0f / areaOfImg) * sumOfHistogram[binIdx]);
                }
            }
        }


        /*
         * Reference: M. PietiKainen, G. Zhao, A. Hadid, T. Ahonen, "Computer Vision using LOcal Binary Patterns," Springer, 2011
         *            Chapter 4: Texture Classification and Segmentation
         */
        public float CompareUsingGstatistic(IntHistogram aHist)
        {
            float sumOfCurrHist = (float)this.Sum();
            float sumOfAHist = (float)aHist.Sum();
            float sumOfLogOfCurrHist = this.SumOfLog();
            float sumOfLogOfAHist = aHist.SumOfLog();

            // 1st component of G
            float firstComponent = sumOfLogOfCurrHist + sumOfLogOfAHist;

            // 2nd component of G
            float secondComponent_currHist = sumOfCurrHist * Log(sumOfCurrHist);
            float secondComponent_aHist = sumOfAHist * Log(sumOfAHist);
            float secondComponent = secondComponent_currHist + secondComponent_aHist;

            // 3rd component of G
            float thirdComponent = 0;
            for (int i = 0; i < this._histogram.Length; i++)
            {
                float thirdComponent_first = this.GetValueAtBin(i) + aHist.GetValueAtBin(i);
                float thirdComponent_second = Log(thirdComponent_first);
                thirdComponent += thirdComponent_first * thirdComponent_second;
            }

            // 4th component of G
            float fourthComponent_first = sumOfCurrHist + sumOfAHist;
            float fourthComponent_second = Log(fourthComponent_first);
            float fourthComponent = fourthComponent_first * fourthComponent_second;

            //
            return 2 * (firstComponent - secondComponent - thirdComponent + fourthComponent);
        }


        // --------------------------------------------------------------------


        private int[] Create(int histogramSize)
        {
            // initialize _histogram
            int[] histogram = new int[histogramSize];
            for (int i = 0; i < histogramSize; i++)
                histogram[i] = 0;

            // calculate _histogram of _srcImg
            for (int y = 0; y < this._srcImg.Height; y++)
            {
                for (int x = 0; x < this._srcImg.Width; x++)
                {
                    int binIdx = (int)(this._srcImg.Get(x, y, 0, this._srcBand) / _sizeOfHistogramBin); // for Image3D
                    //int binIdx = (int)(this._srcImg.GetPixelValue(new Point(x, y), this._srcBand) / _sizeOfHistogramBin); // for Image3D4
                    histogram[binIdx]++;
                }
            }

            return histogram;
        }


        private float SumOfLog()
        {
            float result = 0;
            for (int i = 0; i < this._histogram.Length; i++)
                result += this._histogram[i] * Log(this._histogram[i]);
            return result;
        }


        private float Log(float val)
        {
            if (val == 0)
                return 0;
            return (float)Math.Log((double)val);
        }
    }
}
