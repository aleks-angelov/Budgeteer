using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using Budgeteer_Web.Models;

namespace Budgeteer_Web.Infrastructure
{
    public class ChartFactoryOptions
    {
        public string ChartType { get; set; }
        public string TransactionType { get; set; }
        public DateTime From { get; set; }
        public DateTime Until { get; set; }
        public string PersonName { get; set; }
        public string CategoryName { get; set; }
    }

    public class ChartFactory
    {
        public static Chart CreateChart(ChartFactoryOptions options)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                // Overview Balance chart
                if (options.TransactionType == "Both")
                {
                    // Spending data
                    List<Transaction> spendingRecords = (from record in context.Transactions
                                                         where record.Category.IsDebit && record.Date >= options.From && record.Date <= options.Until
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
                                                       where !record.Category.IsDebit && record.Date >= options.From && record.Date <= options.Until
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
                        .AddSeries("Spending", options.ChartType, xValue: spendingXData, yValues: spendingYData)
                        .AddSeries("Income", options.ChartType, xValue: incomeXData, yValues: incomeYData)
                        .AddLegend();
                }
                else
                {
                    bool isDebit = options.TransactionType == "Debit";

                    // Group-by-person chart
                    if (options.PersonName != null)
                    {
                    }

                    // Group-by-category chart
                    if (options.CategoryName != null)
                    {
                    }

                    // Overview Spending chart
                    List<string> spendingXData = new List<string>();
                    List<double> spendingYData = new List<double>();

                    foreach (Category spendingCategory in context.Categories.Where(c => c.IsDebit == isDebit).ToList())
                    {
                        double categoryTotal = (from record in context.Transactions
                                                where
                                                    record.Category.CategoryID == spendingCategory.CategoryID && record.Date >= options.From &&
                                                    record.Date <= options.Until
                                                select record.Amount).Sum();

                        if (categoryTotal > 0)
                        {
                            spendingXData.Add(spendingCategory.Name);
                            spendingYData.Add(categoryTotal);
                        }
                    }

                    return new Chart(568, 426, ChartTheme.Blue)
                        .AddTitle("Spending Distribution (last 6 months)")
                        .AddSeries("Spending", options.ChartType, xValue: spendingXData, yValues: spendingYData)
                        .AddLegend();
                }
            }
        }
    }
}