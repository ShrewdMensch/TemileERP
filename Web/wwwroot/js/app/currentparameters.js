$(document).ready(function () {
  if ($("#table").length > 0) {
    $("#table").addClass("nowrap").dataTable({
      // columnDefs: [{ orderable: false, targets: 0 }],
    });
  }
});
