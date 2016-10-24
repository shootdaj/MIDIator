using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MIDIator.UIGenerator.Consumables.Tests
{
	class ExtensionsTests
	{
		[TestCase("DriverVersion", "driverVersion")]
		[TestCase("MID", "mid")]
		[TestCase("TranslationMap", "translationMap")]
		[TestCase("InputMessageMatchTarget", "inputMessageMatchTarget")]
		[TestCase("A", "a")]
		[TestCase("ABc", "abc")]
		[TestCase("abc", "abc")]
		[TestCase("abC", "abC")]
		[TestCase("NewTranslationMap", "newTranslationMap")]
		[TestCase("Profile", "profile")]
		[TestCase("DeviceID", "deviceID")]
		public void ToCamelCase_Works(string input, string expectedValue)
		{
			Assert.That(input.ToCamelCase(), Is.EqualTo(expectedValue));
		}
	}
}
