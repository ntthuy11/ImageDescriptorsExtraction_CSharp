using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.ModifiedLocalPattern
{
    public class ModifiedLTP : AModifiedLocalPattern
    {
        private float _thresholdForTenary;
        private int _stepForTenary;


        public ModifiedLTP(ANeighborTopo neighborTopo, float thresholdForTenaryPattern)
            : base(neighborTopo)
        {
            this._lengthOfEncoding      = this._neighborTopo.NPointsOnBorder() * 2;
            this._thresholdForTenary    = thresholdForTenaryPattern;
            this._stepForTenary         = this._neighborTopo.NPointsOnBorder();
        }


        public override void DoTransform(int x, int y, int encodingIdx, float avg, float valOfNeighbor, LPImage lpImg)
        {
            if (avg + this._thresholdForTenary <= valOfNeighbor)
            {
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
                lpImg.Set(x, y, encodingIdx + this._stepForTenary, BinaryValueZero.Singleton);
            }
            else
            {
                if (valOfNeighbor <= avg - this._thresholdForTenary)
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
