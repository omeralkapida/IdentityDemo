
$(document).ready(function () {

    const formHtml = `
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-12 col-sm-10 col-md-8 col-lg-6 col-xl-5 bg-white border rounded-4 p-4 shadow-sm">
                    <h4 class="text-center text-success mb-4">${userName_fullName}</h4>
                </div>
            </div>
        </div>`;

    $('#home-container').html(formHtml);
});
