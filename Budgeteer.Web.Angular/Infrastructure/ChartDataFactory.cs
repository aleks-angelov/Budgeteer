using System;
using System.Collections.Generic;
using System.Linq;
using Budgeteer.Web.Angular.Models;

namespace Budgeteer.Web.Angular.Infrastructure
{
    public static class ChartDataFactory
    {
        // Delegate
        public static ChartData GetChartData(BudgeteerDbContext context, string chartName,
            DateTime dateFrom, DateTime dateUntil, string personName, string categoryName)
        {
            switch (chartName)
            {
                case "OverviewLeftChart":
                    return GetOverviewLeftData(context, dateFrom, dateUntil);

                case "OverviewRightChart":
                    return GetOverviewRightData(context, dateFrom, dateUntil);

                case "SpendingTopLeftChart":
                    return GetSpendingTopLeftData(context, dateFrom, dateUntil, personName);

                case "SpendingBottomLeftChart":
                    return GetSpendingBottomLeftData(context, dateFrom, dateUntil, personName);

                case "SpendingTopRightChart":
                    return GetSpendingTopRightData(context, dateFrom, dateUntil, categoryName);

                case "SpendingBottomRightChart":
                    return GetSpendingBottomRightData(context, dateFrom, dateUntil, categoryName);

                case "IncomeTopLeftChart":
                    return GetIncomeTopLeftData(context, dateFrom, dateUntil, personName);

                case "IncomeBottomLeftChart":
                    return GetIncomeBottomLeftData(context, dateFrom, dateUntil, personName);

                case "IncomeTopRightChart":
                    return GetIncomeTopRightData(context, dateFrom, dateUntil, categoryName);

                case "IncomeBottomRightChart":
                    return GetIncomeBottomRightData(context, dateFrom, dateUntil, categoryName);

                default:
                    return null;
            }
        }

        // Overview Balance chart
        private static ChartData GetOverviewLeftData(BudgeteerDbContext context,
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

            ColumnData data = new ColumnData
            {
                Title = new Title
                {
                    Text = "Budget Balance (last 6 months)"
                },
                XAxisCategories = incomeXData.Count > spendingXData.Count ? incomeXData : spendingXData,
                Series = new List<ColumnSeries>
                {
                    new ColumnSeries
                    {
                        Name = "Income",
                        Data = incomeYData
                    },
                    new ColumnSeries
                    {
                        Name = "Spending",
                        Data = spendingYData
                    }
                }
            };

            return data;
        }

        // Overview Spending chart
        private static ChartData GetOverviewRightData(BudgeteerDbContext context,
            DateTime dateFrom, DateTime dateUntil)
        {
            List<string> spendingXData = new List<string>();
            List<double> spendingYData = new List<double>();
            foreach (
                Categories spendingCategory in context.Categories.Where(c => c.IsDebit).OrderBy(c => c.Name).ToList())
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

            PieData data = new PieData
            {
                Title = new Title
                {
                    Text = "Spending Distribution (last 6 months)"
                },
                Series = new PieSeries
                {
                    Name = "Spending",
                    Data = new List<PiePoint>()
                }
            };
            for (int i = 0; i < spendingXData.Count; i++)
                data.Series.Data.Add(new PiePoint
                {
                    Name = spendingXData[i],
                    Y = spendingYData[i]
                });

            return data;
        }

        // Spending of... chart
        private static ChartData GetSpendingTopLeftData(BudgeteerDbContext context,
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

            ColumnData data = new ColumnData
            {
                Title = new Title
                {
                    Text = "Spending of " + personName
                },
                XAxisCategories = spendingXData,
                Series = new List<ColumnSeries>
                {
                    new ColumnSeries
                    {
                        Name = "Spending",
                        Data = spendingYData
                    }
                }
            };

            return data;
        }

        // Spending Distribution of... chart
        private static ChartData GetSpendingBottomLeftData(BudgeteerDbContext context,
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

            PieData data = new PieData
            {
                Title = new Title
                {
                    Text = "Spending Distribution of " + personName
                },
                Series = new PieSeries
                {
                    Name = "Spending",
                    Data = new List<PiePoint>()
                }
            };
            for (int i = 0; i < spendingXData.Count; i++)
                data.Series.Data.Add(new PiePoint
                {
                    Name = spendingXData[i],
                    Y = spendingYData[i]
                });

            return data;
        }

        // Spending for... chart
        private static ChartData GetSpendingTopRightData(BudgeteerDbContext context,
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

            ColumnData data = new ColumnData
            {
                Title = new Title
                {
                    Text = "Spending for " + categoryName
                },
                XAxisCategories = spendingXData,
                Series = new List<ColumnSeries>
                {
                    new ColumnSeries
                    {
                        Name = "Spending",
                        Data = spendingYData
                    }
                }
            };

            return data;
        }

        // Spending Distribution for... chart
        private static ChartData GetSpendingBottomRightData(BudgeteerDbContext context,
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

            PieData data = new PieData
            {
                Title = new Title
                {
                    Text = "Spending Distribution for " + categoryName
                },
                Series = new PieSeries
                {
                    Name = "Spending",
                    Data = new List<PiePoint>()
                }
            };
            for (int i = 0; i < spendingXData.Count; i++)
                data.Series.Data.Add(new PiePoint
                {
                    Name = spendingXData[i],
                    Y = spendingYData[i]
                });

            return data;
        }

        // Income of... chart
        private static ChartData GetIncomeTopLeftData(BudgeteerDbContext context,
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

            ColumnData data = new ColumnData
            {
                Title = new Title
                {
                    Text = "Income of " + personName
                },
                XAxisCategories = incomeXData,
                Series = new List<ColumnSeries>
                {
                    new ColumnSeries
                    {
                        Name = "Income",
                        Data = incomeYData
                    }
                }
            };

            return data;
        }

        // Income Distribution of... chart
        private static ChartData GetIncomeBottomLeftData(BudgeteerDbContext context,
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

            PieData data = new PieData
            {
                Title = new Title
                {
                    Text = "Income Distribution of " + personName
                },
                Series = new PieSeries
                {
                    Name = "Income",
                    Data = new List<PiePoint>()
                }
            };
            for (int i = 0; i < incomeXData.Count; i++)
                data.Series.Data.Add(new PiePoint
                {
                    Name = incomeXData[i],
                    Y = incomeYData[i]
                });

            return data;
        }

        // Income from... chart
        private static ChartData GetIncomeTopRightData(BudgeteerDbContext context,
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

            ColumnData data = new ColumnData
            {
                Title = new Title
                {
                    Text = "Income from " + categoryName
                },
                XAxisCategories = incomeXData,
                Series = new List<ColumnSeries>
                {
                    new ColumnSeries
                    {
                        Name = "Income",
                        Data = incomeYData
                    }
                }
            };

            return data;
        }

        // Income Distribution from... chart
        private static ChartData GetIncomeBottomRightData(BudgeteerDbContext context,
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

            PieData data = new PieData
            {
                Title = new Title
                {
                    Text = "Income Distribution from " + categoryName
                },
                Series = new PieSeries
                {
                    Name = "Income",
                    Data = new List<PiePoint>()
                }
            };
            for (int i = 0; i < incomeXData.Count; i++)
                data.Series.Data.Add(new PiePoint
                {
                    Name = incomeXData[i],
                    Y = incomeYData[i]
                });

            return data;
        }
    }
}