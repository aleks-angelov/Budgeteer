
function listCategories(data) {

    var target = $("#CategoryName");
    target.empty();
    for (var i = 0; i < data.length; i++) {
        var catName = data[i];
        target.append("<option value=\"" + catName + "\">" + catName + "</option>");
    }
}

function dateString(dateFrom, dateUntil) {

    return "&dateFrom=" +
        (dateFrom.getMonth() + 1) +
        "%2F" +
        dateFrom.getDate() +
        "%2F" +
        dateFrom.getFullYear() +
        "%2000%3A00%3A00" +
        "&dateUntil=" +
        (dateUntil.getMonth() + 1) +
        "%2F" +
        dateUntil.getDate() +
        "%2F" +
        dateUntil.getFullYear() +
        "%2000%3A00%3A00";
}

function brString() {

    return "&br=" + new Date().getTime();
}

function updateOverviewCharts() {

    var dateFrom = new Date();
    dateFrom.setMonth(dateFrom.getMonth() - 6);
    var dateUntil = new Date();

    var target1 = $("#OverviewLeftChart");
    target1
        .prop("src", "/Home/DisplayChart?chartName=OverviewLeftChart" + dateString(dateFrom, dateUntil) + brString());

    var target2 = $("#OverviewRightChart");
    target2.prop("src",
        "/Home/DisplayChart?chartName=OverviewRightChart" + dateString(dateFrom, dateUntil) + brString());
}

function updateSpendingLeftCharts() {

    var dateFrom = document.getElementById("SpendingDateFrom").valueAsDate;
    var dateUntil = document.getElementById("SpendingDateUntil").valueAsDate;

    var target1 = $("#SpendingTopLeftChart");
    target1.prop("src",
        "/Home/DisplayChart?chartName=SpendingTopLeftChart" +
        dateString(dateFrom, dateUntil) +
        "&personName=" +
        $("#SpendingPersonName").val() +
        brString());

    var target2 = $("#SpendingBottomLeftChart");
    target2.prop("src",
        "/Home/DisplayChart?chartName=SpendingBottomLeftChart" +
        dateString(dateFrom, dateUntil) +
        "&personName=" +
        $("#SpendingPersonName").val() +
        brString());
}

function updateSpendingRightCharts() {

    var dateFrom = document.getElementById("SpendingDateFrom").valueAsDate;
    var dateUntil = document.getElementById("SpendingDateUntil").valueAsDate;

    var target1 = $("#SpendingTopRightChart");
    target1.prop("src",
        "/Home/DisplayChart?chartName=SpendingTopRightChart" +
        dateString(dateFrom, dateUntil) +
        "&categoryName=" +
        $("#SpendingCategoryName").val() +
        brString());

    var target2 = $("#SpendingBottomRightChart");
    target2.prop("src",
        "/Home/DisplayChart?chartName=SpendingBottomRightChart" +
        dateString(dateFrom, dateUntil) +
        "&categoryName=" +
        $("#SpendingCategoryName").val() +
        brString());
}

function updateSpendingCharts() {

    updateSpendingLeftCharts();
    updateSpendingRightCharts();
}

$("#SpendingPersonName").change(updateSpendingLeftCharts);

$("#SpendingDateFrom").blur(updateSpendingCharts);

$("#SpendingDateUntil").blur(updateSpendingCharts);

$("#SpendingCategoryName").change(updateSpendingRightCharts);

function updateIncomeLeftCharts() {

    var dateFrom = document.getElementById("IncomeDateFrom").valueAsDate;
    var dateUntil = document.getElementById("IncomeDateUntil").valueAsDate;

    var target1 = $("#IncomeTopLeftChart");
    target1.prop("src",
        "/Home/DisplayChart?chartName=IncomeTopLeftChart" +
        dateString(dateFrom, dateUntil) +
        "&personName=" +
        $("#IncomePersonName").val() +
        brString());

    var target2 = $("#IncomeBottomLeftChart");
    target2.prop("src",
        "/Home/DisplayChart?chartName=IncomeBottomLeftChart" +
        dateString(dateFrom, dateUntil) +
        "&personName=" +
        $("#IncomePersonName").val() +
        brString());
}

function updateIncomeRightCharts() {

    var dateFrom = document.getElementById("IncomeDateFrom").valueAsDate;
    var dateUntil = document.getElementById("IncomeDateUntil").valueAsDate;

    var target1 = $("#IncomeTopRightChart");
    target1.prop("src",
        "/Home/DisplayChart?chartName=IncomeTopRightChart" +
        dateString(dateFrom, dateUntil) +
        "&categoryName=" +
        $("#IncomeCategoryName").val() +
        brString());

    var target2 = $("#IncomeBottomRightChart");
    target2.prop("src",
        "/Home/DisplayChart?chartName=IncomeBottomRightChart" +
        dateString(dateFrom, dateUntil) +
        "&categoryName=" +
        $("#IncomeCategoryName").val() +
        brString());
}

function updateIncomeCharts() {

    updateIncomeLeftCharts();
    updateIncomeRightCharts();
}

$("#IncomePersonName").change(updateIncomeLeftCharts);

$("#IncomeDateFrom").blur(updateIncomeCharts);

$("#IncomeDateUntil").blur(updateIncomeCharts);

$("#IncomeCategoryName").change(updateIncomeRightCharts);