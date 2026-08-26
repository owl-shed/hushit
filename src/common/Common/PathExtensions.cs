using System.IO;

namespace OwlShed.Hushit.Common;

/// <summary>
/// 	Contains various extensions related to the <see cref="Path"/> type.
/// </summary>
public static class PathExtensions
{
	extension(Path)
	{
		#region Functions
		/// <summary>Checks whether the given <paramref name="path"/> is relative to the given <paramref name="possibleBase"/>.</summary>
		/// <param name="possibleBase">The parent directory to check the <paramref name="path"/> against.</param>
		/// <param name="path">The path to check.</param>
		/// <returns>
		/// 	<see langword="true"/> if the given <paramref name="path"/> is relative to
		/// 	the given <paramref name="possibleBase"/>, <see langword="false"/> otherwise.
		/// </returns>
		public static bool IsRelative(string possibleBase, string path)
		{
			string relative = Path.GetRelativePath(possibleBase, path);
			return relative != path;
		}
		#endregion
	}
}
