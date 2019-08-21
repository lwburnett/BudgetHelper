namespace BudgetHelper.DataTypes
{
	public class CategoryMapping
	{
		public CategoryMapping(string establishmentName, string categoryName)
		{
			EstablishmentName = string.Copy(establishmentName);
			CategoryName = string.Copy(categoryName);
		}

		public string EstablishmentName { get; }
		public string CategoryName { get; }
	}
}