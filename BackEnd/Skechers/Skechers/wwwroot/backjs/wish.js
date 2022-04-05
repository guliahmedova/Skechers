//add wishlist////
$(document).on("click", ".add-wishlist", function (e) {

     e.preventDefault();

     //alert("fava eklendi")

     Swal.fire({
         position: 'top-end',
         icon: 'success',
         title: 'Favorilere Eklendi',
         showConfirmButton: false,
         timer: 1500
     })

    let url = $(this).attr("href");

    fetch(url)
        .then(response => response.text())
        .then(data => {
            $("#favoriler .container .row .card").html(data);
        })
 })


//remove wishlist///
//$(document).on("click", ".remove-wishlist", function (e) {

//    e.preventDefault();

//    //alert("fava eklendi")

//    Swal.fire({
//        position: 'top-end',
//        icon: 'success',
//        title: 'Favorilerden Silindi',
//        showConfirmButton: false,
//        timer: 1500
//    })

//    //let url = $(this).attr("href");

//    //fetch(url)
//    //    .then(response => response.text())
//    //    .then(data => {
//    //        $("#favoriler .container .row .card").html(data);
//    //    })
//})





////$(document).on("click", ".remove-wishlist", function (e) {
////    e.preventDefault();

////    let id = $(this).attr("data-id");

////    fetch('http://localhost:45994//wishlist/addwishlist/' + id)
////        .then(response => response.text())
////        .then(data => {
////            //$("#wishlist .container .row tbody").html(data);
////            console.log(data);
////        })
////  })
////})