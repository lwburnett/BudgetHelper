using System;
using System.IO;
using System.Linq;
using Autofac;
using BudgetHelper;
using BudgetHelper.Common;
using BudgetHelper.MapProviders;
using BudgetHelper.Parsers;

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

				BuildIocContainer(config);

				var generator = _container.Resolve<ITransactionsGenerator>();

				var transactions = generator.Generate();

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
				throw;
			}
			finally
			{
				_scope?.Dispose();
				_container?.Dispose();
			}

			Console.WriteLine("Press any key to exit.");
			Console.ReadKey();
		}

		#region Implementation

		#region Fields

		private static IContainer _container;
		private static ILifetimeScope _scope;

		#endregion

		private static void BuildIocContainer(ConfigParameters config)
		{
			var builder = new ContainerBuilder();

			builder.RegisterInstance(
					ExportParserFactory.FromParameter(
						config.Provider,
						config.TransactionExportPath)).
				As<IRawTransactionProvider>().
				SingleInstance();

			builder.RegisterInstance(new CsvMapProvider(config.DatabaseCsvPath)).
				As<IMapProvider>().
				SingleInstance();

			builder.Register(c =>
					new TransactionsGenerator(
						c.Resolve<IMapProvider>(),
						c.Resolve<IRawTransactionProvider>())).
				As<ITransactionsGenerator>().
				SingleInstance();

			_container = builder.Build();
			_scope = _container.BeginLifetimeScope();
		}

		#endregion
	}
}
