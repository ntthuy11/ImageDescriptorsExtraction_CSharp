using IM.Database.Models.Annotation;
using IM.Imaging;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.BasicLocalPattern;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.CenterNeighborAvgLocalPattern;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.CenterSymmetricLocalPattern;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.CentralizedLocalPattern;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.ModifiedLocalPattern;
using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.WinSize;
using IM.Lib.Segmentation.SegmentedObject;
using System.Collections.Generic;


namespace IM.Lib.Descriptor
{
    public class LocalPatternFeature : FeatureCalculator
    {
        public enum Type
        {
            BasicLBP = 1,           // Local Binary Pattern
            CenterNeighborAvgLBP,
            CenterSymmetricLBP,
            CentralizedLBP,
            ModifiedLBP//,
            /*BasicLTP,               // Local Tenary Pattern
            CenterNeighborAvgLTP,
            CenterSymmetricLTP,
            CentralizedLTP,
            ModifiedLTP*/
        }

        public enum NeighborTopo
        {
            Circle = 1,
            Ellipse,
            Square
        }

        public enum WinSize
        {
            Win3x3 = 1,
            Win5x5,
        }


        // ------


        private ALocalPattern _localPattern;


        public LocalPatternFeature()
            : this(Type.CenterNeighborAvgLBP, NeighborTopo.Circle, WinSize.Win3x3)
        {

        }


        public LocalPatternFeature(ALocalPattern localPattern)
        {
            this._localPattern = localPattern;
        }


        public LocalPatternFeature(Type enumType, NeighborTopo enumTopo, WinSize enumSize)
        {
            AWinSize winSize;
            ANeighborTopo neighborTopo;

            // AWinSize
            switch (enumSize)
            {                
                case WinSize.Win5x5:             winSize = WinSize5x5.Singleton;                                 break;
                default:                         winSize = WinSize3x3.Singleton;                                 break;
            }

            // ANeighborTopo
            switch (enumTopo)
            {
                case NeighborTopo.Circle:        neighborTopo = new CircleNeighborTopo(winSize);                 break;
                case NeighborTopo.Ellipse:       neighborTopo = new EllipseNeighborTopo(winSize);                break;
                default:                         neighborTopo = new SquareNeighborTopo(winSize);                 break;                
            }

            // ALocalPattern
            switch (enumType)
            {
                case Type.CenterNeighborAvgLBP:  this._localPattern = new CenterNeighborAvgLBP(neighborTopo);    break;
                case Type.CenterSymmetricLBP:    this._localPattern = new CenterSymmetricLBP(neighborTopo);      break;
                case Type.CentralizedLBP:        this._localPattern = new CentralizedLBP(neighborTopo);          break;
                case Type.ModifiedLBP:           this._localPattern = new ModifiedLBP(neighborTopo);             break;
                default:                         this._localPattern = new BasicLBP(neighborTopo);                break;
            }
        }


        // ------------------------------------


        public override Dictionary<string, float> CalcFeatureValues(Image3D fullImg, int band)
        {
            return CalcFeatures(fullImg, band);
        }


        public override Dictionary<string, float> CalcFeatureValuesWithSegmentedObj(Image3D fullImg, int band, SegmentedObjPO segmentedObj)
        {
            Image3D img = SegmentedObjUtilizer.GetObjImage(fullImg, band, segmentedObj, 0);
            return CalcFeatures(img, 0);
        }


        public override string ToString()
        {
            return "Local Pattern Feature";
        }


        // ------------------------------------


        private Dictionary<string, float> CalcFeatures(Image3D img, int band)
        {
            IntHistogram lpHist = this._localPattern.CalcHistogram(img, band);

            Dictionary<string, float> featureValues = new Dictionary<string, float>();
            for (int i = 0; i < lpHist.Size; i++)
                featureValues.Add("LP HistBin" + (i + 1), (float)(lpHist.GetValueAtBin(i) * 1.0 / img.ImageSize));

            return featureValues;
        }
    }
}
