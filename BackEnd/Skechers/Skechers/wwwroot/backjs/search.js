var searchInputs = document.querySelectorAll(".searching");
var searchResultBox = document.querySelector(".searh_box_main");

searchInputs.forEach(x => {
    x.addEventListener("click", function (e) {
        Swal.fire('Bu fonksiyon henüz tamamlanmadı')
        e.preventDefault();
        console.log("searching");
        searchResultBox.classList.toggle("searh_box_open");
    })
})




$(function () {
    $(".searching_bar input").keyup(function () {
        let markaname = $(this).val();
        console.log(text);
        fetch('http://localhost:45994/search/search?markaname=' + markaname)
            .then(response => response.text())
            .then(data => {
                var road = document.querySelector(".searching_bar .searh_box");
                road.innerHTML = data;
            })
    })
})