const uri = "api/todo";
let todos = null;
function getCount(data) {
    const el = $("#counter");
    let name = "to-do";
    if (data) {
        if (data > 1) {
            name = " to-dos";
        }
        el.text(data + "" + name);
    }
    else {
        el.text("No: " + name);
    }
}

$(document).ready(function () {
    getData("", 1);

    $("ul").on("click", "li #previous", function () {
        getData($("#NameSearch").val(), (parseInt)($("#currentPage").val()) - 1);
    });

    $("ul").on("click", "li #next", function () {
        getData($("#NameSearch").val(), (parseInt)($("#currentPage").val()) + 1);
    });

    $("ul").on("click", "li .number", function () {
        getData($("#NameSearch").val(), $(this).val())
    });
});
function getData(searchString, pageNumber) {
    $.ajax({
        type: "GET",
        url: uri + "?searchString=" + searchString + "&pageNumber=" + pageNumber,
        cache: false,
        success: function (model) {
            const tBody = $("#todos");
            $(tBody).empty();
            getCount(model.totalPages);

            $.each(model.items, function (key, item) {
                const tr = $("<tr></tr>").append(
                    $("<td></td>").append(
                        $("<input/>", {
                            type: "checkbox",
                            disabled: true,
                            checked: item.isComplete
                        })
                    ))
                    .append($("<td></td>").text(item.name))
                    .append($("<td></td>").append(
                        $("<button class='btn btn-primary' data-id=" + item.id + ">Sửa</button>").on("click", function () {
                            editItem($(this).data("id"));
                        })
                    ))
                    .append($("<td></td>").append(
                        $("<button class='btn btn-danger' data-id=" + item.id + ">Xóa</button>").on("click", function () {
                            deleteItem($(this).data("id"));
                        })
                    ))
                tr.appendTo(tBody);
            });

            $(".page").html("");
            var code = "<li><input type='hidden' id='currentPage' value=" + model.currentPage + " /></li>" +
                "<li><button id='previous'><i class='fas fa-angle-double-left'></i></button></li>";
            if ((model.totalPages - model.currentPage) >= 2) {
                var dem = model.currentPage;
                for (var i = dem; i < dem + 3; i++) {
                    code = code + "<li><button class='number' value=" + i + ">" + i + "</button></li>"
                }
            }
            else {
                var tong = model.totalPages;
                for (var i = (tong - 3) > 0 ? (tong - 3) : 1; i < tong; i++) {
                    code = code + "<li><button class='number' value=" + i + ">" + i + "</button></li>"
                }
            }

            code = code + "<li><button id='next'><i class='fas fa-angle-double-right'></i></button></li >";
            $(".page").append(code);

            todos = model.items;

            if (model.totalPages == model.currentPage) {
                $("#next").attr("disabled", "disabled");
            }
            if (model.currentPage == 1) {
                $("#previous").attr("disabled", "disabled");
            }
        }
    });
}

function addItem() {
    const item = {
        name: $("#add-name").val(),
        isComplete: false
    };
    $.ajax({
        url: uri,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(item),
        error: function () {
            alert("Something went wrong!");
        },
        success: function () {
            getData("", (parseInt)($("#currentPage").val()));
            $("#add-name").val("")
        }
    });
}
function deleteItem(id) {
    $.ajax({
        url: uri + '/' + id,
        type: 'delete',
        success: function () {
            getData("", (parseInt)($("#currentPage").val()));
        }
    });
}
function editItem(id) {
    $.each(todos, function (key, item) {
        if (item.id == id) {
            $("#edit-name").val(item.name);
            $("#edit-id").val(item.id);
            $("#edit-isComplete")[0].checked = item.isComplete;
        }
    });
    $("#spoiler").css({ display: "block" });
    $(".my-form").on("submit", function () {
        const item = {
            name: $("#edit-name").val(),
            isComplete: $("#edit-isComplete").is(":checked"),
            id: $("#edit-id").val()
        }
        $.ajax({
            url: uri + '/' + $("#edit-id").val(),
            type: 'put',
            contentType: 'application/json',
            data: JSON.stringify(item),
            success: function () {
                getData((parseInt)($("#currentPage").val()));
            }
        });
        close();
        return false;
    })
}
function close() {
    $("#spoiler").css({ display: "none" });
}