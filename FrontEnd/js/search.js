var searchInputs=document.querySelectorAll(".searching");
var searchResultBox=document.querySelector(".searh_box_main");

searchInputs.forEach(x=>{
    x.addEventListener("click",function(){
        console.log("searchinput");
        searchResultBox.classList.toggle("searh_box_open");
    })
})



// var seacrhinput=document.querySelectorAll(".searching");
// var seacrhBox =document.querySelectorAll(".searh_box");

// seacrhinput.forEach(x => {
// x.addEventListener("click", function () {
//   seacrhBox.classList.toggle("searh_box_open");
// console.log("searching");
// });
// }); 