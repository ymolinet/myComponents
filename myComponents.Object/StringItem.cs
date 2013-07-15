using System;
using System.Collections.Generic;
using System.Text;

namespace myComponents.Object
{
    public class StringItem
    {
        private String _text;
        private String _value;

        public StringItem(String Text, String Value)
        {
            _text = Text;
            _value = Value;
        }

        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public String Value
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
