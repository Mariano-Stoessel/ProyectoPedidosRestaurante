function modificarPersonas(labelId, hiddenId, delta) {
    var label = document.getElementById(labelId);
    var hidden = document.getElementById(hiddenId);
    if (!label || !hidden) return;

    var valor = parseInt(label.innerText, 10) || 1;
    valor += delta;
    if (valor < 1) valor = 1;

    label.innerText = valor;
    hidden.value = valor;
}

function redirigirComanda(idMesa) {
    window.location.href = 'commands.aspx?idMesa=' + encodeURIComponent(idMesa);
}

document.addEventListener("DOMContentLoaded", function () {
    var modal = document.getElementById('modalMesaGeneral');
    modal.addEventListener('hidden.bs.modal', function () {
        // Resetea el DropDownList de mozos
        var ddl = document.getElementById('<%= ddlMozos.ClientID %>');
        if (ddl) ddl.selectedIndex = 0;

        // Resetea el campo de observaciones
        var obs = document.getElementById('<%= txtObservaciones.ClientID %>');
        if (obs) obs.value = '';

        // Resetea la cantidad de personas
        var lbl = document.getElementById('lblPersonas');
        if (lbl) lbl.innerText = '1';

        var hf = document.getElementById('hfPersonas');
        if (hf) hf.value = '1';
    });

    // Evento show.bs.modal: solo carga datos, no resetea
    modal.addEventListener('show.bs.modal', function (event) {
        var button = event.relatedTarget;
        var mesaId = button.getAttribute('data-mesa-id');
        var personas = button.getAttribute('data-personas');
        var mozoId = button.getAttribute('data-mozo-id');

        document.getElementById('modalMesaNumero').textContent = mesaId;
        document.getElementById('<%= hfMesaSeleccionadaId.ClientID %>').value = mesaId;

        // Si hay datos para cargar (por ejemplo, mesa reservada), los cargas aquí
        if (personas) {
            document.getElementById('lblPersonas').innerText = personas;
            document.getElementById('hfPersonas').value = personas;
        }
        if (mozoId) {
            var ddlMozos = document.getElementById('<%= ddlMozos.ClientID %>');
            if (ddlMozos) ddlMozos.value = mozoId;
        }
    });
});