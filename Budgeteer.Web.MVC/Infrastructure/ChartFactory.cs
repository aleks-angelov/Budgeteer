using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using Budgeteer.Web.MVC.Models;

namespace Budgeteer.Web.MVC.Infrastructure
{
    public static class ChartFactory
    {
        // Delegate
        public static Chart CreateChart(string chartName, DateTime dateFrom, DateTime dateUntil, string personName,
            string categoryName)
        {
            switch (chartName)
            {
                case "OverviewLeftChart":
                    return CreateOverviewLeftChart(dateFrom, dateUntil);

                case "OverviewRightChart":
                    return CreateOverviewRightChart(dateFrom, dateUntil);

                case "SpendingTopLeftChart":
                    return CreateSpendingTopLeftChart(dateFrom, dateUntil, personName);

                case "SpendingBottomLeftChart":
                    return CreateSpendingBottomLeftChart(dateFrom, dateUntil, personName);

                case "SpendingTopRightChart":
                    return CreateSpendingTopRightChart(dateFrom, dateUntil, categoryName);

                case "SpendingBottomRightChart":
                    return CreateSpendingBottomRightChart(dateFrom, dateUntil, categoryName);

                case "IncomeTopLeftChart":
                    return CreateIncomeTopLeftChart(dateFrom, dateUntil, personName);

                case "IncomeBottomLeftChart":
                    return CreateIncomeBottomLeftChart(dateFrom, dateUntil, personName);

                case "IncomeTopRightChart":
                    return CreateIncomeTopRightChart(dateFrom, dateUntil, categoryName);

                case "IncomeBottomRightChart":
                    return CreateIncomeBottomRightChart(dateFrom, dateUntil, categoryName);

                default:
                    return null;
            }
        }

        // Overview Balance chart
        private static Chart CreateOverviewLeftChart(DateTime dateFrom, DateTime dateUntil)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                // Income data
                List<Transaction> incomeRecords = (from record in context.Transactions
                    where
                    !record.Category.IsDebit &&
                    (record.Date >= dateFrom) &&
                    (record.Date <= dateUntil)
                    select record).ToList();

                IOrderedEnumerable<IGrouping<string, double>> incomeRecordsByMonth = from record in incomeRecords
                    group record.Amount by record.Date.ToString("yyyy/MM")
                    into monthlyRecords
                    orderby monthlyRecords.Key
                    select monthlyRecords;

                List<string> incomeXData = new List<string>();
                List<double> incomeYData = new List<double>();
                foreach (IGrouping<string, double> period in incomeRecordsByMonth)
                {
                    incomeXData.Add(period.Key);
                    incomeYData.Add(period.Sum());
                }

                // Spending data
                List<Transaction> spendingRecords = (from record in context.Transactions
                    where
                    record.Category.IsDebit &&
                    (record.Date >= dateFrom) &&
                    (record.Date <= dateUntil)
                    select record).ToList();

                IOrderedEnumerable<IGrouping<string, double>> spendingRecordsByMonth =
                    from record in spendingRecords
                    group record.Amount by record.Date.ToString("yyyy/MM")
                    into monthlyRecords
                    orderby monthlyRecords.Key
                    select monthlyRecords;

                List<string> spendingXData = new List<string>();
                List<double> spendingYData = new List<double>();
                foreach (IGrouping<string, double> period in spendingRecordsByMonth)
                {
                    spendingXData.Add(period.Key);
                    spendingYData.Add(period.Sum());
                }

                double budgetBalance = incomeYData.Sum() - spendingYData.Sum();

                return new Chart(568, 426, ChartTheme.Blue)
                    .AddTitle($"Budget Balance (last 6 months): {budgetBalance:C}")
                    .AddSeries("Income", "Column", xValue: incomeXData, yValues: incomeYData)
                    .AddSeries("Spending", "Column", xValue: spendingXData, yValues: spendingYData)
                    .AddLegend();
            }
        }

        // Overview Spending chart
        private static Chart CreateOverviewRightChart(DateTime dateFrom, DateTime dateUntil)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                List<string> spendingXData = new List<string>();
                List<double> spendingYData = new List<double>();
                foreach (
                    Category spendingCategory in context.Categories.Where(c => c.IsDebit).OrderBy(c => c.Name).ToList())
                {
                    List<double> categoryAmounts = (from record in context.Transactions
                        where
                        (record.Category.CategoryID == spendingCategory.CategoryID) &&
                        (record.Date >= dateFrom) &&
                        (record.Date <= dateUntil)
                        select record.Amount).ToList();

                    if (categoryAmounts.Count > 0)
                    {
                        double categoryTotal = categoryAmounts.Sum();
                        spendingXData.Add(spendingCategory.Name);
                        spendingYData.Add(categoryTotal);
                    }
                }

                return new Chart(568, 426, ChartTheme.Blue)
                    .AddTitle("Spending Distribution (last 6 months)")
                    .AddSeries("Spending", "Pie", xValue: spendingXData, yValues: spendingYData)
                    .AddLegend();
            }
        }

        // Spending of... chart
        private static Chart CreateSpendingTopLeftChart(DateTime dateFrom, DateTime dateUntil, string personName)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                List<Transaction> spendingRecords = (from record in context.Transactions
                    where
                    (record.Person.Name == personName) &&
                    record.Category.IsDebit &&
                    (record.Date >= dateFrom) &&
                    (record.Date <= dateUntil)
                    select record).ToList();

                IOrderedEnumerable<IGrouping<string, double>> spendingRecordsByMonth =
                    from record in spendingRecords
                    group record.Amount by record.Date.ToString("yyyy/MM")
                    into monthlyRecords
                    orderby monthlyRecords.Key
                    select monthlyRecords;

                List<string> spendingXData = new List<string>();
                List<double> spendingYData = new List<double>();
                foreach (IGrouping<string, double> period in spendingRecordsByMonth)
                {
                    spendingXData.Add(period.Key);
                    spendingYData.Add(period.Sum());
                }

                return new Chart(568, 426, ChartTheme.Blue)
                    .AddTitle("Spending of " + personName)
                    .AddSeries("Spending", "Column", xValue: spendingXData, yValues: spendingYData);
            }
        }

        // Spending Distribution of... chart
        private static Chart CreateSpendingBottomLeftChart(DateTime dateFrom, DateTime dateUntil, string personName)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                List<string> spendingXData = new List<string>();
                List<double> spendingYData = new List<double>();
                foreach (
                    Category spendingCategory in context.Categories.Where(c => c.IsDebit).OrderBy(c => c.Name).ToList())
                {
                    List<double> categoryAmounts = (from record in context.Transactions
                        where
                        (record.Person.Name == personName) &&
                        (record.Category.CategoryID == spendingCategory.CategoryID) &&
                        (record.Date >= dateFrom) &&
                        (record.Date <= dateUntil)
                        select record.Amount).ToList();

                    if (categoryAmounts.Count > 0)
                    {
                        double categoryTotal = categoryAmounts.Sum();
                        spendingXData.Add(spendingCategory.Name);
                        spendingYData.Add(categoryTotal);
                    }
                }

                return new Chart(568, 426, ChartTheme.Blue)
                    .AddTitle("Spending Distribution of " + personName)
                    .AddSeries("Spending", "Pie", xValue: spendingXData, yValues: spendingYData)
                    .AddLegend();
            }
        }

        // Spending for... chart
        private static Chart CreateSpendingTopRightChart(DateTime dateFrom, DateTime dateUntil, string categoryName)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                List<Transaction> spendingRecords = (from record in context.Transactions
                    where
                    (record.Category.Name == categoryName) &&
                    record.Category.IsDebit &&
                    (record.Date >= dateFrom) &&
                    (record.Date <= dateUntil)
                    select record).ToList();

                IOrderedEnumerable<IGrouping<string, double>> spendingRecordsByMonth =
                    from record in spendingRecords
                    group record.Amount by record.Date.ToString("yyyy/MM")
                    into monthlyRecords
                    orderby monthlyRecords.Key
                    select monthlyRecords;

                List<string> spendingXData = new List<string>();
                List<double> spendingYData = new List<double>();
                foreach (IGrouping<string, double> period in spendingRecordsByMonth)
                {
                    spendingXData.Add(period.Key);
                    spendingYData.Add(period.Sum());
                }

                return new Chart(568, 426, ChartTheme.Blue)
                    .AddTitle("Spending for " + categoryName)
                    .AddSeries("Spending", "Column", xValue: spendingXData, yValues: spendingYData);
            }
        }

        // Spending Distribution for... chart
        private static Chart CreateSpendingBottomRightChart(DateTime dateFrom, DateTime dateUntil, string categoryName)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                List<string> spendingXData = new List<string>();
                List<double> spendingYData = new List<double>();
                foreach (ApplicationUser spendingPerson in context.Users.OrderBy(u => u.Name).ToList())
                {
                    List<double> categoryAmounts = (from record in context.Transactions
                        where
                        (record.Person.Id == spendingPerson.Id) &&
                        record.Category.IsDebit &&
                        (record.Category.Name == categoryName) &&
                        (record.Date >= dateFrom) &&
                        (record.Date <= dateUntil)
                        select record.Amount).ToList();

                    if (categoryAmounts.Count > 0)
                    {
                        double categoryTotal = categoryAmounts.Sum();
                        spendingXData.Add(spendingPerson.Name);
                        spendingYData.Add(categoryTotal);
                    }
                }

                return new Chart(568, 426, ChartTheme.Blue)
                    .AddTitle("Spending Distribution for " + categoryName)
                    .AddSeries("Spending", "Pie", xValue: spendingXData, yValues: spendingYData)
                    .AddLegend();
            }
        }

        // Income of... chart
        private static Chart CreateIncomeTopLeftChart(DateTime dateFrom, DateTime dateUntil, string personName)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                List<Transaction> incomeRecords = (from record in context.Transactions
                    where
                    (record.Person.Name == personName) &&
                    !record.Category.IsDebit &&
                    (record.Date >= dateFrom) &&
                    (record.Date <= dateUntil)
                    select record).ToList();

                IOrderedEnumerable<IGrouping<string, double>> incomeRecordsByMonth =
                    from record in incomeRecords
                    group record.Amount by record.Date.ToString("yyyy/MM")
                    into monthlyRecords
                    orderby monthlyRecords.Key
                    select monthlyRecords;

                List<string> incomeXData = new List<string>();
                List<double> incomeYData = new List<double>();
                foreach (IGrouping<string, double> period in incomeRecordsByMonth)
                {
                    incomeXData.Add(period.Key);
                    incomeYData.Add(period.Sum());
                }

                return new Chart(568, 426, ChartTheme.Blue)
                    .AddTitle("Income of " + personName)
                    .AddSeries("Income", "Column", xValue: incomeXData, yValues: incomeYData);
            }
        }

        // Income Distribution of... chart
        private static Chart CreateIncomeBottomLeftChart(DateTime dateFrom, DateTime dateUntil, string personName)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                List<string> incomeXData = new List<string>();
                List<double> incomeYData = new List<double>();
                foreach (
                    Category incomeCategory in
                    context.Categories.Where(c => c.IsDebit == false).OrderBy(c => c.Name).ToList())
                {
                    List<double> categoryAmounts = (from record in context.Transactions
                        where
                        (record.Person.Name == personName) &&
                        (record.Category.CategoryID == incomeCategory.CategoryID) &&
                        (record.Date >= dateFrom) &&
                        (record.Date <= dateUntil)
                        select record.Amount).ToList();

                    if (categoryAmounts.Count > 0)
                    {
                        double categoryTotal = categoryAmounts.Sum();
                        incomeXData.Add(incomeCategory.Name);
                        incomeYData.Add(categoryTotal);
                    }
                }

                return new Chart(568, 426, ChartTheme.Blue)
                    .AddTitle("Income Distribution of " + personName)
                    .AddSeries("Income", "Pie", xValue: incomeXData, yValues: incomeYData)
                    .AddLegend();
            }
        }

        // Income from... chart
        private static Chart CreateIncomeTopRightChart(DateTime dateFrom, DateTime dateUntil, string categoryName)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                List<Transaction> incomeRecords = (from record in context.Transactions
                    where
                    (record.Category.Name == categoryName) &&
                    !record.Category.IsDebit &&
                    (record.Date >= dateFrom) &&
                    (record.Date <= dateUntil)
                    select record).ToList();

                IOrderedEnumerable<IGrouping<string, double>> incomeRecordsByMonth =
                    from record in incomeRecords
                    group record.Amount by record.Date.ToString("yyyy/MM")
                    into monthlyRecords
                    orderby monthlyRecords.Key
                    select monthlyRecords;

                List<string> incomeXData = new List<string>();
                List<double> incomeYData = new List<double>();
                foreach (IGrouping<string, double> period in incomeRecordsByMonth)
                {
                    incomeXData.Add(period.Key);
                    incomeYData.Add(period.Sum());
                }

                return new Chart(568, 426, ChartTheme.Blue)
                    .AddTitle("Income from " + categoryName)
                    .AddSeries("Income", "Column", xValue: incomeXData, yValues: incomeYData);
            }
        }

        // Income Distribution from... chart
        private static Chart CreateIncomeBottomRightChart(DateTime dateFrom, DateTime dateUntil, string categoryName)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                List<string> incomeXData = new List<string>();
                List<double> incomeYData = new List<double>();
                foreach (ApplicationUser incomePerson in context.Users.OrderBy(u => u.Name).ToList())
                {
                    List<double> categoryAmounts = (from record in context.Transactions
                        where
                        (record.Person.Id == incomePerson.Id) &&
                        !record.Category.IsDebit &&
                        (record.Category.Name == categoryName) &&
                        (record.Date >= dateFrom) &&
                        (record.Date <= dateUntil)
                        select record.Amount).ToList();

                    if (categoryAmounts.Count > 0)
                    {
                        double categoryTotal = categoryAmounts.Sum();
                        incomeXData.Add(incomePerson.Name);
                        incomeYData.Add(categoryTotal);
                    }
                }

                return new Chart(568, 426, ChartTheme.Blue)
                    .AddTitle("Income Distribution from " + categoryName)
                    .AddSeries("Income", "Pie", xValue: incomeXData, yValues: incomeYData)
                    .AddLegend();
            }
        }
    }
}