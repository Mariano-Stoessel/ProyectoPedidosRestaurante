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

    <script>
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
                document.getElementById('<%= hfMesaId.ClientID %>').value = mesaId;

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
    </script>

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

                            switch (mesa.Mesa_Estado)
                            {
                                case "LIBRE":
                                    colorClase = "text-bg-success";
                                    estadoTexto = "Libre";
                                    break;
                                case "RESERVADO":
                                    colorClase = "text-bg-warning";
                                    estadoTexto = "Reservado";
                                    break;
                                case "OCUPADA":
                                    colorClase = "text-bg-danger";
                                    estadoTexto = "Ocupada";
                                    break;
                                default:
                                    colorClase = "bg-secondary";
                                    estadoTexto = "Estado Desconocido";
                                    break;
                            }
                            string modalId = "modalMesa" + mesa.Mesa_Id;
                    %>
                    <div class="col mb-3">
                        <div class="card <%= colorClase %> w-100 text-center d-flex flex-column align-items-center mesa-card">
                            <div class="card-header">MESA <%= mesa.Mesa_Id %></div>
                            <div class="card-body">
                                <h5 class="card-title"><%= estadoTexto %></h5>
                                <% if (mesa.Mesa_Estado == "OCUPADA")
                                    { %>
                                <p class="card-text"><%= mesa.Mesa_Mozo %></p>
                                <% }
                                    else if (mesa.Mesa_Estado == "RESERVADO")
                                    { %>
                                <p class="card-text">X personas</p>
                                <% }
                                    else
                                    { %>
                                <p class="card-text">Para asignar</p>
                                <% } %>
                            </div>
                            <div class="card-footer w-100 d-flex justify-content-center">
                                <% if (mesa.Mesa_Estado == "LIBRE" || mesa.Mesa_Estado == "RESERVADO")
                                    { %>
                                <button type="button" class="btn btn-primary"
                                    data-bs-toggle="modal"
                                    data-bs-target="#modalMesaGeneral"
                                    data-mesa-id="<%= mesa.Mesa_Id %>"
                                    data-mesa-estado="<%= mesa.Mesa_Estado %>"
                                    <% if (mesa.Mesa_Estado == "RESERVADO")
                                    { %>
                                    data-personas="<%= mesa.Mesa_CantPer %>"
                                    data-mozo-id="<%= mesa.Mesa_IdMozo %>"
                                    <% } %>>
                                    Cargar
                                </button>
                                <% }
                                    else if (mesa.Mesa_Estado == "OCUPADA")
                                    { %>
                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Comanda" CommandName="VerComanda" CommandArgument='<% mesa.Numero %>' OnCommand="Comanda_Command" />
                                <% } %>
                            </div>
                        </div>
                    </div>
                    <% } %>
                </div>
            </div>
        </div>
        <!-- Modal Mesa -->
        <div class="modal fade" id="modalMesaGeneral" tabindex="-1" aria-labelledby="modalMesaGeneralLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content bg-dark text-white">
                    <div class="modal-header border-secondary justify-content-center">
                        <h1 class="modal-title" id="modalMesaGeneralLabel">Mesa <span id="modalMesaNumero"></span></h1>
                        <asp:HiddenField ID="hfMesaId" runat="server" />
                    </div>
                    <div class="modal-body">
                        <div class="mb-3">
                            <asp:DropDownList ID="ddlMozos" runat="server" CssClass="form-select bg-primary text-white text-center"></asp:DropDownList>
                        </div>
                        <div class="mb-3 d-flex align-items-center justify-content-between">
                            <label class="form-label me-3">Personas:</label>

                            <div class="input-group" style="max-width: 160px;">
                                <button type="button" class="btn btn-primary"
                                    onclick="modificarPersonas('lblPersonas', 'hfPersonas', -1)">
                                    -
                                </button>
                                <span id="lblPersonas" class="form-control text-center bg-primary text-white border-0">1</span>
                                <button type="button" class="btn btn-primary"
                                    onclick="modificarPersonas('lblPersonas', 'hfPersonas', 1)">
                                    +
                                </button>
                                <input type="hidden" name="hfPersonas<%# Eval("Mesa_Id") %>" id="hfPersonas<%# Eval("Mesa_Id") %>" />
                            </div>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">Observaciones:</label>
                            <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control bg-black text-white" TextMode="MultiLine" Rows="4"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer d-flex justify-content-between">
                        <button type="button" class="btn btn-danger flex-fill" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button ID="btnAceptarMesa" runat="server" CssClass="btn btn-success flex-fill" Text="Aceptar" UseSubmitBehavior="false" OnClick="btnAceptarMesa_Click" />
                        <a href="Commands.aspx" class="btn btn-primary flex-fill">Tomar comanda</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
