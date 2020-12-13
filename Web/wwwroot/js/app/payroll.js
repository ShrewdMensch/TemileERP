var tableFilterParams = {
    startDate: '',
    endDate: '',
    vessel: ''
};

var calendar, table;

$(document).ready(function () {
    InitializeDataTable();

    InitializeVesselsSelect2();

    AddSelectEventForVesselSelect2();

    InitializeFlatPickrCalendar();

    AddViewAllButtonClickEvent();

    AddPayrollPrintModalLogic();

    AddPrintButtonClickEvent();
});

function AddPrintButtonClickEvent() {
    $("#printBtn").click(function () {
        var year = new Date().getFullYear();
        $("#salaryContainer").print({
            title: 'Temile and Sons Limited Monthly Payroll',
            iframe: false,
            deferred: $.Deferred(),
            doctype: "<!doctype html>",
        });
    });
}

function AddPayrollPrintModalLogic() {
    $("#payrollPrintModal").on("shown.bs.modal", function (event) {
        var button = $(event.relatedTarget);
        var payrollId = button.data("payroll-id");
        var modal = $(this);
        $("#Payroll_Id").val(payrollId);

        modal.find(".modal-body .row").attr("hidden", true);
        modal.find("#loader").attr("hidden", false);
        $(".spinner-border").attr("hidden", false);

        LoadValuesFromAPI(payrollId, modal);
    });
}

function AddViewAllButtonClickEvent() {
    $('#viewAll').on('click', function () {
        $("#filterType").text("All");
        $("#vessel-select").val("");
        $("#vessel-select").trigger("change");

        calendar.clear();

        ResetQueryParams();
        FilterTable();
    });
}

function InitializeFlatPickrCalendar() {
    calendar = flatpickr("#salary-range", {
        mode: "range",
        dateFormat: "d/m/Y",
        maxDate: getLastDayOfCurrentMonth(),
        onValueUpdate: function (selectedDates) {
            if (selectedDates.length > 1) {
                var startDate = getStandardShortDate(new Date(selectedDates[0]));
                var endDate = getStandardShortDate(new Date(selectedDates[1]));

                SetQueryParams(startDate, endDate, null);
                FilterTable();
            }
        },
    });
}

function AddSelectEventForVesselSelect2() {
    $("#vessel-select").on('select2:select', function (e) {
        var data = e.params.data;
        console.log(e.params.data);

        SetQueryParams(null, null, data.text);

        FilterTable();
    });
}

function InitializeVesselsSelect2() {
    $("#vessel-select").select2({
        placeholder: "Select vessel...",
        width: "100%",
        minimumResultsForSearch: Infinity,
        ajax: {
            url: "/api/vessels/ForSelect2",
            data: function (params) {
                var query = {
                    search: params.term,
                };
                return query;
            },
            processResults: function (data) {
                return {
                    results: data,
                };
            },
        },
    });
}

function InitializeDataTable() {
    if ($("#table").length > 0) {
        table = $("#table")
            .addClass("nowrap")
            .dataTable({
                columnDefs: [{ orderable: false, targets: -1 }],
                ajax: {
                    url: "/api/payrolls/AllWithPersonnel",
                    dataSrc: "",
                },
                columns: [
                    {
                        data: "personnelName",
                        render: function (data, type, personnelPayroll) {
                            var photo = personnelPayroll.personnelPhoto ? personnelPayroll.personnelPhoto : '/assets/img/user.jpg';

                            return ('<div class="d-flex"><div class="usr-img-frame mr-2 rounded-circle"> ' +
                                '<img alt="avatar" class="img-fluid rounded-circle" src="' + photo +
                                '"></div>' +
                                '<p class="align-self-center mb-0 admin-name">' + personnelPayroll.personnelName + '</p></div>');
                        }
                    },
                    {
                        data: "dailyRateInCurrency",
                    },
                    {
                        data: "daysWorked",
                    },
                    {
                        data: "vessel",
                    },
                    {
                        data: "grossPayInCurrency",
                    },
                    {
                        data: "netPayInCurrency",
                    },
                    {
                        data: "period",
                    },
                    {
                        data: "personnelName",
                        render: function (data, type, personnelPayroll) {
                            return (
                                '<a class="btn btn-primary" data-toggle="modal" data-target="#payrollPrintModal"' +
                                ' data-personnel-id="' + data.personnelId + '" ' +
                                'data-payroll-id="' + personnelPayroll.payrollId + '"> <svg xmlns="http://www.w3.org/2000/svg" ' +
                                'width="24" height="24" viewBox="0 0 24 24" fill="none"' +
                                ' stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" ' +
                                'class="feather feather-file-text"><path d="M14' +
                                ' 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z">' +
                                '</path><polyline points="14 2 14 8 20 8"></polyline><line x1="16" y1="13" x2="8" y2="13">' +
                                '</line><line x1="16" y1="17" x2="8" y2="17"></line><polyline points="10 9 9 9 8 9"></polyline></svg> </a>'
                            );
                        },
                    }
                ]
            });

        $(".custom-filter1").prepend(
            '<form class="form-inline"><div class="input-group mb-2 mr-sm-2"> <label class="sr-only" for="salary-range">Salary Period</label><div class="cal-icon"> <input type="text" class="form-control" id="salary-range" placeholder="Select salary period..."></div></div><div class="input-group mb-2 mr-sm-2"> <label class="sr-only" for="vessel-select">Vessel</label> <select id="vessel-select" class="form-control" title="Select vessel"></select></div> <button id="viewAll" type="button" class="btn btn-primary mb-2">View All</button></form>'
        );
        $(".custom-filter2").prepend(
            '<select id="vessel-select" class="mx-2 mb-2" title="Select vessel"></select>'
        );
        $(".custom-filter3").append(
            '<button class="btn btn-primary btn-sm btn-block mx-2 mb-2">View All</button>'
        );
        $(".message-area").prepend(
            '<p class="font-weight-bold">Showing: <span class="text-primary" id="filterType">All Payroll(s)</span> <span class="text-primary"></span></p>'
        );
    }
}

function LoadValuesFromAPI(payrollId, modal) {
    $.ajax({
        url: "/api/payments/" + payrollId,
        dataType: "json",
        type: "GET",
        success: function (data) {

            UpdateValues(data);

            AddAllowances(modal, data);

            AddDeductions(modal, data);

            AddArrears(modal, data);

            modal.find(".modal-body .row").attr("hidden", false);
            $("#loader").attr("hidden", true);
            $(".spinner-border").attr("hidden", true);
        },
        error: function () {
            alert("Error occurred...");
        },
    });
}

function AddDeductions(modal, data) {
    modal.find("#deductions").html("");

    $.each(data.deductions, function (index, value) {
        var valueHtml = "<tr><td><strong>" +
            value.deductionName +
            '</strong><span class="float-right">' +
            value.deductedAmount +
            " (" +
            value.deductedPercentage +
            ")" +
            "</span></td></tr>";
        modal.find("#deductions").append(valueHtml);
    });

    $.each(data.specificDeductions, function (index, value) {
        var valueHtml = "<tr><td><strong>" +
            value.name +
            '</strong><span class="float-right">' +
            value.amountInCurrency +
            "</span></td></tr>";
        modal.find("#deductions").append(valueHtml);
    });

    $('#deductionsTitle').attr("hidden", true);

    if ($('#deductions tr').length > 0) {

        $('#deductionsTitle').attr("hidden", false);

        var totalHtml = '<tr class="summation"><td><strong>Total Deductions</strong> <span class="float-right"><strong>' +
            data.totalDeductedAmount +
            "</strong></span></tr>";

        modal.find("#deductions").append(totalHtml);
    }

}

function AddAllowances(modal, data) {
    modal.find("#allowances").html("");
    $('#rowAllowancesTable').attr('hidden', true);

    if (data.allowances.length > 0) {
        $('#rowAllowancesTable').attr('hidden', false);

        (data.allowances.length > 1) ? $('#allowanceTitle').text("Allowances") : $('#allowanceTitle').text("Allowance");

        $.each(data.allowances, function (index, value) {
            var valueHtml = "<tr><td><strong>" +
                value.name +
                '</strong><span class="float-right">' +
                value.amountInCurrency +
                "</span></td></tr>";
            modal.find("#allowances").append(valueHtml);
        });
    }

}

function AddArrears(modal, data) {
    modal.find("#arrearContainer").html("");
    $('#arrearTitle').attr('hidden', true);
    $('#totalArrearsContainer').attr('hidden', true);
    $('#arrearContainer').attr('hidden', true);
    $('#arrearRow').attr('hidden', true);

    if (data.arrears.length > 0) {
        $('#arrearTitle').attr('hidden', false);
        $('#totalArrearsContainer').attr('hidden', false);
        $('#arrearContainer').attr('hidden', false);
        $('#arrearRow').attr('hidden', false);

        (data.arrears.length > 1) ? $('#arrearTitle').text("Arrears") : $('#arrearTitle').text("Arrear");

        $.each(data.arrears, function (index, value) {
            var weekendMsg = value.workedWeekend ? " (including weekends)" : " (excluding weekends)";
            var period = value.period + weekendMsg;
            var workDetail = value.vessel + " Vessel as " + value.personnelDesignation;

            var valueHtml = '<table class="table table-borderless"><tbody><tr><td> <strong>Period' +
                '</strong><span class="float-right">' + period +
                '</span></td></tr><tr><td><strong>Days Worked</strong><span class="float-right">' + value.daysWorked + '</span>' +
                '</td></tr><tr><td><strong>Daily Rate</strong><span class="float-right">' + value.personnelDailyRate + '</span></td>' +
                '</tr><tr><td> <strong>Worked on</strong><span class="float-right">' + workDetail + '</span></td>' +
                '</tr><tr><td><strong>Amount</strong><span class="float-right">' + value.amount + '</span></td></tr></tbody></table>';

            modal.find("#arrearContainer").append(valueHtml);
            $('#totalArrears').text(data.totalArrears);
        });
    }

}

function UpdateValues(data) {
    $("#paySlipNumber").text(data.paymentSlipNumber);
    $("#personnelName").text(data.personnelName);
    $("#personnelId").text(data.personnelId);
    $("#personnelDesignation").text(
        data.personnelDesignation ? data.personnelDesignation : ""
    );
    $('#desginationDetails').attr('hidden', data.personnelDesignation == null);
    $("#personnelDateJoined").text(data.personnelDateJoined);
    $("#bank").text(data.bank);
    $("#accountName").text(data.accountName);
    $("#accountNumber").text(data.accountNumber);
    $("#dailyRate").text(data.dailyRate);
    $("#grossPay").text(data.grossPay);
    $("#netPay").text(data.netPay);
    $("#totalEarnings").text(data.totalEarnings);
    $("#netPayInWords").text(
        toWords(parseInt(data.netPayRaw).toString()) + " naira"
    );
    $("#vessel").text(data.vessel);
    $("#date").text(data.period);
    $("#daysWorked").text(data.daysWorked);
    $("#daysWorked2").text(data.daysWorked);
    $("#workedWeekend").text(data.workedWeekend ? "including weekends" : "excluding weekends");
    $("#dateTitle").text(data.period);
}

function SetQueryParams(startDate, endDate, vessel) {
    tableFilterParams.startDate = startDate ? startDate : tableFilterParams.startDate;
    tableFilterParams.endDate = endDate ? endDate : tableFilterParams.endDate;
    tableFilterParams.vessel = vessel ? vessel : tableFilterParams.vessel;
}

function ResetQueryParams() {
    tableFilterParams.startDate = '';
    tableFilterParams.endDate = '';
    tableFilterParams.vessel = '';
}

function GetQueryParams() {
    var param = "?startDate=" + tableFilterParams.startDate +
        "&endDate=" + tableFilterParams.endDate + "&vessel=" + tableFilterParams.vessel;
    return param;
}

function FilterTable() {
    var table = $("#table").DataTable();
    var startDate = tableFilterParams.startDate,
        endDate = tableFilterParams.endDate, vessel = tableFilterParams.vessel;

    var filter =
        ((startDate === '' && endDate === '' && vessel === '') ? "All Payroll(s)" : (startDate === '' && vessel !== '') ?
            "Payroll(s) filtered by vessel" : (startDate !== '' && vessel === '') ?
                "Payroll(s) filtered by period" : "Payroll(s) filtered by period and vessel");


    $("#filterType").text(filter);
    table.ajax.url("/api/payrolls/AllWithPersonnel" + GetQueryParams()).load(null, false);
}