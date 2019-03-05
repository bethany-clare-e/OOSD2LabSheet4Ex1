using System;

namespace Ex2_Expense
{

    public class Expense
    {
        public string Category { get; set; }
        public decimal Cost { get; set; }
        public DateTime Date { get; set; }

        public static decimal TotalExpenses { get; set; }

        public Expense(string category, decimal amt, DateTime date)
        {
            Category = category;
            Cost = amt;
            Date = date;

            TotalExpenses += amt;
        }

        public override string ToString()
        {
            return string.Format("{0} {1:C} {2}", Category, Cost, Date.ToShortDateString());
        }


    }
}