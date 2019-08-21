namespace BudgetHelper
{
	public class ConfigParameters
	{
		public ConfigParameters(
			string transactionExportPath,
			string databaseCsvPath,
			string provider)
		{
			DatabaseCsvPath = databaseCsvPath;
			Provider = provider;
			TransactionExportPath = transactionExportPath;
		}

		public string TransactionExportPath { get; }
		public string DatabaseCsvPath { get; }
		public string Provider { get; }
	}
}
