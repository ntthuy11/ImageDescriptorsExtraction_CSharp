
namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage
{
    public class BinaryValueZero : ABinaryValue
    {
        public static ABinaryValue Singleton = new BinaryValueZero();

        private BinaryValueZero()
        {
            this._value = 0;
        }

        public override bool GetBoolValue()
        {
            return false;
        }
    }
}
