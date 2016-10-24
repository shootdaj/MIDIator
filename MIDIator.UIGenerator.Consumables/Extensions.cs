using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MIDIator.UIGenerator.Consumables
{
	public static class Extensions
	{
		public static string ToCamelCase(this string input)
		{
			input = input.SplitCamelCase();

			// If there are 0 or 1 characters, just return the string.
			if (input == null)
				return input;
			if (input.Length < 2)
				return input.ToLower();

			// Split the string into words.
			string[] words = input.Split(
				new char[] { },
				StringSplitOptions.RemoveEmptyEntries);

			// Combine the words.
			string result = words[0].ToLower();
			for (int i = 1; i < words.Length; i++)
			{
				result +=
					words[i].Substring(0, 1).ToUpper() +
					words[i].Substring(1);
			}

			return result;
		}

		public static string SplitCamelCase(this string input)
		{
			return Regex.Replace(input, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
		}
	}
}
