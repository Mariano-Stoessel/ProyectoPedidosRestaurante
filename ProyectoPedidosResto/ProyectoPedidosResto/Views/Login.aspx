<%@ Page Title="" Language="C#" MasterPageFile="~/Views/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProyectoPedidosResto.Views.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .login-container {
            display: flex;
            justify-content: center;
            height: 65vh;
            margin: 4rem;
        }

        .text-tiny {
            font-size: 0.8rem;
            line-height: 1.1rem !important;
            display: block !important;
            margin-bottom: 0 !important;
            padding: 1rem !important;
        }
    </style>
</asp:Content>

<asp:Content ID="ContentLogin" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="login-container">
        <div class="text-center" style="width: 300px;">
            <h1 class="text-primary fw-bold mb-5">Iniciar sesión</h1>

            <asp:DropDownList ID="ddlEmpresas" runat="server" CssClass="form-control mb-2" OnSelectedIndexChanged="ddlEmpresas_SelectedIndexChanged">
                <asp:ListItem Text="Seleccione una empresa" Value="" Disabled="true" Selected="true"></asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control mb-2" Placeholder="Usuario" required="required"></asp:TextBox>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control mb-3" Placeholder="Contraseña" required="required"></asp:TextBox>
            <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary w-100" Text="Login" OnClick="btnLogin_Click" />
            <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger text-tiny" Text=""></asp:Label>
        </div>
    </div>
</asp:Content>
