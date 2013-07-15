using System;
using System.Collections.Generic;
using System.Text;

namespace myComponents.Object
{
    public class SecurityIdentifierItem
    {
        private String _text;
        private System.Security.Principal.SecurityIdentifier _value;

        public SecurityIdentifierItem(String Text, System.Security.Principal.SecurityIdentifier Value)
        {
            _text = Text;
            _value = Value;
        }


        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public System.Security.Principal.SecurityIdentifier Value
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
