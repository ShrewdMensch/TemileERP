$(document).ready(function () {
    InitializeDataTable();

    AddBankInstructionsPrintModalLogic();

    AddPrintButtonClickEvent();

    AddSendEmailButtonClickEvent();

});

function AddSendEmailButtonClickEvent() {
    $("#sendMailBtn").click(function() {
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

        pdfMake.createPdf(dd).download(todayDate.replace(/ /g, '_') + '_instructionsToBank');
    });
}

function AddPrintButtonClickEvent() {
    $("#printBtn").click(function() {
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
    $("#bankInstructionsPrintModal").on("shown.bs.modal", function(event) {
        var button = $(event.relatedTarget);
        var vessel = button.data("vessel");
        var modal = $(this);

        modal.find(".modal-body .row").attr("hidden", true);
        modal.find("#loader").attr("hidden", false);
        $(".spinner-border").attr("hidden", false);

        $.ajax({
            url: "/api/bankinstructions/" + vessel,
            dataType: "json",
            type: "GET",
            success: function(data) {

                $("#vessel").text(data.vessel);
                $("#dateTitle").text(data.date);
                $("#vessel2").text(data.vessel);
                $("#dateTitle2").text(data.date);

                modal.find("#instructionDetails").html("");

                $.each(data.details, function(index, value) {
                    var valueHtml = "<tr>" +
                        "<td>" + value.personnelName + "</td>" +
                        "<td>" + value.bank + "</td>" +
                        "<td>" + value.accountName + "</td>" +
                        "<td>" + value.accountNumber + "</td>" +
                        "<td class='netPay'>" + value.netPay + "</td>" +
                        "</tr>";
                    modal.find("#instructionDetails").append(valueHtml);
                });



                modal.find(".modal-body .row").attr("hidden", false);
                $("#loader").attr("hidden", true);
                $(".spinner-border").attr("hidden", true);
            },
            error: function() {
                alert("Error occurred...");
            },
        });
    });
}

function InitializeDataTable() {
    if ($("#table").length > 0) {
        $("#table").dataTable({
            columnDefs: [{ orderable: false, targets: -1 }],
        });
    }
}
