document.getElementById('imgFile').addEventListener('change', function () {
    var form = document.getElementById('imageUploadForm');
    var formData = new FormData(form);

    fetch(form.action, {
        method: form.method,
        body: formData
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            if (data.Url) {
                document.getElementById('profile-img-edit').src = data.Url;
                document.querySelector('input[name="ImageUrl"]').value = data.Url;
            } else {
                console.error('No URL returned');
            }
        })
        .catch(error => console.error('Error:', error));
});