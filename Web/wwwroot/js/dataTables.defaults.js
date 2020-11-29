$.extend($.fn.dataTable.defaults, {
  responsive: true,
  stateSave: true,
  processing: true,
  iDisplayLength: 5,
  aLengthMenu: [
    [5, 10, 15, 20, 25, 30, -1],
    [5, 10, 15, 20, 25, 30, "All"],
  ],
  language: {
    emptyTable:
      '<p class="text-center"><strong>No record found...</strong></p>',
    paginate: {
      previous: '<i class="fa fa-angle-double-left"></i>',
      next: '<i class="fa fa-angle-double-right"></i>',
    },
    sLengthMenu: "Show :  _MENU_ records",
    processing:
      '<div class="d-flex justify-content-center"><div class="spinner-border" role="status"> <span class="sr-only">Loading...</span></div></div>',
  },
  dom:
    '<"row d-flex mb-4"<"col-12"<"message-area float-right">>> <"row d-flex mb-4"<"col-sm-12"B<"right-buttons align-content-end">>>' +
    '<"row d-flex" <"col-sm-3"l><"col-sm-2"<"custom-filter1 mb-2">> <"col-sm-2"<"custom-filter2 mb-2">> <"col-sm-2"<"custom-filter3 mb-2">> >' +
    '<"row"<"col-sm-12 float-right"f>>rtip',
  buttons: [
    {
      extend: "print",
      text: '<i class="fa fa-print mr-2"></i>Print',
    },
    {
      extend: "excelHtml5",
      text: '<i class="fa fa-file-o mr-2"></i>Export to excel',
    },
    {
      extend: "pdfHtml5",
      text: '<i class="fa fa-file-o mr-2"></i>Export to PDF',
    },
    {
      extend: "copyHtml5",
      text: '<i class="fa fa-copy mr-2"></i>Copy to clipboard',
    },
  ],
});
$.fn.DataTable.ext.pager.numbers_length = 4;
