using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ex2_Expense
{
    /// <summary>
    /// Interaction logic for AddExpenseWindow.xaml
    /// </summary>
    public partial class AddExpenseWindow : Window
    {
        public AddExpenseWindow()
        {
            InitializeComponent();

            cbxType.ItemsSource = new string[] { "Entertainment", "Office", "Travel" };
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //read data from screen
            string type = cbxType.SelectedItem as string;
            decimal amt = Convert.ToDecimal(tbxAmount.Text);
            DateTime date = dpDate.SelectedDate.Value;

            //create expense object
            Expense newExp = new Expense(type, amt, date);

            //get reference to main window
            MainWindow main = Owner as MainWindow;

            //add that to our collection of expenses
            main.expenses.Add(newExp);

            //close the window
            this.Close();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //close the window
            this.Close();
        
        }
    }
}
