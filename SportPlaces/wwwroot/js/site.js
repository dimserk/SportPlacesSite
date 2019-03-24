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


function GetSportObjectsRecords(date, sportObjectId) {
    $.ajax({
        url: '/api/records/ajax/s/' + date + '/' + sportObjectId,
        type: 'GET',
        contentType: "application/json",
        success: function (recs) {
            $("table tbody").empty();
            var row = "";
            $.each(recs, function (index, rec) {
                row += "<tr><td>" + rec.time + "</td><td>" + rec.login + "</td><td>" + rec.length + "</td></tr>";
            })
            $("table tbody").append(row);

        }
    })
}

function GetUserRecords(userId, date) {
    $.ajax({
        url: '/api/records/ajax/u/' + userId + '/' + date,
        type: 'GET',
        contentType: "application/json",
        success: function (recs) {
            $("table tbody").empty();
            var row = "";
            $.each(recs, function (index, rec) {
                row += "<tr><td>" + rec.date + "</td><td>" + rec.userName + "</td><td>" + rec.length + "</td></tr>";
            })
            $("table tbody").append(row);

        }
    })
}

$(document).ready(function () {
    //
    $("#SportObjectSelect").change(function () {
        var id = $(this).val();
        if (id != -1) {
            GetTime(id);
            GetLength(id);
        }
    })

    //
    $("#DateInput").change(function () {
        GetSportObjectsRecords($(this).val(), $("#SportObjectSelect").val())
    });

    $("#SportObjectSelect").change(function () {
        GetSportObjectsRecords($("#DateInput").val(), $(this).val())
    });

    //
    $("#UserSelect").change(function () {
        GetUserRecords($(this).val(), $("DateInput2").val());
    })

    $("#DateInput2").change(function () {
        var uid = $("#UserSelect").val();
        if (uid != 0) {
            GetUserRecords(uid, $(this).val());
        }
    })
})