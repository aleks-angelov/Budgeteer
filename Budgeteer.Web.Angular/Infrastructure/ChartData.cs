using System.Collections.Generic;

namespace Budgeteer.Web.Angular.Infrastructure
{
    public class Title
    {
        public string Text { get; set; }
    }

    public class PiePoint
    {
        public string Name { get; set; }

        public double Y { get; set; }
    }

    public class Series
    {
        public string Name { get; set; }
    }

    public class ColumnSeries : Series
    {
        public List<double> Data { get; set; }
    }

    public class PieSeries : Series
    {
        public List<PiePoint> Data { get; set; }
    }

    public class ChartData
    {
        public Title Title { get; set; }
    }

    public class ColumnData : ChartData
    {
        public List<string> XAxisCategories { get; set; }

        public List<ColumnSeries> Series { get; set; }
    }

    public class PieData : ChartData
    {
        public PieSeries Series { get; set; }
    }
}