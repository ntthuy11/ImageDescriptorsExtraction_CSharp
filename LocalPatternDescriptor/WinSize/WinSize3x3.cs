using System.Collections.Generic;
using System.Drawing;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.WinSize
{
    public class WinSize3x3 : AWinSize
    {
        public static AWinSize Singleton = new WinSize3x3();

        private WinSize3x3()
        {
            this._winSize = 3;
        }

        public override List<Point> FindListOfRelativePositions()
        {
            int halfWinSize = this._winSize / 2;

            List<Point> result = new List<Point>(); // clockwise adding
            result.Add(new Point(-halfWinSize   , -halfWinSize)); // upper-left
            result.Add(new Point(0              , -halfWinSize)); // upper
            result.Add(new Point(halfWinSize    , -halfWinSize)); // upper-right
            result.Add(new Point(halfWinSize    , 0));            // right
            result.Add(new Point(halfWinSize    , halfWinSize));  // lower-right
            result.Add(new Point(0              , halfWinSize));  // lower
            result.Add(new Point(-halfWinSize   , halfWinSize));  // lower-left
            result.Add(new Point(-halfWinSize   , 0));            // left

            return result;
        }
    }
}
