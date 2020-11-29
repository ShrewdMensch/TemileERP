$(document).ready(function () {
  if ($("#table").length > 0) {
    $("#table").dataTable({
      columnDefs: [{ orderable: false, targets: -1 }],
    });

    $(".right-buttons").append(
      '<div class="float-right mt-2"> <a data-toggle="modal" data-target="#vesselCreateModal" class="btn btn-primary btn-rounded float-right"><i class="fa fa-plus m-r-5"></i> Add Vessel</a></div>'
    );
  }

  //Vessel Edit Information Logic
  //Vessel Edit Form and API pull logic
  var form = $("#vesselEditForm");
  var initialform;

  $(
    "#vesselEditForm :input,#vesselEditForm select #vesselEditForm textarea"
  ).on("change", function () {
    $("#editBtn").attr("disabled", initialform === $(form).serialize());
  });

  $("#vesselEditModal").on("shown.bs.modal", function (event) {
    var button = $(event.relatedTarget);
    var vesselId = button.data("id");

    var modal = $(this);
    $("#vesselId").val(vesselId);

    modal.find(".modal-body .row").attr("hidden", true);
    modal.find("#loader").attr("hidden", false);
    $(".spinner-border").attr("hidden", false);
    $("#editBtn").attr("disabled", true);

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
  });

  //Create Vessel Form Logic
  $("#vesselCreateForm").on("hidden.bs.modal", function (event) {
    $("#vesselCreateForm").parsley().reset();
    $("#vesselCreateForm")[0].reset();
  });

  //Delete Vessel Form Logic
  $("#vesselDeleteForm").on("shown.bs.modal", function (event) {
    var button = $(event.relatedTarget);
    var vesselId = button.data("id");
    var vesselName = button.data("name");

    var modal = $(this);
    $("#deleteVesselId").val(vesselId);
    modal.find("#vesselName").text(vesselName);
  });
});
