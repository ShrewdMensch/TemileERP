function getStandardShortDate(yourDate) {
    const offset = yourDate.getTimezoneOffset()
    yourDate = new Date(yourDate.getTime() - (offset * 60 * 1000))
    return yourDate.toISOString().split('T')[0]
}

function getFirstDayOfCurrentMonth() {
    var date = new Date();
    var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);

    return firstDay;
}

function getFirstDayOfLastMonth() {
    var date = new Date();
    var year = date.getFullYear();
    var month = date.getMonth();

    month === 0 ? year-- : year;
    month === 0 ? month= 12 : month;

    var firstDay = new Date(year, month - 1, 1);

    return firstDay;
}

function getLastDayOfCurrentMonth() {
    var date = new Date();
    var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

    return lastDay;
}