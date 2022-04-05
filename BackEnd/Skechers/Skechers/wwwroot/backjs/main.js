$(function () {


    $(document).on("click", ".size-side .button", function () {
        $(".button").removeClass("size-active");
        $(this).addClass("size-active");
    })

    $(".size-side .button").first().addClass("size-active");
    $(".size-side .button").first().css("color", "white");

    $(document).on('click', '.add-basket', function (event) {

        //event.preventDefault();

        let productId = $(this).attr("data-productId")
        let sizeStr = $(".size-side .size-active input");
        let sizeId = sizeStr.attr("data-sizeId")
        console.log(sizeId)
        if (sizeId == undefined) {
            sizeId = 0
        }


        fetch('http://localhost:45994/product/addbasket?productId=' + productId + '&sizeId=' + sizeId)
            .then(response => response.text())
            .then(data => {
                //$("#last_part_list .text-number").text(data.basketItems.length);
                console.log(data);
            })
    })
})

//$(document).on("click", ".delete-basket", function (e) {

//    e.preventDefault();
//    console.log("delete basket")
//    let productId = $(this).attr("data-productId");
//    let sizeId = $(this).attr("data-sizeId")

//    fetch('http://localhost:45994/product/deletebasket?productId=' + productId + '&sizeId=' + sizeId)
//        .then(response => response.text())
//        .then(data => {
//            var basketview = document.querySelector("#sebet .table tbody");
//            basketview.innerHTML = data;
//            console.log(basketview);
//        })
//})


$(function (e) {
    $(document).on("click", ".delete-basket", function (e) {
        e.preventDefault();

        let url = $(this).attr("href");
        console.log(url);

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url).then(data => {
                    if (data.ok) {
                        window.location.reload(true);
                    }
                    else {
                        console.log("warning")
                    }
                })
            }
        })
    })
})
