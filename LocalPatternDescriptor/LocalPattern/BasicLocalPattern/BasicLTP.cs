using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.BasicLocalPattern
{
    public class BasicLTP : ABasicLocalPattern
    {        
        private float _thresholdForTenary;
        private int _stepForTenary;


        public BasicLTP(ANeighborTopo neighborTopo, float thresholdForTenaryPattern)
            : base(neighborTopo)
        {
            this._lengthOfEncoding      = this._neighborTopo.NPointsOnBorder() * 2;
            this._thresholdForTenary    = thresholdForTenaryPattern;
            this._stepForTenary         = this._neighborTopo.NPointsOnBorder();
        }


        public override void DoTransform(int x, int y, int encodingIdx, float centerVal, float valOfNeighbor, LPImage lpImg)
        {
            if (centerVal + this._thresholdForTenary <= valOfNeighbor)
            {
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
                lpImg.Set(x, y, encodingIdx + this._stepForTenary, BinaryValueZero.Singleton);
            }
            else
            {
                if (valOfNeighbor <= centerVal - this._thresholdForTenary)
                {
                    lpImg.Set(x, y, encodingIdx, BinaryValueZero.Singleton);
                    lpImg.Set(x, y, encodingIdx + this._stepForTenary, BinaryValueOne.Singleton);
                }
                else
                {
                    lpImg.Set(x, y, encodingIdx, BinaryValueZero.Singleton);
                    lpImg.Set(x, y, encodingIdx + this._stepForTenary, BinaryValueZero.Singleton);
                }
            }
        }
    }
}
