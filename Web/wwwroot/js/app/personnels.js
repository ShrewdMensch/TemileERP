$(document).ready(function () {
  if ($("#table").length > 0) {
    $("#table")
      .addClass("nowrap")
      .dataTable({
        columnDefs: [{ orderable: false, targets: 0 }],
      });

    $(".right-buttons").append(
      '<div class="float-right mt-2"> <a data-toggle="modal" data-target="#personnelCreateModal" class="btn btn-purple btn-primary btn-rounded float-right"><i class="fa fa-plus m-r-5"></i> Add Personnel</a></div>'
    );
  }

  $("#Personnel_Nationality, #Edit_Nationality").select2({
    data: countries,
    placeholder: "Select nationality...",
    dropdownParent: $("#personnelCreateModal"),
    width: "100%",
  });

  $("#Personnel_Bank, #Edit_Bank").select2({
    data: banks,
    placeholder: "Select bank...",
    dropdownParent: $("#personnelCreateModal"),
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

  //Parsely Validator triggers for Select2
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

  //Personnel Create Logic
  $("#personnelCreateModal").on("shown.bs.modal", function (event) {
    $("#Personnel_Nationality").val("Nigeria");
    $("#Personnel_Nationality").trigger("change");
    $("#Personnel_Bank").val("");
    $("#Personnel_Bank").trigger("change");
  });

  $("#personnelCreateModal").on("hidden.bs.modal", function (event) {
    $("#personnelCreateForm").parsley().reset();
    $("#personnelCreateForm")[0].reset();
  });

  //Personnel Edit Information Logic
  //Personnel Edit Form and API pull logic
  var form = $("#personnelEditForm");
  var initialform = $(form).serialize();

  $("#personnelEditForm :input, #personnelEditForm textarea").on(
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

    $.ajax({
      url: "/api/personnels/" + id,
      dataType: "json",
      type: "GET",
      success: function (data) {
        $("#Edit_Surname").val(data.lastName);
        $("#Edit_FirstName").val(data.firstName);
        $("#Edit_OtherName").val(data.otherName);
        $("#Edit_Sex").val(data.sex);
        $("#Edit_Religion").val(data.religion);
        $("#Edit_Email").val(data.email);
        $("#Edit_DailyRate").val(data.dailyRate);
        $("#Edit_AccountName").val(data.accountName);
        $("#Edit_AccountNumber").val(data.accountNumber);
        $("#Edit_BVN").val(data.bvn);
        $("#Edit_PhoneNo").val(data.phoneNumber);
        $("#Edit_NextOfKin").val(data.nextOfKin);
        $("#Edit_NextOfKinPhoneNo").val(data.nextOfKinPhoneNumber);
        $("#Edit_Address").val(data.address);

        if (data.bank) {
          $("#Edit_Bank").val(data.bank);
          $("#Edit_Bank").trigger("change");
        } else {
          $("#Edit_Bank").val("");
          $("#Edit_Bank").trigger("change");
        }

        if (data.nationality) {
          $("#Edit_Nationality").val(data.nationality);
          $("#Edit_Nationality").trigger("change");
        } else {
          $("#Edit_Nationality").val("");
          $("#Edit_Nationality").trigger("change");
        }

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
});
