namespace OwlShed.Hushit.Monolith.Actions.Meta;

internal sealed class CustomHelpAction : SynchronousCommandLineAction
{
	#region Fields
	private readonly HelpAction _defaultHelp;
	#endregion

	#region Constructors
	public CustomHelpAction(HelpAction defaultHelp) => _defaultHelp = defaultHelp;
	#endregion

	#region Methods
	public override int Invoke(ParseResult parseResult)
	{
		FigletText text = new("hushit");

		AnsiConsole.Write(text);
		AnsiConsole.WriteLine();

		return _defaultHelp.Invoke(parseResult);
	}
	#endregion
}
