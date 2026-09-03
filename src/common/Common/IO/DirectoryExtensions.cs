using System.IO;

namespace OwlShed.Hushit.Common.IO;

/// <summary>
/// 	Contains various extensions related to the <see cref="Directory"/> type.
/// </summary>
public static class DirectoryExtensions
{
	extension(Directory)
	{
		#region Functions
		/// <summary>Tries to delete the given <paramref name="directory"/>, but only if it's empty.</summary>
		/// <param name="directory">The directory to try and delete.</param>
		public static void DeleteIfEmpty(string? directory)
		{
			if (directory is null || Directory.Exists(directory) is false)
				return;

			try
			{
				if (Directory.EnumerateFileSystemEntries(directory).Any())
					return;
			}
			catch (DirectoryNotFoundException) { }

			try
			{
				Directory.Delete(directory, recursive: false);
			}
			catch (DirectoryNotFoundException) { }
			catch (IOException) { }
		}

		/// <summary>Tries to delete the given <paramref name="directory"/>, and any sub-directories, but only if they're also empty.</summary>
		/// <param name="directory">The base directory to try and delete.</param>
		public static void DeleteHierarchyIfEmpty(string? directory)
		{
			if (directory is null || Directory.Exists(directory) is false)
				return;

			try
			{
				if (Directory.EnumerateFiles(directory).Any())
					return;
			}
			catch (DirectoryNotFoundException)
			{
				return;
			}

			foreach (string child in Directory.EnumerateDirectories(directory))
				DeleteHierarchyIfEmpty(child);

			DeleteIfEmpty(directory);
		}
		#endregion
	}
}
