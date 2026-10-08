namespace OwlShed.Hushit.Data.EFCore.Tests.Images.Kinds;

[TestClass]
public sealed class ImageKindTests : BaseDatabaseTest
{
	#region Tests
	[TestMethod]
	public void Create_CreatesValidInstance()
	{
		// Arrange
		string expectedId = Guid.CreateVersion7().ToString("N");
		const string expectedDescription = "unknown_image_kind1";

		// Act
		using (DataContext context = CreateContext())
		{
			ImageKind kind = new(expectedId, expectedDescription);

			context.ImageKinds.Add(kind);
			context.SaveChanges();
		}

		// Assert
		using (DataContext context = CreateContext())
		{
			ImageKind result = context.ImageKinds.Single(kind => kind.Id == expectedId);

			Assert.AreEqual(expectedDescription, result.UnknownKindDescription);
		}
	}
	#endregion
}
