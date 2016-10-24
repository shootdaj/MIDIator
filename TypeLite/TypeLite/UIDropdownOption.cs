using System;

namespace TypeLite
{
	public class UIDropdownOption : Attribute
	{
	    public UIDropdownOption(string valueProperty, string labelProperty = null)
	    {
            ValueProperty = valueProperty;
            LabelProperty = labelProperty ?? LabelProperty;
	    }

	    public string LabelProperty { get; } = "Name";
	    public string ValueProperty { get; }

	}
}
