$(document).ready(function () {
  if ($("#table").length > 0) {
    $("#table")
      .addClass("nowrap")
      .dataTable({
        columnDefs: [{ orderable: false, targets: 0 }],
      });

    // $(".right-buttons").append(
    //   '<div class="float-right mr-2 mt-2"> <a data-toggle="modal" data-target="#userCreateFormModal" class="btn btn-purple btn-primary btn-rounded float-right"><i class="fa fa-pencil m-r-5"></i> Edit Timesheet</a></div>'
    // );

    // $(".dt-buttons").prepend(
    //   '<div class="btn-group mb-2"> <button type="button" class="btn btn-purple btn-primary dropdown-toggle drop-left mr-2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"> View </button><div class="dropdown-menu"> <a id="completedBtn" class="dropdown-item">Completed</a> <a id="oldBtn" class="dropdown-item">Old</a> <a id="upcomingBtn" class="dropdown-item">Upcoming</a> <a id="todayBtn" class="dropdown-item">Today</a><div class="dropdown-divider"></div> <a id="allBtn" class="dropdown-item">All</a></div></div>'
    // );
    $(".custom-filter1").prepend(
      '<select id="period-select" class="mx-2 mb-2" title="Select vessel"><option>Period 1</option><option>Period 2</option><option>Period 3</option></select>'
    );
    $(".custom-filter2").prepend(
      '<select id="vessel-select" class="mx-2 mb-2" title="Select vessel"><option>Vessel 1</option><option>Vessel 2</option><option>Vessel 3</option></select>'
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
    minimumResultsForSearch: Infinity,
    width: "100%",
  });

  $("#personnelCreateModal").on("shown.bs.modal", function (event) {
    $("#Personnel_Nationality").val("Nigeria");
    $("#Personnel_Nationality").trigger("change");
    $("#Personnel_Bank").val("");
    $("#Personnel_Bank").trigger("change");
  });
});
