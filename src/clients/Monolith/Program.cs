RootCommand root = new("The Hushit music player.");

root.CustomiseOption<VersionOption>(o =>
{
	o.Aliases.Add("-v");
	o.Description += ".";

	o.Action = new CustomVersionAction();
});
root.CustomiseOption<HelpOption>(o =>
{
	o.Description += ".";
	if (o.Action is HelpAction defaultHelp)
		o.Action = new CustomHelpAction(defaultHelp);
});

ParseResult result = root.Parse(args);
return await result.InvokeAsync();
