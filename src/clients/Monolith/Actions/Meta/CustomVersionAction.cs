using System.Reflection;

namespace OwlShed.Hushit.Monolith.Actions.Meta;

internal sealed class CustomVersionAction : SynchronousCommandLineAction
{
	#region Methods
	public override int Invoke(ParseResult parseResult)
	{
		// Todo(Nightowl): Figure out how we actually want to indicate the version later;

		string version = GetAssemblyVersion();
		AnsiConsole.WriteLine($"{version}-dev");

		return 0;
	}
	#endregion

	#region Helpers
	private static string GetAssemblyVersion()
	{
		// Note(Nightowl): Not actually git info but easier to have it here;
		AssemblyInformationalVersionAttribute? attribute = Assembly
				.GetExecutingAssembly()
				.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

		string? version = attribute?.InformationalVersion.Split('+').FirstOrDefault();
		if (version?.IsWhiteSpace() is false)
			return $"v{version}";

		return "v0.0.0";
	}
	#endregion
}
