using System.Collections.Generic;
using System.Drawing;
using IM.Lib.Descriptor.LocalPatternDescriptor.WinSize;


namespace IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo
{
    public abstract class ANeighborTopo
    {
        protected AWinSize _winSize;
        protected List<Point> _relativePositions;

        protected ANeighborTopo(AWinSize winSize)
        {
            this._winSize = winSize;
            this._relativePositions = this._winSize.FindListOfRelativePositions();
        }


        /// <summary>
        /// Run the coordinate list of neighborhood pixels
        /// </summary>
        /// <param name="x">Center pixel's x</param>
        /// <param name="y">Center pixel's y</param>
        /// <returns>List of neighborhood coordinates</returns>
        public abstract List<RectangleF> FindListOfNeighbors(int x, int y);


        // ---------------------------------------------------------


        public int GetWinSize()
        {
            return this._winSize.GetWinSize();
        }


        public int NPointsOnBorder()
        {
            int perimeter = this.GetWinSize() * 4; // 4 edges
            return perimeter - 4; // - 4 corners
        }
    }
}
