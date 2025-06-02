const formHtml = `
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-12 col-sm-10 col-md-8 col-lg-6 col-xl-5 bg-white border rounded-4 p-4 shadow-sm">

                <h4 class="text-center text-success mb-4">Giriş Yap</h4>

                <div id="loginResult" class="mb-3"></div>

                <form id="loginForm" class="needs-validation" novalidate>
                    <div class="mb-3">
                        <label for="userName" class="form-label">Kullanıcı Adı</label>
                        <input type="text" class="form-control" id="userName" required />
                        <div class="invalid-feedback">Lütfen kullanıcı adınızı girin.</div>
                    </div>

                    <div class="mb-3">
                        <label for="password" class="form-label">Şifre</label>
                        <input type="password" class="form-control" id="password" required />
                        <div class="invalid-feedback">Lütfen şifrenizi girin.</div>
                    </div>

                    <button type="submit" class="btn btn-success w-100" id="loginButton">Giriş Yap</button>

                    <div class="text-center mt-2 small">
                        <a href="/Account/ForgotPassword" class="text-muted d-block mb-1">Şifremi Unuttum!</a>
                    </div>

                    <div id="loading" class="text-center mt-3" style="display: none;">
                        <div class="spinner-border text-success" role="status"></div>
                    </div>
                </form>

                <div class="text-center mt-2 small">
                    <span class="text-muted d-block mb-1">Hesabınız yok mu?</span>
                    <a href="/Account/Register" class="text-decoration-none text-primary fw-bold">Üyelik Sayfasına Git</a>
                </div>

            </div>
        </div>
    </div>`;


$('#login-container').html(formHtml);

$('#loginButton').on('click', function (e) {
    e.preventDefault();

    const form = $('#loginForm')[0];
    if (!form.checkValidity()) {
        e.preventDefault();
        e.stopPropagation();
        form.classList.add('was-validated');
        return;
    }

    $('#loginButton').prop('disabled', true).text('Giriş yapılıyor...');
    $('#loading').show();

    const data = {
        userName: $('#userName').val(),
        password: $('#password').val()
    };

    $.ajax({
        url: '/Account/Login',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (response) {
            window.location.href = response.redirectUrl;
        },
        error: function (xhr) {
            const response = xhr.responseJSON;
            let html = '';

            if (Array.isArray(response)) {
                html = '<ul>';
                response.forEach(err => html += `<li>${err}</li>`);
                html += '</ul>';
            } else {
                html = response || "Giriş yapılamadı.";
            }

            $('#loginResult').html(`<div class="alert alert-danger">${html}</div>`);
        },
        complete: function () {
            $('#loginButton').prop('disabled', false).text('Giriş Yap');
            $('#loading').hide();
        }
    });
});
