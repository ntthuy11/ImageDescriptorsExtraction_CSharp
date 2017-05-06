using System.Collections.Generic;
using System.Drawing;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.WinSize
{
    public abstract class AWinSize
    {
        protected int _winSize;

        public abstract List<Point> FindListOfRelativePositions();


        // ---------------------------------------------------------


        public int GetWinSize()
        {
            return this._winSize;
        }
    }
}
