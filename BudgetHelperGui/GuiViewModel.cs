using BudgetHelper;
using BudgetHelper.Api;
using BudgetHelper.Common;
using BudgetHelper.Enums;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace BudgetHelperGui
{
    public class GuiViewModel : ViewModelBase
    {
        public GuiViewModel()
        {
            _transactionPath = string.Empty;
            _databasePath = string.Empty;
            _selectedProvider = Provider.ChaseBank;
            _outputDestination = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
                "BudgetHelperOutput.txt");
            _statusText = string.Empty;

            OnBrowseForTransactionFile = new RelayCommand(() => CommandWrapper("Browse for transaction file", () => BrowseCommand(f => TransactionPath = f )));
            OnBrowseForDatabaseFile = new RelayCommand(() => CommandWrapper("Browse for database file", () => BrowseCommand(f => DatabasePath = f)));
            OnBrowseForDestinationFile = new RelayCommand(() => CommandWrapper("Browse for destination file", () => BrowseCommand(f => OutputDestination = f)));
            OnGenerate = new RelayCommand(() => CommandWrapper("Generate output", GenerateCommand));
        }

        #region Dependency Properties
        public string TransactionPath
        {
            get => _transactionPath;
            set => Set(ref _transactionPath, value);
        }

        public string DatabasePath
        {
            get => _databasePath;
            set => Set(ref _databasePath, value);
        }

        public IEnumerable<Provider> Providers => Enum.GetValues(typeof(Provider)).Cast<Provider>();

        public Provider SelectedProvider
        {
            get => _selectedProvider;
            set => Set(ref _selectedProvider, value);
        }

        public string OutputDestination
        {
            get => _outputDestination;
            set => Set(ref _outputDestination, value);
        }

        public string StatusText
        {
            get => _statusText;
            set => Set(ref _statusText, value);
        }
        #endregion

        #region Commands

        public ICommand OnBrowseForTransactionFile { get; }
        public ICommand OnBrowseForDatabaseFile { get; }
        public ICommand OnBrowseForDestinationFile { get; }
        public ICommand OnGenerate { get; }

        #endregion

        private void CommandWrapper(string iCommandDescription, Action iCommandToRun)
        {
            try
            {
                iCommandToRun();
            }
            catch (Exception ex)
            {
                StatusText = $"{DateTime.Now} | {iCommandDescription} failed: {ex.Message}\nTry again and talk to Luke if this keeps happening.";
            }
        }

        private static void BrowseCommand(Action<string> iOnSuccessCallback)
        {
            var dlg = new OpenFileDialog 
            {
                DefaultExt = ".txt",
                Filter = "Text Documents (*.txt)|*.txt|CSV Files (*.csv)|*.csv"
            };

            var result = dlg.ShowDialog();

            if (!result.HasValue)
                throw new Exception("Failed to get result from dialog.");

            if (!result.Value)
                throw new Exception("No result from dialog. Dialog perhaps canceled?");

            iOnSuccessCallback(dlg.FileName);
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(TransactionPath))
                throw new Exception("No Transaction Path given.");
            else if (!File.Exists(TransactionPath))
            { 
                throw new Exception("Given Transaction Path does not exist.");
            }

            if (string.IsNullOrWhiteSpace(DatabasePath))
                throw new Exception("No Database Path given.");
            else if (!File.Exists(DatabasePath))
            {
                throw new Exception("Given Database Path does not exist.");
            }
        }

        private void GenerateCommand()
        {
            Validate();

            var config = new ConfigParameters(
                    TransactionPath,
                    DatabasePath,
                    SelectedProvider.ToString());

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

            File.WriteAllLines(OutputDestination, transactionCsvLines);

            StatusText = $"{DateTime.Now} | Generate output successful.";
        }

        private string _transactionPath;
        private string _databasePath;
        private Provider _selectedProvider;
        private string _outputDestination;
        private string _statusText;
    }
}
