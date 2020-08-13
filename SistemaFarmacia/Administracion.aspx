<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFarmacia.Master" AutoEventWireup="true" CodeBehind="Administracion.aspx.cs" Inherits="SistemaFarmacia.Administracion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Css/Administración.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">
        window.onload = function () {
            $("#nuevoUsuario").hide();
            $("#divPerfiles").hide();
        };
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceMensaje" runat="server">
    <div id="sombraMensaje" class="sombra" runat="server">
        <div runat="server" id="divMensaje" class="divMensaje" visible="false">
            <div id="MCabecera">
                <label id="MTitulo">Mensaje</label>
            </div>
            <div id="MContenido">
                <asp:Label runat="server" ID="lblMensaje" CssClass="lblMensaje"></asp:Label>
                <br />
                <asp:Button runat="server" ID="MOk" CssClass="MBoton" Text="Aceptar" OnClick="MOk_Click" />
                <asp:Button runat="server" ID="btnOkSalir" CssClass="MBoton" Text="Aceptar" OnClick="btnOkSalir_Click" Visible="false" />
            </div>
        </div>
        <div runat="server" id="divFormularioG" class="FormGerente">
            <div id="FCabecera" class="FCabecera">
                <label id="FTitulo" class="FTitulo">Agregar usuario</label>
            </div>
            <div id="FContenido" class="FContenido">
                <br />
                <label class="FGEtiqueta">Usuario: </label>
                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtUsuario" CssClass="columna2 FGInput"></asp:TextBox>
                <br />
                <label class="FGEtiqueta">Nombre: </label>
                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtNombre" CssClass="FGColumna2 FGInput"></asp:TextBox>
                <br />
                <label class="FGEtiqueta">Apellido Paterno: </label>
                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtApellidoP" CssClass="FGColumna2 FGInput"></asp:TextBox>
                <br />
                <label class="FGEtiqueta">Apellido Materno: </label>
                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtApellidoM" CssClass="FGColumna2 FGInput"></asp:TextBox>
                <br />
                <label class="FGEtiqueta">Perfil: </label>
                <asp:DropDownList runat="server" ID="ddlPerfil" DataValueField="Id_perfil" DataTextField="Descripcion" CssClass="FGColumna2 FGInput" />
                <br />
                <label class="FGEtiqueta">Contraseña: </label>
                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtContrasenia" CssClass="FGColumna2 FGInput" TextMode="Password"></asp:TextBox>
                <asp:Label runat="server" ID="lblId_usuario" Visible="false"></asp:Label>
                <br />
                <label class="FGEtiqueta">Correo electrónico: </label>
                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtCorreoE" CssClass="FGColumna2 FGInput"></asp:TextBox>
                <br />
                <label class="FGEtiqueta">Pregunta de seguridad: </label>
                <asp:DropDownList runat="server" ID="ddlPreguntaS" DataTextField="Pregunta" CssClass="FGColumna2 FGInput"> 
                    <asp:ListItem Text="Lugar de nacimiento de la madre" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Mejor amigo de la infancia" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Nombre de la primera mascota" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Profesor favorito" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Personaje histórico favorito" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Lugar donde conociste a tu pareja" Value="6"></asp:ListItem>
                </asp:DropDownList>
                <br />
                <label class="FGEtiqueta">Respuesta: </label>
                <asp:TextBox onkeypress="return DisableEnterKey(event);" MaxLength="50" runat="server" ID="txtRespuestaS" CssClass="FGColumna2 FGInput"></asp:TextBox>
                <br />
                <br />
                <div id="FBotonera">
                    <asp:Button runat="server" ID="FGCancelar" CssClass="FGBoton" Text="Cancelar" OnClick="FGCancelar_Click" />
                    <asp:Button runat="server" ID="FGAgregar" CssClass="FGBoton" Text="Agregar" OnClick="FGAgregar_Click" />
                    <asp:Button runat="server" ID="FGActualizar" CssClass="FGBoton" Text="Actualizar" OnClick="FGActualizar_Click" />
                </div>
            </div>
        </div>
        <div runat="server" id="divFormularioP" class="FormPerfil">
            <div id="FCabeceraP" class="FCabecera">
                <label id="FTituloP" class="FTitulo">Agregar Perfil</label>
            </div>
            <div id="FContenidoP" class="FContenido">
                <br />
                <table>
                    <tr>
                        <td class="tdColumn1" ><label class="FGEtiqueta">Perfil: </label></td> 
                        <td><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtPerfil" CssClass="columna2 FGInput divCheck"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdColumn1" ><label class="FGEtiqueta">Clientes:</label></td>
                        <td><%--<label class="FGEtiqueta">Permisos   </label>--%>
                            <div class="FGInput FGContenedorTerm divCheck">
                                <asp:CheckBoxList runat="server"  id="CHKLCliente" CssClass="chbFG" RepeatColumns="4">
                                    <asp:ListItem Value="14">Ver</asp:ListItem>
                                    <asp:ListItem Value="11">Nuevo</asp:ListItem>
                                    <asp:ListItem Value="12">Actualizar</asp:ListItem>
                                    <asp:ListItem Value="13">Eliminar</asp:ListItem>
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdColumn1" ><label class="FGEtiqueta">Clientes del mes:</label></td>
                        <td><asp:CheckBox runat="server" ID="chkClienteMes" CssClass="chkPrimer" /></td>
                    </tr>
                    <tr>
                        <td class="tdColumn1" ><label class="FGEtiqueta">Administracion:</label></td>
                        <td>
                            <div class="FGInput FGContenedorTerm divCheck">
                                <asp:Label runat="server" id="lblUsuario" CssClass="lblDivisor1">Usuario</asp:Label>
                                <asp:CheckBoxList runat="server"  id="chkPerforUsuario" CssClass="chbFG" RepeatColumns="4">
                                    <asp:ListItem Value="31">Ver</asp:ListItem>
                                    <asp:ListItem Value="32">Nuevo</asp:ListItem>
                                    <asp:ListItem Value="33">Actualizar</asp:ListItem>
                                    <asp:ListItem Value="34">Eliminar</asp:ListItem>
                                </asp:CheckBoxList>
                                <br />
                                <asp:Label runat="server" id="lblPerfiles" CssClass="lblDivisor2">Perfiles</asp:Label>
                                <asp:CheckBoxList runat="server"  id="chkPerforPerfiles" CssClass="chbFG" RepeatColumns="4">
                                    <asp:ListItem Value="35">Ver</asp:ListItem>
                                    <asp:ListItem Value="36">Nuevo</asp:ListItem>
                                    <asp:ListItem Value="37">Actualizar</asp:ListItem>
                                    <asp:ListItem Value="38">Eliminar</asp:ListItem>
                                </asp:CheckBoxList>
                                <br />
                                <asp:Label runat="server" id="Label1" CssClass="lblDivisor2">Correo</asp:Label>
                                <asp:CheckBoxList runat="server"  id="chkPerforCorreo" CssClass="chbFG" RepeatColumns="3">
                                    <asp:ListItem Value="39">Técnico</asp:ListItem>
                                    <asp:ListItem Value="310">Configuración</asp:ListItem>
                                    <asp:ListItem Value="311">Contenido</asp:ListItem>
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:Label runat="server" ID="lblId_perfil" Visible="false"></asp:Label>
                <br />
                <div id="FBotonera2">
                    <asp:Button runat="server" ID="FPCancelar" CssClass="FGBoton" Text="Cancelar" OnClick="FPCancelar_Click" />
                    <asp:Button runat="server" ID="FPAgregar" CssClass="FGBoton" Text="Agregar" OnClick="FPAgregar_Click" />
                    <asp:Button runat="server" ID="FPActualizar" CssClass="FGBoton" Text="Actualizar" OnClick="FPActualizar_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="mySidenav" class="sidenav">
        <asp:Button runat="server" ID ="btnOpcionUsuario" OnClick="btnOpcionUsuario_Click" Text="Usuarios"/>
        <br />
        <asp:Button runat="server" ID ="btnOpcionPerfil" OnClick="btnOpcionPerfil_Click" Text="Perfiles"/>
        <br />
        <asp:Button runat="server" ID ="btnOpcionCorreo" OnClick="btnOpcionCorreo_Click" Text="Correo" Visible="true"/>
        <asp:Button runat="server" ID ="btnConfiguracionTecnica" OnClick="btnConfiguracionTecnica_Click" Text="Configuración técnica" Visible="false" CssClass="subOpcion"/>
        <asp:Button runat="server" ID ="btnConfiguracionCorreo" OnClick="btnConfiguracionCorreo_Click" Text="Configuración de correo" Visible="false" CssClass="subOpcion"/>
        <asp:Button runat="server" ID ="btnContenidCorreo" OnClick="btnContenidCorreo_Click" Text="Contenido de correo" Visible="false" CssClass="subOpcion"/>
        <br />
        <asp:Button runat="server" ID ="btnRegresar" OnClick="btnRegresar_Click" Text="Regresar" Visible="false"/>
    </div>
    <div class="main">
        <div runat="server" id="divGerentes" class="divContenido" visible="true">
            <div id="divGerentes1">
                <asp:Button runat="server" ID="btnAgrUsuarioG" Text="Agregar usuario" OnClick="btnAgrUsuarioG_Click" CssClass="btnAgrUsuarioG" />
                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtBusquedaG" placeholder="Usuario" CssClass="txtBusquedaG" OnTextChanged="txtBusquedaG_TextChanged" AutoPostBack="true"></asp:TextBox>
                <asp:Image runat="server" ID="imgBusquedaG" ImageUrl="~/Imagenes/imgBusqueda.png" CssClass="imgBusquedaG" />
            </div>
            <div id="divGerentes2">
                <asp:GridView ID="gvGerentes" runat="server" HeaderStyle-BackColor="#5b9bd5" ShowHeader="true" AutoGenerateColumns="false"
                    CssClass="gridview" CellPadding="5" Width="100%" OnRowDataBound="gvGerentes_RowDataBound" OnRowEditing="gvGerentes_RowEditing" OnRowCancelingEdit="gvGerentes_RowCancelingEdit"
                    OnRowUpdated="gvGerentes_RowUpdated" OnRowDeleted="gvGerentes_RowDeleted" OnRowDeleting="gvGerentes_RowDeleting">
                    <AlternatingRowStyle BackColor="#f2f2f2" />
                    <RowStyle BackColor="#FFFFFF" />
                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblIdUsuario' CssClass="lblUsuario" Text='<%# Bind("ID_USUARIO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Usuario" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblUsuario' CssClass="lblUsuario" Text='<%# Bind("Usuario") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblNombre' CssClass="lblGerentes" Text='<%# Bind("Nombre") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido Paterno" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblApellidoP' CssClass="lblGerentes" Text='<%# Bind("Apellido_paterno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido Materno" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblApellidoM' CssClass="lblGerentes" Text='<%# Bind("Apellido_materno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Perfil" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblPerfil' CssClass="lblGerentes" Text='<%# Bind("Perfil") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" HeaderText="Opciones" HeaderStyle-Width="15%" ShowCancelButton="true" />

                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div runat="server" id="divPerfiles" class="divContenido" visible="false">
            <div id="divPerfiles1">
                <asp:Button runat="server" ID="btnAgregarP" Text="Agregar perfil" OnClick="btnAgregarP_Click" CssClass="btnAgrUsuarioG" />
                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtBusquedaP" placeholder="Perfil" CssClass="txtBusquedaG" OnTextChanged="txtBusquedaP_TextChanged" AutoPostBack="true"></asp:TextBox>
                <asp:Image runat="server" ID="Image1" ImageUrl="~/Imagenes/imgBusqueda.png" CssClass="imgBusquedaG" />
            </div>
            <div id="Perfiles2">
                <asp:GridView ID="gvPerfiles" runat="server" HeaderStyle-BackColor="#5b9bd5" ShowHeader="true" AutoGenerateColumns="false"
                    CssClass="gridview" CellPadding="5" Width="100%" OnRowDataBound="gvPerfiles_RowDataBound" OnRowEditing="gvPerfiles_RowEditing" OnRowCancelingEdit="gvPerfiles_RowCancelingEdit"
                    OnRowUpdated="gvPerfiles_RowUpdated" OnRowDeleted="gvPerfiles_RowDeleted" OnRowDeleting="gvPerfiles_RowDeleting" OnRowUpdating="gvPerfiles_RowUpdating">
                    <AlternatingRowStyle BackColor="#f2f2f2" />
                    <RowStyle BackColor="#FFFFFF" />
                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblIdPerfil' CssClass="lblUsuario" Text='<%# Bind("ID_PERFIL") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Perfil" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblDescripcion' CssClass="lblUsuario" Text='<%# Bind("DESCRIPCION") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblPermisos' CssClass="lblUsuario" Text='<%# Bind("PERMISOS") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" HeaderText="Opciones" HeaderStyle-Width="15%" ShowCancelButton="true" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div runat="server" id="divCorreo" class="divContenido" visible="false">
            <div id="divCorreo1">
                <asp:Button runat="server" ID="btnActCorreoTecn" Text="Actualizar" OnClick="btnActCorreoTecn_Click" CssClass="btnAgrUsuarioG" />
                <asp:Button runat="server" ID="btnActCorreoConf" Text="Actualizar" OnClick="btnActCorreoConf_Click" CssClass="btnAgrUsuarioG" />
                <asp:Button runat="server" ID="btnActCorreoCont" Text="Actualizar" OnClick="btnActCorreoCont_Click" CssClass="btnAgrUsuarioG" />
            </div>
            <div id="divCorreo2">
                <div runat="server" id="divCorreoConfTec">
                    <div class="divCorreoTitulo"><h5>Configuración técnica</h5></div>
                    <asp:Label runat="server" CssClass="lblTituloParametro"></asp:Label>
                    <table class="tblParametros">
                         <tr>
                            <td style="width: 15%;"><label>Host:</label></td>
                            <td style="width: 20%;"><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtHost" CssClass="ParamInput"></asp:TextBox></td>
                            <td style="width: 10%;"></td>
                            <td style="width: 15%;"><label>Puerto:</label></td>
                            <td style="width: 20%;"><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtPuerto" CssClass="ParamInput" TextMode="Number"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td><label>SSL:</label></td>
                            <td><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtSSL" CssClass="ParamInput" ></asp:TextBox></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="width: 15%;"><label>Cuenta de correo base:</label></td>
                            <td style="width: 20%;"><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtCorreo" CssClass="ParamInput" TextMode="Email"></asp:TextBox></td>
                            <td style="width: 10%;"></td>
                            <td style="width: 15%;"><label>Contraseña:</label></td>
                            <td style="width: 20%;"><asp:TextBox type="password" onkeypress="return DisableEnterKey(event);" runat="server" ID="txtCorreoContraseña" CssClass="ParamInput" ></asp:TextBox></td>
                        </tr>
                    </table>
                <br />
                </div>
                <div runat="server" id="divCorreoConfCorreo">
                    <div class="divCorreoTitulo"><h5>Configuración de correo</h5></div>
                    <table class="tblParametros">
                        <tr>
                            <td style="width: 20%;"><label>Enviar antes de inicio de mes:</label></td>
                            <td style="width: 30%;"><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtDiasAntes" CssClass="ParamInput" TextMode="Number"></asp:TextBox></td>
                            <td style="width: 10%;"></td>
                            <td style="width: 10%;"><label>Enviar correo a:</label></td>
                            <td style="width: 20%;">
                                <div class="ml-1 mt-1 text-center">
                                <asp:DropDownList runat="server" ID="ddlEnvCorreo" CssClass="ddlEnvCorreo">
                                    <asp:ListItem Value="1,0">Todos</asp:ListItem>
                                    <asp:ListItem Value="0">Inactivos</asp:ListItem>
                                    <asp:ListItem Value="1">Activos</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td><label>Correo de prueba:</label></td>
                            <td><asp:TextBox runat="server" ID="txtCorreoPrueba" CssClass="ParamInput" ></asp:TextBox></td>
                            <td></td>
                            <td><label>Pruebas:</label></td>
                            <td><asp:CheckBox runat="server" ID="chkPruebas" CssClass="chkPruebas" ></asp:CheckBox></td>
                        </tr>
                        <tr>
                            <td><label>Ultimo correo:</label></td>
                            <td><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtUltimoC" CssClass="ParamInput" Enabled="false"></asp:TextBox></td>
                        </tr>
                    </table>
                <br />
                </div>
                <div runat="server" id="divCorreoCont">
                    <div class="divCorreoTitulo"><h5>Contenido del correo</h5></div>
                    <table class="tblParametros">
                        <tr>
                            <td style="width: 10%;"><label>Asunto:</label></td>
                            <td><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtSujeto" CssClass="ParamInput"></asp:TextBox></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td><label>Mensaje:</label></td>
                            <td><%--<asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtMensaje" CssClass="ParamInput"></asp:TextBox>--%>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtMensaje" CssClass="ParamInput" TextMode="multiline" Columns="50" Rows="5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><label>Imagen correo:</label></td>
                            <td><%--<asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtImagen" CssClass="ParamInput" TextMode="Url" ></asp:TextBox>--%>
                                <asp:FileUpload runat="server" ID="uploadImgCorreo" CssClass="ParamInput"  accept="image/jpeg" />
                                <asp:Label runat="server" ID="lblErrorImage" CssClass="lblError"></asp:Label>
                            </td>
                        </tr>
                    
                        <tr>
                            <td><label>Firma:</label></td>
                            <td><asp:TextBox runat="server" ID="txtFirma" CssClass="ParamInput" TextMode="MultiLine"></asp:TextBox></td>
                        </tr>
                    </table>
                    <br />
                </div>
            </div>
        </div>
    </div>
</asp:Content>