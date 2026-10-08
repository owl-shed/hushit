namespace OwlShed.Hushit.Data.EFCore.Tests.Universes;

[TestClass]
public sealed class UniverseTests : BaseDatabaseTest
{
	#region Tests
	[TestMethod]
	public void Create_CreatesValidInstance()
	{
		// Arrange
		string expectedId = Guid.CreateVersion7().ToString("N");
		const string expectedName = "custom_universe1";
		const string expectedParentId = "real";

		// Act
		using (DataContext context = CreateContext())
		{
			Universe universe = new(expectedId, expectedParentId, expectedName, isCustom: true);

			context.Universes.Add(universe);
			context.SaveChanges();
		}

		// Assert
		using (DataContext context = CreateContext())
		{
			Universe result = context.Universes.Single(kind => kind.Id == expectedId);

			Assert.AreEqual(expectedParentId, result.ParentId);
			Assert.AreEqual(expectedName, result.Name);
			Assert.IsTrue(result.IsCustom);
		}
	}
	#endregion
}
