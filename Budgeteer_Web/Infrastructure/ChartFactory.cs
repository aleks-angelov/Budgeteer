using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using Budgeteer_Web.Models;

namespace Budgeteer_Web.Infrastructure
{
    public enum TransactionType
    {
        Debit = 1,
        Credit = 2,
        Both = 3
    }

    public class ChartFactoryOptions
    {
        public bool IsColumn { get; set; }
        public TransactionType Type { get; set; }
        public DateTime From { get; set; }
        public DateTime Until { get; set; }
        public string PersonName { get; set; }
        public string CategoryName { get; set; }
    }

    public class ChartFactory
    {
        public static Chart CreateChart(ChartFactoryOptions options, ApplicationDbContext context)
        {
            

            return new Chart(568, 426, ChartTheme.Blue)
                .AddTitle("SAMPLE CHART")
                .AddLegend()
                .AddSeries("WebSite", "Pie", xValue: new[] { "Digg", "DZone", "DotNetKicks", "StumbleUpon" },
                    yValues: new[] { "150000", "180000", "120000", "250000" });
        }
    }
}