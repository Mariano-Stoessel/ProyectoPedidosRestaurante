<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Commands.aspx.cs" Inherits="ProyectoPedidosResto.Views.Commands" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <style>
        .commands-container {
            padding-top: 0 !important;
        }

        .product-list {
            margin: 0.5rem auto;
            background-color: #2b2b2b;
            border-radius: 0.5rem;
            font-size: smaller;
            margin-bottom: 10px;
            height: 200px;
            overflow-y: auto;
        }

        .product-item {
            padding: 0.3125rem 0;
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
            padding: 2rem;
        }
    </style>
    <script>
        function seleccionarProducto(element) {
            if (element.classList.contains('selected')) {
                element.classList.remove('selected');
                document.getElementById('productoSeleccionado').value = '';
                eliminarBarraControlPedido();
                return;
            }
            document.querySelectorAll('.fila-producto.selected').forEach(function (fila) {
                fila.classList.remove('selected');
            });
            element.classList.add('selected');
            document.getElementById('productoSeleccionado').value = element.getAttribute('data-producto-id');
            var spans = element.querySelectorAll('span');
            var cantidad = spans.length > 1 ? spans[1].textContent.trim() : "1";
            crearBarraControlPedido(cantidad);
        }

        function crearBarraControlPedido(cantidad) {
            var container = document.getElementById('controlPedidoBarraContainer');
            if (!container) return;
            var barraAnterior = document.getElementById('controlPedidoBarra');
            if (barraAnterior) barraAnterior.parentNode.removeChild(barraAnterior);

            var barra = document.createElement('div');
            barra.id = 'controlPedidoBarra';
            barra.className = 'controlPedido-barra d-flex align-items-center justify-content-between mb-3';

            barra.innerHTML = `
                <button class="btn btn-eliminar me-2" type="button" onclick="eliminarProductoSeleccionado()">
                    Eliminar
                </button>
                <button class="btn btn-modificar me-2" type="button" onclick="abrirModalModificarCantidad()">
                    Modificar
                </button>
            `;
            container.appendChild(barra);
        }

        function eliminarBarraControlPedido() {
            var barra = document.getElementById('controlPedidoBarra');
            if (barra && barra.parentNode) {
                barra.parentNode.removeChild(barra);
            }
        }

        function abrirModalModificarCantidad() {
            var fila = document.querySelector('.fila-producto.selected');
            if (fila) {
                var spans = fila.querySelectorAll('span');
                var cantidad = spans.length > 1 ? spans[1].textContent.trim() : "1";
                document.getElementById('nuevaCantidad').value = cantidad;
                $('#ModalModificarCantidad').modal('show');
            }
        }

        function guardarCantidadYPostback() {
            var valor = document.getElementById('nuevaCantidad').value;
            document.getElementById('<%= hfNuevaCantidad.ClientID %>').value = valor;
            // Permite el postback
            return true;
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
                <asp:Repeater ID="rptProductos" runat="server">
                    <HeaderTemplate>
                        <div class="row bg-dark table-dark pb-2">
                            <div class="col-6"><span>Producto</span></div>
                            <div class="col-2 text-end"><span>Un</span></div>
                            <div class="col text-end"><span>Subtotal</span></div>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="product-item fila-producto"
                            data-producto-id='<%# Eval("Id") %>'
                            onclick="seleccionarProducto(this)">
                            <div class="row pb-1">
                                <div class="col-7"><span><%# Eval("Nombre") %></span></div>
                                <div class="col-1 text-end"><span><%# Eval("Cantidad") %></span></div>
                                <div class="col text-end">
                                    <span>
                                        <%# (Convert.ToInt32(Eval("Cantidad")) * Convert.ToDecimal(Eval("PrecioUnitario"))).ToString("N2") %>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <input type="hidden" id="productoSeleccionado" name="productoSeleccionado" value="" />
            </div>
        </div>
        <div class="controlPedido">
            <div id="controlPedidoBarraContainer" class="mb-0"></div>
            <div class="d-flex justify-content-end">
                <button type="button" class="btn btn-success w-100" data-bs-toggle="modal" data-bs-target="#ModalComandas">Agregar</button>
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
                    <h4 class="modal-title" id="modalModificarCantidadLabel">Modificar cantidad</h4>
                </div>
                <div class="modal-body">
                    <div class="d-flex align-items-center justify-content-between mb-3">
                        <label for="nuevaCantidad" class="me-2">Cantidad:</label>
                        <input type="number" class="form-control ms-3" style="width: 80px;" id="nuevaCantidad" value="1" min="1">
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
                <div class="modal-header border-0 justify-content-center pb-1">
                    <h3 class="modal-title" id="modalProductosLabel">Productos</h3>
                </div>
                <div class="modal-body">
                    <div class="row mb-3">
                        <div class="col-8">
                            <select class="form-select me-2">
                                <option selected>Categoría</option>
                                <option value="1">Bebidas</option>
                                <option value="2">Comidas</option>
                            </select>
                        </div>
                        <div class="col-4 d-flex justify-content-end">
                            <button class="btn btn-secondary me-2 w-100" type="button">Limpiar</button>
                        </div>
                    </div>
                    <div class="row pb-3">
                        <div class="col-8">
                            <input type="text" class="form-control me-2" placeholder="Buscar producto...">
                        </div>
                        <div class="col-4 d-flex justify-content-end">
                            <button class="btn btn-primary me-2 w-100" type="button">Buscar</button>
                        </div>
                    </div>


                    <div class="table-responsive mb-3" style="max-height: 200px; overflow-y: auto;">
                        <table class="table table-dark table-bordered table-hover text-white">
                            <thead>
                                <tr>
                                    <th>Producto</th>
                                    <th>Stock</th>
                                    <th>Unitario</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Coca cola 1.5 L</td>
                                    <td>122</td>
                                    <td>$5000,00</td>
                                </tr>
                                <tr>
                                    <td>Fanta 1.5 L</td>
                                    <td>100</td>
                                    <td>$4500,00</td>
                                </tr>
                                <tr>
                                    <td>Flan c/ dulce de leche</td>
                                    <td>24</td>
                                    <td>$8000,00</td>
                                </tr>
                                <tr>
                                    <td>Milanesa c/ guarnición</td>
                                    <td>12</td>
                                    <td>$15000,00</td>
                                </tr>
                                <tr>
                                    <td>Sorrentinos Promo</td>
                                    <td>9</td>
                                    <td>$20000,00</td>
                                </tr>
                            </tbody>

                        </table>
                    </div>

                    <div class="d-flex align-items-center mb-3">
                        <label for="cantidadModal" class="me-2">Cantidad:</label>
                        <input type="number" class="form-control w-25 me-2" id="cantidadModal" value="2" min="1">
                    </div>

                    <div class="d-flex justify-content-between">
                        <button class="btn btn-danger" data-bs-dismiss="modal" type="button">Volver</button>
                        <button class="btn btn-success" data-bs-dismiss="modal" type="button">Agregar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
