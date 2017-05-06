
namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage
{
    public abstract class ABinaryValue
    {
        protected int _value;

        public abstract bool GetBoolValue();

        public int GetValue()
        {
            return this._value;
        }
    }
}
