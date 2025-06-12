<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Commands.aspx.cs" Inherits="ProyectoPedidosResto.Views.Commands" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <style>
        .commands-container {
            padding-top: 0 !important;
        }

        .product-list {
            background-color: #2b2b2b;
            border-radius: 0.5rem;
            font-size: smaller;
            margin-bottom: 10px;
            height: 200px;
            overflow-y: auto;
        }

            .product-list .row {
                padding-left: 0.5rem;
                padding-right: 0.5rem;
                font-size: smaller;
                border-bottom: 0.0625rem solid #444;
            }

        .product-catalog {
            background-color: #2b2b2b;
            border-radius: 0.5rem;
            font-size: smaller;
            margin-bottom: 10px;
            height: 200px;
            overflow-y: auto;
        }

            .product-catalog .row {
                padding-left: 0.5rem;
                padding-right: 0.5rem;
                font-size: smaller;
                border-bottom: 0.0625rem solid #444;
            }

        .fila-producto {
            transition: background 0.2s, color 0.2s, padding 0.2s;
            padding: 0 0.5rem;
        }

            .fila-producto.selected {
                background-color: #0d6efd !important;
                color: #fff;
                border-radius: 0.3125rem;
                margin-left: -0.5rem;
                margin-right: -0.5rem;
                width: calc(100% + 1rem);
                padding: 0 1rem;
            }

        .controlPedido-barra {
            background-color: #777777;
            border-radius: 0 0 5px 5px;
            margin-top: -15px;
            border-top: 1px solid #2b2b2b;
            min-height: 48px;
            height: 48px;
            display: flex;
            align-items: stretch;
        }

        .btn-modificar,
        .btn-eliminar {
            color: #fff;
            background: transparent;
            border: none;
            font-weight: normal;
            box-shadow: none;
            padding: 0 1rem;
            height: 100%;
            align-items: center;
            justify-content: center;
            transition: background 0.2s, color 0.2s;
            text-align: center;
            border-radius: 0 0 5px 5px;
        }

        .btn-modificar,
        .btn-eliminar {
            transition: background 0.2s, color 0.2s;
        }

            .btn-modificar:hover,
            .btn-modificar:focus,
            .btn-eliminar:hover,
            .btn-eliminar:focus {
                background: #495057;
                color: #fff;
            }

            .btn-modificar:active,
            .btn-eliminar:active {
                background: #212529;
                color: #fff;
            }

        .modal-cantidad {
            padding: 1.5rem;
        }

        @media (min-width: 600px) {
            .product-list .row {
                padding-left: 2.5rem;
                padding-right: 2.5rem;
            }
        }

        #ModalModificarCantidad .modal-dialog {
            max-width: 350px;
            width: 100%;
            margin: auto;
        }
    </style>
    <script>
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
            crearBarraControlPedidoLista(element);
        }

        function seleccionarProductoCatalogo(element) {
            if (element.classList.contains('selected')) {
                element.classList.remove('selected');
                document.getElementById('<%= hfProductoSeleccionado.ClientID %>').value = '';
                return;
            }
            document.querySelectorAll('.fila-producto-catalogo.selected').forEach(function (fila) {
                fila.classList.remove('selected');
            });
            element.classList.add('selected');
            document.getElementById('<%= hfProductoSeleccionado.ClientID %>').value = element.getAttribute('data-producto-id');
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="commands-container p-3">
        <div class="row mt-0 mb-2" style="text-align: center;">
            <h1 class="mb-2 mt-0" style="font-size: 1.8rem;">Consumos</h1>
        </div>
        <div class="estadoPedido pb-2 mb-3">
            <div class="row mb-2">
                <div class="col-4">
                    <h6>Mesa 1</h6>
                </div>
                <div class="col-8 text-end">
                    <h6>Wanchope Avila</h6>
                </div>
            </div>
            <div class="row m-0">
                <asp:DropDownList ID="ddlEstadoPedido" runat="server" CssClass="form-select bg-primary text-white text-center">
                    <asp:ListItem Text="Estado del Pedido" Value="" Disabled="true" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="Pendiente" Value="1"></asp:ListItem>
                    <asp:ListItem Text="En preparación" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Servido" Value="3"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="listaPedido pb-2">
            <div class="product-list">
                <!-- rptProductosLista -->
                <asp:Repeater ID="rptProductosLista" runat="server">
                    <HeaderTemplate>
                        <div class="row bg-dark table-dark pb-2">
                            <div class="col-6"><span>Producto</span></div>
                            <div class="col-2 text-end"><span>Un</span></div>
                            <div class="col text-end"><span>Subtotal</span></div>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="row pb-1 fila-producto fila-producto-lista"
                            data-producto-id='<%# Eval("Id") %>'
                            onclick="seleccionarProductoLista(this)">
                            <div class="col-7"><span><%# Eval("Nombre") %></span></div>
                            <div class="col-1 text-end"><span><%# Eval("Cantidad") %></span></div>
                            <div class="col text-end">
                                <span>
                                    <%# (Convert.ToInt32(Eval("Cantidad")) * Convert.ToDecimal(Eval("PrecioUnitario"))).ToString("N2") %>
                                </span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:HiddenField ID="hfProductoListaSeleccionado" runat="server" />
            </div>
        </div>
        <div class="controlPedido">
            <div id="controlPedidoBarraContainer" class="mb-0">
                <div id="controlPedidoBarra" class="controlPedido-barra align-items-center justify-content-between mb-3" style="display: none;">
                    <asp:Button ID="btnEliminarProducto" runat="server"
                        CssClass="btn btn-eliminar me-2"
                        Text="Eliminar"
                        OnClick="btnEliminarProducto_Click"
                        Style="display: none;" />
                    <asp:Button ID="btnModificarProducto" runat="server"
                        CssClass="btn btn-modificar me-2"
                        Text="Modificar"
                        OnClientClick="abrirModalModificarCantidad(); return false;"
                        Style="display: none;" />
                </div>
            </div>
            <div class="d-flex justify-content-end">
                <button type="button" class="btn btn-success w-100"
                    onclick="abrirModalAgregarProducto()" data-bs-toggle="modal" data-bs-target="#ModalComandas">
                    Agregar
                </button>
            </div>
            <div class="row pt-3 mb-4">
                <div class="col">
                    <h5>Total:</h5>
                </div>
                <div class="col text-end">
                    <h5><strong>
                        <asp:Label ID="lblTotal" runat="server" CssClass="fw-bold" /></strong></h5>
                </div>
            </div>
        </div>
        <div class="FinalizarPedido d-flex justify-content-between">
            <asp:Button ID="BtnVolver" runat="server" Text="Volver" CssClass="btn btn-secondary w-100" OnClick="BtnVolver_Click" />
        </div>
    </div>

    <!-- Modal para modificar cantidad -->
    <div class="modal fade" id="ModalModificarCantidad" tabindex="-1" aria-labelledby="modalModificarCantidadLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-cantidad">
            <div class="modal-content bg-dark text-white">
                <div class="modal-header border-0 justify-content-center pb-1">
                    <h4 class="modal-title" id="modalModificarCantidadLabel">Modificar</h4>
                </div>
                <div class="modal-body">
                    <div class="d-flex align-items-center justify-content-between mb-3">
                        <label class="me-2">Cantidad:</label>
                        <div class="input-group">
                            <button type="button" class="btn btn-primary" onclick="cambiarCantidad(-1)" tabindex="-1">-</button>
                            <span id="lblCantidadLista" class="form-control bg-primary text-white text-center" style="border: none;">1</span>
                            <button type="button" class="btn btn-primary" onclick="cambiarCantidad(1)" tabindex="-1">+</button>
                        </div>
                        <asp:HiddenField ID="hfNuevaCantidad" runat="server" />
                    </div>
                    <div class="d-flex justify-content-between">
                        <button class="btn btn-danger" data-bs-dismiss="modal" type="button">Cancelar</button>
                        <asp:Button ID="btnAceptarCantidad" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClientClick="return guardarCantidadYPostback();" OnClick="btnAceptarCantidad_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Productos -->
    <div class="modal fade" id="ModalComandas" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content bg-dark text-white">
                <div class="modal-header border-secondary justify-content-center pb-2">
                    <h1 class="modal-title">Productos</h1>
                </div>
                <div class="modal-body">
                    <asp:ScriptManager ID="smMain" runat="server" />
                    <div class="controlPedido">
                        <div class="row mb-3">
                            <div class="col-12">
                                <asp:DropDownList ID="ddlCategorias" runat="server" CssClass="form-select me-2 bg-primary text-white text-center" AutoPostBack="false" onchange="filterProductos()">
                                    <asp:ListItem Text="Todos" Value="" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row pb-3">
                            <div class="col-8">
                                <asp:TextBox ID="txtBusqueda" runat="server" CssClass="form-control me-2" Placeholder="Buscar..." AutoPostBack="false" onkeyup="filterProductos()" />
                            </div>
                            <div class="col-4 d-flex justify-content-end">
                                <button class="btn btn-secondary" type="button" onclick="clearFilters()">Limpiar</button>
                            </div>
                        </div>
                    </div>

                    <div class="listaPedido pb-2">
                        <div class="product-catalog">
                            <!-- rptProductos -->
                            <asp:Repeater ID="rptProductos" runat="server">
                                <HeaderTemplate>
                                    <div class="row bg-dark table-dark pb-2">
                                        <div class="col-6"><span>Producto</span></div>
                                        <div class="col-2 text-end"><span>Stock</span></div>
                                        <div class="col text-end"><span>PU</span></div>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="row pb-1 fila-producto fila-producto-catalogo"
                                        data-producto-id='<%# Eval("Articulo_Indice" )%>'
                                        data-cat-id='<%# Eval("Articulo_Categoria" )%>'
                                        onclick="seleccionarProductoCatalogo(this)">
                                        <div class="col-7"><span><%# Eval("Articulo_Nombre") %></span></div>
                                        <div class="col-1 text-end"><span><%# Eval("Articulo_Stock") %></span></div>
                                        <div class="col text-end"><span><span><%# Convert.ToDecimal(Eval("Articulo_Precio")).ToString("N2") %></span></div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:HiddenField ID="hfProductoSeleccionado" runat="server" />
                        </div>
                    </div>

                    <div class="row d-flex align-items-center mb-3">
                        <div class="col">
                            <label class="me-2">Cantidad:</label>
                        </div>
                        <div class="col">
                            <div class="input-group">
                                <button type="button" class="btn btn-primary" onclick="cambiarCantidadCatalogo(-1)" tabindex="-1">-</button>
                                <span id="lblCantidad" class="form-control bg-primary text-white text-center" style="border: none;">1</span>
                                <button type="button" class="btn btn-primary" onclick="cambiarCantidadCatalogo(1)" tabindex="-1">+</button>
                            </div>
                        </div>
                        <asp:HiddenField ID="hfCantidad" runat="server" />
                    </div>

                    <div class="d-flex justify-content-between">
                        <button class="btn btn-danger" data-bs-dismiss="modal" type="button">Volver</button>
                        <asp:Button ID="btnAgregarProducto" runat="server"
                            CssClass="btn btn-success"
                            Text="Agregar"
                            OnClientClick="guardarCantidadCatalogo();"
                            OnClick="btnAgregarProducto_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
