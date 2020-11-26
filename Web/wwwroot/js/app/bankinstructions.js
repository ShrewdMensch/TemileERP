$(document).ready(function () {
    if ($("#table").length > 0) {
        $("#table").dataTable({
            columnDefs: [{ orderable: false, targets: 4 }],
        });
    }



    $("#bankInstructionsPrintModal").on("shown.bs.modal", function (event) {
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
            success: function (data) {

                $("#vessel").text(data.vessel);
                $("#dateTitle").text(data.date);

                modal.find("#instructionDetails").html("");

                $.each(data.details, function (index, value) {
                    var valueHtml =
                        "<tr>" +
                        "<td>"+value.payrollId+"</td>" +
                        "<td>"+value.personnelName+"</td>" +
                        "<td>"+value.personnelId+"</td>" +
                        "<td>"+value.bank+"</td>" +
                        "<td>" + value.accountName + "</td>" +
                        "<td>" + value.accountNumber + "</td>" +
                        "<td>"+value.netPay+"</td>" +
                        "</tr>";
                    modal.find("#instructionDetails").append(valueHtml);
                });



                modal.find(".modal-body .row").attr("hidden", false);
                $("#loader").attr("hidden", true);
                $(".spinner-border").attr("hidden", true);
            },
            error: function () {
                alert("Error occurred...");
            },
        });
    });

    $("#printBtn").click(function () {
        var year = new Date().getFullYear();
        $("#bankInstructionContainer").print({
            title: "Instructions to Bank",
            append: "",
            iframe: false,
            deferred: $.Deferred(),
            doctype: "<!doctype html>",
            prepend:
                '<br><p class="text-primary font-weight-bold">Temile and Sons Limited &copy;' +
                year +
                "</p>",
        });
    });

});
