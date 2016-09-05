using System.Reflection;
using NUnit.Framework;

namespace MIDIator.Tests
{
	public class InputMatchFunctionsTests
	{
		[TestCase(InputMatchFunction.CatchAll, "CatchAll")]
		[TestCase(InputMatchFunction.NoteMatch, "Data1Match")]
		[TestCase(InputMatchFunction.Data1Match, "Data1Match")]
		public void Get_ReturnsCorrectMethodReference(InputMatchFunction func, string methodName)
		{
			var evaluatedFunc = InputMatchFunctions.Get(func);

			Assert.That(evaluatedFunc.GetMethodInfo().Name.Contains(methodName));
		}
	}
}
