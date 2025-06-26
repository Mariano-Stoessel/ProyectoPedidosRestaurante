var categoria = document.getElementById('<%= ddlCategorias.ClientID %>').value.toLowerCase();
var catId = row.getAttribute('data-cat-id').toLowerCase();
var okCat = !categoria || categoria === catId;
document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('<%= txtBusqueda.ClientID %>')
        .addEventListener('keyup', filterProductos);
    document.getElementById('<%= ddlCategorias.ClientID %>')
        .addEventListener('change', filterProductos);
    $('#ModalComandas').on('shown.bs-modal shown.bs.modal', filterProductos);
});


function filterProductos() {
    var filtro = document.getElementById('<%= txtBusqueda.ClientID %>').value.toLowerCase();
    var categoria = document.getElementById('<%= ddlCategorias.ClientID %>').value;

    document.querySelectorAll('.fila-producto-catalogo').forEach(function (row) {
        var nombre = row.querySelector('.col-7 span').textContent.toLowerCase();
        var catId = row.getAttribute('data-cat-id');

        // okTexto: pasa si está vacío el buscador o coincide el texto
        var okTexto = !filtro || nombre.indexOf(filtro) > -1;
        // okCat: pasa si “Todos” (value="") o coincide la categoría
        var okCat = (!categoria || categoria === catId);

        row.style.display = (okTexto && okCat) ? '' : 'none';
    });
}
function clearFilters() {
    // 1) reseteo textbox y dropdown a “Todos” (value="")
    document.getElementById('<%= txtBusqueda.ClientID %>').value = '';
    document.getElementById('<%= ddlCategorias.ClientID %>').value = '';
    // 2) reaplico el filtrado para mostrar todo
    filterProductos();
}

function seleccionarProductoLista(element) {
    if (element.classList.contains('selected')) {
        element.classList.remove('selected');
        document.getElementById('<%= hfProductoListaSeleccionado.ClientID %>').value = '';
        eliminarBarraControlPedido();
        return;
    }
    document.querySelectorAll('.fila-producto-lista.selected').forEach(function (fila) {
        fila.classList.remove('selected');
    });
    element.classList.add('selected');
    document.getElementById('<%= hfProductoListaSeleccionado.ClientID %>').value = element.getAttribute('data-producto-id');
    document.getElementById('<%= hfArticuloNombreoListaSeleccionado.ClientID %>').value = element.getAttribute('data-Nombre-Articulo');

    crearBarraControlPedidoLista(element);
}

function seleccionarProductoCatalogo(element) {
    if (element.classList.contains('selected')) {
        element.classList.remove('selected');
        document.getElementById('<%= hfProductoSeleccionado.ClientID %>').value = '';
        document.getElementById('<%= hfNombreProductoSeleccionado.ClientID %>').value = '';

        return;
    }
    document.querySelectorAll('.fila-producto-catalogo.selected').forEach(function (fila) {
        fila.classList.remove('selected');
    });
    element.classList.add('selected');
    document.getElementById('<%= hfProductoSeleccionado.ClientID %>').value = element.getAttribute('data-producto-id');
    document.getElementById('<%=hfNombreProductoSeleccionado.ClientID %>').value = element.getAttribute('data-producto-nombre');
    document.getElementById('<%=hfPrecioProductoSeleccionado.ClientID %>').value = element.getAttribute('data-precio-producto');
}

function crearBarraControlPedidoLista(element) {
    var barra = document.getElementById('controlPedidoBarra');
    if (barra) barra.style.display = 'flex';
    var btnEliminar = document.getElementById('<%= btnEliminarProducto.ClientID %>');
    if (btnEliminar) btnEliminar.style.display = 'inline-block';
    var btnModificar = document.getElementById('<%= btnModificarProducto.ClientID %>');
    if (btnModificar) btnModificar.style.display = 'inline-block';
}

function eliminarBarraControlPedido() {
    var barra = document.getElementById('controlPedidoBarra');
    if (barra) barra.style.display = 'none';
    var btnEliminar = document.getElementById('<%= btnEliminarProducto.ClientID %>');
    if (btnEliminar) btnEliminar.style.display = 'none';
    var btnModificar = document.getElementById('<%= btnModificarProducto.ClientID %>');
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
            document.getElementById('<%= ddlEstado.ClientID %>').value = estado;
        }

        $('#ModalModificarCantidad').modal('show');
    }
}
function abrirModalAgregarProducto() {
    document.getElementById('lblCantidad').textContent = '1';
    document.getElementById('<%= hfCantidad.ClientID %>').value = '1';
    // Si usas Bootstrap 5, puedes abrir el modal así:
    // var modal = new bootstrap.Modal(document.getElementById('ModalComandas'));
    // modal.show();
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
    document.getElementById('<%= hfNuevaCantidad.ClientID %>').value = valor;
    // Permite el postback
    return true;
}
function guardarCantidadCatalogo() {
    var valor = document.getElementById('lblCantidad').textContent;
    document.getElementById('<%= hfCantidad.ClientID %>').value = valor;
}