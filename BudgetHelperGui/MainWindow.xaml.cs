using System.Windows;

namespace BudgetHelperGui
{
    // ReSharper disable once UnusedMember.Global
    public partial class MainWindow
    {
        public MainWindow()
        {
            DataContext = new GuiViewModel();
            InitializeComponent();
        }
    }
}
