$(document).ready(function () {
    $("div#uploadFile").hide();
    $("div#enterData").hide();
    $("div#run").hide();
    

    $("input[name=action]").on("click", function () {
        var selectedValue = $("input[name=action]:checked").val();

        if (selectedValue == "enterData") {
            $("div#uploadFile").hide();
            $("div#enterData").show();
            $("div#run").show();
        }
        else if (selectedValue == "uploadFile") {
            $("div#uploadFile").show();
            $("div#enterData").hide();
            $("div#run").show();
        }
    })



});