$(document).ready(function () {
    if ($("#table").length > 0) {
        $("#table")
            .addClass("nowrap")
            .dataTable({
                columnDefs: [{ orderable: false, targets: 8 }],
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

        modal.find(".modal-body .row").attr("hidden", true);
        modal.find("#loader").attr("hidden", false);
        $(".spinner-border").attr("hidden", false);
        $("#editBtn").attr("disabled", true);

        $.ajax({
            url: "/api/personnels/" + personnelId + "/currentpayroll",
            dataType: "json",
            type: "GET",
            success: function (data) {
                $("#Edit_Name").val(data.personnelName);
                $("#Edit_Name").attr("title", data.personnelName);
                $("#Edit_DailyRate").val(data.dailyRate);

                $("#Edit_Vessel").val(data.vessel);
                $("#Edit_Vessel").trigger("change");

                $("#Edit_DaysWorked").val(data.daysWorked);

                initialform = $(form).serialize();
                modal.find(".modal-body .row").attr("hidden", false);
                $("#loader").attr("hidden", true);
                $(".spinner-border").attr("hidden", true);
            },
            error: function () {
                alert("Error occurred...");
            },
        });
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
    $("input[name='DaysWorked']").TouchSpin({
        min: 1,
        max: 30,
        step: 1,
        decimals: 0,
        boostat: 5,
        postfix: "days",
        buttondown_class: "btn btn-classic btn-danger",
        buttonup_class: "btn btn-classic btn-primary",
    });
});
