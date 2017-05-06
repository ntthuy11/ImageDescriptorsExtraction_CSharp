
namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage
{
    public class BinaryValueOne : ABinaryValue
    {
        public static ABinaryValue Singleton = new BinaryValueOne();

        private BinaryValueOne()
        {
            this._value = 1;
        }

        public override bool GetBoolValue()
        {
            return true;
        }
    }
}
