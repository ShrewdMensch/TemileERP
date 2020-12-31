$(document).ready(function () {
    InitializeDataTable();

    //Deduction Edit Information Logic
    //Deduction Edit Form and API pull logic
    AddDeductionEditLogic();

    //Create Deduction Form Logic
    AddDeductionCreateLogic();

    //Delete Deduction Form Logic
    AddDeductionDeleteLogic();

    AddSpinToDeductionPercentage();
});

function AddSpinToDeductionPercentage() {
    $("input[name='Percentage']").TouchSpin({
        min: 1,
        max: 100,
        step: 0.1,
        decimals: 2,
        boostat: 5,
        maxboostedstep: 10,
        initval: 0,
        postfix: "%",
        buttondown_class: "btn btn-classic btn-danger",
        buttonup_class: "btn btn-classic btn-primary",
    });
}

function AddDeductionDeleteLogic() {
    $("#deductionDeleteForm").on("shown.bs.modal", function (event) {
        var button = $(event.relatedTarget);
        var deductionId = button.data("id");
        var deductionName = button.data("name");

        var modal = $(this);
        $("#deleteDeductionId").val(deductionId);
        modal.find("#deductionName").text(deductionName);
    });
}

function AddDeductionCreateLogic() {
    $("#deductionCreateForm").on("shown.bs.modal", function (event) {
        $("#Percentage").val(1);
    });
    $("#deductionCreateForm").on("hidden.bs.modal", function (event) {
        $("#deductionCreateForm").parsley().reset();
        $("#deductionCreateForm")[0].reset();
    });
}

function AddDeductionEditLogic() {
    var form = $("#deductionEditForm");
    var initialform;

    $(
        "#deductionEditForm :input,#deductionEditForm select #deductionEditForm textarea"
    ).on("change", function () {
        $("#editBtn").attr("disabled", initialform === $(form).serialize());
    });

    $("#deductionEditModal").on("shown.bs.modal", function (event) {
        var button = $(event.relatedTarget);
        var deductionId = button.data("id");

        var modal = $(this);
        $("#deductionId").val(deductionId);

        modal.find(".modal-body .row").attr("hidden", true);
        modal.find("#loader").attr("hidden", false);
        $(".spinner-border").attr("hidden", false);
        $("#editBtn").attr("disabled", true);

        $.ajax({
            url: "/api/deductions/" + deductionId,
            dataType: "json",
            type: "GET",
            success: function (data) {
                $("#Edit_Name").val(data.name);
                $("#Edit_Percentage").val(data.percentage);

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
}

function InitializeDataTable() {
    if ($("#table").length > 0) {
        $("#table").addClass('nowrap').dataTable(
            {
                responsive: true,
                columnDefs: [{ orderable: false, targets: -1 }],
                buttons: [
                    {
                        extend: "excelHtml5",
                        text: '<i class="fa fa-file-o mr-2"></i>Export to excel',
                        exportOptions: {
                            columns: ':not(.not-export-col)'
                        },
                        title: "List of Temile General Deductions"
                    },
                    {
                        extend: "copyHtml5",
                        text: '<i class="fa fa-copy mr-2"></i>Copy to clipboard',
                        exportOptions: {
                            columns: ':not(.not-export-col)'
                        },
                        title: "List of Temile General Deductions"

                    },
                    {
                        extend: "pdfHtml5",
                        text: '<i class="fa fa-file-o mr-2"></i>Export to PDF',
                        exportOptions: {
                            columns: ':visible :not(.not-export-col)'
                        },
                        title: "List of Temile General Deductions"
                    },
                ]
            });

        $(".right-buttons").append(
            '<div class="float-right mt-2"> <a data-toggle="modal" data-target="#deductionCreateModal" class="btn btn-primary btn-rounded float-right"><i class="fa fa-plus m-r-5"></i> Add Deduction</a></div>'
        );
        $(".right-buttons").append(
            '<div class="float-right mt-2 mr-2"> <a href="/Accounting/Deductions/ReApply" class="btn btn-primary btn-rounded float-right">' +
            '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"' +
            ' stroke-linecap="round" stroke-linejoin="round" class="feather feather-refresh-ccw"> <polyline points="1 4 1 10 7 10">' +
            '</polyline> <polyline points="23 20 23 14 17 14"></polyline>' +
            ' <path d="M20.49 9A9 9 0 0 0 5.64 5.64L1 10m22 4l-4.64 4.36A9 9 0 0 1 3.51 15"></path> </svg>' +
            ' Reapply Deductions</a></div>'
        );
    }
}
