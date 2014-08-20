using System;

namespace System.ComponentModel
{
    [AttributeUsageAttribute(AttributeTargets.Class|AttributeTargets.Method|AttributeTargets.Property|AttributeTargets.Event)]
    public class DisplayNameAttribute : Attribute
    {
        public DisplayNameAttribute (string name)
        {
        }
    }
}

