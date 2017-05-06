using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;

// Reference: 
// D. Kim, J. Jung, T. T. Nguyen, D. Kim, M. Kim, K. H. Kwon, J. W. Jeon, "An FPGA-based Parallel Hardware Architecture for Real-time Eye Detection,"
//          J. Semiconductor Technology and Science, vol. 12, no. 2, pp. 150-161, 2012

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.ModifiedLocalPattern
{
    public class ModifiedLBP : AModifiedLocalPattern
    {
        public ModifiedLBP(ANeighborTopo neighborTopo)
            : base(neighborTopo)
        {
            this._lengthOfEncoding = this._neighborTopo.NPointsOnBorder();
        }

        public override void DoTransform(int x, int y, int encodingIdx, float avg, float valOfNeighbor, LPImage lpImg)
        {
            if (avg <= valOfNeighbor)
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
            else
                lpImg.Set(x, y, encodingIdx, BinaryValueZero.Singleton);
        }
    }
}
