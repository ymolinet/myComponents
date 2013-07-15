using System;
using System.Collections.Generic;
using System.Text;

namespace myComponents.Object
{
    public class IntItem
    {
        private String _text;
        private Int32 _value;

        public IntItem(String Text, Int32 Value)
        {
            _text = Text;
            _value = Value;
        }

        public IntItem(IntItem anItem)
        {
            _text = anItem.Text;
            _value = anItem.Value;
        }

        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public Int32 Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public override String ToString()
        {
            return _text;
        }

    }
}
