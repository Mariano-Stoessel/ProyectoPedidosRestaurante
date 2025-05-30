<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Tables.aspx.cs" Inherits="ProyectoPedidosResto.Views.Tables" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="ContentTables" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="tables-container">
        <div class="filtros">
            <div class="row mb-2">
                <div class="col-6 col-md-6 mb-2 mb-md-0">
                    <asp:DropDownList ID="ddlFiltros" runat="server" CssClass="form-select" AutoPostBack="true">
                        <asp:ListItem Text="Todos" Value="Todos" Selected="True" />
                        <asp:ListItem Text="Disponible" Value="Disponible" />
                        <asp:ListItem Text="Reservado" Value="Reservado" />
                        <asp:ListItem Text="Ocupado" Value="Ocupado" />
                    </asp:DropDownList>
                </div>

                <div class="col-6 col-md-3 mb-2 mb-md-0">
                    <asp:CheckBox ID="chkMisMesas" runat="server" CssClass="form-check-input me-1" />
                    <label class="form-check-label" for="chkMisMesas">Mis mesas</label>
                </div>

                <div class="col-12 col-md-3">
                    <asp:Button ID="btnLimpiar" runat="server" CssClass="btn btn-secondary w-100" Text="Limpiar" />
                </div>
            </div>

            <div class="row">
                <div class="col-12 col-md-8 mb-2 mb-md-0">
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" Placeholder="Buscar..." />
                </div>
                <div class="col-12 col-md-4">
                    <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary w-100" Text="Buscar" />
                </div>
            </div>
        </div>

        <div class="mesas">

        </div>

    </div>
</asp:Content>
