using System.Collections.Generic;
using System.Drawing;
using IM.Lib.Descriptor.LocalPatternDescriptor.WinSize;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo
{
    public class SquareNeighborTopo : ANeighborTopo
    {
        private const float WEIGHT_NEIGHBOR = 1.0f;
        private const float WEIGHT_CENTER = 0.0f;

        public SquareNeighborTopo(AWinSize winSize)
            : base(winSize)
        {            
        }

        public override List<RectangleF> FindListOfNeighbors(int x, int y)
        {
            List<RectangleF> result = new List<RectangleF>(); // clockwise adding
            foreach (Point relativePosition in _relativePositions)
            {
                Point position = new Point(x + relativePosition.X, y + relativePosition.Y);
                result.Add(new RectangleF(position.X, position.Y, WEIGHT_NEIGHBOR, WEIGHT_CENTER));
            }
            return result;
        }
    }
}
