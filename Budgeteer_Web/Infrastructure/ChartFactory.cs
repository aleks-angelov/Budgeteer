using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using Budgeteer_Web.Models;

namespace Budgeteer_Web.Infrastructure
{
    public static class ChartFactory
    {
        public static Chart CreateChart(string name)
        {
            switch (name)
            {
                case "OverviewLeftChart":
                    return CreateOverviewLeftChart(DateTime.Today.AddMonths(-6), DateTime.Today);
                case "OverviewRightChart":
                    return CreateOverviewRightChart(DateTime.Today.AddMonths(-6), DateTime.Today);
                default:
                    return null;
            }
        }

        // Overview Balance chart
        private static Chart CreateOverviewLeftChart(DateTime dateFrom, DateTime dateUntil)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                // Spending data
                List<Transaction> spendingRecords = (from record in context.Transactions
                    where
                        record.Category.IsDebit && record.Date >= dateFrom &&
                        record.Date <= dateUntil
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

                // Income data
                List<Transaction> incomeRecords = (from record in context.Transactions
                    where
                        !record.Category.IsDebit && record.Date >= dateFrom &&
                        record.Date <= dateUntil
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

                return new Chart(568, 426, ChartTheme.Blue)
                    .AddTitle("Budget Balance (last 6 months)")
                    .AddSeries("Spending", "Column", xValue: spendingXData, yValues: spendingYData)
                    .AddSeries("Income", "Column", xValue: incomeXData, yValues: incomeYData)
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
                foreach (Category spendingCategory in context.Categories.Where(c => c.IsDebit).ToList())
                {
                    double categoryTotal = (from record in context.Transactions
                        where
                            record.Category.CategoryID == spendingCategory.CategoryID &&
                            record.Date >= dateFrom &&
                            record.Date <= dateUntil
                        select record.Amount).Sum();

                    if (categoryTotal > 0)
                    {
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
    }
}