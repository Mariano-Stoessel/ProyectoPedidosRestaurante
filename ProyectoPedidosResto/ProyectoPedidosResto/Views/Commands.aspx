<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Commands.aspx.cs" Inherits="ProyectoPedidosResto.Views.Commands" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .btn-finalize {
            background-color: #2980b9;
            color: white;
        }

        .product-list {
            margin: 0.5rem auto;
            background-color: #2b2b2b;
            padding: 0.5rem;
            border-radius: 5px;
            font-size: smaller;
            margin-bottom: 10px;
            height: 200px;
            overflow-y: auto;
        }

        .product-item {
            padding: 5px 0;
            font-size: smaller;
            border-bottom: 1px solid #444;
        }

        select, input {
            background-color: #2b2b2b !important;
            color: white !important;
            border: 1px solid #555 !important;
        }
    </style>
    <div class="commands-container p-3">
        <div class="row" style="text-align: center;">
            <h1>Consumos</h1>
        </div>
        <div class="estadoPedido pb-2">
            <div class="row mb-2">
                <div class="col-4">
                    <h5>Mesa 1</h5>
                </div>
                <div class="col-8 text-end">
                    <strong>Wanchope Avila</strong>
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
                <div class="row bg-dark pb-2">
                    <div class="col-6">
                        <span>Producto</span>
                    </div>
                    <div class="col-2 text-end">
                        <span>Un</span>
                    </div>
                    <div class="col text-end">
                        <span>Subtotal</span>
                    </div>
                </div>
                <div class="product-item">
                    <div class="row pb-1">
                        <div class="col-6">
                            <span>Coca cola 1.5 L</span>
                        </div>
                        <div class="col-2 text-end">
                            <span>2</span>
                        </div>
                        <div class="col text-end">
                            <span>$10.000,00</span>
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-6">
                            <span>Milanesa c/ guarnición</span>
                        </div>
                        <div class="col-2 text-end">
                            <span>1</span>
                        </div>
                        <div class="col text-end">
                            <span>$15.000,00</span>
                        </div>
                    </div>
                    <div class="row pb-1">
                        <div class="col-6">
                            <span>Sorrentinos Promo</span>
                        </div>
                        <div class="col-2 text-end">
                            <span>1</span>
                        </div>
                        <div class="col text-end">
                            <span>$20.000,00</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="controlPedido pb-2">
            <div class="row pb-2">
                <div class="col-6">
                    <button class="btn btn-danger">Eliminar</button>
                </div>
                <div class="col-6">
                    <div class="input-group">
                        <button type="button" class="btn btn-primary">- </button>
                        <span id="lblCantidad" class="form-control text-center bg-primary text-white border-0">1</span>
                        <button type="button" class="btn btn-primary">+ </button>
                    </div>
                </div>
            </div>
            <div class="row m-0">
                <button type="button" class="btn btn-success" data-bs-toggle="modal" data-bs-target="#ModalComandas">Agregar</button>
            </div>
            <div class="row pt-3">
                <div class="col">
                    <h5>Total:</h5>
                </div>
                <div class="col text-end">
                    <h5><strong>$45.000,00</strong></h5>
                </div>
            </div>
        </div>
        <div class="FinalizarPedido d-flex justify-content-between">
            <asp:Button ID="BtnVolver" runat="server" Text="Volver" Class="btn btn-secondary" OnClick="BtnVolver_Click" />

            <button class="btn btn-primary">Finalizar</button>
        </div>

        <!-- Modal Productos -->
        <div class="modal fade" id="ModalComandas" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content bg-dark text-white">
                    <div class="modal-header border-0">
                        <h5 class="modal-title" id="modalProductosLabel">Productos</h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                    </div>
                    <div class="modal-body">
                        <div class="mb-3 d-flex">
                            <select class="form-select me-2">
                                <option selected>Categoría</option>
                                <option value="1">Bebidas</option>
                                <option value="2">Comidas</option>
                            </select>
                            <button class="btn btn-secondary me-2" type="button">Limpiar</button>
                        </div>
                        <div class="mb-3 d-flex">
                            <input type="text" class="form-control me-2" placeholder="Buscar producto...">
                            <button class="btn btn-primary me-2" type="button">Buscar</button>

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
    </div>


</asp:Content>
