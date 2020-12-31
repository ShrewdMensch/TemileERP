$(document).ready(function () {
    InitializeVesselDataTable();

    //Vessel Edit Information Logic
    //Vessel Edit Form and API pull logic
    AddVesselEditLogic();

    //Create Vessel Form Logic
    AddVesselCreateLogic();

    //Delete Vessel Form Logic
    AddVesselDeleteLogic();
});

function AddVesselDeleteLogic() {
    $("#vesselDeleteForm").on("shown.bs.modal", function (event) {
        var button = $(event.relatedTarget);
        var vesselId = button.data("id");
        var vesselName = button.data("name");

        var modal = $(this);
        $("#deleteVesselId").val(vesselId);
        modal.find("#vesselName").text(vesselName);
    });
}

function AddVesselCreateLogic() {
    $("#vesselCreateForm").on("hidden.bs.modal", function (event) {
        $("#vesselCreateForm").parsley().reset();
        $("#vesselCreateForm")[0].reset();
    });
}

function AddVesselEditLogic() {
    var form = $("#vesselEditForm");
    var initialform;

    $(
        "#vesselEditForm :input,#vesselEditForm select #vesselEditForm textarea"
    ).on("change", function () {
        $("#editBtn").attr("disabled", initialform === $(form).serialize());
    });

    initialform = AddVesselEditModalOpenEvent(initialform, form);
}

function AddVesselEditModalOpenEvent(initialform, form) {
    $("#vesselEditModal").on("shown.bs.modal", function (event) {
        var button = $(event.relatedTarget);
        var vesselId = button.data("id");

        var modal = $(this);
        $("#vesselId").val(vesselId);

        modal.find(".modal-body .row").attr("hidden", true);
        modal.find("#loader").attr("hidden", false);
        $(".spinner-border").attr("hidden", false);
        $("#editBtn").attr("disabled", true);

        initialform = LoadValuesFromApiToVesselEditModal(vesselId, initialform, form, modal);
    });
    return initialform;
}

function LoadValuesFromApiToVesselEditModal(vesselId, initialform, form, modal) {
    $.ajax({
        url: "/api/vessels/" + vesselId,
        dataType: "json",
        type: "GET",
        success: function (data) {
            $("#Edit_Name").val(data.name);

            initialform = $(form).serialize();
            modal.find(".modal-body .row").attr("hidden", false);
            $("#loader").attr("hidden", true);
            $(".spinner-border").attr("hidden", true);
        },
        error: function () {
            alert("Error occurred...");
        },
    });
    return initialform;
}

function InitializeVesselDataTable() {
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
                        title: "List of Temile Vessels"
                    },
                    {
                        extend: "copyHtml5",
                        text: '<i class="fa fa-copy mr-2"></i>Copy to clipboard',
                        exportOptions: {
                            columns: ':not(.not-export-col)'
                        },
                        title: "List of Temile Vessels"

                    },
                    {
                        extend: "pdfHtml5",
                        text: '<i class="fa fa-file-o mr-2"></i>Export to PDF',
                        exportOptions: {
                            columns: ':visible :not(.not-export-col)'
                        },
                        title: "List of Temile Vessels"
                    },
                ]
            });

        $(".right-buttons").append(
            '<div class="float-right mt-2"> <a data-toggle="modal" data-target="#vesselCreateModal" class="btn btn-primary btn-rounded float-right"><i class="fa fa-plus m-r-5"></i> Add Vessel</a></div>'
        );
    }
}
