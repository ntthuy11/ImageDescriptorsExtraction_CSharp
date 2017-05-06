using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;

// Reference: 
// Ojala, T., Pietikainen, M.,  Harwood, D.: A Comparative Study of Texture Measures with Classification based on Feature Distributions, Pattern Recogn. 29, 51-59 (1996)

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.BasicLocalPattern
{
    public class BasicLBP : ABasicLocalPattern
    {
        public BasicLBP(ANeighborTopo neighborTopo)
            : base(neighborTopo)
        {
            this._lengthOfEncoding = this._neighborTopo.NPointsOnBorder();
        }

        public override void DoTransform(int x, int y, int encodingIdx, float centerVal, float valOfNeighbor, LPImage lpImg)
        {
            if (centerVal <= valOfNeighbor)
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
            else
                lpImg.Set(x, y, encodingIdx, BinaryValueZero.Singleton);
        }
    }
}
