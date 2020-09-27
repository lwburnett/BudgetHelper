using System.Windows;

namespace BudgetHelperGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new GuiViewModel();
            InitializeComponent();
        }
    }
}
