$(document).ready(function () {

    const formHtml = `
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-12 col-sm-10 col-md-8 col-lg-6 col-xl-5 bg-white border rounded-4 p-4 shadow-sm">
                    <div id="registerResult" class="mt-3"></div>
                    <form id="registerForm" class="needs-validation" novalidate>
                        <div class="mb-3">
                            <label for="fullName" class="form-label">Ad Soyad</label>
                            <input type="text" class="form-control" id="fullName" placeholder="Adınızı ve soyadınızı girin" required />
                        </div>
                        <div class="mb-3">
                            <label for="fullName" class="form-label">Kullanıcı Adı</label>
                            <input type="text" id="userName" class="form-control" placeholder="Küçük harf, rakam, _ veya . içerebilir" required />
                            <small class="form-text text-muted">
                                Kullanıcı adı küçük harf olmalı ve Türkçe karakter içermemelidir.
                            </small>
                        </div>
                        <div class="mb-3">
                            <label for="email" class="form-label">E-posta</label>
                            <input type="email" class="form-control" id="email" placeholder="ornek@mail.com" required />
                        </div>
                        <div class="mb-3">
                            <label for="password" class="form-label">Şifre</label>
                            <input type="password" class="form-control" id="password" placeholder="Şifrenizi girin" required />
                        </div>
                        <span class="btn btn-primary w-100" id="registerButton">Kayıt Ol</span>
                        <div id="loading" class="text-center mt-3" style="display:none;">
                            <div class="spinner-border text-primary" role="status"></div>
                        </div>
                    </form>
                    <div class="text-center mt-4">
                        <span class="text-muted d-block">Hesabınız varsa hemen giriş yapabilirsiniz</span>
                        <a href="/Account/Login" class="text-decoration-none text-primary fw-bold">Giriş Sayfasına Git</a>
                    </div>
                </div>
            </div>
        </div>`;

    $("#register-container").html(formHtml);

    $('#registerButton').on('click', function (e) {
        e.preventDefault();

        const form = $('#registerForm')[0];
        if (!form.checkValidity()) {
            e.preventDefault();
            e.stopPropagation();
            form.classList.add('was-validated');
            return;
        }


        $('#registerButton').prop('disabled', true).text('Kayıt olunuyor...');
        $('#loading').show();

        const data = {
            fullName: $('#fullName').val(),
            email: $('#email').val(),
            password: $('#password').val(),
            userName: $("#userName").val()
        };

        $.ajax({
            url: '/Account/Register',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (response) {
                $('#registerResult').html(`<div class="alert alert-success"><ul><li>Kayıt başarılı!</li></ul></div>`);
                $('#registerForm')[0].reset();
                $('#registerForm').addClass("d-none");
            },
            error: function (xhr) {
                let response = xhr.responseJSON;
                let errorHtml = '';

                if (Array.isArray(response)) {
                    errorHtml = '<ul>';
                    response.forEach(function (err) {
                        errorHtml += `<li>${err}</li>`;
                    });
                    errorHtml += '</ul>';
                } else {
                    errorHtml = `<div>${xhr.responseText}</div>`;
                }

                $('#registerResult').html(`<div class="alert alert-danger">${errorHtml}</div>`);
            },
            complete: function () {
                $('#registerButton').prop('disabled', false).text('Kayıt Ol');
                $('#loading').hide();
            }
        });
    });
}); 