using System.Collections.Generic;
using System.Drawing;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.WinSize
{
    public class WinSize5x5 : AWinSize
    {
        public static AWinSize Singleton = new WinSize5x5();

        private WinSize5x5()
        {
            this._winSize = 5;
        }

        public override List<Point> FindListOfRelativePositions()
        {
            int halfWinSize = this._winSize / 2;

            //      (-2, -2)    (-1, -2)    ( 0, -2)    ( 1, -2)    ( 2, -2)
            //      (-2, -1)    (-1, -1)    ( 0, -1)    ( 1, -1)    ( 2, -1)
            //      (-2,  0)    (-1,  0)                ( 1,  0)    ( 2,  0)
            //      (-2,  1)    (-1,  1)    ( 0,  1)    ( 1,  1)    ( 2,  1)
            //      (-2,  2)    (-1,  2)    ( 0,  2)    ( 1,  2)    ( 2,  2)

            List<Point> result = new List<Point>(); // clockwise adding

            
            result.Add(new Point(-halfWinSize       , -halfWinSize      )); // upper-left           (-2, -2)
            result.Add(new Point(-halfWinSize + 1   , -halfWinSize      )); // next-left of upper   (-1, -2)
            result.Add(new Point(0                  , -halfWinSize      )); // upper                ( 0, -2)
            result.Add(new Point(halfWinSize - 1    , -halfWinSize      )); // next-right of upper  ( 1, -2)
            result.Add(new Point(halfWinSize        , -halfWinSize      )); // upper-right          ( 2, -2)

            result.Add(new Point(halfWinSize        , -halfWinSize + 1  )); // next-up of right     ( 2, -1)
            result.Add(new Point(halfWinSize        , 0                 )); // right                ( 2,  0)
            result.Add(new Point(halfWinSize        , halfWinSize - 1   )); // next-down of right   ( 2,  1)

            result.Add(new Point(halfWinSize        , halfWinSize       )); // lower-right          ( 2,  2)
            result.Add(new Point(halfWinSize - 1    , halfWinSize       )); // next-right of lower  ( 1,  2)
            result.Add(new Point(0                  , halfWinSize       )); // lower                ( 0,  2)
            result.Add(new Point(-halfWinSize + 1   , halfWinSize       )); // next-left of lower   (-1,  2)
            result.Add(new Point(-halfWinSize       , halfWinSize       )); // lower-left           (-2,  2)

            result.Add(new Point(-halfWinSize       , halfWinSize - 1   )); // next-down of left    (-2,  1)
            result.Add(new Point(-halfWinSize       , 0                 )); // left                 (-2,  0)
            result.Add(new Point(-halfWinSize       , -halfWinSize + 1  )); // next-down of left    (-2, -1)
            

            /*
            result.Add(new Point(-_halfWinSize       , -_halfWinSize      )); // (-2, -2)
            result.Add(new Point(-_halfWinSize + 1   , -_halfWinSize      )); // (-1, -2)
            result.Add(new Point(0                  , -_halfWinSize      )); // ( 0, -2)
            result.Add(new Point(_halfWinSize - 1    , -_halfWinSize      )); // ( 1, -2)
            result.Add(new Point(_halfWinSize        , -_halfWinSize      )); // ( 2, -2)

            result.Add(new Point(-_halfWinSize       , -_halfWinSize + 1  )); // (-2, -1)
            result.Add(new Point(-_halfWinSize + 1   , -_halfWinSize + 1  )); // (-1, -1)
            result.Add(new Point(0                  , -_halfWinSize + 1  )); // ( 0, -1)
            result.Add(new Point(_halfWinSize - 1    , -_halfWinSize + 1  )); // ( 1, -1)
            result.Add(new Point(_halfWinSize        , -_halfWinSize + 1  )); // ( 2, -1)

            result.Add(new Point(-_halfWinSize       , 0                 )); // (-2,  0)
            result.Add(new Point(-_halfWinSize + 1   , 0                 )); // (-1,  0)
            result.Add(new Point(_halfWinSize - 1    , 0                 )); // ( 1,  0)
            result.Add(new Point(_halfWinSize        , 0                 )); // ( 2,  0)

            result.Add(new Point(-_halfWinSize       , _halfWinSize - 1   )); // (-2,  1)
            result.Add(new Point(-_halfWinSize + 1   , _halfWinSize - 1   )); // (-1,  1)
            result.Add(new Point(0                  , _halfWinSize - 1   )); // ( 0,  1)
            result.Add(new Point(_halfWinSize - 1    , _halfWinSize - 1   )); // ( 1,  1)
            result.Add(new Point(_halfWinSize        , _halfWinSize - 1   )); // ( 2,  1)

            result.Add(new Point(-_halfWinSize       , _halfWinSize       )); // (-2,  2)
            result.Add(new Point(-_halfWinSize + 1   , _halfWinSize       )); // (-1,  2)
            result.Add(new Point(0                  , _halfWinSize       )); // ( 0,  2)
            result.Add(new Point(_halfWinSize - 1    , _halfWinSize       )); // ( 1,  2)
            result.Add(new Point(_halfWinSize        , _halfWinSize       )); // ( 2,  2)
            */
             
            return result;
        }
    }
}
