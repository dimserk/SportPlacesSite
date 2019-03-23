// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function GetTime(id) {
    $.ajax({
        url: '/api/records/ajax/t/' + id,
        type: 'GET',
        contentType: "application/json",
        success: function (times) {
            var options = '';
            $.each(times, function (index, time) {
                options += '<option value="' + time.time + '">' + time.showTime + '</option>';
            })
            $('#TimeSelect').html(options);
        }
    });
}

function GetLength(id) {
    $.ajax({
        url: '/api/records/ajax/l/' + id,
        type: 'GET',
        contentType: "application/json",
        success: function (lens) {
            var options = '';
            $.each(lens, function (index, len) {
                options += '<option value=' + len.length + '>' + len.name + '</option>';
            })
            $('#LengthSelect').html(options);
        }
    });
}


$(document).ready(function () {
    $("#SportObjectSelect").change(function () {
        var id = $(this).val();
        if (id != -1) {
            GetTime(id);
            GetLength(id);
        }
    })
})