$(document).ready(function () {

    const formHtml = `
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-12 col-sm-10 col-md-8 col-lg-6 col-xl-5 bg-white border rounded-4 p-4 shadow-sm">
                    <h4 class="text-center text-primary mb-4">Şifremi Unuttum</h4>
                    <div id="forgotPasswordResult" class="mt-3"></div>
                    <form id="registerForm" class="needs-validation" novalidate>
                        <div class="mb-3">
                            <label for="email" class="form-label">E-posta</label>
                            <input type="email" class="form-control" id="email" placeholder="ornek@mail.com" required />
                        </div>
                        <span class="btn btn-primary w-100" id="forgotPasswordButton"></span>
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

    $("#forgotPassword-container").html(formHtml);

    $('#forgotPasswordButton').on('click', function (e) {
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