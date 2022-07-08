using System;
using NUnit.Framework;
using TextDecoder;
using TextDecoder.Parser;

public class ActionParserTests
{
    private const string TEST_ACTION_NAME = "ACTION";
    private const string TEST_ACTION_PARAMETER_1 = "TestActionParameter1";
    private const string TEST_ACTION_PARAMETER_2 = "TestActionParameter2";

    private static readonly string TEST_ACTION = $"{ScriptAction.ACTION_TOKEN}{TEST_ACTION_NAME}{ScriptAction.ACTION_SEPARATOR}{TEST_ACTION_PARAMETER_1}{ScriptAction.PARAMETER_SEPARATOR}{TEST_ACTION_PARAMETER_2}";

    [Test]
    public void ActionLinesAreSplitIntoNameAndParameters()
    {
        var scriptAction = new ScriptAction(TEST_ACTION);
        Assert.AreEqual(TEST_ACTION_NAME, scriptAction.Name);
        Assert.AreEqual(TEST_ACTION_PARAMETER_1, scriptAction.Parameters[0]);
        Assert.AreEqual(TEST_ACTION_PARAMETER_2, scriptAction.Parameters[1]);
    }

    [Test]
    public void ActionLinesCannotHaveMultipleActionSeparators()
    {
        Assert.Throws<ScriptParsingException>(() =>
        {
            var scriptAction = new ScriptAction($"{TEST_ACTION}{ScriptAction.ACTION_SEPARATOR}");
        });
    }

    [Test]
    public void ActionCanHaveZeroParameters()
    {
        var scriptAction = new ScriptAction($"&{TEST_ACTION_NAME}");
        Assert.AreEqual(TEST_ACTION_NAME, scriptAction.Name);
        Assert.AreEqual(Array.Empty<string>(), scriptAction.Parameters);
    }

    [Test]
    public void IsActionTest()
    {
        Assert.IsTrue(ScriptAction.IsAction(TEST_ACTION));
        Assert.IsFalse(ScriptAction.IsAction(TEST_ACTION.Substring(1, TEST_ACTION.Length - 1)));
    }
}
