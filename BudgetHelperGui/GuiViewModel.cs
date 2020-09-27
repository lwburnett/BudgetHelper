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
            _selectedProvider = Provider.Chase;
            _outputDestination = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
                "BudgetHelperOutput.txt");
            _statusText = string.Empty;

            OnBrowseForTransactionFile = new RelayCommand(() => CommandWrapper("Browse for transaction file", () => BrowseCommand(f => TransactionPath = f )));
            OnBrowseForDatabaseFile = new RelayCommand(() => CommandWrapper("Browse for database file", () => BrowseCommand(f => DatabasePath = f)));
            OnBrowseForDestinationFile = new RelayCommand(() => CommandWrapper("Browse for destination file", () => BrowseCommand(f => OutputDestination = f)));
            OnGenerate = new RelayCommand(() => CommandWrapper("Generate output", GenerateCommand), CanGenerate);
        }

        #region Dependency Properties
        public string TransactionPath
        {
            get { return _transactionPath; }
            set { Set(ref _transactionPath, value); }
        }

        public string DatabasePath
        {
            get { return _databasePath; }
            set { Set(ref _databasePath, value); }
        }

        public IEnumerable<Provider> Providers
        {
            get { return Enum.GetValues(typeof(Provider)).Cast<Provider>(); }
        }

        public Provider SelectedProvider
        {
            get { return _selectedProvider; }
            set { Set(ref _selectedProvider, value); }
        }

        public string OutputDestination
        {
            get { return _outputDestination; }
            set { Set(ref _outputDestination, value); }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { Set(ref _statusText, value); }
        }
        #endregion

        #region Commands

        public ICommand OnBrowseForTransactionFile { get; private set; }
        public ICommand OnBrowseForDatabaseFile { get; private set; }
        public ICommand OnBrowseForDestinationFile { get; private set; }
        public ICommand OnGenerate { get; private set; }

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

        private void BrowseCommand(Action<string> OnSuccessCallback)
        {
            var dlg = new OpenFileDialog 
            {
                DefaultExt = ".txt",
                Filter = "Text Files (*.txt) | *.txt | CSV Files (*.csv) | *.csv"
            };

            var result = dlg.ShowDialog();

            if (!result.HasValue)
                throw new Exception("Failed to get result from dialog.");

            if (!result.Value)
                throw new Exception("No result from dialog. Dialog perhaps canceled?");

            OnSuccessCallback(dlg.FileName);
        }

        private bool CanGenerate()
        {
            if (string.IsNullOrWhiteSpace(TransactionPath))
                return false;
            else if (!File.Exists(TransactionPath))
            { 
                StatusText = $"{DateTime.Now} | Given Transaction Path does not exist.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(DatabasePath))
                return false;
            else if (!File.Exists(DatabasePath))
            {
                StatusText = $"{DateTime.Now} | Given DatabasePath Path does not exist.";
                return false;
            }

            return true;
        }

        private void GenerateCommand()
        {
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
