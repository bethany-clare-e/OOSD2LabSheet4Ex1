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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections.ObjectModel;

using Newtonsoft.Json;
using System.IO;

namespace Ex2_Expense
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Expense> expenses = new ObservableCollection<Expense>();

        ObservableCollection<Expense> matchingExpenses = new ObservableCollection<Expense>();

        string[] categories = { "Travel", "Office", "Entertainment" };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Random randomFactory = new Random();


            //create 3 expense objects
            Expense e1 = new Expense("Office", 19.99M, new DateTime(2018, 1, 15));
            Expense e2 = GetRandomExpense(randomFactory);
            Expense e3 = GetRandomExpense(randomFactory);
            Expense e4 = GetRandomExpense(randomFactory);
            Expense e5 = GetRandomExpense(randomFactory);

            //add to collection
            expenses.Add(e1);
            expenses.Add(e2);
            expenses.Add(e3);
            expenses.Add(e4);
            expenses.Add(e5);

            //display on screen
            lbxExpenses.ItemsSource = expenses;

            decimal total = Expense.TotalExpenses;
            tblkTotal.Text = string.Format("{0:C}", total);

            //populate combo box
            Array.Sort(categories);
            cbxFilter.ItemsSource = categories;



        }

        //generate random expense
        private Expense GetRandomExpense(Random randomFactory)
        {
            Random rf = new Random();

            int randNumber = randomFactory.Next(0, 3);
            string randomCategory = categories[randNumber];

            decimal randomAmount = (decimal)randomFactory.Next(1, 10001) / 100;

            DateTime randomDate = DateTime.Now.AddDays(-randomFactory.Next(0, 32));

            Expense randomExpense = new Expense(randomCategory, randomAmount, randomDate);

            return randomExpense;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            //identify which expense is selected
            Expense selectedExpense = lbxExpenses.SelectedItem as Expense;

            if (selectedExpense != null)
            {
                //remove that expense
                Expense.TotalExpenses -= selectedExpense.Cost;
                expenses.Remove(selectedExpense);

                decimal total = Expense.TotalExpenses;
                tblkTotal.Text = string.Format("{0:C}", total);
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //Random randomFactory = new Random();
            //Expense exp = GetRandomExpense(randomFactory);
            //expenses.Add(exp);

            //decimal total = Expense.TotalExpenses;
            //tblkTotal.Text = string.Format("{0:C}", total);


            AddExpenseWindow addExp = new AddExpenseWindow();
            addExp.Owner = this;
            addExp.ShowDialog();

        }

        //Searches for Expenese by category
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //read info from screen - what is user looking for
            string searchTerm = tbxSearch.Text;

            if (! String.IsNullOrEmpty(searchTerm))
            {
                //clear expenses to that blank at start of every search
                matchingExpenses.Clear();

                //search collection of expenses for matches
                foreach (Expense exp in expenses)
                {
                    string expenseType = exp.Category;

                    if (expenseType.Equals(searchTerm))
                    {
                        matchingExpenses.Add(exp);
                    }
                }

                //display matches on screen
                lbxExpenses.ItemsSource = matchingExpenses;
            }



        }

        //shows all expenses, used after a search to see everything, remove filters
        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            lbxExpenses.ItemsSource = expenses;
        }

        //filter by type of expense
        private void cbxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //determine what the user selected
            string selectedExpenseType = cbxFilter.SelectedItem as string;

            if (selectedExpenseType != null)
            {
                //clear search results
                matchingExpenses.Clear();

                //search in the collection
                foreach (Expense exp in expenses)
                {
                    //find match
                    string expCategory = exp.Category;

                    if (expCategory.Equals(selectedExpenseType))
                    {
                        //add match to search results
                        matchingExpenses.Add(exp);
                    }
                }

                //update display
                lbxExpenses.ItemsSource = matchingExpenses;
            }

        }

        //save expense objects to JSON
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //get string of objects - json formatted
            string json = JsonConvert.SerializeObject(expenses, Formatting.Indented);

            //write that to file
            using (StreamWriter sw = new StreamWriter(@"c:\temp\expenses.json"))
            {
                sw.Write(json);
            }
        }

        //loads json file
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            //connect to a file
            using (StreamReader sr = new StreamReader(@"c:\temp\expenses.json"))
            {
                //read text
                string json = sr.ReadToEnd();

                //convert from json to objects
                expenses = JsonConvert.DeserializeObject<ObservableCollection<Expense>>(json);

                //refresh the display
                lbxExpenses.ItemsSource = expenses;
            }


        }
    }
}
