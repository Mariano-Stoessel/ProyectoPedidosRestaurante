<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Commands.aspx.cs" Inherits="ProyectoPedidosResto.Views.Commands" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        main {
            flex: 1;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px 0;
        }

        .container-box {
            width: 100%;
            max-width: 400px;
            padding: 20px;
            background-color: #2b2b2b;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
        }

        .table-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 10px;
        }

        .btn-delete {
            background-color: #c0392b;
            color: white;
        }

        .btn-add {
            background-color: #27ae60;
            color: white;
        }

        .btn-finalize {
            background-color: #2980b9;
            color: white;
        }

        .product-list {
            background-color: black;
            padding: 10px;
            border-radius: 5px;
            margin-bottom: 10px;
            height: 200px;
            overflow-y: auto;
        }

        .product-item {
            display: flex;
            justify-content: space-between;
            padding: 5px 0;
            border-bottom: 1px solid #444;
        }

        select, input {
            background-color: #2b2b2b !important;
            color: white !important;
            border: 1px solid #555 !important;
        }
    </style>
    <div class="Comandas">

        <main>
            <div class="container-box">
                <div class="table-header">
                    <h5>Mesa 1</h5>

                    <div class="form-check">
                        <input class="form-check-input" type="checkbox" id="consumos">
                        <label class="form-check-label" for="consumos">Consumos</label>
                    </div>
                </div>

                <div class="mb-3">
                    <label for="estadoPedido" class="form-label">Estado de pedido</label>
                    <select class="form-select" id="estadoPedido">
                        <option selected>Estado de pedido</option>
                        <option value="1">Pendiente</option>
                        <option value="2">En preparación</option>
                        <option value="3">Servido</option>
                    </select>
                </div>

                <p class="mb-2">Mozo a cargo: <strong>Wanchope Avila</strong></p>

                <div class="d-flex justify-content-between mb-2">
                    <button class="btn btn-delete">Eliminar</button>
                    <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#ModalComandas" >Agregar</button>
                </div>

                <div class="product-list">
                    <div class="product-item">
                        <span>Coca cola 1.5 L (2)</span>
                        <span>$10.000,00</span>
                    </div>
                    <div class="product-item">
                        <span>Milanesa c/ guarnición (1)</span>
                        <span>$15.000,00</span>
                    </div>
                    <div class="product-item">
                        <span>Sorrentinos Promo (1)</span>
                        <span>$20.000,00</span>
                    </div>
                </div>

                <div class="d-flex align-items-center mb-3">
                    <label for="cantidad" class="me-2">Cantidad:</label>
                    <input type="number" class="form-control w-25 me-2" id="cantidad" value="1" min="1">
                </div>

                <h5 class="text-end mb-3">Total: <strong>$45.000,00</strong></h5>

                <div class="d-flex justify-content-between">
                    <button class="btn btn-secondary">Cancelar</button>
                    <asp:Button ID="btnFinalizar" runat="server" Text="Finalizar" CssClass="btn btn-success" />
                </div>
            </div>
        </main>

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
                            <button class="btn btn-primary" type="button">Buscar</button>
                        </div>

                        <input type="text" class="form-control mb-3" placeholder="Buscar producto...">

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
                                    <tr >
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
                            <button class="btn btn-danger" data-bs-dismiss="modal" type="button">Cancelar</button>
                            <button class="btn btn-success" type="button">Agregar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
