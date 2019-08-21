using System;
using System.IO;
using System.Linq;
using BudgetHelper;
using BudgetHelper.Api;
using BudgetHelper.Common;

namespace BudgetHelperCli
{
	public class BudgetHelperCli
	{
		private static void Main(string[] args)
		{
			try
			{
				// TODO: use fluent command line parser
				// TODO: do validation
				var config = new ConfigParameters(
					args[0],
					args[1],
					args[2]);

				var transactions = BudgetHelperWrapper.GetTransactions(config);

				var transactionCsvLines = transactions.
					Select(t =>
						CsvUtil.Serialize(new object[]
						{
							t.Date.ToShortDateString(),
							t.Month,
							t.Establishment,
							t.Amount,
							t.Category,
							t.PaymentMethod
						})).
					ToList();

				File.WriteAllLines(args[3], transactionCsvLines);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Unhandled exception.");
				Console.WriteLine(ex);
			}

			Console.WriteLine("Press any key to exit.");
			Console.ReadKey();
		}
	}
}
