using System;
using System.Collections.Generic;
using System.Text;

namespace myComponents.Object
{
    public class Int64Item
    {
        private String _text;
        private Int64 _value;

        public Int64Item(String Text, Int64 Value)
        {
            _text = Text;
            _value = Value;
        }

        public Int64Item(Int64Item anItem)
        {
            _text = anItem.Text;
            _value = anItem.Value;
        }

        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public Int64 Value
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
