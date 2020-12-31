var joinedCalendar;
$(document).ready(function () {
    InitializePersonnelDataTables();

    InitializeSelect2Components();

    //Parsely Validator triggers for Select2
    Select2ParselyValidatorTriggers();

    //Personnel Create Logic
    AddPersonnelCreateLogic();

    //Personnel Edit Information Logic
    //Personnel Edit Form and API pull logic
    AddPersonnelEditLogic();

    //Personnel Details Logic
    AddPersonnelDetailsLogic();


    AddSpinToDailyRate();

    InitializeDateJoinedCalendarPicker();

});

function InitializeDateJoinedCalendarPicker() {
    flatpickr("#Personnel_DateJoined", {
        dateFormat: "d/m/Y",
        maxDate: 'today',
        position: "auto center"
    });

    joinedCalendar = flatpickr("#Edit_DateJoined", {
        dateFormat: "d/m/Y",
        maxDate: 'today',
        position: "auto center"
    });
}

function AddPersonnelDetailsLogic() {
    $("#personnelDetailsModal").on("shown.bs.modal", function (event) {
        var button = $(event.relatedTarget);
        var id = button.data("id");
        var modal = $(this);
        modal.find(".modal-body .row").attr("hidden", true);
        modal.find("#loader").attr("hidden", false);
        $(".spinner-border").attr("hidden", false);

        LoadValuesFromApiToPersonnelDetailsModal(id, modal);
    });

    $("#personnelDetailsModal").on("hidden.bs.modal", function (event) {
        $("#profile-data").find("#status").html("");
    });
}

function AddPersonnelEditLogic() {
    var form = $("#personnelEditForm");
    var initialform = $(form).serialize();

    $("#personnelEditForm :input, #personnelEditForm textarea, #personnelEditForm select").on(
        "change",
        function () {
            $("#editBtn").attr("disabled", initialform === $(form).serialize());
        }
    );

    $("#personnelEditModal").on("shown.bs.modal", function (event) {
        var button = $(event.relatedTarget);
        var id = button.data("id");
        var modal = $(this);
        $("#Personnel_Id").val(id);

        modal.find(".modal-body .row").attr("hidden", true);
        modal.find("#loader").attr("hidden", false);
        $(".spinner-border").attr("hidden", false);
        $("#editBtn").attr("disabled", true);

        initialform = LoadValuesFromApiToPersonnelEditForm(id, initialform, form, modal);
    });

    $("#personnelEditModal").on("hidden.bs.modal", function (event) {
        $("#personnelEditForm").parsley().reset();
        $("#personnelEditForm")[0].reset();
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

function LoadValuesFromApiToPersonnelDetailsModal(id, modal) {
    $.ajax({
        url: "/api/personnels/" + id,
        dataType: "json",
        type: "GET",
        success: function (data) {
            var profile = $("#profile-data");
            profile.find("#id").text(data.id);
            $("#name").text(data.name);
            profile.find("#fullName").text(data.fullName);
            profile.find("#dateJoined").text(data.dateJoined);
            profile.find("#dailyRate").text(data.dailyRateStr);
            profile.find("#religion").text(data.religion);
            profile.find("#bank").text(data.bank);
            profile.find("#accountName").text(data.accountName);
            profile.find("#accountNumber").text(data.accountNumber);
            profile.find("#kinNumber").text(data.nextOfKinPhoneNumber);
            profile.find("#kin").text(data.nextOfKin);
            profile
                .find("#occupation")
                .text(data.designation ? data.designation : "Temile Personnel");
            profile
                .find("#vessel")
                .text(data.vessel ? data.vessel : "N/A");

            profile.find("#photo")
                .attr("src", data.photo ? data.photo : "/assets/img/user.jpg");

            profile.find("#phone").text(data.phoneNumber ? data.phoneNumber : "N/A");
            profile.find("#address").text(data.address ? data.address : "N/A");
            profile.find("#sex").text(data.sex ? data.sex : "N/A");

            profile
                .find("#nationality")
                .text(data.nationality ? data.nationality : "N/A");

            profile
                .find("#messageBtn")
                .html(
                    '<a href="mailto:' +
                    data.email +
                    '"' +
                    'class="btn btn-primary">Send Mail</a>'
                );

            if (data.isActive) {
                profile
                    .find("#status")
                    .html(
                        '<span class="badge badge-success">Active <i class="fa fa-check" aria-hidden="true"></i></span>'
                    );
            } else {
                profile
                    .find("#status")
                    .html(
                        '<span class="badge badge-danger">Inactive <i class="fa fa-times"aria-hidden="true"></i></span>'
                    );
            }

            modal.find(".modal-body .row").attr("hidden", false);
            $("#loader").attr("hidden", true);
            $(".spinner-border").attr("hidden", true);
        },
        error: function () {
            alert("Error occurred...");
        },
    });
}

function LoadValuesFromApiToPersonnelEditForm(id, initialform, form, modal) {
    $.ajax({
        url: "/api/personnels/" + id,
        dataType: "json",
        type: "GET",
        success: function (data) {
            console.log(data);
            $("#Edit_Surname").val(data.lastName);
            $("#Edit_FirstName").val(data.firstName);
            $("#Edit_OtherName").val(data.otherName);
            $("#Edit_Email").val(data.email);
            $("#Edit_DailyRate").val(data.dailyRate);
            $("#Edit_AccountName").val(data.accountName);
            $("#Edit_AccountNumber").val(data.accountNumber);
            $("#Edit_PhoneNo").val(data.phoneNumber);
            $("#Edit_NextOfKin").val(data.nextOfKin);
            $("#Edit_NextOfKinPhoneNo").val(data.nextOfKinPhoneNumber);
            $("#Edit_Address").val(data.address);

            joinedCalendar.setDate(data.dateJoined, true, 'M. d, Y');

            $("#Edit_Bank").val(data.bank);
            $("#Edit_Bank").trigger("change");

            $("#Edit_Religion").val(data.religion);
            $("#Edit_Religion").trigger("change");

            $("#Edit_Sex").val(data.sex);
            $("#Edit_Sex").trigger("change");

            $("#Edit_Vessel").val(data.vessel);
            $("#Edit_Vessel").trigger("change");

            $("#Edit_Nationality").val(data.nationality);
            $("#Edit_Nationality").trigger("change");

            $("#Edit_Position").val(data.designation);
            $("#Edit_Position").trigger("change");

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

function AddPersonnelCreateLogic() {
    $("#personnelCreateModal").on("shown.bs.modal", function (event) {
        $("#Personnel_Nationality").val("Nigeria");
        $("#Personnel_Nationality").trigger("change");
        $("#Personnel_Bank").val("");
        $("#Personnel_Bank").trigger("change");
        $("#Personnel_Vessel").val("");
        $("#Personnel_Vessel").trigger("change");
        $("#Personnel_Sex").val("");
        $("#Personnel_Sex").trigger("change");
        $("#Personnel_Religion").val("");
        $("#Personnel_Religion").trigger("change");
        $("#Personnel_Position").val("");
        $("#Personnel_Position").trigger("change");
    });

    $("#personnelCreateModal").on("hidden.bs.modal", function (event) {
        $("#personnelCreateForm").parsley().reset();
        $("#personnelCreateForm")[0].reset();
        $("#Personnel_bank_group span.select2-selection__rendered, #Personnel_sex_group span.select2-selection__rendered, #Personnel_religion_group span.select2-selection__rendered, #Personnel_position_group span.select2-selection__rendered, #Personnel_vessel_group span.select2-selection__rendered").removeClass(
            "parsley-error"
        );
        $("#Personnel_bank_group span.select2-selection__rendered, #Personnel_sex_group span.select2-selection__rendered, #Personnel_religion_group span.select2-selection__rendered, #Personnel_position_group span.select2-selection__rendered, #Personnel_vessel_group span.select2-selection__rendered").removeClass(
            "parsley-success"
        );
    });
}

function InitializeSelect2Components() {
    $("#Personnel_Religion").select2({
        minimumResultsForSearch: Infinity,
        width: "100%",
        dropdownParent: $("#personnelCreateModal"),
        placeholder: "Select religion...",
    });
    $("#Personnel_Sex").select2({
        minimumResultsForSearch: Infinity,
        dropdownParent: $("#personnelCreateModal"),
        width: "100%",
        placeholder: "Select sex...",
    });

    $("#Personnel_Nationality").select2({
        data: countries,
        placeholder: "Select nationality...",
        dropdownParent: $("#personnelCreateModal"),
        width: "100%",
    });

    $("#Personnel_Bank").select2({
        data: banks,
        placeholder: "Select bank...",
        dropdownParent: $("#personnelCreateModal"),
        width: "100%",
    });
    $("#Personnel_Position").select2({
        data: positions,
        minimumResultsForSearch: Infinity,
        placeholder: "Select position...",
        dropdownParent: $("#personnelCreateModal"),
        width: "100%",
    });

    $("#Personnel_Vessel").select2({
        placeholder: "Select vessel...",
        dropdownParent: $("#personnelCreateModal"),
        width: "100%",
        minimumResultsForSearch: 20,
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

    $("#Edit_Religion").select2({
        minimumResultsForSearch: Infinity,
        width: "100%",
        dropdownParent: $("#personnelEditModal"),
    });
    $("#Edit_Sex").select2({
        minimumResultsForSearch: Infinity,
        dropdownParent: $("#personnelEditModal"),
        width: "100%",
    });
    $("#Edit_Nationality").select2({
        data: countries,
        placeholder: "Select nationality...",
        dropdownParent: $("#personnelEditModal"),
        width: "100%",
    });

    $("#Edit_Bank").select2({
        data: banks,
        placeholder: "Select bank...",
        dropdownParent: $("#personnelEditModal"),
        width: "100%",
    });

    $("#Edit_Position").select2({
        data: positions,
        minimumResultsForSearch: Infinity,
        placeholder: "Select position...",
        dropdownParent: $("#personnelEditModal"),
        width: "100%",
    });

    $.ajax({
        url: "/api/vessels/ForSelect2",
        dataType: "json",
        type: "GET",
        success: function (data) {
            $("#Edit_Vessel").select2({
                data: data,
                placeholder: "Select vessel...",
                dropdownParent: $("#personnelEditModal"),
                width: "100%",
                minimumResultsForSearch: 20,

            });
        },
        error: function () {
            alert("Error occurred...");
        },
    });
}

function InitializePersonnelDataTables() {
    if ($("#table").length > 0) {
        $("#table").addClass('nowrap').DataTable(
            {
                responsive: true,
                columnDefs: [
                    { orderable: false, targets: [-2, -1] },
                    {
                        visible: false,
                        targets: [0, 2, 5, 6, 10, 11, 13, 14, 15, 16, 17, 19]
                    }
                ],
                buttons: [
                    {
                        extend: "excelHtml5",
                        text: '<i class="fa fa-file-o mr-2"></i>Export to excel',
                        exportOptions: {
                            columns: ':not(.not-export-col)'
                        },
                        title: "List of Temile Personnels"
                    },
                    {
                        extend: "copyHtml5",
                        text: '<i class="fa fa-copy mr-2"></i>Copy to clipboard',
                        exportOptions: {
                            columns: ':not(.not-export-col)'
                        },
                        title: "List of Temile Personnels"

                    },
                    {
                        extend: "pdfHtml5",
                        text: '<i class="fa fa-file-o mr-2"></i>Export to PDF',
                        exportOptions: {
                            columns: ':visible :not(.not-export-col)'
                        },
                        title: "List of Temile Personnels"
                    },
                ]
            });

        $(".right-buttons").append(
            '<div class="float-right mt-2"> <a data-toggle="modal" data-target="#personnelCreateModal" class="btn btn-purple btn-primary btn-rounded float-right"><i class="fa fa-plus m-r-5"></i> Add Personnel</a></div>'
        );
    }
}

function Select2ParselyValidatorTriggers() {
    $("#Edit_Bank")
        .parsley()
        .on("field:success", function () {
            $("#bank_group span.select2-selection__rendered").removeClass(
                "parsley-error"
            );
            $("#bank_group span.select2-selection__rendered").addClass(
                "parsley-success"
            );
        });
    $("#Edit_Bank")
        .parsley()
        .on("field:error", function () {
            $("#bank_group span.select2-selection__rendered").removeClass(
                "parsley-success"
            );
            $("#bank_group span.select2-selection__rendered").addClass(
                "parsley-error"
            );
        });
    $("#Personnel_Bank")
        .parsley()
        .on("field:success", function () {
            $("#Personnel_bank_group span.select2-selection__rendered").removeClass(
                "parsley-error"
            );
            $("#Personnel_bank_group span.select2-selection__rendered").addClass(
                "parsley-success"
            );
        });
    $("#Personnel_Bank")
        .parsley()
        .on("field:error", function () {
            $("#Personnel_bank_group span.select2-selection__rendered").removeClass(
                "parsley-success"
            );
            $("#Personnel_bank_group span.select2-selection__rendered").addClass(
                "parsley-error"
            );
        });
    $("#Personnel_Vessel")
        .parsley()
        .on("field:success", function () {
            $("#Personnel_vessel_group span.select2-selection__rendered").removeClass(
                "parsley-error"
            );
            $("#Personnel_vessel_group span.select2-selection__rendered").addClass(
                "parsley-success"
            );
        });
    $("#Personnel_Vessel")
        .parsley()
        .on("field:error", function () {
            $("#Personnel_vessel_group span.select2-selection__rendered").removeClass(
                "parsley-success"
            );
            $("#Personnel_vessel_group span.select2-selection__rendered").addClass(
                "parsley-error"
            );
        });
    $("#Personnel_Sex")
        .parsley()
        .on("field:success", function () {
            $("#Personnel_sex_group span.select2-selection__rendered").removeClass(
                "parsley-error"
            );
            $("#Personnel_sex_group span.select2-selection__rendered").addClass(
                "parsley-success"
            );
        });
    $("#Personnel_Sex")
        .parsley()
        .on("field:error", function () {
            $("#Personnel_sex_group span.select2-selection__rendered").removeClass(
                "parsley-success"
            );
            $("#Personnel_sex_group span.select2-selection__rendered").addClass(
                "parsley-error"
            );
        });
    $("#Personnel_Religion")
        .parsley()
        .on("field:success", function () {
            $(
                "#Personnel_religion_group span.select2-selection__rendered"
            ).removeClass("parsley-error");
            $("#Personnel_religion_group span.select2-selection__rendered").addClass(
                "parsley-success"
            );
        });
    $("#Personnel_Religion")
        .parsley()
        .on("field:error", function () {
            $(
                "#Personnel_religion_group span.select2-selection__rendered"
            ).removeClass("parsley-success");
            $("#Personnel_religion_group span.select2-selection__rendered").addClass(
                "parsley-error"
            );
        });

    $("#Personnel_Position")
        .parsley()
        .on("field:success", function () {
            $(
                "#Personnel_position_group span.select2-selection__rendered"
            ).removeClass("parsley-error");
            $("#Personnel_position_group span.select2-selection__rendered").addClass(
                "parsley-success"
            );
        });
    $("#Personnel_Position")
        .parsley()
        .on("field:error", function () {
            $(
                "#Personnel_position_group span.select2-selection__rendered"
            ).removeClass("parsley-success");
            $("#Personnel_position_group span.select2-selection__rendered").addClass(
                "parsley-error"
            );
        });
}
