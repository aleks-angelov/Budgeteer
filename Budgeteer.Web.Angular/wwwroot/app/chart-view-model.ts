class Title {
    text: string;
}

class PiePoint {
    name: string;
    y: number;
}

class Series {
    name: string;
}

class ColumnSeries extends Series {
    data: number[];
}

class PieSeries extends Series {
    data: PiePoint[];
}

export class ChartData {
    title: Title;
}

export class ColumnData extends ChartData {
    xAxisCategories: string[];
    series: ColumnSeries[];
}

export class PieData extends ChartData {
    series: PieSeries;
}