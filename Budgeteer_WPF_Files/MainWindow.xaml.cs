using System.Collections.Generic;
using System.Windows;

namespace Budgeteer_WPF_Files
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Transaction> records = new List<Transaction>();

        public MainWindow()
        {
            InitializeComponent();


        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}