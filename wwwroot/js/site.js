// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

$(document).ready(function () {
    const email = getCookie('user');
    fetch(`/find-account?email=${email}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
    })
    .then(response => response.json())
    .then(data => {
        $('.posting-url').attr('href', `/companies/${data.id}/post`);
        $('.profile-con').html(`
            <div class="btn-group dropdown-primary">
                <a class='dropdown-toggle hidden-arrow d-flex align-items-center'
                    href='#'
                    id='navbarDropdownMenuLink'
                    role='button'
                    data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false"
                    style='gap: 10px'>
                    <p class='my-0'>${data.name}</p>
                    <img src='${data.avatar}'
                            class='rounded-circle'
                            height='35'
                            alt=''
                            loading='lazy' />
                </a>
                <div class="dropdown-menu" style="line-height:1.5">
                    <a class='dropdown-item' href='/${data.id}'>Hồ sơ</a>
                    <a class='dropdown-item' href='/auth/logout'>Đăng xuất</a>
                </div>
            </div>`)
    })
    .catch(error => console.error(error));
})
