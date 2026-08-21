namespace OwlShed.Hushit.Monolith.Actions;

internal static class CommandLineExtensions
{
	extension(Command command)
	{
		#region Methods
		public void CustomiseOption<T>(Action<T> callback) where T : Option
		{
			foreach (T option in command.Options.OfType<T>())
				callback.Invoke(option);
		}

		public void AddGroup(string[] args, Command group)
		{
			// Note(Nightowl): Automatically invoke the help command if a command group (instead of a command) is invoked;
			group.SetAction(parsing => command.Parse([.. args, "--help"]).Invoke());

			command.Add(group);
		}
		#endregion
	}
}
