$(document).ready(function () {
    if ($("#table").length > 0) {
        $("#table")
            .addClass("nowrap")
            .dataTable({
                columnDefs: [{ orderable: false, targets: -1 }],
            });

        $(".custom-filter1").prepend(
            '<select id="period-select" class="mx-2 mb-2" title="Select vessel"><option>Period 1</option><option>Period 2</option><option>Period 3</option></select>'
        );
        $(".custom-filter2").prepend(
            '<select id="vessel-select" class="mx-2 mb-2" title="Select vessel"></select>'
        );
        $(".custom-filter3").append(
            '<button class="btn btn-primary mx-2 mb-2">View All</button>'
        );
        $(".message-area").prepend(
            '<p class="font-weight-bold">Showing: <span class="text-primary" id="appointmentsType">Current</span> <span class="text-primary">Payroll</span></p>'
        );
    }

    $("#period-select").select2({
        placeholder: "Select period...",
        minimumResultsForSearch: Infinity,
        width: "100%",
    });


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

    $("#personnelCreateModal").on("shown.bs.modal", function (event) {
        $("#Personnel_Nationality").val("Nigeria");
        $("#Personnel_Nationality").trigger("change");
        $("#Personnel_Bank").val("");
        $("#Personnel_Bank").trigger("change");
    });

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

    $("#printBtn").click(function () {
        var year = new Date().getFullYear();
        $("#salaryContainer").print({
            title: "Monthly Payroll",
            append: "",
            iframe: false,
            deferred: $.Deferred(),
            doctype: "<!doctype html>",
            prepend:
                '<br><p class="text-primary font-weight-bold">Temile and Sons Limited &copy;' +
                year +
                "</p>",
        });
    });
});

function LoadValuesFromAPI(payrollId, modal) {
    $.ajax({
        url: "/api/payments/" + payrollId,
        dataType: "json",
        type: "GET",
        success: function (data) {

            UpdateValues(data);

            AddAllowances(modal, data);

            AddDeductions(modal, data);

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
    $.each(data.allowances, function (index, value) {
        var valueHtml = "<tr><td><strong>" +
            value.name +
            '</strong><span class="float-right">' +
            value.amountInCurrency +
            "</span></td></tr>";
        modal.find("#allowances").append(valueHtml);
    });
}

function UpdateValues(data) {
    $("#paySlipNumber").text(data.paymentSlipNumber);
    $("#personnelName").text(data.personnelName);
    $("#personnelId").text(data.personnelId);
    $("#personnelDesignation").text(
        data.personnelDesignation ? data.personnelDesignation : ""
    );
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
