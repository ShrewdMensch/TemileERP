$(document).ready(function () {
    if ($("#table").length > 0) {
        $("#table")
            .addClass("nowrap")
            .dataTable({
                columnDefs: [{ orderable: false, targets: -1 }],
            });

        $(".right-buttons").append(
            '<div class="float-right mt-2"> <a href="/Accounting/Deductions/ApplyDeduction" class="btn btn-purple btn-primary btn-rounded float-right"><i class="fas fa-sync m-r-5"></i> Reapply Deductions</a></div>'
        );
    }

    if ($("#table2").length > 0) {
        $("#table2")
            .addClass("nowrap")
            .DataTable({
                stateSave: false,
                dom: "tip",
                lengthMenu: [[10], [10]],
                ajax: {
                    url: "/api/personnels/AllPlusPayroll",
                    dataSrc: "",
                },
                columns: [
                    {
                        data: "personnelName",
                    },
                    {
                        data: "dailyRate",
                    },
                    {
                        data: "daysWorked",
                    },
                    {
                        data: "vessel",
                    },
                    {
                        data: "grossPay",
                    },
                ],
            });
    }

    InitializeVesselsSelect2();

    $('#addAllowance').on('click', function () {
        AddAllowanceItem('', '', $('.allowance').length + 1);
    })

    $('#addDeduction').on('click', function () {
        AddDeductionItem('', '', $('.deduction').length + 1);
    })


    AddDeleteHandler();

    //Personnel Edit Information Logic
    //Personnel Edit Form and API pull logic
    var form = $("#currentPayrollVariablesForm");
    var initialform;

    $(
        "#currentPayrollVariablesForm :input, #currentPayrollVariablesForm textarea"
    ).on("change", function () {
        $("#editBtn").attr("disabled", initialform === $(form).serialize());
    });

    $("#currentPayrollVariablesModal").on("shown.bs.modal", function (event) {
        var button = $(event.relatedTarget);
        var personnelId = button.data("personnel-id");
        var payrollId = button.data("payroll-id");
        var modal = $(this);
        $("#Personnel_Id").val(personnelId);
        $("#Payroll_Id").val(payrollId);


        AddSpinForAllowanceAmounts();
        AddSpinForSpecificDeductionAmounts();

        ShowSpinner(modal);

        initialform = LoadValuesFromAPI(personnelId, initialform, form, modal);
        ShowAccordionOnValidationFailure();
    });

    $("#personnelPayrollTableModal").on("shown.bs.modal", function (event) {
        var button = $(event.relatedTarget);
        var modal = $(this);
        var query = button.data("query");
        var tableTitle = button.data("title");
        var table = $("#table2").DataTable();
        modal.find("#tableTitle").text(tableTitle);
        table.ajax.url("/api/personnels/" + query).load(null, false);
    });

    $("input[name='DailyRate']").TouchSpin({
        min: 500,
        max: 10000000,
        step: 10,
        decimals: 2,
        boostat: 5,
        initval: 0,
        prefix: "₦",
        buttondown_class: "btn btn-classic btn-danger",
        buttonup_class: "btn btn-classic btn-primary",
    });
});

function LoadValuesFromAPI(personnelId, initialform, form, modal) {
    $.ajax({
        url: "/api/personnels/" + personnelId + "/currentpayroll",
        dataType: "json",
        type: "GET",
        success: function (data) {

            UpdateFields(data);
            AddDaysWorkedRange(data);

            initialform = $(form).serialize();

            HideSpinner(modal);

            ShowOrHideDeductionsAccordion(data.specificDeductions.length > 0);
            ShowOrHideAllowancesAccordion(data.allowances.length > 0);
        },
        error: function () {
            alert("Error occurred...");
        },
    });
    return initialform;
}

function HideSpinner(modal) {
    modal.find(".modal-body .row").attr("hidden", false);
    $("#loader").attr("hidden", true);
    $(".spinner-border").attr("hidden", true);
}

function ShowSpinner(modal) {
    modal.find(".modal-body .row").attr("hidden", true);
    modal.find("#loader").attr("hidden", false);
    $(".spinner-border").attr("hidden", false);
    $("#editBtn").attr("disabled", true);
}

function AddAllowanceItem(name = '', amount = 10, count = '') {
    (count == null || count == '') ? count = 1 : count = count;
    var allowanceHtml = '<tr class="allowance"><td class="serial-no"></td><td>' +
        '<input required class="form-control table-input description" type="text"' +
        'data-parsley-required-message="Description is required"' +
        ' name="AllowanceNames" value="' + name + '" data-parsley-trigger-after-failure="input change">' +
        '</td><td> <input required data-parsley-type="number" data-parsley-min="10"' +
        ' class="form-control table-input amount" type="text" data-parsley-errors-container="#allowance_error' + count + '"' +
        ' name="AllowanceAmounts" data-parsley-required-message="Amount is required" value="' + amount
        + '" data-parsley-trigger-after-failure="input change"> <span id="allowance_error' + count + '"></span>' +
        '</td><td><a href="javascript:void(0)" title="Remove item" class="js-delete">' +
        '<i class="text-danger fa fa-trash"></i></a></td></tr>';

    $('#allowancesBody').append(allowanceHtml);
    ReorderAllowances();
    AddSpinForAllowanceAmounts();
    AddDeleteHandler();
    HideAppropriateTable();
}

function AddDeductionItem(name = '', amount = 10, count = '') {
    (count == null || count == '') ? count = 1 : count = count;
    var deductionHtml = '<tr class="deduction"><td class="serial-no"></td><td>' +
        '<input required class="form-control table-input description" type="text"' +
        'data-parsley-required-message="Description is required"' +
        ' name="SpecificDeductionNames" value="' + name + '" data-parsley-trigger-after-failure="input change">' +
        '</td><td> <input required data-parsley-type="number" data-parsley-min="10"' +
        ' class="form-control table-input amount" type="text" data-parsley-errors-container="#deduction_error' + count + '"' +
        ' name="SpecificDeductionAmounts" data-parsley-required-message="Amount is required" value="' + amount
        + '" data-parsley-trigger-after-failure="input change"> <span id="deduction_error' + count + '"></span>' +
        '</td><td><a href="javascript:void(0)" title="Remove item" class="js-delete">' +
        '<i class="text-danger fa fa-trash"></i></a></td></tr>';

    $('#deductionsBody').append(deductionHtml);
    ReorderDeductions();
    AddSpinForSpecificDeductionAmounts();
    AddDeleteHandler();
    HideAppropriateTable();
}

function ReorderAllowances() {
    $('.allowance').each(function (index) {
        var allowance = $(this);
        allowance.children('.serial-no').text(index + 1);
    });
}
function ReorderDeductions() {
    $('.deduction').each(function (index) {
        var deduction = $(this);
        deduction.children('.serial-no').text(index + 1);
    });
}

function HideAppropriateTable() {
    var noAllowance = $('.allowance').length < 1;
    var noDeduction = $('.deduction').length < 1;

    $("#deductionTable").attr("hidden", noDeduction);
    $("#allowanceTable").attr("hidden", noAllowance);

    ShowOrHideAllowancesAccordion(!noAllowance);
    ShowOrHideDeductionsAccordion(!noDeduction);
}

function AddDeleteHandler() {
    if ($(".js-delete").length > 0) {
        $('.js-delete').on('click', function () {
            var button = $(this);
            button.parents('tr').remove();

            ReorderAllowances();
            ReorderDeductions();
            HideAppropriateTable();
        })
    }

}

function ShowOrHideAllowancesAccordion(show) {
    $("#allowancesAccordion").attr('class', show ? "collapsed show" : "collapse");
    $("#allowancesAccordion").attr('aria-expanded', show);
}

function ShowOrHideDeductionsAccordion(show) {
    $("#deductionsAccordion").attr('class', show ? "collapsed show" : "collapse");
    $("#deductionsAccordion").attr('aria-expanded', show);
}

function ShowAccordionOnValidationFailure() {
    $("#currentPayrollVariablesForm")
        .parsley()
        .on("form:error", function () {
            ShowOrHideAllowancesAccordion(true);
            ShowOrHideDeductionsAccordion(true);
        });
}

function InitializeVesselsSelect2() {
    $.ajax({
        url: "/api/vessels/ForSelect2",
        dataType: "json",
        type: "GET",
        success: function (data) {
            $("#Edit_Vessel").select2({
                data: data,
                placeholder: "Select vessel...",
                dropdownParent: $("#currentPayrollVariablesModal"),
                width: "100%",
                minimumResultsForSearch: Infinity,
            });
        },
        error: function () {
            alert("Error occurred...");
        },
    });
}

function UpdateFields(data) {
    $("#Edit_Name").val(data.personnelName);
    $("#Edit_Name").attr("title", data.personnelName);
    $("#Edit_DailyRate").val(data.dailyRate);

    $("#Edit_Vessel").val(data.vessel);
    $("#Edit_Vessel").trigger("change");

    $("#allowancesBody").empty();
    $("#deductionsBody").empty();

    $("#Edit_WorkedWeekend").prop("checked", data.workedWeekend);

    $.each(data.specificDeductions, function (index, value) {
        AddDeductionItem(value.name, value.amount, index + 1);
    });

    $.each(data.allowances, function (index, value) {
        AddAllowanceItem(value.name, value.amount, index + 1);
    });

    HideAppropriateTable();
}

function AddDaysWorkedRange(data) {
    flatpickr("#Edit_DaysWorked", {
        mode: "range",
        minDate: getFirstDayOfLastMonth(),
        maxDate: getLastDayOfCurrentMonth(),
        mode: "range",
        dateFormat: "Y-m-d",
        defaultDate: [data.startDate, data.endDate],
        onClose: function (selectedDates) {
            $('#Edit_StartDate').val(getStandardShortDate(new Date(selectedDates[0])));
            $('#Edit_EndDate').val(getStandardShortDate(new Date(selectedDates[1])));
        },
        onReady: function (selectedDates) {
            $('#Edit_StartDate').val(getStandardShortDate(new Date(selectedDates[0])));
            $('#Edit_EndDate').val(getStandardShortDate(new Date(selectedDates[1])));
        },
    });
}

function AddSpinForAllowanceAmounts() {
    $("input[name='AllowanceAmounts']").TouchSpin({
        max: 10000000,
        step: 10,
        decimals: 2,
        boostat: 5,
        initval: 0,
        prefix: "₦",
        buttondown_class: "btn btn-classic btn-danger",
        buttonup_class: "btn btn-classic btn-primary",
    });
}
function AddSpinForSpecificDeductionAmounts() {
    $("input[name='SpecificDeductionAmounts']").TouchSpin({
        max: 10000000,
        step: 10,
        decimals: 2,
        boostat: 5,
        initval: 0,
        prefix: "₦",
        buttondown_class: "btn btn-classic btn-danger",
        buttonup_class: "btn btn-classic btn-primary",
    });
}