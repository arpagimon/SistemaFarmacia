<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFarmacia.Master" AutoEventWireup="true" CodeBehind="Principal.aspx.cs" Inherits="SistemaFarmacia.MenuPrincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Css/Principal.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">
        $('#iconoClientes').click(function () {
            alert("Entro");
        });
        //document.getElementById('iconoClientes').addEventListener("click", function () { window.location.replace("/ConsultarCliente.aspx") }, false);
        //document.getElementById('iconoAdministracion').addEventListener("click", function () { window.location.replace("/Administracion.aspx") }, false);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <table class="row justify-content-center pt-5 mt-5 mr-1 align-items-center text-center c">
        <tr>
            <td>
                <div class="align-items-center text-center Icono" id="iconoClientes" runat="server">
                    <asp:ImageButton runat="server" ID="Clientes" PostBackUrl="~/ConsultarCliente.aspx" ImageUrl="~/Imagenes/CLIENTE.png" OnClick="Clientes_Click" CssClass="BotonMenu " />
                    <br />
                    <h5 class="text-dark" style="background: #ddddff; border-radius: 21px; padding: 2px 10px; vertical-align: middle;">Clientes</h5>
                </div>
            </td>
            <td>
                <div class="align-items-center text-center Icono" id="iconoClientesMes" runat="server">
                    <asp:ImageButton runat="server" ID="ClientesMes" PostBackUrl="~/ConsultarClienteMes.aspx" ImageUrl="~/Imagenes/cumple.png" OnClick="ClientesMes_Click" CssClass="BotonMenu " />
                    <br />
                    <h5 class="text-dark" style="background: #ddddff; border-radius: 21px; padding: 2px 10px; vertical-align: middle;">Cumpleañeros</h5>
                </div>
            </td>
            <td>
                <div class="align-items-center text-center Icono" id="iconoAdministracion" runat="server">
                    <asp:ImageButton runat="server" ID="Administración" PostBackUrl="~/Administracion.aspx" ImageUrl="~/Imagenes/Administracion.png" OnClick="Administración_Click" CssClass="BotonMenu" />
                    <br />
                    <h5 class="text-dark" style="background: #ddddff; border-radius: 21px; padding: 2px 10px; vertical-align: middle;">Administración</h5>
                </div>
            </td>
        </tr>
    </table>

</asp:Content>
