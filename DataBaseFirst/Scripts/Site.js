$('#btnPlus').click(function (e) {
    e.preventDefault();
    var SK = $(".SkillContainer")
    $.ajax({
        url: "/Employees/AddSlills",
        type: "GET",
        success: function (data) {
            SK.append(data)
        }
    })
})
$(document).on("click", "#deleteSkill", function (e) {
    e.preventDefault();
    $(this).parent().parent().remove()
})

function ReadUrl(i) {
    if (i.files && i.files[0]) {
        var readar = new FileReader();
        readar.onload = function (e) {
            $("#Imgfile").attr('src', e.target.result);
        }
        readar.readAsDataURL(i.files[0])
    }
}