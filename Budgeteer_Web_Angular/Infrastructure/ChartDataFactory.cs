using System;
using System.Collections.Generic;
using System.Linq;
using Budgeteer_Web_Angular.Models;

namespace Budgeteer_Web_Angular.Infrastructure
{
    public static class ChartDataFactory
    {
        // Delegate
        public static IEnumerable<Transactions> GetChartTransactions(BudgeteerDbContext context, string chartName,
            DateTime dateFrom, DateTime dateUntil, string personName, string categoryName)
        {
            switch (chartName)
            {
                case "OverviewLeftChart":
                    return GetOverviewLeftTransactions(context, dateFrom, dateUntil);

                case "OverviewRightChart":
                    return GetOverviewRightTransactions(context, dateFrom, dateUntil);

                case "SpendingTopLeftChart":
                    return GetSpendingTopLeftTransactions(context, dateFrom, dateUntil, personName);

                case "SpendingBottomLeftChart":
                    return GetSpendingBottomLeftTransactions(context, dateFrom, dateUntil, personName);

                case "SpendingTopRightChart":
                    return GetSpendingTopRightTransactions(context, dateFrom, dateUntil, categoryName);

                case "SpendingBottomRightChart":
                    return GetSpendingBottomRightTransactions(context, dateFrom, dateUntil, categoryName);

                case "IncomeTopLeftChart":
                    return GetIncomeTopLeftTransactions(context, dateFrom, dateUntil, personName);

                case "IncomeBottomLeftChart":
                    return GetIncomeBottomLeftTransactions(context, dateFrom, dateUntil, personName);

                case "IncomeTopRightChart":
                    return GetIncomeTopRightTransactions(context, dateFrom, dateUntil, categoryName);

                case "IncomeBottomRightChart":
                    return GetIncomeBottomRightTransactions(context, dateFrom, dateUntil, categoryName);

                default:
                    return null;
            }
        }

        // Overview Balance chart
        private static IEnumerable<Transactions> GetOverviewLeftTransactions(BudgeteerDbContext context,
            DateTime dateFrom, DateTime dateUntil)
        {
            // Income data
            List<Transactions> incomeRecords = (from record in context.Transactions
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
            List<Transactions> spendingRecords = (from record in context.Transactions
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

            return new Chart(568, 426, ChartTheme.Blue)
                .AddTitle("Budget Balance (last 6 months)")
                .AddSeries("Income", "Column", xValue: incomeXData, yValues: incomeYData)
                .AddSeries("Spending", "Column", xValue: spendingXData, yValues: spendingYData)
                .AddLegend();
        }

        // Overview Spending chart
        private static IEnumerable<Transactions> GetOverviewRightTransactions(BudgeteerDbContext context,
            DateTime dateFrom, DateTime dateUntil)
        {
            List<string> spendingXData = new List<string>();
            List<double> spendingYData = new List<double>();
            foreach (
                Categories spendingCategory in
                context.Categories.Where(c => c.IsDebit).OrderBy(c => c.Name).ToList())
            {
                List<double> categoryAmounts = (from record in context.Transactions
                                                where
                                                (record.Category.CategoryId == spendingCategory.CategoryId) &&
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

        // Spending of... chart
        private static IEnumerable<Transactions> GetSpendingTopLeftTransactions(BudgeteerDbContext context,
            DateTime dateFrom, DateTime dateUntil,
            string personName)
        {
            List<Transactions> spendingRecords = (from record in context.Transactions
                                                  where
                                                  (record.User.Name == personName) &&
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

        // Spending Distribution of... chart
        private static IEnumerable<Transactions> GetSpendingBottomLeftTransactions(BudgeteerDbContext context,
            DateTime dateFrom, DateTime dateUntil,
            string personName)
        {
            List<string> spendingXData = new List<string>();
            List<double> spendingYData = new List<double>();
            foreach (
                Categories spendingCategory in
                context.Categories.Where(c => c.IsDebit).OrderBy(c => c.Name).ToList())
            {
                List<double> categoryAmounts = (from record in context.Transactions
                                                where
                                                (record.User.Name == personName) &&
                                                (record.Category.CategoryId == spendingCategory.CategoryId) &&
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

        // Spending for... chart
        private static IEnumerable<Transactions> GetSpendingTopRightTransactions(BudgeteerDbContext context,
            DateTime dateFrom, DateTime dateUntil,
            string categoryName)
        {
            List<Transactions> spendingRecords = (from record in context.Transactions
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

        // Spending Distribution for... chart
        private static IEnumerable<Transactions> GetSpendingBottomRightTransactions(BudgeteerDbContext context,
            DateTime dateFrom,
            DateTime dateUntil, string categoryName)
        {
            List<string> spendingXData = new List<string>();
            List<double> spendingYData = new List<double>();
            foreach (AspNetUsers spendingPerson in context.AspNetUsers.OrderBy(u => u.Name).ToList())
            {
                List<double> categoryAmounts = (from record in context.Transactions
                                                where
                                                (record.User.Id == spendingPerson.Id) &&
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

        // Income of... chart
        private static IEnumerable<Transactions> GetIncomeTopLeftTransactions(BudgeteerDbContext context,
            DateTime dateFrom, DateTime dateUntil,
            string personName)
        {
            List<Transactions> incomeRecords = (from record in context.Transactions
                                                where
                                                (record.User.Name == personName) &&
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

        // Income Distribution of... chart
        private static IEnumerable<Transactions> GetIncomeBottomLeftTransactions(BudgeteerDbContext context,
            DateTime dateFrom, DateTime dateUntil,
            string personName)
        {
            List<string> incomeXData = new List<string>();
            List<double> incomeYData = new List<double>();
            foreach (
                Categories incomeCategory in
                context.Categories.Where(c => c.IsDebit == false).OrderBy(c => c.Name).ToList())
            {
                List<double> categoryAmounts = (from record in context.Transactions
                                                where
                                                (record.User.Name == personName) &&
                                                (record.Category.CategoryId == incomeCategory.CategoryId) &&
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

        // Income from... chart
        private static IEnumerable<Transactions> GetIncomeTopRightTransactions(BudgeteerDbContext context,
            DateTime dateFrom, DateTime dateUntil,
            string categoryName)
        {
            List<Transactions> incomeRecords = (from record in context.Transactions
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

        // Income Distribution from... chart
        private static IEnumerable<Transactions> GetIncomeBottomRightTransactions(BudgeteerDbContext context,
            DateTime dateFrom, DateTime dateUntil,
            string categoryName)
        {
            List<string> incomeXData = new List<string>();
            List<double> incomeYData = new List<double>();
            foreach (AspNetUsers incomePerson in context.AspNetUsers.OrderBy(u => u.Name).ToList())
            {
                List<double> categoryAmounts = (from record in context.Transactions
                                                where
                                                (record.User.Id == incomePerson.Id) &&
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