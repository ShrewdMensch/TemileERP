$(document).ready(function () {
  if ($("#table").length > 0) {
    $("#table").dataTable({
      columnDefs: [{ orderable: false, targets: -1 }],
    });

    $(".right-buttons").append(
      '<div class="float-right mt-2"> <a data-toggle="modal" data-target="#deductionCreateModal" class="btn btn-primary btn-rounded float-right"><i class="fa fa-plus m-r-5"></i> Add Deduction</a></div>'
    );
    $(".right-buttons").append(
      '<div class="float-right mt-2 mr-2"> <a href="/Accounting/Deductions/ApplyDeduction" class="btn btn-primary btn-rounded float-right"><i class="fas fa-sync m-r-5"></i> Reapply Deductions On All Current Payroll</a></div>'
    );
  }

  //Deduction Edit Information Logic
  //Deduction Edit Form and API pull logic
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

  //Create Deduction Form Logic
  $("#deductionCreateForm").on("shown.bs.modal", function (event) {
    $("#Percentage").val(1);
  });
  $("#deductionCreateForm").on("hidden.bs.modal", function (event) {
    $("#deductionCreateForm").parsley().reset();
    $("#deductionCreateForm")[0].reset();
  });

  //Delete Deduction Form Logic
  $("#deductionDeleteForm").on("shown.bs.modal", function (event) {
    var button = $(event.relatedTarget);
    var deductionId = button.data("id");
    var deductionName = button.data("name");

    var modal = $(this);
    $("#deleteDeductionId").val(deductionId);
    modal.find("#deductionName").text(deductionName);
  });

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
});
