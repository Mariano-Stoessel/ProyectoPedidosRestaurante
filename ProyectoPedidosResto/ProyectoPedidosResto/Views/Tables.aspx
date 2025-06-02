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
                <div class="row row-cols-2 row-cols-md-3 row-cols-lg-4 g-2">
                    <% foreach (var mesa in Mesas)
                        {
                            string colorClase = "";
                            string estadoTexto = "";

                            switch (mesa.Estado)
                            {
                                case "libre":
                                    colorClase = "text-bg-success";
                                    estadoTexto = "Libre";
                                    break;
                                case "reservado":
                                    colorClase = "text-bg-warning";
                                    estadoTexto = "Reservada";
                                    break;
                                case "ocupado":
                                    colorClase = "text-bg-danger";
                                    estadoTexto = "Ocupada";
                                    break;
                                default:
                                    colorClase = "bg-secondary";
                                    estadoTexto = "Estado Desconocido";
                                    break;
                            }
                            string modalId = "modalMesa" + mesa.Numero;
                    %>
                    <div class="col mb-3">
                        <div class="card <%= colorClase %> w-100 text-center d-flex flex-column align-items-center mesa-card">
                            <div class="card-header">MESA <%= mesa.Numero %></div>
                            <div class="card-body">
                                <h5 class="card-title"><%= estadoTexto %></h5>
                                <% if (mesa.Estado == "ocupado")
                                    { %>
                                <p class="card-text"><%= mesa.NombrePersona %></p>
                                <% }
                                    else if (mesa.Estado == "reservado")
                                    { %>
                                <p class="card-text">X personas</p>
                                <% }
                                    else
                                    { %>
                                <p class="card-text">Para asignar</p>
                                <% } %>
                            </div>
                            <div class="card-footer w-100 d-flex justify-content-center">
                                <% if (mesa.Estado == "libre" || mesa.Estado == "reservado")
                                    { %>
                                <button type="button" class="btn btn-primary"
                                    data-bs-toggle="modal"
                                    data-bs-target="#<%= modalId %>">
                                    <%= mesa.Estado == "libre" ? "Cargar" : "Cargar" %>
                                </button>
                                <% }
                                    else if (mesa.Estado == "ocupado")
                                    { %>
                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Comanda" CommandName="VerComanda" CommandArgument='<% mesa.Numero %>' OnCommand="Comanda_Command" />
                                <% } %>
                            </div>
                        </div>
                    </div>

                    <% if (mesa.Estado == "libre" || mesa.Estado == "reservado")
                        { %>
                    <div class="modal fade" id="<%= modalId %>" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="label<%= modalId %>" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-centered">
                            <div class="modal-content bg-dark text-white">
                                <div class="modal-header border-secondary">
                                    <h5 class="modal-title" id="label<%= modalId %>">Mesa <%= mesa.Numero %></h5>
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
                                            <button class="btn btn-primary" type="button">-</button>
                                            <input type="text" class="form-control text-center bg-primary text-white border-0" value="1">
                                            <button class="btn btn-primary" type="button">+</button>
                                        </div>
                                    </div>
                                    <div class="mb-3">
                                        <label class="form-label">Observaciones:</label>
                                        <textarea class="form-control bg-black text-white" rows="4"></textarea>
                                    </div>
                                </div>
                                <div class="modal-footer d-flex justify-content-between">
                                    <button type="button" class="btn btn-danger flex-fill">Cancelar</button>
                                    <button type="button" class="btn btn-success flex-fill">Aceptar</button>
                                    <a href="Commands.aspx" class="btn btn-primary flex-fill">Tomar comanda</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <% } %>


                    <% } %>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
