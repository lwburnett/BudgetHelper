using System.Collections.Generic;
using Autofac;
using BudgetHelper.DataTypes;
using BudgetHelper.MapProviders;
using BudgetHelper.Parsers;

namespace BudgetHelper.Api
{
    public class BudgetHelperWrapper
    {
        public static IEnumerable<Transaction> GetTransactions(ConfigParameters config)
        {
            try
            {
                BuildIocContainer(config);

                var generator = _container.Resolve<ITransactionsGenerator>();

                return generator.Generate();
            }
            finally
            {
                _scope?.Dispose();
                _container?.Dispose();
            }
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
