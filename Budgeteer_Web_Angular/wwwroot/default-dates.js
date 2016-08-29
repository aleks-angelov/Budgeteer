var date_0 = new Date();
var date_6M = new Date();
date_6M.setMonth(date_6M.getMonth() - 6);

var overviewDate = document.getElementById("overviewDate");
if (overviewDate)
    overviewDate.valueAsDate = date_0;

var spendingDateFrom = document.getElementById("spendingDateFrom");
if (spendingDateFrom)
    spendingDateFrom.valueAsDate = date_6M;

var spendingDateUntil = document.getElementById("spendingDateUntil");
if (spendingDateUntil)
    spendingDateUntil.valueAsDate = date_0;

var incomeDateFrom = document.getElementById("incomeDateFrom");
if (incomeDateFrom)
    incomeDateFrom.valueAsDate = date_6M;

var incomeDateUntil = document.getElementById("incomeDateUntil");
if (incomeDateUntil)
    incomeDateUntil.valueAsDate = date_0;