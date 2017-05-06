using IM.Imaging;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage
{
    public class LPImage
    {
        private int[] _binaryPatterns = { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768 };

        private bool[][][] _encodedImg;
        private int _width;
        private int _height;
        private int _lengthOfEncoding;


        public LPImage(int width, int height, int lengthOfEncoding)
        {
            this._width = width;
            this._height = height;
            this._lengthOfEncoding = lengthOfEncoding;

            Init();
        }


        public ABinaryValue Get(int x, int y, int encodingIdx)
        {
            if (this._encodedImg[y][x][encodingIdx] == true)
                return BinaryValueOne.Singleton;
            else
                return BinaryValueZero.Singleton;
        }


        public bool[] GetEncodedArray(int x, int y)
        {
            return this._encodedImg[y][x];
        }


        public int GetSumValue(int x, int y)
        {
            int value = 0;
            for (int i = 0; i < this._lengthOfEncoding; i++)
                value += this.Get(x, y, i).GetValue() * this._binaryPatterns[i];
            return value;
        }


        public void Set(int x, int y, int encodingIdx, ABinaryValue binaryValue) 
        {
            this._encodedImg[y][x][encodingIdx] = binaryValue.GetBoolValue();
        }


        public Image3D ConvertToImage3D()
        {
            Image3D resultImg = new Image3D(this._width, this._height, 1, 1); // 1-depth, 1-band image
            for (int y = 0; y < this._height; y++)
                for (int x = 0; x < this._width; x++)
                    resultImg.Set(x, y, 0, 0, this.GetSumValue(x, y));
            return resultImg;
        }


        // ====================================================================


        private void Init()
        {
            this._encodedImg = new bool[this._height][][];
            for (int y = 0; y < this._height; y++)
            {
                this._encodedImg[y] = new bool[this._width][];
                for (int x = 0; x < this._width; x++)
                {
                    this._encodedImg[y][x] = new bool[this._lengthOfEncoding];
                    for (int e = 0; e < this._lengthOfEncoding; e++)
                        this._encodedImg[y][x][e] = false;
                }
            }
        }
    }
}
