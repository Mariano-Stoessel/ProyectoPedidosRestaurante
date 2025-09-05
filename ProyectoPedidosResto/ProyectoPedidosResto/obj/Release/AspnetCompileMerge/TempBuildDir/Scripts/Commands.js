document.addEventListener('DOMContentLoaded', function () {
    var ddlCategorias = document.getElementById(window.ddlCategoriasId);
    var txtBusqueda = document.getElementById(window.txtBusquedaId);

    if (ddlCategorias) {
        ddlCategorias.addEventListener('change', filterProductos);
    }
    if (txtBusqueda) {
        txtBusqueda.addEventListener('keyup', filterProductos);
    }
});

function filterProductos() {
    var filtro = document.getElementById(window.txtBusquedaId).value.toLowerCase();
    var categoria = document.getElementById(window.ddlCategoriasId).value;

    document.querySelectorAll('.fila-producto-catalogo').forEach(function (row) {
        var nombre = row.querySelector('.col-6 span').textContent.toLowerCase();
        var catId = row.getAttribute('data-cat-id');
        var okTexto = !filtro || nombre.indexOf(filtro) > -1;
        var okCat = (!categoria || categoria === catId);
        row.style.display = (okTexto && okCat) ? '' : 'none';
    });
}
function clearFilters() {
    // 1) reseteo textbox y dropdown a “Todos” (value="")
    document.getElementById(txtBusquedaId).value = '';
    document.getElementById(ddlCategoriasId).value = '';
    // 2) reaplico el filtrado para mostrar todo
    filterProductos();
}

function seleccionarProductoLista(element) {
    if (element.classList.contains('selected')) {
        element.classList.remove('selected');
        document.getElementById(hfProductoListaSeleccionadoId).value = '';
        eliminarBarraControlPedido();
        return;
    }
    document.querySelectorAll('.fila-producto-lista.selected').forEach(function (fila) {
        fila.classList.remove('selected');
    });
    element.classList.add('selected');
    document.getElementById(hfProductoListaSeleccionadoId).value = element.getAttribute('data-producto-id');
    document.getElementById(hfArticuloNombreoListaSeleccionadoId).value = element.getAttribute('data-Nombre-Articulo');

    crearBarraControlPedidoLista(element);
}

function seleccionarProductoCatalogo(element) {
    if (element.classList.contains('selected')) {
        element.classList.remove('selected');
        document.getElementById(hfProductoSeleccionadoId).value = '';
        document.getElementById(hfNombreProductoSeleccionadoId).value = '';

        return;
    }
    document.querySelectorAll('.fila-producto-catalogo.selected').forEach(function (fila) {
        fila.classList.remove('selected');
    });
    element.classList.add('selected');
    document.getElementById(hfProductoSeleccionadoId).value = element.getAttribute('data-producto-id');
    document.getElementById(hfNombreProductoSeleccionadoId).value = element.getAttribute('data-producto-nombre');
    document.getElementById(hfPrecioProductoSeleccionadoId).value = element.getAttribute('data-precio-producto');
}

function crearBarraControlPedidoLista(element) {
    var barra = document.getElementById('controlPedidoBarra');
    if (barra) barra.style.display = 'flex';
    var btnEliminar = document.getElementById(btnEliminarProductoId);
    if (btnEliminar) btnEliminar.style.display = 'inline-block';
    var btnModificar = document.getElementById(btnModificarProductoId);
    if (btnModificar) btnModificar.style.display = 'inline-block';
}

function eliminarBarraControlPedido() {
    var barra = document.getElementById('controlPedidoBarra');
    if (barra) barra.style.display = 'none';
    var btnEliminar = document.getElementById(btnEliminarProductoId);
    if (btnEliminar) btnEliminar.style.display = 'none';
    var btnModificar = document.getElementById(btnModificarProductoId);
    if (btnModificar) btnModificar.style.display = 'none';
}

function abrirModalModificarCantidad() {
    var fila = document.querySelector('.fila-producto-lista.selected');
    if (fila) {
        var spans = fila.querySelectorAll('span');
        var cantidad = spans.length > 1 ? spans[1].textContent.trim() : "1";
        document.getElementById('lblCantidadLista').textContent = cantidad;

        // Obtén el estado desde el atributo data-estado
        var estado = fila.getAttribute('data-estado');
        if (estado) {
            document.getElementById(ddlEstadoId).value = estado;
        }

        $('#ModalModificarCantidad').modal('show');
    }
}
function abrirModalAgregarProducto() {
    document.getElementById('lblCantidad').textContent = '1';
    document.getElementById(hfCantidadId).value = '1';
}

function cambiarCantidad(delta) {
    var label = document.getElementById('lblCantidadLista');
    var valor = parseInt(label.textContent) || 0;
    valor += delta;
    if (valor < 0) valor = 0;
    label.textContent = valor;
}

function cambiarCantidadCatalogo(delta) {
    var label = document.getElementById('lblCantidad');
    var valor = parseInt(label.textContent) || 1;
    valor += delta;
    if (valor < 1) valor = 1;
    label.textContent = valor;
}

function guardarCantidadYPostback() {
    var valor = document.getElementById('lblCantidadLista').textContent;
    document.getElementById(hfNuevaCantidadId).value = valor;
    // Permite el postback
    return true;
}
function guardarCantidadCatalogo() {
    var valor = document.getElementById('lblCantidad').textContent;
    document.getElementById(hfCantidadId).value = valor;
}