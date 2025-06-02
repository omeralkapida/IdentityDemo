$(document).ready(function () {

    const formHtml = `
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-12 col-sm-10 col-md-8 col-lg-6 col-xl-5 bg-white border rounded-4 p-4 shadow-sm">
                    <h4 class="text-center text-primary mb-4">Şifremi Unuttum</h4>
                    <div id="forgotPasswordResult" class="mt-3"></div>
                    <form id="forgotPasswordForm" class="needs-validation" novalidate>
                        <div class="mb-3">
                            <label for="email" class="form-label">E-posta</label>
                            <input type="email" class="form-control" id="email" placeholder="ornek@mail.com" required />
                            <div class="invalid-feedback">Lütfen e-posta adresinizi giriniz.</div>
                        </div>
                        <span class="btn btn-primary w-100" id="forgotPasswordButton">Şifremi Sıfırla</span>
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

        const form = $('#forgotPasswordForm')[0];
        if (!form.checkValidity()) {
            e.preventDefault();
            e.stopPropagation();
            form.classList.add('was-validated');
            return;
        }

        $('#forgotPasswordButton').prop('disabled', true).text('E-Posta adresinize şifre sıfırlama için gerekli adımlar gönderiliyor.');
        $('#loading').show();

        const email = $('#email').val();

        $.ajax({
            url: '/Account/ForgotPassword',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(email),
            success: function (response) {
                $('#forgotPasswordResult').html(`<div class="alert alert-success"><ul><li>Şifre sıfırlama linki e-posta adresinize gönderildi.</li></ul></div>`);
                $('#forgotPasswordForm')[0].reset();
                $('#forgotPasswordForm').addClass("d-none");
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

                $('#forgotPasswordResult').html(`<div class="alert alert-danger">${errorHtml}</div>`);
            },
            complete: function () {
                $('#forgotPasswordButton').prop('disabled', false).text('Şifremi Sıfırla');
                $('#loading').hide();
            }
        });
    });
}); 