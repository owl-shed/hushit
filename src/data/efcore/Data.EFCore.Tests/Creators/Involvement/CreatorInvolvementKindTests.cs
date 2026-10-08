namespace OwlShed.Hushit.Data.EFCore.Tests.Creators.Involvement;

[TestClass]
public sealed class CreatorInvolvementKindTests : BaseDatabaseTest
{
	#region Tests
	[TestMethod]
	public void Create_CreatesValidInstance()
	{
		// Arrange
		string expectedId = Guid.CreateVersion7().ToString("N");
		const string expectedName = "involvement_kind1";

		// Act
		using (DataContext context = CreateContext())
		{
			CreatorInvolvementKind kind = new(expectedId, expectedName, isCustom: true);

			context.CreatorInvolvementKinds.Add(kind);
			context.SaveChanges();
		}

		// Assert
		using (DataContext context = CreateContext())
		{
			CreatorInvolvementKind result = context.CreatorInvolvementKinds.Single(kind => kind.Id == expectedId);

			Assert.AreEqual(expectedName, result.Name);
			Assert.IsTrue(result.IsCustom);
		}
	}
	#endregion
}
