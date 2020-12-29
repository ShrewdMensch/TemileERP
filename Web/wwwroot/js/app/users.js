$(document).ready(function () {
    InitializeAppUserDataTable();

    //AppUser Edit Information Logic
    //AppUser Edit Form and API pull logic
    AddAppUserEditLogic();

    //Create AppUser Form Logic
    AddAppUserCreateLogic();

    //Delete AppUser Form Logic
    AddAppUserDeleteLogic();

    var clipboard = new Clipboard('.btn-copy');

});

function AddAppUserDeleteLogic() {
    $("#vesselDeleteForm").on("shown.bs.modal", function (event) {
        var button = $(event.relatedTarget);
        var vesselId = button.data("id");
        var vesselName = button.data("name");

        var modal = $(this);
        $("#deleteAppUserId").val(vesselId);
        modal.find("#vesselName").text(vesselName);
    });
}

function AddAppUserCreateLogic() {
    $("#userCreateForm").on("hidden.bs.modal", function (event) {
        $("#userCreateForm").parsley().reset();
        $("#userCreateForm")[0].reset();
    });
}

function AddAppUserEditLogic() {
    var form = $("#userEditForm");
    var initialform;

    $(
        "#userEditForm :input,#userEditForm select #userEditForm textarea"
    ).on("change", function () {
        $("#editBtn").attr("disabled", initialform === $(form).serialize());
    });

    initialform = AddAppUserEditModalOpenEvent(initialform, form);
}

function AddAppUserEditModalOpenEvent(initialform, form) {
    $("#userEditModal").on("shown.bs.modal", function (event) {
        var button = $(event.relatedTarget);
        var vesselId = button.data("id");

        var modal = $(this);
        $("#userId").val(vesselId);

        modal.find(".modal-body .row").attr("hidden", true);
        modal.find("#loader").attr("hidden", false);
        $(".spinner-border").attr("hidden", false);
        $("#editBtn").attr("disabled", true);

        initialform = LoadValuesFromApiToAppUserEditModal(vesselId, initialform, form, modal);
    });
    return initialform;
}

function LoadValuesFromApiToAppUserEditModal(vesselId, initialform, form, modal) {
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

function InitializeAppUserDataTable() {
    if ($("#table").length > 0) {
        $("#table").addClass('nowrap').dataTable(
            {
                responsive: true,
                columnDefs: [
                    { orderable: false, targets: 4 },
                    { orderable: false, targets: -1 }
                ],
                buttons: [
                    {
                        extend: "excelHtml5",
                        text: '<i class="fa fa-file-o mr-2"></i>Export to excel',
                        exportOptions: {
                            columns: ':not(.not-export-col)'
                        },
                        title: "List of Temile AppUsers"
                    },
                    {
                        extend: "copyHtml5",
                        text: '<i class="fa fa-copy mr-2"></i>Copy to clipboard',
                        exportOptions: {
                            columns: ':not(.not-export-col)'
                        },
                        title: "List of Temile AppUsers"

                    },
                ]
            });

        $(".right-buttons").append(
            '<div class="float-right mt-2"> <a data-toggle="modal" data-target="#userCreateModal" class="btn btn-primary btn-rounded float-right"><i class="fa fa-plus m-r-5"></i> Add AppUser</a></div>'
        );
    }
}
