var title, subject, fileName, author = "E.A Temile and Sons";

$(document).ready(function () {
    InitializeDataTable();

    AddBankInstructionsPrintModalLogic();

    AddSendMailModalLogic();

    AddPrintButtonClickEvent();

    AddClickEvents();

    AddDeleteHandler();

    AddCcInputEvents();
});


function AddCcInputEvents() {
    $("#ccInput")
        .parsley()
        .on("field:error", function () {
            $('#addCcBtn').attr('disabled', true);
        });

    $("#ccInput")
        .parsley()
        .on("field:success", function () {
            $('#addCcBtn').attr('disabled', false);
        });

    $("#ccInput")
        .keyup(function () {
            var input = $(this);
            if (input.val() == '')
                $('#addCcBtn').attr('disabled', true);
        });
}

function AddCc() {
    if ($('#ccInput').val() == '') return;

    var ccHtml = '<tr class="ccRow"><td class="serial-no">1</td><td> <input class="form-control table-input ' +
        'ccRecipient" readonly type="text" name="CcRecipients" value="' + $('#ccInput').val() + '">' +
        '</td><td><a href="javascript:void(0)" title="Remove item" class="js-delete">' +
        '<i class="text-danger fa fa-trash"></i></a></td></tr>';

    $('#ccTableBody').append(ccHtml);

    ReorderCcRows();
    AddDeleteHandler();
    HideAppropriateTable();
    $('#ccInput').val('');
}


function AddDeleteHandler() {
    if ($(".js-delete").length > 0) {
        $('.js-delete').on('click', function () {
            var button = $(this);
            button.parents('tr').remove();
            ReorderCcRows();
            HideAppropriateTable();
        })
    }
}

function ReorderCcRows() {
    $('.ccRow').each(function (index) {
        var ccRow = $(this);
        ccRow.children('.serial-no').text(index + 1);
    });
}

function HideAppropriateTable() {
    var noCc = $('.ccRow').length < 1;
    $("#ccTable").attr("hidden", noCc);
    $("#ccTableDiv").attr("hidden", noCc);
}

function GetCreatedPdfFromTable() {
    var todayDate = new Date().toDateString();
    $('#todayDate').text(todayDate);
    var val = htmlToPdfmake($('#forPDF').html(), {
        tableAutoSize: true
    });

    var dd = {
        content: val, pageSize: 'A4', pageOrientation: 'portrait',
        styles: {
            'payslip-title': {
                alignment: 'center',
                decoration: 'underline',
                color: '#555'
            },
            'date-left': {
                alignment: 'right',
                bold: true,
                color: '#555'
            },
            'netPay': {
                bold: true
            }
        }
    };

    return dd;
}

function GetWorkBook() {
    /* create new workbook */
    var workbook = XLSX.utils.book_new();

    /* convert table 'table1' to worksheet named "Sheet1" */
    var ws1 = XLSX.utils.table_to_sheet(document.getElementById('table'));
    XLSX.utils.book_append_sheet(workbook, ws1, "Sheet1", { raw: true });

    /* convert table 'table2' to worksheet named "Sheet2" */
    /*var ws2 = XLSX.utils.table_to_sheet(document.getElementById('table2'));
    XLSX.utils.book_append_sheet(workbook, ws2, "Sheet2");*/

    /* workbook now has 2 worksheets */

    var wscols = [
        { wch: 20 },
        { wch: 20 },
        { wch: 20 },
        { wch: 20 },
        { wch: 20 }
    ];

    ws1['!cols'] = wscols;

    if (!workbook.Props) workbook.Props = {};
    workbook.Props.Title = title;
    workbook.Props.Author = author;
    workbook.Props.Subject = subject;

    return workbook;
}

function AddClickEvents() {
    $("#exportToPdfBtn").click(function () {

        var dd = GetCreatedPdfFromTable();

        var todayDate = new Date().toDateString();

        pdfMake.createPdf(dd).download(todayDate.replace(/ /g, '_') + '_instructionsToBank');
    });


    $("#exportToExcelBtn").click(function () {
        var workbook = GetWorkBook();
        XLSX.writeFile(workbook, fileName + ".xlsx", { raw: true, type: "base64", bookType: "xlsx" });
    });

    $('#addCc').click(function () {
        var chkBox = $('#addCc');
        $('#ccDetails').attr('hidden', !chkBox.prop("checked"));
        $('#ccTable').attr('hidden', !chkBox.prop("checked"));

    });

    $('#addCcBtn').click(function () {
        AddCc();
        HideAppropriateTable();
    });
}

function AddSendMailModalLogic() {
    $("#sendMailModal").on("shown.bs.modal", function (event) {

        $('#discardBtn').attr("disabled", false);

        var dd = GetCreatedPdfFromTable();

        pdfMake.createPdf(dd).getBase64(function (data) {
            $("#PdfContent").val(data);
        });

        var workbook = GetWorkBook();

        var XlsxToBase64 = XLSX.write(workbook, { raw: true, type: "base64", bookType: "xlsx" });
        $("#ExcelContent").val(XlsxToBase64);

    });

    $("#sendMailModal").on("hidden.bs.modal", function (event) {
        $("#sendMailForm").parsley().reset();
        $("#sendMailForm")[0].reset();
    });

    $("#sendMailForm")
        .on("submit", function () {
            $("#discardBtn").attr('disabled', true);
            $("#sendMailBtn").attr('disabled', true);

            $('#VesselName').val($('#sendBtn').data("vessel"));
            $('#BankInstructionId').val($('#sendBtn').data("id"));
        });
}

function AddPrintButtonClickEvent() {
    $("#printBtn").click(function () {
        var year = new Date().getFullYear();
        $("#bankInstructionContainer").print({
            title: "Instructions to Bank",
            append: "",
            iframe: false,
            deferred: $.Deferred(),
            doctype: "<!doctype html>",
            prepend: '<br><p class="text-primary font-weight-bold">Temile and Sons Limited &copy;' +
                year +
                "</p>",
        });
    });
}

function AddBankInstructionsPrintModalLogic() {
    $("#bankInstructionsPrintModal").on("shown.bs.modal", function (event) {
        var button = $(event.relatedTarget);
        var vessel = button.data("vessel");
        var id = button.data("id");
        var modal = $(this);

        modal.find('#sendBtn').attr('data-vessel', vessel);
        modal.find('#sendBtn').attr('data-id', id);

        modal.find(".modal-body .row").attr("hidden", true);
        modal.find("#loader").attr("hidden", false);
        $(".spinner-border").attr("hidden", false);

        $.ajax({
            url: "/api/bankinstructions/" + vessel,
            dataType: "json",
            type: "GET",
            success: function (data) {

                $("#vessel").text(data.vessel);
                $("#dateTitle").text(data.date);
                $("#vessel2").text(data.vessel);
                $("#dateTitle2").text(data.date);

                title = "Instructions to Bank for " + data.vessel + " vessel";
                subject = "For " + data.date;
                fileName = "Instructions_To_Bank_For_" + data.vessel + "_Vessel_" + data.date.replace(/,/g, '_');

                modal.find("#instructionDetails").html("");

                $.each(data.details, function (index, value) {
                    var valueHtml = "<tr>" +
                        "<td>" + value.accountNameSplit[0] + "</td>" +
                        "<td>" + value.accountNameSplit[1] + "</td>" +
                        "<td>" + value.bank + "</td>" +
                        "<td>" + value.accountNumber + "</td>" +
                        "<td class='netPay'>" + value.netPay + "</td>" +
                        "</tr>";
                    modal.find("#instructionDetails").append(valueHtml);
                });

                var grandTotalRow = "<tr>" +
                    "<td></td>" +
                    "<td></td>" +
                    "<td></td>" +
                    "<td class='text-right font-weight-bolder netPay'>Grand Total:</td>" +
                    "<td class='font-weight-bolder text-right netPay'>" + data.grandTotal + "</td>" +
                    "</tr>";
                modal.find("#instructionDetails").append(grandTotalRow);

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
    if ($("#table2").length > 0) {
        $("#table2").dataTable({
            columnDefs: [{ orderable: false, targets: -1 }],
        });
    }
}
