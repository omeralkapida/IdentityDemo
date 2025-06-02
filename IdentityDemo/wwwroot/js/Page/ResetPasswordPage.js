const formHtml = `
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-12 col-sm-10 col-md-8 col-lg-6 col-xl-5 bg-white border rounded-4 p-4 shadow-sm">

                <h4 class="text-center text-success mb-4">Şifrenizi Sıfırlayın</h4>

                <div id="resetPasswordResult" class="mb-3"></div>

                <form id="resetPasswordForm" class="needs-validation" novalidate>
                    <div class="mb-3">
                        <label for="password" class="form-label">Şifre</label>
                        <input type="password" class="form-control" id="newPassword" required />
                        <div class="invalid-feedback">Lütfen şifrenizi girin.</div>
                    </div>
                    <div class="mb-3">
                        <label for="password" class="form-label">Tekrar Şifre</label>
                        <input type="password" class="form-control" id="confirmPassword" required />
                        <div class="invalid-feedback">Lütfen şifrenizi girin.</div>
                    </div>

                    <button type="submit" class="btn btn-success w-100" id="resetPasswordButton">Sıfırla</button>

                    <div id="loading" class="text-center mt-3" style="display: none;">
                        <div class="spinner-border text-success" role="status"></div>
                    </div>
                </form>
                <div class="text-center mt-4">
                    <span class="text-muted d-block">Hesabınız varsa hemen giriş yapabilirsiniz</span>
                    <a href="/Account/Login" class="text-decoration-none text-primary fw-bold">Giriş Sayfasına Git</a>
                </div>
            </div>
        </div>
    </div>`;


$('#resetPassword-container').html(formHtml);

$('#resetPasswordButton').on('click', function (e) {
    e.preventDefault();

    const form = $('#resetPasswordForm')[0];
    if (!form.checkValidity()) {
        e.preventDefault();
        e.stopPropagation();
        form.classList.add('was-validated');
        return;
    }

    $('#loading').show();

    const data = {
        UserId: GetUrlParameter("userId"),
        Token: GetUrlParameter("token"),
        NewPassword: $('#newPassword').val(),
        ConfirmPassword: $('#confirmPassword').val()
    };

    $.ajax({
        url: '/Account/ResetPassword',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (response) {

            $('#resetPasswordResult').html(`<div class="alert alert-success"><ul><li>Şifre Sıfırlama Tamamlandı Giriş Sayfasına Yönlendiriliyorsunuz</li></ul></div>`);
            $('#resetPasswordForm')[0].reset();
            $('#resetPasswordForm').addClass("d-none");

            setTimeout(function () {
                window.location.href = response.redirectUrl;
            }, 3000);
        },
        error: function (xhr) {
            const response = xhr.responseJSON;
            let html = '';

            if (Array.isArray(response)) {
                html = '<ul>';
                response.forEach(err => html += `<li>${err}</li>`);
                html += '</ul>';
            } else {
                html = response || "Şifre sıfırlanırken hata oluştu. Lütfen daha sonra tekrar deneyin.";
            }

            $('#resetPasswordResult').html(`<div class="alert alert-danger">${html}</div>`);
        },
        complete: function () {
            $('#loading').hide();
        }
    });
});

function GetUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    const regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    const results = regex.exec(location.search);
    return results === null ? null : decodeURIComponent(results[1].replace(/\+/g, ' '));
}
