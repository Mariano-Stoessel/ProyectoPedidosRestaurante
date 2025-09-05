<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Commands.aspx.cs" Inherits="ProyectoPedidosResto.Views.Commands" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/Content/Commands.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var ddlCategoriasId = '<%= ddlCategorias.ClientID %>';
        var txtBusquedaId = '<%= txtBusqueda.ClientID %>';
        var hfProductoListaSeleccionadoId = '<%= hfProductoListaSeleccionado.ClientID %>';
        var hfArticuloNombreoListaSeleccionadoId = '<%= hfArticuloNombreoListaSeleccionado.ClientID %>';
        var hfProductoSeleccionadoId = '<%= hfProductoSeleccionado.ClientID %>';
        var hfNombreProductoSeleccionadoId = '<%= hfNombreProductoSeleccionado.ClientID %>';
        var hfPrecioProductoSeleccionadoId = '<%= hfPrecioProductoSeleccionado.ClientID %>';
        var btnEliminarProductoId = '<%= btnEliminarProducto.ClientID %>';
        var btnModificarProductoId = '<%= btnModificarProducto.ClientID %>';
        var ddlEstadoId = '<%= ddlEstado.ClientID %>';
        var hfNuevaCantidadId = '<%= hfNuevaCantidad.ClientID %>';
        var hfCantidadId = '<%= hfCantidad.ClientID %>';
        var ddlEstadoId = '<%= ddlEstado.ClientID %>';
        
    </script>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <script src="<%= ResolveUrl("~/Scripts/Commands.js") %>"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="commands-container p-3">
        <div class="row mt-0 mb-2" style="text-align: center;">
            <h1 class="mb-2 mt-0" style="font-size: 1.8rem;">Consumos</h1>
        </div>
        <div class="estadoPedido pb-2 mb-3">
            <div class="row mb-2">
                <div class="col-4">
                    <div class="mb-2">
                        <strong>Mesa:</strong>
                        <asp:Label ID="lblIdMesa" runat="server" CssClass="text-white"></asp:Label>
                    </div>
                </div>
                <div class="col-8 text-end">
                    <asp:Label ID="lblMozo" runat="server" CssClass="text-white small" Text="Mozo: "></asp:Label>
                </div>
            </div>
            <div class="row m-0">
                <button type="button" class="btn btn-primary text-white text-center" data-bs-toggle="modal" data-bs-target="#ModalCambiarMesa">Cambiar de mesa</button>
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
                        <div class='row pb-1 fila-producto fila-producto-lista <%# Eval("Com_Estado").ToString() == "PREPARACION" ? "bg-warning" : Eval("Com_Estado").ToString() == "ENTREGADO" ? "bg-success" : "" %>'
                            data-producto-id='<%# Eval("Com_Indice") %>'
                            data-nombre-articulo='<%# Eval("ArticuloNombre") %>'
                            data-estado='<%# Eval("Com_Estado") %>'
                            onclick="seleccionarProductoLista(this)">
                            <div class="col-7"><span><%# Eval("ArticuloNombre") %></span></div>
                            <div class="col-1 text-end"><span><%# Eval("Com_Cant") %></span></div>
                            <div class="col text-end">
                                <span>
                                    <%# (Convert.ToInt32(Eval("Com_Cant")) * Convert.ToDecimal(Eval("Com_Unitario"))).ToString("N2") %>
                                </span>
                            </div>
                        </div>

                    </ItemTemplate>
                </asp:Repeater>
                <asp:HiddenField ID="hfProductoListaSeleccionado" runat="server" />
                <asp:HiddenField ID="hfArticuloNombreoListaSeleccionado" runat="server" />
            </div>
        </div>
        <div class="controlPedido">
            <div id="controlPedidoBarraContainer" class="mb-0">
                <div id="controlPedidoBarra" class="controlPedido-barra align-items-center justify-content-between mb-3" style="display: none;">
                    <button type="button" class="btn btn-eliminar me-2" data-bs-toggle="modal" data-bs-target="#ModalEliminarArticulo">Eliminar</button>
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

    <!-- Modal para cambiar mesa -->
    <div class="modal fade" id="ModalCambiarMesa" tabindex="-1" aria-labelledby="modalCambiarMesaLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-cantidad">
            <div class="modal-content bg-dark text-white">
                <div class="modal-header border-0 justify-content-center pb-1">
                    <h4 class="modal-title" id="modalCambiarMesaLabel">Cambiar Mesa</h4>
                </div>
                <div class="modal-body">
                    <div class="row d-flex align-items-center justify-content-between mb-3">
                        <label class="me-2">Mesas:</label>
                        <div class="input-group">
                            <asp:DropDownList ID="ddlMesasLibres" runat="server" class="form-control bg-primary text-white text-center" Style="border: none;" />
                        </div>
                    </div>
                    <div class="d-flex justify-content-between">
                        <button class="btn btn-danger" data-bs-dismiss="modal" type="button">Cancelar</button>
                        <asp:Button ID="btnAceptarCambiarMesa" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptarCambiarMesa_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal para modificar -->
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
                    <div class="d-flex align-items-center justify-content-between mb-3">
                        <label class="me-2">Estado:</label>
                        <div class="input-group">
                            <asp:DropDownList ID="ddlEstado" runat="server" class="form-control bg-primary text-white text-center" Style="border: none;">
                                <asp:ListItem Text="PEDIDO" />
                                <asp:ListItem Text="PREPARACION" />
                                <asp:ListItem Text="ENTREGADO" />
                            </asp:DropDownList>
                        </div>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                    </div>
                    <div class="d-flex justify-content-between">
                        <button class="btn btn-danger" data-bs-dismiss="modal" type="button">Cancelar</button>
                        <asp:Button ID="btnAceptarCantidad" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClientClick="return guardarCantidadYPostback();" OnClick="btnModificarProducto_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal para Eliminar Articulo -->
    <div class="modal fade" id="ModalEliminarArticulo" tabindex="-1" aria-labelledby="modalModificarCantidadLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-cantidad">
            <div class="modal-content bg-dark text-white">
                <div class="modal-header border-0 justify-content-center pb-1">
                    <h4 class="modal-title" id="modalEliminarArticuloLabel">Eliminar</h4>
                </div>
                <div class="modal-body">
                    <div>
                        <p class="modal-eliminar-texto text-center">¿Eliminar este artículo?</p>
                    </div>
                    <div class="d-flex justify-content-between">
                        <button class="btn btn-danger" data-bs-dismiss="modal" type="button">Cancelar</button>
                        <asp:Button ID="btnEliminarProducto" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnEliminarProducto_Click" Style="display: none;" />

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
                                <asp:DropDownList ID="ddlCategorias" runat="server" CssClass="form-select me-2 bg-primary text-white text-center" AutoPostBack="false"
                                    onchange="filterProductos('<%= txtBusqueda.ClientID %>', '<%= ddlCategorias.ClientID %>')">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row pb-3">
                            <div class="col-8">
                                <asp:TextBox ID="txtBusqueda" runat="server" CssClass="form-control me-2" Placeholder="Buscar..." AutoPostBack="false"
                                    onkeyup="filterProductos('<%= txtBusqueda.ClientID %>', '<%= ddlCategorias.ClientID %>')" />
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
                                        data-producto-nombre='<%# Eval("Articulo_Nombre")%>'
                                        data-precio-producto='<%# Eval("Articulo_Precio")%>'
                                        onclick="seleccionarProductoCatalogo(this)">
                                        <div class="col-6"><span><%# Eval("Articulo_Nombre") %></span></div>
                                        <div class="col-2 text-end"><span><%# Eval("Articulo_Stock") %></span></div>
                                        <div class="col text-end"><span><span><%# Convert.ToDecimal(Eval("Articulo_Precio")).ToString("N2") %></span></div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:HiddenField ID="hfProductoSeleccionado" runat="server" />
                            <asp:HiddenField ID="hfNombreProductoSeleccionado" runat="server" />
                            <asp:HiddenField ID="hfPrecioProductoSeleccionado" runat="server" />
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
                        <asp:Button ID="btnAgregarProducto" runat="server" CssClass="btn btn-success" Text="Agregar" OnClientClick="guardarCantidadCatalogo();" OnClick="btnAgregarProducto_Click" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
