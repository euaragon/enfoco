// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const tiempoRecargaSegundos = 600; // 10 minutos (cambia este valor según tus necesidades)

setTimeout(function () {
    // Recargar la página después de que el tiempo de expiración se agote
    location.reload();
}, tiempoRecargaSegundos * 1000);

document.querySelector(".btn-edit").addEventListener("click", function () {
    document.querySelector(".form-edit").classList.remove("d-none");
});

document.querySelector('#createForm').addEventListener('submit', function (e) {
    e.preventDefault(); // Prevenir el envío automático del formulario

    // Mostrar una ventana de confirmación con SweetAlert
    Swal.fire({
        title: '¿Estás seguro?',
        text: '¿Quieres guardar esta noticia?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Guardar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Si el usuario confirma, enviar el formulario
            this.submit();
        }
    });
});