<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFarmacia.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SistemaFarmacia.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link href="Css/Login.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            

        });

        
        ///Version 1.36.2 Prod


        function ModalUpdate(titulo, MSG) {
            var update = $('#alertas');
            update.fadeIn('2000')
             $('#headA > h5 > strong').html(titulo);
            $('#bodyA > p').html(MSG);

            var Close = $('#CerrarUpdate');
            Close.click(function () {
                $(update).fadeOut("2000");
            });
        }

        function ModalOlvide() {
            var modalOlvide = $('#ModalOlvide')
            modalOlvide.fadeIn('2000');

            var CerrarModal = $('#cancelar')
            CerrarModal.click(function () {
                $(modalOlvide).fadeOut("1000");
            });

        }


        function LoginError() {

            var LogError = $('#LoginError')
            LogError.fadeIn('2000');

            var CerrarError = $('#cerrarError')

            CerrarError.click(function () {
                $(LogError).fadeOut('2000');
            })
        }





    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    
    <div class="container">
        <div class="row justify-content-center pt-5 mt-5 mr-1">
            <div class="col-md-5 formulario">
                <%--<form class="formInicio" >--%>
                    <div class="form-group text-center pt-3">
                        <h1 class="tex-light">Iniciar Sesión</h1>
                    </div>
                    <div class="input-group mb-4">
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="basic-addon1"><i class="fa fa-user-circle" aria-hidden="true"></i></span>
                        </div>
                        <asp:TextBox type="text" runat="server" ID="Usuario" class="form-control" placeholder="Usuario" aria-label="Usuario" aria-describedby="basic-addon1"></asp:TextBox>
                    </div>
                    <div class="input-group mb-4">
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="basic-addon"><i class="fa fa-lock" aria-hidden="true"></i></span>
                        </div>
                        <asp:TextBox type="text" runat="server" class="form-control" ID="Pass" TextMode="Password" placeholder="Contraseña" aria-label="Contraseña" aria-describedby="basic-addon1"></asp:TextBox>
                    </div>
                    <div class="pt-3">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Button runat="server" ID="btnIngresar" type="submit" class="btnlogin " CssClass="btn btn-block ingresar" Text="Ingresar" OnClick="btnIngresar_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>


                    <div class="form-group mx-sm-4 pt-3 text-center reactive">
                        <button type="button" class="btn btn-link text-white" onclick="ModalOlvide()">Olvidé mi contraseña</button>
                    </div>
                    <%--</form>--%>
            </div>
        </div>
    </div>


    <%--  FORMULARIO  MODAL OLVIDE MI CONTRASEÑA    --%>
    <div class="modal" tabindex="-1" role="dialog" id="ModalOlvide">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center cabecero">
                    <h5 class="modal-title text-center"><strong>Recuperación de Contraseña</strong></h5>
                </div>
                <div class="modal-body">
                    <form>
                        <div class="form-group">
                            <label for="usuario">Usuario:</label>
                            <asp:TextBox runat="server" type="text" class="form-control" id="usuarioModal"></asp:TextBox>
                                    
                        </div>
                        <div class="form-group">
                            <label for="nombre">Nombre:</label>
                              <asp:TextBox runat="server"  type="text" class="form-control" id="nombreModal"></asp:TextBox>                                              
                        </div>
                        <div class="form-group">
                            <label for="apellido">Primer Apellido:</label>
                            <asp:TextBox runat="server" type="text" class="form-control" id="apellidoModal" ></asp:TextBox>    
                        </div>
                        <div class="form-group">
                            <label for="apellido2">Segundo Apellido:</label>
                            <asp:TextBox  runat="server" type="text" class="form-control" id="apellidoModal2" ></asp:TextBox>    
                        </div>
                        <div class="form-group">
                            <label for="exampleInputPassword1">Nueva Contraseña:</label>
                            <asp:TextBox runat="server" type="password" class="form-control" id="exampleInputPassword1"></asp:TextBox>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger btn-lg" id="cancelar" data-dismiss="modal"><i class="fa fa-times" aria-hidden="true"></i></button>
                    <button type="button" class="btn btn-primary btn-lg" id="continuar" onclick="<%=btnCrearPass.ClientID%>.click();" ><i class="fa fa-check" aria-hidden="true"></i></button>
                    <asp:Button runat="server" ID="btnCrearPass" Style="visibility:hidden;"  Text="Button" OnClick="btnCrearPass_Click"/>
                </div>
            </div>
        </div>
    </div>

    <%-- MODAL MENSAJE DE ERROR--%>

    <div class="modal" tabindex="-1" role="dialog" id="LoginError">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <h5 class="modal-title"><strong>Error de Autentificación</strong></h5>
                </div>
                <div class="modal-body">
                    <p>Usuario y/o Contraseña Incorrecta.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" id="cerrarError" data-dismiss="modal"><i class="fa fa-times" aria-hidden="true"></i></button>
                </div>
            </div>
        </div>
    </div>

   <%-- MODAL ALERT DE ACTUALIZACION--%>
    <div class="modal" tabindex="-1" role="dialog" id="alertas">
        <div class="modal-dialog" role="document">
            <div class="modal-content headA">
                <div class="modal-header text-center " id="headA">
                    <h5 class="modal-title"><strong></strong></h5>
                </div>
                <div class="modal-body" id="bodyA">
                    <p></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" id="CerrarUpdate" data-dismiss="modal"><i class="fa fa-times" aria-hidden="true"></i></button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
