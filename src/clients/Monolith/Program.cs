using Microsoft.Extensions.Logging;

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

root.SetAction(async (parse, cancellation) =>
{
	HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new()
	{
		DisableDefaults = true,
		ApplicationName = "Hushit",
		ContentRootPath = AppContext.BaseDirectory,
#if RELEASE
		EnvironmentName = Environments.Production,
#else
		EnvironmentName = Environments.Development,
#endif
	});

	builder.Logging.ClearProviders();
	builder.Logging.AddDebug();

	IHost host = builder.Build();
	await host.RunAsync(cancellation);
});

ParseResult result = root.Parse(args);
return await result.InvokeAsync();
