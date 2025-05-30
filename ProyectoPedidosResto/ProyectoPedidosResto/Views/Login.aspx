<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProyectoPedidosResto.Views.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="d-flex justify-content-center align-items-center vh-100">
        <div class="text-center" style="width: 300px;">
            <h2 class="text-primary fw-bold mb-4">Iniciar sesión</h2>

            <asp:TextBox ID="txtEmpresa" runat="server" CssClass="form-control mb-2" Placeholder="Empresa"></asp:TextBox>
            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control mb-2" Placeholder="Usuario"></asp:TextBox>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control mb-3" Placeholder="Contraseña"></asp:TextBox>

            <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary w-100" Text="Login" />

            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
        </div>
    </div>
</asp:Content>
