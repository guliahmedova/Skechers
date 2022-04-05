// show-hide  password
function showHide()
{
    let icon=document.querySelector('.icon');
    let input =document.getElementById('passwordregister');

    if(input.type === 'password')
    {
        input.type='text';
    }
    else 
    {
        input.type='password'
    }

    icon.classList.toggle('is-active')
}


