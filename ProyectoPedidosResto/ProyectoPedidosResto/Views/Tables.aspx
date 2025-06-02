<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Tables.aspx.cs" Inherits="ProyectoPedidosResto.Views.Tables" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="ContentTables" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .filtros {
            background-color: #0d6efd;
            padding: 1rem;
        }

        .mesa-card {
            aspect-ratio: 1 / 1;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            border-radius: 0.5rem;
        }

            .mesa-card .card-body {
                flex-grow: 1;
                display: flex;
                flex-direction: column;
                justify-content: center;
                align-items: center;
                text-align: center;
            }

        .row.g-2 {
            display: flex;
            flex-wrap: wrap;
        }

            .row.g-2 > [class^="col"] {
                display: flex;
            }
    </style>

    <div class="tables-container">
        <div class="filtros">
            <div class="row justify-content-center">
                <div class="col-7 col-md-6 mb-2 mb-md-2 d-flex justify-content-center align-items-center">
                    <asp:DropDownList ID="ddlFiltros" runat="server" CssClass="form-select w-100 w-md-auto" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltros_SelectedIndexChanged" />
                </div>

                <div class="col-5 col-md-3 mb-2 mb-md-2 d-flex justify-content-center align-items-center">
                    <asp:CheckBox ID="chkMisMesas" runat="server" ClientIDMode="Static" OnCheckedChanged="chkMisMesas_CheckedChanged" />
                    <label for="chkMisMesas" class="form-check-label" style="font-size: smaller; margin: .5rem;">Mis mesas</label>
                </div>
            </div>

            <div class="row justify-content-center">
                <div class="col-7 col-md-6 d-flex justify-content-center align-items-center">
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control w-100 w-md-auto" Placeholder="Buscar..." AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged" />
                </div>
                <div class="col-5 col-md-3 d-flex justify-content-center align-items-center">
                    <asp:Button ID="btnLimpiar" runat="server" CssClass="btn btn-secondary" Text="Limpiar" OnClick="btnLimpiar_Click" />
                </div>
            </div>
        </div>



        <div class="mesas">
            <div class="container mt-3">
                <div class="row row-cols-2 g-2">
                    <%--<div class="col d-flex">
                        <div class="card text-bg-success mesa-card w-100">
                            <div class="card-header text-center">Libre</div>
                            <div class="card-body">
                                <h5 class="card-title">MESA 1</h5>
                                <p class="card-text"></p>
                            </div>
                            <div class="card-footer">
                                <asp:Button Text="Cargar Mesa" runat="server" CssClass="btn btn-light" data-bs-toggle="modal" data-bs-target="#Modal" ID="BtnCargarMesa" OnClick="BtnCargarMesa_Click" />
                            </div>
                        </div>
                    </div>


                    <div class="col d-flex">

                        <div class="card text-bg-danger mesa-card w-100">
                            <div class="card-header text-center">Ocupada</div>
                            <div class="card-body">
                                <h5 class="card-title">Mesa 2</h5>
                                <p class="card-text">Atendidos por: Maxi</p>
                            </div>
                            <div class="card-footer">
                                <asp:Button Text="Cargar Mesa" runat="server" CssClass="btn btn-light" ID="Button1" OnClick="BtnCargarMesa_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="col d-flex">

                        <div class="card text-bg-success mesa-card w-100">
                            <div class="card-header text-center">Libre</div>
                            <div class="card-body">
                                <h5 class="card-title">MESA 1</h5>
                                <p class="card-text"></p>
                            </div>
                            <div class="card-footer">
                                <asp:Button Text="Cargar Mesa" runat="server" CssClass="btn btn-light" ID="Button7" OnClick="BtnCargarMesa_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="col d-flex">

                        <div class="card text-bg-warning mesa-card w-100">
                            <div class="card-header text-center">Reservada</div>
                            <div class="card-body">
                                <h5 class="card-title">MESA 3</h5>
                                <p class="card-text">Reserva para 3.</p>
                            </div>
                            <div class="card-footer">
                                <asp:Button Text="Cargar Mesa" runat="server" CssClass="btn btn-light" ID="Button2" OnClick="BtnCargarMesa_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="col d-flex">

                        <div class="card text-bg-warning mesa-card w-100">
                            <div class="card-header text-center">Reservada</div>
                            <div class="card-body">
                                <h5 class="card-title">MESA 3</h5>
                                <p class="card-text">Reserva para 3.</p>
                            </div>
                            <div class="card-footer">
                                <asp:Button Text="Cargar Mesa" runat="server" CssClass="btn btn-light" ID="Button3" OnClick="BtnCargarMesa_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="col d-flex">

                        <div class="card text-bg-success mesa-card w-100">
                            <div class="card-header text-center">Libre</div>
                            <div class="card-body">
                                <h5 class="card-title">MESA 1</h5>
                                <p class="card-text"></p>
                            </div>
                            <div class="card-footer">
                                <asp:Button Text="Cargar Mesa" runat="server" CssClass="btn btn-light" ID="Button6" OnClick="BtnCargarMesa_Click" />
                            </div>
                        </div>
                    </div>--%>


                    <div class="col d-flex">

                        <div class="card text-bg-warning mesa-card w-100">
                            <div class="card-header text-center">Reservada</div>
                            <div class="card-body">
                                <h5 class="card-title">MESA 3</h5>
                                <p class="card-text">Reserva para 3.</p>
                            </div>
                            <div class="card-footer">
                                <button type="button" class="btn btn-light" data-bs-toggle="modal" data-bs-target="#staticBackdrop">
                                    Cargar Mesa
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <%--Modal--%>
            <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content bg-dark text-white">
                        <div class="modal-header border-secondary">
                            <h5 class="modal-title" id="exampleModalLabel">Mesa 1</h5>
                            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <div class="mb-3">
                                <label class="form-label">Seleccione mozo</label>
                                <select class="form-select bg-primary text-white">
                                    <option selected>Seleccione mozo</option>
                                    <option value="1">Mozo 1</option>
                                    <option value="2">Mozo 2</option>
                                </select>
                            </div>

                            <div class="mb-3 d-flex align-items-center justify-content-between">
                                <label class="form-label me-3">Personas:</label>
                                <div class="input-group" style="max-width: 160px;">
                                    <button class="btn btn-danger" type="button">-</button>
                                    <input type="text" class="form-control text-center" value="2">
                                    <button class="btn btn-success" type="button">+</button>
                                </div>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Observaciones:</label>
                                <textarea class="form-control bg-black text-white" rows="4">Puto el que lee</textarea>
                            </div>
                        </div>
                        <div class="modal-footer d-flex justify-content-between">
                            <button type="button" class="btn btn-danger flex-fill me-2">Cancelar</button>
                            <button type="button" class="btn btn-success flex-fill me-2">Aceptar</button>
                            <button type="button" class="btn btn-warning flex-fill">Tomar Comanda</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
