using System.IO;

namespace OwlShed.Hushit.Common.IO;

/// <summary>
/// 	Contains various extensions related to the <see cref="File"/> type.
/// </summary>
public static class FileExtensions
{
	extension(File)
	{
		#region Functions
		/// <summary>Tries to delete the file at the given <paramref name="path"/>, if it exists and is not in use.</summary>
		/// <param name="path">The path of the file to try and delete.</param>
		public static void TryDelete(string path)
		{
			if (File.Exists(path) is false)
				return;

			try
			{
				File.Delete(path);
			}
			catch (IOException) { }
		}
		#endregion
	}
}
