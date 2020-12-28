var calendar;
var arrearsCalendar;

$(document).ready(function () {

    InitializeDataTables();

    InitializeVesselsSelect2();

    AddButtonClickListerners();

    InitializeCalendar();

    AddDeleteHandler();

    AddUpdateCurrentPayrollVariablesLogic();

    AddPersonnelPayrollTableModalOnShowEvent();

    AddArrearPeriodValidator();
});

function AddArrearPeriodValidator() {
    Parsley.addValidator('validateArrear', {
        validateString: function(value) {

            if (value == null || value == '')
                return false;

            var personId = $('#Personnel_Id').val();

            console.clear();

            return $.ajax("/api/arrears/validate?" + "period=" + value + "&personnelId=" + personId);
        },
        messages: {
            en: "Invalid arrear period specified"
        }
    });
}

function AddPersonnelPayrollTableModalOnShowEvent() {
    $("#personnelPayrollTableModal").on("shown.bs.modal", function (event) {
        var button = $(event.relatedTarget);
        var modal = $(this);
        var query = button.data("query");
        var tableTitle = button.data("title");
        var table = $("#table2").DataTable();
        modal.find("#tableTitle").text(tableTitle);
        table.ajax.url("/api/personnels/" + query).load(null, false);
    });
}

function AddUpdateCurrentPayrollVariablesLogic() {
    var form = $("#currentPayrollVariablesForm");
    var initialform;

    $(
        "#currentPayrollVariablesForm :input, #currentPayrollVariablesForm textarea, #currentPayrollVariablesForm select"
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

        AddSpinToDailyRate();
        AddSpinForAllowanceAmounts();
        AddSpinForSpecificDeductionAmounts();

        ShowSpinner(modal);

        initialform = LoadValuesFromApiToModal(personnelId, initialform, form, modal);
        ShowAccordionOnValidationFailure();
    });

    $("#currentPayrollVariablesModal").on("hidden.bs.modal", function (event) {
        $("#currentPayrollVariablesForm").parsley().reset();
        $("#currentPayrollVariablesForm")[0].reset();
    });

}

function AddSpinToDailyRate() {
    $("input[name='DailyRate']").TouchSpin({
        min: 500,
        max: 10000000,
        step: 10,
        decimals: 2,
        boostat: 5,
        prefix: "₦",
        buttondown_class: "btn btn-classic btn-danger",
        buttonup_class: "btn btn-classic btn-primary",
    });
}

function AddButtonClickListerners() {
    $('#addAllowance').on('click', function () {
        AddAllowanceItem('', '', $('.allowance').length + 1);
    });

    $('#addDeduction').on('click', function () {
        AddDeductionItem('', '', $('.deduction').length + 1);
    });

    $('#addArrear').on('click', function () {
        AddNewArrearItem();
    });
}

function InitializeCalendar() {
    calendar = flatpickr("#Edit_DaysWorked", {
        mode: "range",
        dateFormat: "d/m/Y",
        minDate: getFirstDayOfLastMonth(),
        maxDate: getLastDayOfCurrentMonth(),
        mode: "range",
        position: "auto center",
        onValueUpdate: function (selectedDates, dateStr, instance) {
            if (selectedDates.length > 1) {
                $('#Edit_StartDate').val(getStandardShortDate(new Date(selectedDates[0])));
                $('#Edit_EndDate').val(getStandardShortDate(new Date(selectedDates[1])));

                console.log("Calendar Instance " + instance);
                console.log("Calendar Instance Value " + instance[0]);
            }
        }
    });
}

function UpdateArrearsPeriod(arrears) {

    $.each(arrears, function () {
        AddArrearItem();
    });

    if ($('.arrearPeriod').length > 0) {
        arrearsCalendar = flatpickr(".arrearPeriod", {
            mode: "range",
            dateFormat: "d/m/Y",
            maxDate: getLastDayOfLastMonth(),
            mode: "range",
            position: "auto auto"
        });

        if ($('.arrearPeriod').length === 1) {
            $.each(arrears, function (index, value) {
                arrearsCalendar.setDate([arrears[index].startDate, arrears[index].endDate], true, 'Y-m-d');
            });
        }
        else {
            $.each(arrears, function (index, value) {
                arrearsCalendar[index].setDate([arrears[index].startDate, arrears[index].endDate], true, 'Y-m-d');
            });
        }
    }

}


function InitializeDataTables() {
    if ($("#table").length > 0) {
        $("#table")
            .addClass("nowrap")
            .dataTable({
                columnDefs: [{ orderable: false, targets: -1 }],
            });

        $(".right-buttons").append(
            '<div class="float-right mt-2"> <a href="/Accounting/CurrentVariables/ReApply" class="btn btn-purple btn-primary' +
            ' btn-rounded float-right">' +
            '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"' +
            ' stroke-linecap="round" stroke-linejoin="round" class="feather feather-refresh-ccw"> <polyline points="1 4 1 10 7 10">' +
            '</polyline> <polyline points="23 20 23 14 17 14"></polyline>' +
            ' <path d="M20.49 9A9 9 0 0 0 5.64 5.64L1 10m22 4l-4.64 4.36A9 9 0 0 1 3.51 15"></path> </svg>' +
            '  Reapply Variables</a></div>'
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
                ],
            });
    }
}

function LoadValuesFromApiToModal(personnelId, initialform, form, modal) {
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
            ShowOrHideArrearsAccordion(data.arrears.length > 0);
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

function AddAllowanceItem(name, amount, count) {
    var allowanceHtml = '<tr class="allowance"><td class="serial-no"></td><td>' +
        '<input required class="form-control table-input description" type="text"' +
        'data-parsley-required-message="Description is required"' +
        ' name="AllowanceNames" value="' + name + '" data-parsley-trigger-after-failure="input change">' +
        '</td><td> <input required data-parsley-type="number" data-parsley-min="10"' +
        ' class="form-control table-input amount" type="text" data-parsley-errors-container="#allowance_error' + count + '"' +
        ' name="AllowanceAmounts" data-parsley-required-message="Amount is required" value="' + amount
        + '" data-parsley-trigger-after-failure="input change" data-parsley-min-message="Amount cannot be less than 10">' +
        ' <span id="allowance_error' + count + '"></span>' +
        '</td><td><a href="javascript:void(0)" title="Remove item" class="js-delete">' +
        '<i class="text-danger fa fa-trash"></i></a></td></tr>';

    $('#allowancesBody').append(allowanceHtml);
    ReorderAllowances();
    AddSpinForAllowanceAmounts();
    AddDeleteHandler();
    HideAppropriateTable();
}

function AddDeductionItem(name, amount, count) {
    var deductionHtml = '<tr class="deduction"><td class="serial-no"></td><td>' +
        '<input required class="form-control table-input description" type="text"' +
        'data-parsley-required-message="Description is required"' +
        ' name="SpecificDeductionNames" value="' + name + '" data-parsley-trigger-after-failure="input change">' +
        '</td><td> <input required data-parsley-type="number" data-parsley-min="10"' +
        ' class="form-control table-input amount" type="text" data-parsley-errors-container="#deduction_error' + count + '"' +
        ' name="SpecificDeductionAmounts" data-parsley-required-message="Amount is required" value="' + amount
        + '" data-parsley-trigger-after-failure="input change" data-parsley-min-message="Amount cannot be less than 10">' +
        ' <span id="deduction_error' + count + '"></span>' +
        '</td><td><a href="javascript:void(0)" title="Remove item" class="js-delete">' +
        '<i class="text-danger fa fa-trash"></i></a></td></tr>';

    $('#deductionsBody').append(deductionHtml);
    ReorderDeductions();
    AddSpinForSpecificDeductionAmounts();
    AddDeleteHandler();
    HideAppropriateTable();
}

function AddNewArrearItem() {
    var arrearHtml = '<tr class="arrear"><td class="serial-no">1</td><td><div class="cal-icon">' +
        '<input required="" class="form-control table-input arrearPeriod" type="text" data-parsley-validate-arrear ' +
        'data-parsley-required-message="Arrear period is required" data-parsley-no-focus name="ArrearPeriods" '+
        'value="" data-parsley-trigger="input"></div></td><td><a href="javascript:void(0)" title="Remove item"' +
        ' class="js-delete"><i class="text-danger fa fa-trash"></i></a></td></tr>';

    $('#arrearsBody').append(arrearHtml);

    var arrearPeriod = $(".arrear .arrearPeriod").last();

    flatpickr(arrearPeriod, {
        mode: "range",
        dateFormat: "d/m/Y",
        maxDate: getLastDayOfLastMonth(),
        mode: "range",
        position: "auto auto"
    });
    ReorderArrears();
    AddDeleteHandler();
    HideAppropriateTable();
}


function AddArrearItem() {
    var arrearHtml = '<tr class="arrear"><td class="serial-no">1</td><td><div class="cal-icon">' +
        '<input required="" class="form-control table-input arrearPeriod" type="text" data-parsley-validate-arrear ' +
        'data-parsley-required-message="Arrear period is required" data-parsley-no-focus name="ArrearPeriods" ' 
        + 'data-parsley-trigger="input"' +
        'value=""></div></td><td><a href="javascript:void(0)" title="Remove item"' +
        ' class="js-delete"><i class="text-danger fa fa-trash"></i></a></td></tr>';

    $('#arrearsBody').append(arrearHtml);
    ReorderArrears();
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

function ReorderArrears() {
    $('.arrear').each(function (index) {
        var arrear = $(this);
        arrear.children('.serial-no').text(index + 1);
    });
}

function HideAppropriateTable() {
    var noAllowance = $('.allowance').length < 1;
    var noDeduction = $('.deduction').length < 1;
    var noArrear = $('.arrear').length < 1;

    $("#deductionTable").attr("hidden", noDeduction);
    $("#allowanceTable").attr("hidden", noAllowance);
    $("#arrearTable").attr("hidden", noArrear);

    $("#deleteAllAllowances").attr("hidden", noAllowance);
    $("#deleteAllDeductions").attr("hidden", noDeduction);
    $("#deleteAllArrears").attr("hidden", noArrear);

    ShowOrHideAllowancesAccordion(!noAllowance);
    ShowOrHideDeductionsAccordion(!noDeduction);
    ShowOrHideArrearsAccordion(!noArrear);
}

function AddDeleteHandler() {
    if ($(".js-delete").length > 0) {
        $('.js-delete').on('click', function () {
            var button = $(this);
            button.parents('tr').remove();

            ReorderAllowances();
            ReorderDeductions();
            ReorderArrears();
            HideAppropriateTable();
        })
    }

    $('#deleteAllAllowances').on('click', function () {
        $('#allowanceTable').find(' tbody tr').remove();
        HideAppropriateTable();
    });

    $('#deleteAllDeductions').on('click', function () {
        $('#deductionTable').find('tbody tr').remove();
        HideAppropriateTable();
    });


}

function ShowOrHideAllowancesAccordion(show) {
    $("#allowancesAccordion").attr('class', show ? "collapse show" : "collapse");
    $("#allowancesAccordion").attr('aria-expanded', show);
}

function ShowOrHideDeductionsAccordion(show) {
    $("#deductionsAccordion").attr('class', show ? "collapse show" : "collapse");
    $("#deductionsAccordion").attr('aria-expanded', show);
}

function ShowOrHideArrearsAccordion(show) {
    $("#arrearsAccordion").attr('class', show ? "collapse show" : "collapse");
    $("#arrearsAccordion").attr('aria-expanded', show);
}

function ShowAccordionOnValidationFailure() {
    $("#currentPayrollVariablesForm")
        .parsley()
        .on("form:error", function () {
            var noAllowance = $('.allowance').length < 1;
            var noDeduction = $('.deduction').length < 1;
            var noArrear = $('.arrear').length < 1;

            ShowOrHideAllowancesAccordion(!noAllowance);
            ShowOrHideDeductionsAccordion(!noDeduction);
            ShowOrHideArrearsAccordion(!noArrear);
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
    $("#arrearsBody").empty();

    $("#Edit_WorkedWeekend").prop("checked", data.workedWeekend);

    $.each(data.specificDeductions, function (index, value) {
        AddDeductionItem(value.name, value.amount, index + 1);
    });

    $.each(data.allowances, function (index, value) {
        AddAllowanceItem(value.name, value.amount, index + 1);
    });

    UpdateArrearsPeriod(data.arrears);

    HideAppropriateTable();
}

function AddDaysWorkedRange(data) {
    if (data.startDate) {
        calendar.setDate([data.startDate, data.endDate], true, 'Y-m-d');

    }

}

function AddSpinForAllowanceAmounts() {
    $("input[name='AllowanceAmounts']").TouchSpin({
        min: 10,
        max: 10000000,
        step: 10,
        decimals: 2,
        boostat: 5,
        prefix: "₦",
        buttondown_class: "btn btn-classic btn-danger",
        buttonup_class: "btn btn-classic btn-primary",
    });
}
function AddSpinForSpecificDeductionAmounts() {
    $("input[name='SpecificDeductionAmounts']").TouchSpin({
        min: 10,
        max: 10000000,
        step: 10,
        decimals: 2,
        boostat: 5,
        prefix: "₦",
        buttondown_class: "btn btn-classic btn-danger",
        buttonup_class: "btn btn-classic btn-primary",
    });
}