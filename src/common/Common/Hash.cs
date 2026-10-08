using System.IO;
using System.Security.Cryptography;

namespace OwlShed.Hushit.Common;

/// <summary>
/// 	Helper utility for obtaining different hashes.
/// </summary>
public static class Hash
{
	#region Sha256
	/// <summary>Gets a sha256 hash of the file at the given <paramref name="path"/>.</summary>
	/// <param name="path">The path of the file to hash.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A hex encoded sha256 hash of the file at the given <paramref name="path"/>.</returns>
	public static async ValueTask<string> Sha256FromFileAsync(string path, CancellationToken cancellation = default)
	{
		using (FileStream file = File.OpenRead(path))
			return await Sha256Async(file, cancellation).ConfigureAwait(false);
	}

	/// <summary>Gets a sha256 hash of the given <paramref name="stream"/>.</summary>
	/// <param name="stream">The stream to hash.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>A hex encoded sha256 hash of the given <paramref name="stream"/>.</returns>
	public static async ValueTask<string> Sha256Async(Stream stream, CancellationToken cancellation = default)
	{
		byte[] bytes = await SHA256.HashDataAsync(stream, cancellation);
		return GetHash(bytes);
	}
	#endregion

	#region Helpers
	private static string GetHash(byte[] bytes) => Convert.ToHexStringLower(bytes).Replace("-", "");
	#endregion
}
