// Show filtered elements
function w3AddClass(element1, name1) {
    var d, arr11, arr22;
    arr11 = element1.className.split(" ");
    arr22 = name1.split(" ");
    for (d = 0; d < arr22.length; d++) {
        console.log("show element")
      if (arr11.indexOf(arr22[d]) == -1) {
        element1.className += " " + arr22[d];
      }
    }
  }

// Hide elements that are not selected
function w3RemoveClass(element2, name2) {
    var b, arr1, arr2;
    arr1 = element2.className.split(" ");
    arr2 = name2.split(" ");
    for (b = 0; b < arr2.length; b++) {
        console.log("hide element")
      while (arr1.indexOf(arr2[b]) > -1) {
        arr1.splice(arr1.indexOf(arr2[b]), 1);
      }
    }
    element2.className = arr1.join(" ");
  }

// Add active class to the current button (highlight it)
var btnContainer = document.getElementById("filter-navbar");
var btns = btnContainer.getElementsByClassName("clickbtn");
for (var r = 0; r < btns.length; r++) {
  btns[r].addEventListener("click", function(){
    var current = document.getElementsByClassName("active");
    current[0].className = current[0].className.replace(" active", "");
    this.className += " active";
  });
}


// owl carusel
$('.owl-carousel').owlCarousel({
    loop:true,
    margin:10,
    nav:false,
    dots: false,
    autoplay:true,
    autoplayTimeout:1500,
    autoplayHoverPause:true,
    responsive:{
        0:{
            items:2
        },
        300:
        {
            items:2
        },
        338:{
            items:1
        },
        375:
        {
            items:1
        },
        400:
        {
            items:2
        },
        567:{
            items:1
        },
        600:{
            items:2
        },
        800:
        {
            items:2
        },
        1000:{
            items:5
        },
        1015:{
            items:4
        },
        992:{
            items:2
        }
    }
})



 //about section read more btn 
const readMoretextBtn=document.getElementById('oku');
const text=document.getElementById('moretext');
readMoretextBtn.addEventListener('click',(e)=>{
    text.classList.toggle('show-more');
    if(readMoretextBtn.innerText=='Devamını Oku')
    {
        readMoretextBtn.innerText='Kapat';
    }
    else
    {
        readMoretextBtn.innerText='Devamını Oku';
    }
})




//navbar buttons
filterSelection("all")
function filterSelection(c) {
    var x, p;
    x = document.getElementsByClassName("filterDiv");
    if (c == "all") c = "";
     //Add the "show" class (display:block) to the filtered elements, and remove the "show" class from the elements that are not selected
    for (p = 0; p < x.length; p++) {
        w3RemoveClass(x[p], "show");
        if (x[p].className.indexOf(c) > -1) w3AddClass(x[p], "show");
    }
}



// back to-top button
const toTop = document.querySelector('.to-top');
window.addEventListener('scroll', () => {
    if (window.pageYOffset > 100) {
        toTop.classList.add('active');
    }
    else {
        toTop.classList.remove('active');
    }
})



// navbar scroll
var lastscrolltop = 0;
let navbar = document.getElementById('navheader');
window.addEventListener('scroll', function () {
    var scrolltop = window.pageYOffset || document.documentElement.scrollTop;
    if (scrolltop > lastscrolltop) {
        navbar.style.top = "-80px";
    }
    else {
        navbar.style.top = "0";
    }
    lastscrolltop = scrolltop;
})



// autotext-navbar
let i = 0;
let placeholder = "";
const txt = "Ara: Ayakkabı";
const speed = 120;
function type() {
    placeholder += txt.charAt(i);
    document.getElementById("demo").setAttribute("placeholder", placeholder);
    i++;
    setTimeout(type, speed);
}
type();
