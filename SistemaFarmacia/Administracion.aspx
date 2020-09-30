<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFarmacia.Master" AutoEventWireup="true" CodeBehind="Administracion.aspx.cs" Inherits="SistemaFarmacia.Administracion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Css/Administración.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">
        window.onload = function () {
            $("#nuevoUsuario").hide();
            $("#divPerfiles").hide();
        };

        function abrePreview() {
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : window.screenX;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : window.screenY;

            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

            var w = 900;

            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = dualScreenTop;
            //window.open('PreviewCorreo.aspx', 'Correo previo', 'location=no, toolbar=no, Top=0, Left=0');
            var windowPreview = window.open('PreviewCorreo.aspx', 'Correo previo', 'location=no, toolbar=no, width=' + w + ', height=' + height + ' Top=' + top + ', Left=' + left);

            if (window.focus) {
                windowPreview.focus();
            }
        }



        function mostrarmensajejs() {
            try {
                $("#<%=divConfirmacionCorreo.ClientID%>").hide();
            } catch (error) { }
            
            try{
                $("#<%=divGpoClienteTipoSelec.ClientID%>").hide();
            } catch (error) { }

            try {
                $("#<%=divMsjConfirmaEnvio.ClientID%>").hide();
            } catch (error) { }

            try {
                $("#<%=MContenidoJS.ClientID%>").show();
            } catch (error) { }
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceMensaje" runat="server">
    <div id="sombraMensaje" class="sombra" runat="server">
        <%--<asp:Label runat="server" ID="lblErrorGeneral" CssClass="oculto"></asp:Label>--%>
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
        <div runat="server" id="divmensaje2" class="divMensaje" visible="false">
            <div id="MCabecera2">
                <label id="MPreguntar" class="verificacion">¡Alerta!</label>
            </div>
            <div id="MContenido2">
                <asp:Label runat="server" ID="lblPregunta" CssClass="lblMensaje lblMensajeB0">¿Está seguro que desea reactivar el registro del cliente </asp:Label><br /><asp:Label runat="server" ID="lblPregunta2" CssClass="lblMensaje lblMensajeT0"></asp:Label>
                <br />
                <asp:TextBox runat="server" ID="recupera" Visible="false" AutoComplete="off" />
                <asp:Button runat="server" ID="BCancelar" CssClass="MBoton" Text="Cancelar" OnClick="BCancelar_Click" Visible="false" />
                <asp:Button runat="server" OnClick="BAceptar_Click" CssClass="MBoton" Visible="false" Text="Reactivar" ID="BAceptar" />
            </div>
        </div>
        <div runat="server" id="divFormularioG" class="FormGerente" visible="false">
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
                <label class="FGEtiqueta">Doctor: </label>
                <asp:DropDownList runat="server" ID="ddlDoctor" CssClass="FGInput">
                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Sí" Value="1"></asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
                <div id="FBotonera">
                    <asp:Button runat="server" ID="FGCancelar" CssClass="FGBoton" Text="Cancelar" OnClick="FGCancelar_Click" />
                    <asp:Button runat="server" ID="FGAgregar" CssClass="FGBoton" Text="Agregar" OnClick="FGAgregar_Click" />
                    <asp:Button runat="server" ID="FGActualizar" CssClass="FGBoton" Text="Actualizar" OnClick="FGActualizar_Click" />
                </div>
            </div>
        </div>
        <div runat="server" id="divFormularioP" class="FormPerfil" visible="false">
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
                                    
                                    <asp:ListItem Value="315">Configuración correo recordatorio</asp:ListItem>
                                    <asp:ListItem Value="312">Tipo Correo de promociones</asp:ListItem>
                                    <asp:ListItem Value="316">Envio de promociones</asp:ListItem>
                                </asp:CheckBoxList>
                                <asp:CheckBoxList runat="server"  id="chkAdmin" CssClass="chbFG" RepeatColumns="1">
                                    <asp:ListItem Value="313">Reactivar clientes</asp:ListItem>
                                    <asp:ListItem Value="317">Configuración citas</asp:ListItem>
                                    <asp:ListItem Value="318">Grupo clientes</asp:ListItem>
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdColumn1" ><label class="FGEtiqueta">Citas:</label></td>
                        <td><asp:CheckBox runat="server" ID="chkCitas" CssClass="chkPrimer" /></td>
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
        <div runat="server" id="divFormCliente" class="FormClienteCorreo" visible="false">
            <div id="divFormCliCabecera" class="FCabecera">
                <label id="lblFormCliTitulo" class="FTitulo">Buscar cliente</label>
            </div>
            <asp:Panel runat="server" ID="panelMsj">
                <div id="FormCliContenido" class="FContenido FCCContenido">
                    <div class="row justify-content-start">
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Nombre</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtFormCliNombre" CssClass="form-control " AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Apellido Paterno</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtFormCliApellidoP" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Apellido Materno</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtFormCliApellidoM" CssClass="form-control " AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-center">
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Fecha Ingreso</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFormCliFechaI" CssClass="form-control InpGrande" TextMode="Date" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Fecha Nacimiento</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFormCliFechaN" CssClass="form-control InpGrande" TextMode="Date" AutoComplete="off" ></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Edad</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFormCliEdad" CssClass="form-control-edad" AutoComplete="off" TextMode="Number" ></asp:TextBox>
                                <asp:Label runat="server" id="lblFormCliA" Visible="false" CssClass="lblA"> a </asp:Label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliEdad2" CssClass="form-control-edad" AutoComplete="off" TextMode="Number" Visible="false"></asp:TextBox>
                                <asp:CheckBox runat="server" ID="chkFormCliRango" OnCheckedChanged="chkFormCliRango_CheckedChanged" Visible="false" AutoPostBack="true" />
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-start">
                        <div class="col-3 px-2">
                            <div class="form-group">
                                <label>Telefono Fijo</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFormCliTelFijo" CssClass="form-control InpMediano" AutoComplete="off" TextMode="Phone"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-2 px-2">
                            <div class="form-group">
                                <label>Ext.</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFormCliExtension" CssClass="form-control InpExt" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-3 px-2">
                            <div class="form-group">
                                <label>Celular</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFormCliCelular" CssClass="form-control InpMediano " AutoComplete="off" TextMode="Phone"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Medio</label>
                                <asp:DropDownList runat="server" ID="ddlFormCliMedio" CssClass="form-control InpGrande">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-start">
                        <div class="col px-2">
                            <div class="form-group">
                                <label>Enviar Email</label>
                                <asp:DropDownList runat="server" CssClass="form-control InpChico" ID="ddlFormCliEnviarCorreo">
                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col px-2">
                            <div class="form-group">
                                <label>Email</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFormCliEmail" CssClass="form-control InpExtGrande" AutoComplete="off" TextMode="Email"></asp:TextBox>
                            </div>
                        </div>
                        
                        <!--Ingresar ID Para Factura-->
                        <div class="col px-2">
                            <div class="form-group">
                                <label>Factura:</label>
                                <asp:DropDownList runat="server" CssClass="form-control InpChico" ID="ddlFormCliFactura">
                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-start">
                        <div class="col px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Estado:</label>
                                <asp:DropDownList OnSelectedIndexChanged="ddlFormCliEstado_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control " ID="ddlFormCliEstado">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col px-2">
                            <div class="form-group">
                                <div runat="server" id="divFormCliMunicipio" class="divCelda">
                                    <label class="FGEtiqueta">Municipio: </label>
                                    <asp:DropDownList runat="server" CssClass="form-control " ID="ddlFormCliMunicipio" Enabled="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div runat="server" id="divFormCliPais" visible="false">
                                    <label class="FGEtiqueta">Pais: </label>
                                    <asp:DropDownList runat="server" CssClass="form-control InpGrande" ID="ddlFormCliPais">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col px-2">
                            <div class="form-group">
                                <label>Estatus</label>
                                <asp:DropDownList runat="server" ID="ddlFormCliEstatus" CssClass="form-control InpMediano">
                                    <asp:ListItem Text="Activo" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Inactivo" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-start" runat="server" visible="false">
                        <div class="col px-2">
                            <div class="form-group" runat="server" id="divFormCliObservacionesNota">
                                <label for="exampleFormControlTextarea1">Observaciones</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtFormCliObservaciones" CssClass="form-control" AutoComplete="off" TextMode="MultiLine">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-start" runat="server" visible="false">
                        <div class="col-12 px-2">
                            <div class="form-group">
                                <label for="exampleFormControlTextarea1">Notas</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtFormCliNota" CssClass="form-control" AutoComplete="off" TextMode="MultiLine"></asp:TextBox>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFormCliIdCliente" CssClass="txtObsNota" Visible="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-center">
                        <div class="col-6 px-2">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblFormCliError" CssClass="lblError"></asp:Label>
                            </div>
                        </div>
                    </div>
                    
                    <br/>
                    <div class="row justify-content-center">
                        <div class="col px-2 text-center FBotonera">
                            <asp:Button runat="server" ID="btnFormCliBuscar" CssClass="FGBoton FCCBoton" Text="Buscar" OnClick="btnFormCliBuscar_Click"/>
                            <asp:Button runat="server" ID="btnFormCliLimpiar" CssClass="FGBoton FCCBoton" Text="Limpiar" OnClick="btnFormCliLimpiar_Click"/>
                            <asp:Button runat="server" ID="btnFormCliCerrar" CssClass="FGBoton FCCBoton" Text="Cerrar" OnClick="btnFormCliCerrar_Click"/>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div runat="server" id="divConfirmacionCorreo" class="divMensaje divMensajeConfirmacion" visible="false">
            <div id="divConfCorreoCabecera" class="FCabecera"   >
                <label id="lblConfCorreoTitulo">Mensaje</label>
            </div>
            <div id="divConfCorreoContenido" class="divConfCorreoContenido">
                <asp:Label runat="server" ID="lblConfCorreoMensaje" CssClass="lblMensaje">Continuar con el envio de correo a los siguientes clientes:</asp:Label>
                <br />
                <asp:GridView ID="gvDestinoCorreo" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" PagerSettings-Mode="NumericFirstLast"
                    CssClass="gridview2" CellPadding="5" Width="100%" OnRowDataBound="gvDestinoCorreo_RowDataBound" OnPageIndexChanging="gvDestinoCorreo_PageIndexChanging" OnSorting="gvDestinoCorreo_Sorting" AllowSorting="true">
                    <AlternatingRowStyle BackColor="#f9c3f7"  />
                    <RowStyle BackColor="#f9def8" />
                    <PagerStyle BackColor="#CCCCCC" />
                    <SortedAscendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedDescendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedAscendingCellStyle CssClass="Seleccionada" />
                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                    <PagerStyle CssClass="Paginador" />

                    <Columns>
                        <asp:Templatefield HeaderText="indice" Visible ="true">
                             <ItemTemplate>
                                    <asp:Label runat='server' ID='lblindice' CssClass="lblUsuario" ></asp:Label>
                                </ItemTemplate>
                        </asp:Templatefield>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblIdCliente' CssClass="lblUsuario" Text='<%# Bind("ID_CLIENTE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblNombre' CssClass="lblEnviarCorreo" Text='<%# Bind("Nombre") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido Paterno" SortExpression="Apellido_paterno">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblApellidoP' CssClass="lblEnviarCorreo" Text='<%# Bind("Apellido_paterno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido Materno" SortExpression="Apellido_materno">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblApellidoM' CssClass="lblEnviarCorreo" Text='<%# Bind("Apellido_materno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email" SortExpression="EMAIL">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblEmail' CssClass="lblEnviarCorreo" Text='<%# Bind("EMAIL") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <asp:Label runat="server" ID="lblMsj2" CssClass="lblMensaje">O puede realizar el envío a un grupo de clientes ya creado</asp:Label>
                <div id="divTipoCorreoSelec" runat="server" visible="false">
                    <table>                        
                        <tr>
                            <td><asp:Label runat="server" ID="lblMsjTipoSelec" CssClass="lblMensaje">Seleccione el tipo de correo: </asp:Label></td>
                            <td><asp:DropDownList runat="server" ID="ddlTipoCorreoS" CssClass="form-control FGInput FGCliente" OnSelectedIndexChanged="ddlTipoCorreoS_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList></td>
                            <td><asp:Button runat="server" id="btnPreviewTipoS" visible="false" class="btnAgrUsuarioG" OnClientClick="abrePreview()" Text="Preview" onclick="btnPreviewTipoS_Click"/></td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="row BotoneraConfCorreo">
                    <div class="col-md">
                        <asp:Button runat="server" ID="btnConfcorreoCancelar" CssClass="MBoton ConfCorreoButton" Text="Cancelar" OnClick="btnConfcorreoCancelar_Click" />
                    </div>
                    <div>
                        <asp:Button runat="server" ID="btnGpoClientes" CssClass="MBoton ConfCorreoButton" Text="Enviar a Grupo de Clientes" OnClick="btnGpoClientes_Click" />
                    </div>
                    <div class="col-md" runat="server" id="DivContinuaSelec">
                        <asp:Button runat="server" ID="btnContinuaSelec" CssClass="MBoton ConfCorreoButton" Text="Continuar con selección" OnClick="btnContinuaSelec_Click"/>
                    </div>
                    <div class="col-md" runat="server" id="DivConfOK">
                        <asp:Button runat="server" ID="btnConfCorreoOK" CssClass="MBoton ConfCorreoButton" Text="Aceptar" OnClick="btnConfCorreoOK_Click" Visible="false"/>
                    </div>
                </div>
            </div>
        </div>
        <div runat="server" id="divMsjConfirmaEnvio" class="divMensaje divMensajeConfirmacion" visible="false">
            <div id="divConfEnvioCabecera" class="FCabecera">
                <label id="lblConfEnvioTitulo">Mensaje</label>
            </div>
            <div id="divConfEnvio" class="divConfCorreoContenido">
                <asp:Label runat="server" ID="lblMsjConfEnvioS" CssClass="lblMensaje"></asp:Label>
                <br />
                <asp:Button runat="server" ID="btnCancelEnvioS" CssClass="MBoton" Text="Cancelar" OnClick="btnCancelaEnvioS_Click" />
                <asp:Button runat="server" ID="btnContinuaEnvioS" CssClass="MBoton" Text="Aceptar" OnClick="btnContinuaEnvioS_Click" OnClientClick="return mostrarmensajejs();" />
                <asp:Button runat="server" ID="btnCancelEnvioGpo" CssClass="MBoton" Text="Cancelar" OnClick="btnCancelaEnvioG_Click" />
                <asp:Button runat="server" ID="btnContinuaEnvioGpo" CssClass="MBoton" Text="Aceptar" OnClick="btnContinuaEnvioG_Click" OnClientClick="return mostrarmensajejs();" />
            </div>
        </div>
        <div runat="server" id="divGpoClienteTipoSelec" class="divMensaje divMensajeConfirmacion" visible="false">
            <div id="divGpoClSCabecera" class="FCabecera">
                <label id="lblGpoClsTitulo">Mensaje</label>
            </div>
            <div id="divGpoClsCont" class="divConfCorreoContenido">
                <table>
                    <tr>
                        <td><asp:Label runat="server" ID="lblMsjGpoCl" CssClass="lblMensaje">Seleccione el grupo de clientes para realizar el envío: </asp:Label></td>
                        <td><asp:DropDownList runat="server" ID="ddlGpoCliente" CssClass="form-control FGInput FGCliente" OnSelectedIndexChanged="ddlGpoCliente_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList></td>
                        <td><asp:LinkButton ID="linkGpoConsult" ToolTip="Ver clientes en este grupo" OnClick="linkGpoConsult_Click" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-eye' aria-hidden='true'></i>" Visible="false" /></td>
                    </tr>
                </table>
                <div id="VistaGpoCl" runat="server" visible="false">
                    <asp:GridView ID="gvGpoClDatos" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-Mode="NumericFirstLast"
                    CssClass="gridview2" CellPadding="5" Width="100%" OnPageIndexChanging="gvDestinoCorreo_PageIndexChanging" OnSorting="gvDestinoCorreo_Sorting" AllowSorting="true">
                    <AlternatingRowStyle BackColor="#f2f2f2" />
                    <RowStyle BackColor="#f9c3f7" />
                    <PagerStyle BackColor="#f9def8" />
                    <SortedAscendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedDescendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedAscendingCellStyle CssClass="Seleccionada" />
                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                    <PagerStyle CssClass="Paginador" />

                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblIdCliente' CssClass="lblUsuario" Text='<%# Bind("ID_CLIENTE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblNombre' CssClass="lblEnviarCorreo" Text='<%# Bind("Nombre") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido Paterno" SortExpression="Apellido_paterno">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblApellidoP' CssClass="lblEnviarCorreo" Text='<%# Bind("Apellido_paterno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido Materno" SortExpression="Apellido_materno">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblApellidoM' CssClass="lblEnviarCorreo" Text='<%# Bind("Apellido_materno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Correo" SortExpression="Email">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblEmail' CssClass="lblEnviarCorreo" Text='<%# Bind("Email") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
                <div id="divTipoCorreoGpo" runat="server">
                    <table>
                        <tr>
                            <td><asp:Label runat="server" ID="lblMsjTipoGpoCl" CssClass="lblMensaje">Seleccione el tipo de correo: </asp:Label></td>
                            <td><asp:DropDownList runat="server" ID="ddlTipoCorreoG" CssClass="form-control FGInput FGCliente" OnSelectedIndexChanged="ddlTipoCorreoG_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList></td>
                            <td><asp:Button runat="server" id="btnPreviewTipoGpo" visible="false" class="btnAgrUsuarioG" OnClientClick="abrePreview()" Text="Preview" onclick="btnPreviewTipoGpo_Click"/></td>
                        </tr>
                    </table>
                </div>
                <br />
                <asp:Button runat="server" ID="btnCancelaGpoCl" CssClass="MBoton" Text="Cancelar" OnClick="btnGpoClCancelar_Click" />
                <asp:Button runat="server" ID="btnContinuaGpoCl" CssClass="MBoton" Text="Continuar" OnClick="btnGpoClContinua_Click" Enabled="false" />
                <asp:Button runat="server" ID="btnEnviarGpoCl" CssClass="MBoton" Text="Aceptar" OnClick="btnEnviaCorreoGpo_Click" Enabled="false" Visible="false"/>
            </div>
        </div>
        <div id="MContenidoJS" runat="server" style="display:none;" class="divMensaje divMensajeConfirmacion" >
            <div id="divEnvCorreoCabecera" class="FCabecera">
                <label id="lblEnvCorreoTitulo">¡Alerta!</label>
            </div>
            <div class="botonera">
                <asp:Image runat="server" ID="CargaJS" ImageUrl="~/Imagenes/cargando-gif.gif" CssClass="CargaJS" />
                <asp:Label runat="server" ID="lblMensajeJS" CssClass="lblMensaje" Text="Enviando correos a los clientes seleccionados"></asp:Label>
                <br />
                <asp:Button runat="server" ID="btnCorreoEnviadoOK" CssClass="MBoton" Text="Aceptar" OnClick="btnCorreoEnviadoOK_Click" Visible="false"/>
            </div>
        </div>
        <div runat="server" id="divFormBusqCl" class="FormClienteCorreo" visible ="false">
            <div id="FCabeceraCl" class="FCabecera">
                <asp:Label runat="server" ID="lblTituloCl" CssClass="FTitulo">Buscar Cliente</asp:Label>
            </div>
                <div id="FContenidoCl" class="FContenido">
                    <div class="row justify-content-start">
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Nombre</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtNombreCl" CssClass="form-control FGInput FGCliente" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Apellido Paterno</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtApellidoPCl" CssClass="form-control FGInput FGCliente" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Apellido Materno</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtApellidoMCl" CssClass="form-control FGInput FGCliente" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-center">
                        <div class="col-2 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Género</label>
                                <asp:DropDownList runat="server" ID="ddlSexo" CssClass="form-control FGInput FGCliente ddlSexo">
                                    <asp:ListItem Text="TODOS" Value="-1"></asp:ListItem>
                                    <%--<asp:ListItem Text="OTRO" Value="OTRO"></asp:ListItem>--%>
                                    <asp:ListItem Text="HOMBRE" Value="HOMBRE"></asp:ListItem>
                                    <asp:ListItem Text="MUJER" Value="MUJER"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-3 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Fecha Ingreso</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFechaI" CssClass="form-control FGInput FGCliente txtFecha" TextMode="Date" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-3 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Fecha Nacimiento</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFechaN" CssClass="form-control FGInput FGCliente txtFecha" TextMode="Date" AutoComplete="off" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-4 px-2">
                                <div class="form-group">
                                    <label class="FGEtiqueta">Edad</label>
                                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtEdad" CssClass="form-control-edad" AutoComplete="off" TextMode="Number" Enabled="false"></asp:TextBox>
                                    <asp:Label runat="server" id="lblA" Visible="false" CssClass="lblA"> a </asp:Label>
                                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtEdad2" CssClass="form-control-edad" AutoComplete="off" TextMode="Number" Visible="false"></asp:TextBox>
                                    <asp:CheckBox runat="server" ID="chkRango" OnCheckedChanged="chkRango_CheckedChanged" Visible="false" AutoPostBack="true" />
                                </div>
                        </div>
                    </div>

                    <div class="row justify-content-start">
                        <div class="col-3 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Telefono Fijo</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtTelFijo" CssClass="form-control FGInput FGCliente" AutoComplete="off" TextMode="Phone"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-2 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Ext.</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtExtension" CssClass="form-control FGInput FGCliente" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-3 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Celular</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtCelular" CssClass="form-control FGInput FGCliente " AutoComplete="off" TextMode="Phone"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Medio</label>
                                <asp:DropDownList runat="server" ID="ddlMedio" CssClass="form-control FGInput FGCliente">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-start">
                        
                        <div class="col px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Enviar Email</label>
                                <asp:DropDownList runat="server" CssClass="form-control FGInput FGCliente" ID="ddlEnviarCorreo">
                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Email</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtEmail" CssClass="form-control FGInput FGCliente" AutoComplete="off" TextMode="Email"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Factura:</label>
                                <asp:DropDownList runat="server" CssClass="form-control FGInput FGCliente" ID="ddlFactura">
                                    <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-start" id="divDatosFactura" visible="false" runat="server">
                        
                        <div class="col-12 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Nombre o Razón Social:</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtNmR" CssClass="form-control FGInput FGCliente" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">RFC</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtRfc" CssClass="form-control FGInput FGCliente" AutoComplete="off"></asp:TextBox>
                                
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Entidad</label>
                                <asp:DropDownList runat="server" CssClass="form-control FGInput FGCliente" ID="ddlEntidad">
                                    <asp:ListItem Value="1" Text="Persona moral"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Persona física"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        
                        <div class="col-12 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Dirección fiscal:</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtDirFiscal" CssClass="form-control FGInput FGCliente" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-start">
                        <div class="col px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Estado:</label>
                                <asp:DropDownList OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control FGInput FGCliente" ID="ddlEstado">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col px-2">
                            <div class="form-group">
                                <div runat="server" id="divMunicipio" class="divCelda">
                                    <label class="FGEtiqueta">Municipio: </label>
                                    <asp:DropDownList runat="server" CssClass="form-control FGInput FGCliente" ID="ddlMunicipio" Enabled="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div runat="server" id="divPais" visible="false">
                                    <label class="FGEtiqueta">Pais: </label>
                                    <asp:DropDownList runat="server" CssClass="form-control FGInput FGCliente" ID="ddlPais">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row justify-content-start" runat="server" visible="false">
                        <div class="col px-2">
                            <div class="form-group" runat="server" id="divObservacionesNota">
                                <label for="exampleFormControlTextarea1" class="FGEtiqueta">Observaciones</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtObservaciones" CssClass="form-control FGInput FGCliente" AutoComplete="off" TextMode="MultiLine">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-start" runat="server" visible="false">
                        <div class="col-12 px-2">
                            <div class="form-group">
                                <label for="exampleFormControlTextarea1" class="FGEtiqueta">Notas</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtNota" CssClass="form-control FGInput FGCliente" AutoComplete="off" TextMode="MultiLine"></asp:TextBox>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtIdCliente" CssClass="txtObsNota FGInput FGCliente" Visible="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-6 px-2">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblError" CssClass="lblError"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-center">
                        <div class="col px-2 text-center FBotonera">
                            <asp:Button runat="server" ID="btnBuscarCl" CssClass="FGBoton FCCBoton" Text="Buscar" OnClick="btnBuscarCl_Click" />
                            <asp:Button runat="server" ID="btnLimpiarCl" CssClass="FGBoton FCCBoton" Text="Limpiar" OnClick="btnLimpiarCl_Click" />
                            <asp:Button runat="server" ID="btnCancelarCl" CssClass="FGBoton FCCBoton" Text="Cancelar" OnClick="FGCancelarCl_Click" />
                        </div>
                    </div>
                    <br />

                </div>
        </div>
        <%--<div runat="server" id="divFormTipo" class="FormClienteCorreo" visible="false">
            <div id="divCabeceraTipo" class="FCabecera">
                <label id="lblTipoTitulo" class="FTitulo" runat="server"></label>
            </div>
            <asp:Panel runat="server" ID="plTipo">
                <div id="FormTipoCorreo" class="FContenido FCCContenido">
                    <asp:Label runat="server" ID="lblIDTipo" Visible="false"></asp:Label>
                    <table class="tblParametros">
                        <tr>
                            <td style="width: 10%;"><label>Tipo de correo:</label></td>
                            <td><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtTipo" CssClass="ParamInput"></asp:TextBox></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="width: 10%;"><label>Asunto:</label></td>
                            <td><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtAsuntoTipo" CssClass="ParamInput"></asp:TextBox></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td><label>Mensaje:</label></td>
                            <td>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtMsjTipo" CssClass="ParamInput" TextMode="multiline" Columns="50" Rows="5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="imgCorreoTipo" runat="server" visible="false">
                            <td><label>Imagen correo:</label></td>
                            <td>
                                <asp:FileUpload runat="server" ID="fuploadTipo" CssClass="ParamInput"  accept="image/jpeg" />
                                <asp:Label runat="server" ID="lblErrorUploadTipo" CssClass="lblError"></asp:Label>
                                <asp:Label runat="server" ID="lblRutaImgTipo" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    
                        <tr>
                            <td><label>Firma:</label></td>
                            <td><asp:TextBox runat="server" ID="txtFirmaTipo" CssClass="ParamInput" TextMode="MultiLine"></asp:TextBox></td>
                        </tr>
                    </table>
                    <br/>
                    <div class="row justify-content-center">
                        <div class="col px-2 text-center FBotonera">
                            <asp:Button runat="server" ID="btnCerrarTipo" CssClass="FGBoton FCCBoton" Text="Cerrar" OnClick="btnTipoCerrar_Click"/>
                            <asp:Button runat="server" ID="btnLimpiaTipo" CssClass="FGBoton FCCBoton" Text="Limpiar" OnClick="btnTipoLimpiar_Click"/>
                            <asp:Button runat="server" ID="btnTipoAgregar" CssClass="FGBoton FCCBoton" Text="Agregar" OnClick="btnFormTipoAgregar_Click"/>
                            <asp:Button runat="server" ID="btnTipoModificar" CssClass="FGBoton FCCBoton" Text="Modificar" OnClick="btnFormTipoModificar_Click"/>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>--%>
        <div runat="server" id="divConfirmaDeleteTipo" class="divMensaje divMensajeConfirmacion" visible="false">
            <div id="divTipoCabeceraD" class="FCabecera"   >
                <label id="lblMsjTipoD">¡Alerta!</label>
            </div>
            <div id="divMsjContTipoD" class="divConfCorreoContenido">
                <asp:Label runat="server" ID="lblMsjDeleteTipo" CssClass="lblMensaje"></asp:Label><asp:Label runat="server" ID="lblIDtipoDelete" visible="false"></asp:Label>                
                <br />
                <asp:Button runat="server" ID="btnCancelarDTipo" CssClass="MBoton" Text="Cancelar" OnClick="btnCancelarDeleteTipo_Click" />
                <asp:Button runat="server" ID="btnokDtipo" CssClass="MBoton" Text="Aceptar" OnClick="btnOKDeleteTipo_Click"/>
            </div>
        </div>
        <div runat="server" id="divConfirmaDeleteGrupo" class="divMensaje divMensajeConfirmacion" visible="false">
            <div id="divTipoCabeceraDG" class="FCabecera"   >
                <label id="lblMsjTipoDG">¡Alerta!</label>
            </div>
            <div id="divMsjContTipoDG" class="divConfCorreoContenido">
                <asp:Label runat="server" ID="lblTextDeleteGrupo" CssClass="lblMensaje"></asp:Label><asp:Label runat="server" ID="lblIDGrupoDeleteGrupo" visible="false"></asp:Label>                
                <br />
                <asp:Button runat="server" ID="btnCancelarDeleteGrupo" CssClass="MBoton" Text="Cancelar" OnClick="btnCancelarDeleteGrupo_Click" />
                <asp:Button runat="server" ID="btnAceptarDeleteGrupo" CssClass="MBoton" Text="Aceptar" OnClick="btnAceptarDeleteGrupo_Click"/>
            </div>
        </div>
        <div runat="server" id="divFormCliGrupo" class="FormClienteCorreo" visible="false">
            <div id="divFormCliGrupoCabecera" class="FCabecera">
                <label id="lblFormCliGrupoTitulo" class="FTitulo">Buscar cliente</label>
            </div>
            <asp:Panel runat="server" ID="panFormCliGrupo">
                <div id="FormCliGrupoContenido" class="FContenido FCCContenido">
                    <div class="row justify-content-start">
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Nombre</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="txtFormCliGrupoNombre" CssClass="form-control " AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Apellido Paterno</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="txtFormCliGrupoApePat" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Apellido Materno</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="txtFormCliGrupoApeMat" CssClass="form-control " AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-center">
                        <div class="col-2 px-2">
                            <div class="form-group">
                                <label>Género</label>
                                <asp:DropDownList runat="server" ID="ddlFormCliGrupoSexo" CssClass="form-control ddlSexo">
                                    <asp:ListItem Text="TODOS" Value="-1"></asp:ListItem>
                                    <%--<asp:ListItem Text="OTRO" Value="OTRO"></asp:ListItem>--%>
                                    <asp:ListItem Text="HOMBRE" Value="HOMBRE"></asp:ListItem>
                                    <asp:ListItem Text="MUJER" Value="MUJER"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-3 px-2">
                            <div class="form-group">
                                <label>Fecha Ingreso</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliGrupoFecIngreso" CssClass="form-control txtFecha" TextMode="Date" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-3 px-2">
                            <div class="form-group">
                                <label>Fecha Nacimiento</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliGrupoFecNac" CssClass="form-control txtFecha" TextMode="Date" AutoComplete="off" ></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Edad</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliGrupoEdad" CssClass="form-control-edad" AutoComplete="off" TextMode="Number" ></asp:TextBox>
                                <asp:Label runat="server" id="lblFormCliGrupoA" Visible="false" CssClass="lblA"> a </asp:Label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliGrupoEdad2" CssClass="form-control-edad" AutoComplete="off" TextMode="Number" Visible="false"></asp:TextBox>
                                <asp:CheckBox runat="server" ID="chkFormCliGrupoEdad" OnCheckedChanged="chkFormCliGrupoEdad_CheckedChanged" AutoPostBack="true" />
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-start">
                        <div class="col-3 px-2">
                            <div class="form-group">
                                <label>Telefono Fijo</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliGrupoTelFijo" CssClass="form-control InpMediano" AutoComplete="off" TextMode="Phone"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-2 px-2">
                            <div class="form-group">
                                <label>Ext.</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliGrupoExt" CssClass="form-control InpExt" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-3 px-2">
                            <div class="form-group">
                                <label>Celular</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliGrupoCelular" CssClass="form-control InpMediano " AutoComplete="off" TextMode="Phone"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Medio</label>
                                <asp:DropDownList runat="server" ID="ddlFormCliGrupoMedio" CssClass="form-control InpGrande">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-start">
                        <div class="col px-2">
                            <div class="form-group">
                                <label>Email</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliGrupoEmail" CssClass="form-control InpExtGrande" AutoComplete="off" TextMode="Email"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-start">
                        <div class="col px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Estado:</label>
                                <asp:DropDownList OnSelectedIndexChanged="ddlFormCliGrupoEstado_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control " ID="ddlFormCliGrupoEstado">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col px-2">
                            <div class="form-group">
                                <div runat="server" id="divFormCliGrupoMunicipio" class="divCelda">
                                    <label class="FGEtiqueta">Municipio: </label>
                                    <asp:DropDownList runat="server" CssClass="form-control " ID="ddlFormCliGrupoMunicipio" Enabled="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div runat="server" id="divFormCliGrupoPais" visible="false">
                                    <label class="FGEtiqueta">Pais: </label>
                                    <asp:DropDownList runat="server" CssClass="form-control InpGrande" ID="ddlFormCliGrupoPais">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col px-2">
                            <div class="form-group">
                                <label>Estatus</label>
                                <asp:DropDownList runat="server" ID="ddlFormCliGrupoEstatus" CssClass="form-control InpMediano">
                                    <asp:ListItem Text="Activo" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Inactivo" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-center">
                        <div class="col-6 px-2">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblFormCliGrupoError" CssClass="lblError"></asp:Label>
                            </div>
                        </div>
                    </div>
                    
                    <br/>
                    <div class="row justify-content-center">
                        <div class="col px-2 text-center FBotonera">
                            <asp:Button runat="server" ID="btnFormCliGrupoBuscar" CssClass="FGBoton FCCBoton" Text="Buscar" OnClick="btnFormCliGrupoBuscar_Click"/>
                            <asp:Button runat="server" ID="btnFormCliGrupoLimpiar" CssClass="FGBoton FCCBoton" Text="Limpiar" OnClick="btnFormCliGrupoLimpiar_Click"/>
                            <asp:Button runat="server" ID="btnFormCliGrupoCerrar" CssClass="FGBoton FCCBoton" Text="Cerrar" OnClick="btnFormCliGrupoCerrar_Click"/>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>

        <div id="divAgregarMedico" class="FormGerente text-center" runat="server" visible="false">
            <div id="divAgregaMedicoCabecera" class="FCabecera">
                <label id="lblAgregaMedicoTitulo" class="FTitulo">Agregar Médico</label>
            </div>
            <asp:Panel runat="server" ID="panelMedicoAgrega">
                <div id="FormAgregaMedico" class="FContenido FCCContenido">
                    <div id="SeleccionaUsuarioMed" runat="server">
                        <div class="row justify-content-start">
                            <div class="col px-2">
                                <h5>Seleccione el usuario que quiere añadir como médico</h5>
                            </div>
                        </div>
                        <div class="row justify-content-start">
                            <div class="col px-2">
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtBusqUsuarioMed" placeholder="Usuario" CssClass="txtBusquedaG" OnTextChanged="txtBusqUsuMed_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <asp:Image runat="server" ID="imgBusqUsuMed" ImageUrl="~/Imagenes/imgBusqueda.png" CssClass="imgBusquedaG" />
                            </div>
                        </div>
                        <div class="row justify-content-start">
                            <div class="col px-2">
                                <asp:GridView ID="gvUsuariosMedicoA" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="5" PagerSettings-Mode="NumericFirstLast"
                            CssClass="gridview2" CellPadding="5" Width="100%" OnRowDataBound="gvUsuariosMedicoA_RowDataBound" OnPageIndexChanging="gvUsuariosMedicoA_PageIndexChanging">
                            <AlternatingRowStyle BackColor="#f9c3f7" />
                            <RowStyle BackColor="#f9def8" />
                            <PagerStyle BackColor="#CCCCCC" />
                            <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                            <PagerStyle CssClass="Paginador" />
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label runat='server' ID='lblIdUsuario' CssClass="lblUsuario" Text='<%# Bind("ID_USUARIO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnSeleccionarMed" ToolTip="Seleccionar" runat="server" Text="Seleccionar" OnClick="btnSeleccionarMed_Click" AutoPostBack="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre">
                                    <ItemTemplate>
                                        <asp:Label runat='server' ID='lblNombre' CssClass="lblEnviarCorreo" Text='<%# Bind("Nombre") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Apellido Paterno" SortExpression="Apellido_paterno">
                                    <ItemTemplate>
                                        <asp:Label runat='server' ID='lblApellidoP' CssClass="lblEnviarCorreo" Text='<%# Bind("Apellido_paterno") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Apellido Materno" SortExpression="Apellido_materno">
                                    <ItemTemplate>
                                        <asp:Label runat='server' ID='lblApellidoM' CssClass="lblEnviarCorreo" Text='<%# Bind("Apellido_materno") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div id="ConfirmaMedicoSelec" runat="server">
                        <div class="row justify-content-center">
                            <div class="col px-2">
                                <asp:label id="IDMedicoAgregar" runat="server" visible="false"></asp:label>
                                <asp:label id="nombreMedicoAgregar" runat="server" visible="false"></asp:label>
                                <asp:label id="appMedicoAgregar" runat="server" visible="false"></asp:label>
                                <asp:label id="apmMedicoAgregar" runat="server" visible="false"></asp:label>
                                <asp:label id="lblConfirmaMedico" class="text-center" runat="server"></asp:label>
                            </div>
                        </div>
                    </div>
                    <div id="BotoneraMedicoAgrega" class="justify-content-center">
                        <asp:Button runat="server" ID="btnCancelarMedicoAg" CssClass="FGBoton" Text="Cancelar" OnClick="MedicoAgCancelar_Click" />
                        <asp:Button runat="server" ID="btnAceptarMedicoAg" CssClass="FGBoton" Text="Aceptar" OnClick="MedicoAgAceptar_Click" />
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="divConfirmaDeleteMedico" class="divMensaje divMensajeConfirmacion text-center" runat="server" visible="false">
            <div id="divMedicoCabeceraDelete" class="FCabecera"   >
                <label id="lblMsjMedicoDelete">¡Alerta!</label>
            </div>
            <div id="divMsjContMedicoDelete" class="divConfCorreoContenido">
                <asp:Label runat="server" ID="lblMensajeDeleteMedico" CssClass="lblMensaje"></asp:Label><asp:Label runat="server" ID="lblIdsuarioDeleteMed" visible="false"></asp:Label>                
                <br />
                <asp:Button runat="server" ID="btnCancelDelMed" CssClass="MBoton" Text="Cancelar" OnClick="btnCancelarDeleteMedico_Click" />
                <asp:Button runat="server" ID="btnAceptaDelMed" CssClass="MBoton" Text="Aceptar" OnClick="btnAceptarDeleteMedico_Click"/>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="mySidenav" class="sidenav">
        <asp:Button runat="server" ID ="btnOpcionUsuario" OnClick="btnOpcionUsuario_Click" Text="Usuarios"/>
        <br />
        <asp:Button runat="server" ID="btnOpcionMedico" OnClick="btnOpcionMedico_Click" Text="Médico"/>
        <br />
        <asp:Button runat="server" ID ="btnOpcionPerfil" OnClick="btnOpcionPerfil_Click" Text="Perfiles"/>
        <br />
        <asp:Button runat="server" ID ="btnOpcionCorreo" OnClick="btnOpcionCorreo_Click" Text="Correo" Visible="true"/>
        <asp:Button runat="server" ID ="btnConfiguracionTecnica" OnClick="btnConfiguracionTecnica_Click" Text="Configuración técnica" Visible="false" CssClass="subOpcion"/>
        <asp:Button runat="server" ID ="btnConfiguracionCorreo" OnClick="btnConfiguracionCorreo_Click" Text="Configuración de correo" Visible="false" CssClass="subOpcion"/>
        <asp:Button runat="server" ID ="btnContenidCorreo" OnClick="btnContenidCorreo_Click" Text="Contenido de correo" Visible="false" CssClass="subOpcion"/>
        <%--<asp:Button runat="server" ID ="btnContenidCorreoSelec" OnClick="btnContenidCorreoSelec_Click" Text="Contenido de correo selectivo" Visible="false" CssClass="subOpcion"/>--%>
        <asp:Button runat="server" ID ="btnCorreoRecordatorio" OnClick="btnCorreoRecordatorio_Click" Text="Configuración de correo recordatorio" Visible="false" CssClass="subOpcion"/>
        <asp:Button runat="server" ID ="btnTiposCorreo" OnClick="btnTiposCorreo_Click" Text="Tipos de correo promociones" Visible="false" CssClass="subOpcion"/>
        <asp:Button runat="server" ID ="btnEnvioCorreo" OnClick="btnEnvioCorreo_Click" Text="Envío de correo promociones" Visible="false" CssClass="subOpcion" />
        
        <%--<asp:Button runat="server" ID ="btnEnvioCorreo" OnClick="btnEnvioCorreo_Click" Text="Envio de promociones" Visible="true" />--%>
        <br />
        <asp:Button runat="server" ID ="btnOpcionClientes" OnClick="btnOpcionClientes_Click" Text="Reactivar clientes" />
        <br />
        <asp:Button runat="server" ID="btnConfigCitas" OnClick="btnConfigCitas_Click" Text="Config. de citas" />
        <br />
        <asp:Button runat="server" ID="btnGruposClientes" OnClick="btnGruposClientes_Click" Text="Grupos Clientes" Visible="true" />
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
                <asp:GridView ID="gvGerentes" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false"
                    CssClass="gridview" CellPadding="5" Width="100%" OnRowDataBound="gvGerentes_RowDataBound" OnRowEditing="gvGerentes_RowEditing" OnRowCancelingEdit="gvGerentes_RowCancelingEdit"
                    OnRowUpdated="gvGerentes_RowUpdated" OnRowDeleted="gvGerentes_RowDeleted" OnRowDeleting="gvGerentes_RowDeleting">
                    <AlternatingRowStyle BackColor="#f9c3f7" />
                    <RowStyle BackColor="#f9def8" />
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
                        <asp:TemplateField HeaderText="Doctor" HeaderStyle-Width="20%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblDoctor' CssClass="lblGerentes" Text='<%# Bind("Medico") %>'></asp:Label>
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
                <asp:GridView ID="gvPerfiles" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false"
                    CssClass="gridview" CellPadding="5" Width="100%" OnRowDataBound="gvPerfiles_RowDataBound" OnRowEditing="gvPerfiles_RowEditing" OnRowCancelingEdit="gvPerfiles_RowCancelingEdit"
                    OnRowUpdated="gvPerfiles_RowUpdated" OnRowDeleted="gvPerfiles_RowDeleted" OnRowDeleting="gvPerfiles_RowDeleting" OnRowUpdating="gvPerfiles_RowUpdating">
                    <AlternatingRowStyle BackColor="#f9c3f7" />
                    <RowStyle BackColor="#f9def8" />
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
                <asp:Button runat="server" ID="btnActCorreoContSelec" Text="Actualizar" OnClick="btnActCorreoContSelec_Click" CssClass="btnAgrUsuarioG" />
                <asp:Button runat="server" ID="btnActCorreoRecordatorio" Text="Actualizar" OnClick="btnActCorreoRecordatorio_Click" CssClass="btnAgrUsuarioG" />
                <button runat="server" id="btnPreview" visible="false" class="btnAgrUsuarioG" onclick="abrePreview()">Preview</button>
                <asp:Button runat="server" ID="btnActTipo" Text="Agregar tipo" ToolTip="Agregar Tipo de correo" OnClick="btnAgregarTipo_Click" CssClass="btnAgrUsuarioG" />
                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtBusqTipo" placeholder="Tipo de correo" CssClass="txtBusquedaG" OnTextChanged="txtBusqTipoC_TextChanged" AutoPostBack="true"></asp:TextBox>
                <asp:Image runat="server" ID="imgBusqTipo" ImageUrl="~/Imagenes/imgBusqueda.png" CssClass="imgBusquedaG" />

                <asp:Button runat="server" ID="btnCerrarTipo" CssClass="btnAgrUsuarioG" Text="Cancelar" OnClick="btnTipoCerrar_Click" Visible ="false"/>
                <asp:Button runat="server" ID="btnLimpiaTipo" CssClass="btnAgrUsuarioG" Text="Limpiar" OnClick="btnTipoLimpiar_Click"  Visible ="false"/>
                <asp:Button runat="server" ID="btnTipoAgregar" CssClass="btnAgrUsuarioG" Text="Agregar" OnClick="btnFormTipoAgregar_Click"  Visible ="false"/>
                <asp:Button runat="server" ID="btnTipoModificar" CssClass="btnAgrUsuarioG" Text="Modificar" OnClick="btnFormTipoModificar_Click"  Visible ="false"/>
                <asp:Button runat="server" ID="btnTipoPreview" CssClass="btnAgrUsuarioG" Text="Preview" OnClientClick="abrePreview()" OnClick="btnFormTipoPreview_Click"  Visible ="false"/>

                <div class="btnEnviarCorreo" id="chkdivSelecTodo" runat="server" visible="false"><asp:CheckBox runat="server" ID="chkECSelectTodo" Text="Todos" OnCheckedChanged="chkECSelectTodo_CheckedChanged" AutoPostBack="true"/></div>
                <asp:Button runat="server" ID="btnLimipiaS" CssClass="btnAgrClienteG" Text="Limpiar Selección" OnClick="btnLimpiaS_Click" Visible="false" />
                <asp:Button runat="server" ID="btnEnviarCorreo" CssClass="btnEnviarCorreo" AutoPostBack="true" Text="Enviar correos" OnClick="btnEnviarCorreo_Click" Visible="false"></asp:Button>
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" style="background-position: 20px center; background-image: url('Imagenes/imgBusqueda_opt.png'); background-repeat: no-repeat; width: 140px;"  CssClass="btnAgrClienteG floatDerecha btnBuscar" Visible="false" />
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
                <div runat="server" id="divCorreoContSelec">
                    <div class="divCorreoTitulo"><h5>Contenido del correo selectivo</h5></div>
                    <table class="tblParametros">
                        <tr>
                            <td style="width: 10%;"><label>Asunto:</label></td>
                            <td><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtAsuntoSelec" CssClass="ParamInput"></asp:TextBox></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td><label>Mensaje:</label></td>
                            <td><%--<asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtMensaje" CssClass="ParamInput"></asp:TextBox>--%>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtMsjSelec" CssClass="ParamInput" TextMode="multiline" Columns="50" Rows="5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><label>Imagen correo:</label></td>
                            <td><%--<asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtImagen" CssClass="ParamInput" TextMode="Url" ></asp:TextBox>--%>
                                <asp:FileUpload runat="server" ID="uploadImgSelec" CssClass="ParamInput"  accept="image/jpeg" />
                                <asp:Label runat="server" ID="lblErrorSelec" CssClass="lblError"></asp:Label>
                            </td>
                        </tr>
                    
                        <tr>
                            <td><label>Firma:</label></td>
                            <td><asp:TextBox runat="server" ID="txtFirmaSelec" CssClass="ParamInput" TextMode="MultiLine"></asp:TextBox></td>
                        </tr>
                    </table>
                    <br />
                </div>
                <div runat="server" id="divCorreoRecordatorio">
                    <div runat="server" id="divConfCorreoRec">
                        <div class="divCorreoTitulo"><h5>Configuración de correo recordatorio</h5></div>
                        <table class="tblParametros">
                            <tr>
                                <td style="width: 20%;"><label>Enviar antes de la cita:</label></td>
                                <td style="width: 10%;"><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtDiasAntesCita" CssClass="ParamInput" TextMode="Number"></asp:TextBox></td>
                                <td style="width: 70%;"></td>
                            
                            </tr>
                        </table>
                    </div>
                    <div runat="server" id="divContenidoRecordatorio">
                        <div class="divCorreoTitulo" style="border-radius:unset;"><h5>Contenido del correo recordatorio</h5></div>
                        <table class="tblParametros">
                            <tr>
                                <td style="width: 10%;"><label>Asunto:</label></td>
                                <td><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtAsuntoRec" CssClass="ParamInput"></asp:TextBox></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td><label>Mensaje:</label></td>
                                <td>
                                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtMesajeRec" CssClass="ParamInput" TextMode="multiline" Columns="50" Rows="5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td><label>Imagen correo:</label></td>
                                <td>
                                    <asp:FileUpload runat="server" ID="fuploadRec" CssClass="ParamInput"  accept="image/jpeg" />
                                    <asp:Label runat="server" ID="lblErrorRec" CssClass="lblError"></asp:Label>
                                </td>
                            </tr>
                    
                            <tr>
                                <td><label>Firma:</label></td>
                                <td><asp:TextBox runat="server" ID="txtFirmaRec" CssClass="ParamInput" TextMode="MultiLine"></asp:TextBox></td>
                            </tr>
                        </table>
                        <br />
                    </div>
                </div>
                <div id="divTiposCorreo" runat="server">
                    <asp:GridView ID="gvTiposCorreo" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false"
                    CssClass="gridview" CellPadding="5" Width="100%" style="table-layout:fixed;" EmptyDataText="Sin registros">
                        <AlternatingRowStyle BackColor="#f9c3f7" />
                        <RowStyle BackColor="#f9def8" />
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblIdTipo' CssClass="lblUsuario" Text='<%# Bind("ID_TIPO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo de correo" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblTipo' CssClass="lblUsuario" Text='<%# Bind("NOMBRE_TIPO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sujeto" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblSujeto' CssClass="lblUsuario" Text='<%# Bind("SUJETO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mensaje" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                        <asp:Label runat='server' ID='lblMensaje' CssClass="lblUsuario" Text='<%# Bind("MENSAJE") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblImagen' CssClass="lblUsuario" Text='<%# Bind("IMGCORREO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblFirma' CssClass="lblUsuario" Text='<%# Bind("FIRMA") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Opciones" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkEditT" ToolTip="Editar" OnClick="gvTipo_Edit" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-pencil-square-o' aria-hidden='true'></i>" />
                                <asp:LinkButton ID="linkConsultT" ToolTip="Ver tipo de correo" OnClick="gvTipo_Consult" OnClientClick="abrePreview()" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-eye' aria-hidden='true'></i>" />
		                        <asp:LinkButton ID="linkDeleteT" ToolTip="Eliminar" OnClick="gvTipo_Delete" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-trash' aria-hidden='true'></i>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div runat="server" id="divFormTipo" visible="false">
                    <div id="FormTipoCorreo" class="FContenido FCCContenido">
                        <asp:Label runat="server" ID="lblIDTipo" Visible="false"></asp:Label>
                        <table class="tblParametros">
                            <tr>
                                <td style="width: 10%;"><label>Tipo de correo:</label></td>
                                <td><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtTipo" CssClass="ParamInput"></asp:TextBox></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 10%;"><label>Asunto:</label></td>
                                <td><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtAsuntoTipo" CssClass="ParamInput"></asp:TextBox></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td><label>Mensaje:</label></td>
                                <td>
                                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtMsjTipo" CssClass="ParamInput" TextMode="multiline" Columns="50" Rows="5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="imgCorreoTipo" runat="server">
                                <td><label>Imagen correo:</label></td>
                                <td>
                                    <asp:FileUpload runat="server" ID="fuploadTipo" CssClass="ParamInput"  accept="image/jpeg" />
                                    <asp:Label runat="server" ID="lblErrorUploadTipo" CssClass="lblError"></asp:Label>
                                </td>
                            </tr>
                    
                            <tr>
                                <td><label>Firma:</label></td>
                                <td><asp:TextBox runat="server" ID="txtFirmaTipo" CssClass="ParamInput" TextMode="MultiLine"></asp:TextBox></td>
                            </tr>
                        </table>        
                    </div>
                </div>
                <div runat="server" id="div1" visible="false">
                    <asp:GridView ID="GridView1" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" PagerSettings-Mode="NumericFirstLast"
                        CssClass="gridview2" CellPadding="5" Width="100%" OnRowDataBound="gvClientesCorreo_RowDataBound" OnPageIndexChanging="gvClientesCorreo_PageIndexChanging" OnSorting="gvClientesCorreo_Sorting" AllowSorting="true">
                        <AlternatingRowStyle BackColor="#f9c3f7" />
                        <RowStyle BackColor="#f9def8" />
                        <PagerStyle BackColor="#CCCCCC" />
                        <SortedAscendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                        <SortedDescendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                        <SortedAscendingCellStyle CssClass="Seleccionada" />
                        <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                        <PagerStyle CssClass="Paginador" />

                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSeleccionado" ToolTip="Seleccionar" runat="server" OnCheckedChanged="chkSeleccionado_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:Templatefield HeaderText="indice" Visible ="true">
                                    <ItemTemplate>
                                        <asp:Label runat='server' ID='lblindice' CssClass="lblUsuario" ></asp:Label>
                                    </ItemTemplate>
                            </asp:Templatefield>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblIdCliente' CssClass="lblUsuario" Text='<%# Bind("ID_CLIENTE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblNombre' CssClass="lblEnviarCorreo" Text='<%# Bind("Nombre") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apellido Paterno" SortExpression="Apellido_paterno">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblApellidoP' CssClass="lblEnviarCorreo" Text='<%# Bind("Apellido_paterno") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apellido Materno" SortExpression="Apellido_materno">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblApellidoM' CssClass="lblEnviarCorreo" Text='<%# Bind("Apellido_materno") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Género" SortExpression="Sexo">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblSexo' CssClass="lblEnviarCorreo" Text='<%# Bind("Sexo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="ESTADO">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblEstado' CssClass="lblEnviarCorreo" Text='<%# Bind("ESTADO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Municipio" SortExpression="MUNICIPIO">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblMunicipio' CssClass="lblEnviarCorreo" Text='<%# Bind("MUNICIPIO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edad" SortExpression="EDAD">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblEdad' CssClass="lblEnviarCorreo" Text='<%# Bind("EDAD") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Ingreso" SortExpression="FECHA_INGRESO">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblFechaI' CssClass="lblEnviarCorreo" Text='<%# Bind("FECHA_INGRESO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Medio" SortExpression="MEDIO">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblMedio' CssClass="lblEnviarCorreo" Text='<%# Bind("MEDIO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tel casa" SortExpression="tel_casa_fijo">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblTelFijo' CssClass="lblEnviarCorreo" Text='<%# Bind("tel_casa_fijo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ext." SortExpression="Extension">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblExtension' CssClass="lblEnviarCorreo" Text='<%# Bind("Extension") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Celular" SortExpression="Celular">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblCelular' CssClass="lblEnviarCorreo" Text='<%# Bind("Celular") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha nacimiento" SortExpression="FECHA_NACIMIENTO">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblFechaN' CssClass="lblEnviarCorreo" Text='<%# Bind("FECHA_NACIMIENTO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email" SortExpression="EMAIL">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblEmail' CssClass="lblEnviarCorreo" Text='<%# Bind("EMAIL") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Observaciones" SortExpression="OBSERVACIONES" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblObservaciones' CssClass="lblEnviarCorreo" Text='<%# Bind("OBSERVACIONES") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nota" SortExpression="NOTA" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblNota' CssClass="lblEnviarCorreo" Text='<%# Bind("NOTA") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estatus" SortExpression="Estatus">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblEstatus' CssClass="lblEnviarCorreo" Text='<%# Bind("ESTATUS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pais" SortExpression="Pais" Visible="true">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblPais' CssClass="lblEnviarCorreo" Text='<%# Bind("PAIS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EnvCorreo" SortExpression="Enviar_Correo" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblEnvCorreo' CssClass="lblEnviarCorreo" Text='<%# Bind("Enviar_Correo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Requiere Factura" SortExpression="Req_Factura">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblFactura' CssClass="lblEnviarCorreo" Text='<%# Bind("REQ_FACTURA") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <div runat="server" id="divEnvioCorreo" class="divContenido2" visible="false">
            <%--<div id="divEnvioCorreo1">
                <div class="btnEnviarCorreo"><asp:CheckBox runat="server" ID="chkECSelectTodo" Text="Todos" OnCheckedChanged="chkECSelectTodo_CheckedChanged" AutoPostBack="true"/></div>
                <asp:Button runat="server" ID="btnLimipiaS" CssClass="btnAgrClienteG" Text="Limpiar Selección" OnClick="btnLimpiaS_Click" Visible="false" />
                <asp:Button runat="server" ID="btnEnviarCorreo" CssClass="btnEnviarCorreo" AutoPostBack="true" Text="Enviar correos" OnClick="btnEnviarCorreo_Click" Visible="true"></asp:Button>
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" style="background-position: 20px center; background-image: url('Imagenes/imgBusqueda_opt.png'); background-repeat: no-repeat; width: 140px;"  CssClass="btnAgrClienteG floatDerecha btnBuscar"  />
            </div>--%>
            <div id="divEnvioCorreo2">
                <asp:GridView ID="gvClientesCorreo" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" PagerSettings-Mode="NumericFirstLast"
                    CssClass="gridview2" CellPadding="5" Width="100%" OnRowDataBound="gvClientesCorreo_RowDataBound" OnPageIndexChanging="gvClientesCorreo_PageIndexChanging" OnSorting="gvClientesCorreo_Sorting" AllowSorting="true">
                    <AlternatingRowStyle BackColor="#f9c3f7" />
                    <RowStyle BackColor="#f9def8" />
                    <PagerStyle BackColor="#CCCCCC" />
                    <SortedAscendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedDescendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedAscendingCellStyle CssClass="Seleccionada" />
                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                    <PagerStyle CssClass="Paginador" />

                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSeleccionado" ToolTip="Seleccionar" runat="server" OnCheckedChanged="chkSeleccionado_CheckedChanged" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:Templatefield HeaderText="indice" Visible ="true">
                             <ItemTemplate>
                                    <asp:Label runat='server' ID='lblindice' CssClass="lblUsuario" ></asp:Label>
                                </ItemTemplate>
                        </asp:Templatefield>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblIdCliente' CssClass="lblUsuario" Text='<%# Bind("ID_CLIENTE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblNombre' CssClass="lblEnviarCorreo" Text='<%# Bind("Nombre") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido Paterno" SortExpression="Apellido_paterno">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblApellidoP' CssClass="lblEnviarCorreo" Text='<%# Bind("Apellido_paterno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido Materno" SortExpression="Apellido_materno">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblApellidoM' CssClass="lblEnviarCorreo" Text='<%# Bind("Apellido_materno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="ESTADO">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblEstado' CssClass="lblEnviarCorreo" Text='<%# Bind("ESTADO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Municipio" SortExpression="MUNICIPIO">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblMunicipio' CssClass="lblEnviarCorreo" Text='<%# Bind("MUNICIPIO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edad" SortExpression="EDAD">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblEdad' CssClass="lblEnviarCorreo" Text='<%# Bind("EDAD") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Ingreso" SortExpression="FECHA_INGRESO">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblFechaI' CssClass="lblEnviarCorreo" Text='<%# Bind("FECHA_INGRESO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Medio" SortExpression="MEDIO">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblMedio' CssClass="lblEnviarCorreo" Text='<%# Bind("MEDIO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tel casa" SortExpression="tel_casa_fijo">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblTelFijo' CssClass="lblEnviarCorreo" Text='<%# Bind("tel_casa_fijo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ext." SortExpression="Extension">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblExtension' CssClass="lblEnviarCorreo" Text='<%# Bind("Extension") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Celular" SortExpression="Celular">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblCelular' CssClass="lblEnviarCorreo" Text='<%# Bind("Celular") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha nacimiento" SortExpression="FECHA_NACIMIENTO">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblFechaN' CssClass="lblEnviarCorreo" Text='<%# Bind("FECHA_NACIMIENTO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email" SortExpression="EMAIL">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblEmail' CssClass="lblEnviarCorreo" Text='<%# Bind("EMAIL") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Observaciones" SortExpression="OBSERVACIONES" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblObservaciones' CssClass="lblEnviarCorreo" Text='<%# Bind("OBSERVACIONES") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nota" SortExpression="NOTA" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblNota' CssClass="lblEnviarCorreo" Text='<%# Bind("NOTA") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estatus" SortExpression="Estatus">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblEstatus' CssClass="lblEnviarCorreo" Text='<%# Bind("ESTATUS") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pais" SortExpression="Pais" Visible="true">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblPais' CssClass="lblEnviarCorreo" Text='<%# Bind("PAIS") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EnvCorreo" SortExpression="Enviar_Correo" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblEnvCorreo' CssClass="lblEnviarCorreo" Text='<%# Bind("Enviar_Correo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Requiere Factura" SortExpression="Req_Factura">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblFactura' CssClass="lblEnviarCorreo" Text='<%# Bind("REQ_FACTURA") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        
        <div runat="server" id="divClientes" class="divContenido2" visible="false">
            <div id="divClientes1" visible="false">
                <asp:Button runat="server" ID="btnBuscarCli" Text="Buscar" OnClick="btnBuscarCli_Click" 
                    style="background-position: 20px center; background-image: url('Imagenes/imgBusqueda_opt.png'); background-repeat: no-repeat; width: 140px;"  CssClass="btnAgrClienteG floatDerecha btnBuscar"  />
            </div>
            <div id="divClientes2">
               
                <div runat="server" id="divClientesG">
                    <asp:GridView ID="gvClientes" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="7" PagerSettings-Mode="NumericFirstLast"
                    CssClass="gridview" CellPadding="5" Width="100%" OnRowDataBound="gvClientes_RowDataBound">
                    <AlternatingRowStyle BackColor="#f9c3f7" />
                    <RowStyle BackColor="#f9def8" />
                    <PagerStyle BackColor="#CCCCCC" />
                    <SortedAscendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedDescendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedAscendingCellStyle CssClass="Seleccionada" />
                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                    <PagerStyle CssClass="Paginador" />

                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblIdCliente' CssClass="lblUsuario" Text='<%# Bind("ID_CLIENTE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblNombreC' CssClass="lblUsuario" Text='<%# Bind("NOMBRE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido Paterno" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblApellidoP' CssClass="lblUsuario" Text='<%# Bind("APELLIDO_PATERNO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido Materno" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblApellidoM' CssClass="lblUsuario" Text='<%# Bind("APELLIDO_MATERNO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    <asp:TemplateField HeaderText="Género" HeaderStyle-Width="20%" SortExpression="Sexo">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblSexo' CssClass="lblGerentes" Text='<%# Bind("Sexo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estado" HeaderStyle-Width="20%" SortExpression="ESTADO">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblEstado' CssClass="lblGerentes" Text='<%# Bind("ESTADO") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Municipio" HeaderStyle-Width="20%" SortExpression="MUNICIPIO">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblMunicipio' CssClass="lblGerentes" Text='<%# Bind("MUNICIPIO") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edad" HeaderStyle-Width="20%" SortExpression="EDAD">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblEdad' CssClass="lblGerentes" Text='<%# Bind("EDAD") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Ingreso" HeaderStyle-Width="20%" SortExpression="FECHA_INGRESO">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblFechaI' CssClass="lblGerentes" Text='<%# Bind("FECHA_INGRESO") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Medio" HeaderStyle-Width="20%" SortExpression="MEDIO">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblMedio' CssClass="lblGerentes" Text='<%# Bind("MEDIO") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tel casa" HeaderStyle-Width="20%" SortExpression="tel_casa_fijo">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblTelFijo' CssClass="lblGerentes" Text='<%# Bind("tel_casa_fijo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ext." HeaderStyle-Width="20%" SortExpression="Extension">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblExtension' CssClass="lblGerentes" Text='<%# Bind("Extension") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Celular" HeaderStyle-Width="20%" SortExpression="Celular">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblCelular' CssClass="lblGerentes" Text='<%# Bind("Celular") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha nacimiento" HeaderStyle-Width="20%" SortExpression="FECHA_NACIMIENTO">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblFechaN' CssClass="lblGerentes" Text='<%# Bind("FECHA_NACIMIENTO") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email" HeaderStyle-Width="20%" SortExpression="EMAIL">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblEmail' CssClass="lblGerentes" Text='<%# Bind("EMAIL") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Observaciones" HeaderStyle-Width="20%" SortExpression="OBSERVACIONES" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblObservaciones' CssClass="lblGerentes" Text='<%# Bind("OBSERVACIONES") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nota" HeaderStyle-Width="20%" SortExpression="NOTA" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblNota' CssClass="lblGerentes" Text='<%# Bind("NOTA") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estatus" HeaderStyle-Width="20%" SortExpression="Estatus" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblEstatus' CssClass="lblGerentes" Text='<%# Bind("ESTATUS") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pais" HeaderStyle-Width="20%" SortExpression="Pais" Visible="true">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblPais' CssClass="lblGerentes" Text='<%# Bind("PAIS") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EnvCorreo" HeaderStyle-Width="20%" SortExpression="Enviar_Correo" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblEnvCorreo' CssClass="lblGerentes" Text='<%# Bind("Enviar_Correo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Requiere Factura" HeaderStyle-Width="20%" SortExpression="Req_Factura">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblFactura' CssClass="lblGerentes" Text='<%# Bind("REQ_FACTURA") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nombre o Razón Fiscal" HeaderStyle-Width="20%" SortExpression="NombRazon_Factura" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblNomRazon' CssClass="lblGerentes" Text='<%# Bind("NOMBRAZON_FACTURA") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RFC" HeaderStyle-Width="20%" SortExpression="rfc" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblRfc' CssClass="lblGerentes" Text='<%# Bind("RFC") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Entidad" HeaderStyle-Width="20%" SortExpression="entidad" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblEntidad' CssClass="lblGerentes" Text='<%# Bind("ENTIDAD") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dirección fiscal" HeaderStyle-Width="20%" SortExpression="Dir_factura" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblDirFactura' CssClass="lblGerentes" Text='<%# Bind("DIR_FACTURA") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    --%>                
                    <asp:TemplateField HeaderText="Opciones">
                        <ItemTemplate>
		                    <asp:LinkButton ID="linkReactiveCl" ToolTip="Reactivar" OnClick="gvClientes_RowDeleting" CssClass="linkOpcion" runat="server" Text="Reactivar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>         
            </div>
        </div>

        
        <div runat="server" id="divConfigCitas" class="mx-5 px-5" visible="false">
            <div class="divCorreo2">
                <div class="divCorreoTitulo text-center">
                    <h5>Configuración de citas</h5>
                </div>
                <div class=" container px-5 py-2">
                    <div>
                        <div class="row ">
                            <div class="col-3">
                                <div class="">
                                    <label>Horario de atención :</label>
                                    <input type="text" runat="server" id="actualHorario" disabled />
                                </div>
                            </div>
                            <div class="col-3">
                                <div class="">
                                    <label>Hora de apertura :</label><br /><input type="time" value="07:30" runat="server" id="horaApertura" />
                                </div>
                            </div>
                            <div class="col-3">
                                <div class="">
                                    <label>Hora de cierre :</label><br /><input type="time" value="19:30" id="horaCierre" runat="server" />
                                </div>
                            </div>
                            <div class="col-3">
                                <div class="">
                                    <asp:Button runat="server" ID="yyyy" CssClass="btnAgrUsuarioG" Text="Actualizar" OnClick="btnActualizarCitasHrsSemana" />
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="row ">
                            <div class="col">
                                Dias no hábiles
                            </div>
                            <div class="col-6">
                                <div class="form-check form-check-inline">
                                    <asp:CheckBox class="form-check-input" type="checkbox" ID="DiaCheckbox1" runat="server"></asp:CheckBox>
                                    <label class="form-check-label" for="DiaCheckbox1">Lunes</label>
                                </div>
                                <div class="form-check form-check-inline">
                                    <input class="form-check-input" type="checkbox" id="DiaCheckbox2" runat="server" value="option2">
                                    <label class="form-check-label" for="DiaCheckbox2">Martes</label>
                                </div>
                                <div class="form-check form-check-inline">
                                    <input class="form-check-input" type="checkbox" id="DiaCheckbox3" runat="server" value="option3">
                                    <label class="form-check-label" for="DiaCheckbox3">Miercoles</label>
                                </div>
                                <div class="form-check form-check-inline">
                                    <input class="form-check-input" type="checkbox" id="DiaCheckbox4" runat="server" value="option1">
                                    <label class="form-check-label" for="DiaCheckbox4">Jueves</label>
                                </div>
                                <div class="form-check form-check-inline">
                                    <input class="form-check-input" type="checkbox" id="DiaCheckbox5" runat="server" value="option1">
                                    <label class="form-check-label" for="DiaCheckbox5">Viernes</label>
                                </div>
                                <div class="form-check form-check-inline">
                                    <input class="form-check-input" type="checkbox" id="DiaCheckbox6" runat="server" value="option1">
                                    <label class="form-check-label" for="DiaCheckbox6">Sábado</label>
                                </div>
                                <div class="form-check form-check-inline">
                                    <input class="form-check-input" type="checkbox" id="DiaCheckbox7" runat="server" value="option1">
                                    <label class="form-check-label" for="DiaCheckbox7">Domingo</label>
                                </div>
                            </div>
                            <div class="col-3">
                                <div>
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="row ">
                            <div class="col">
                                <div>
                                    Fechas inhábiles para citas:
                                </div>
                            </div>
                            <div class="col">
                                <div>
                                    <input type="date" id="nuevaFecha" runat="server" />
                                </div>
                            </div>
                            <div class="col-3">
                                <div>
                                    <asp:Button runat="server" ID="xxxx" CssClass="btnAgrUsuarioG" Text="Agregar" OnClick="FAgregarFechasInhabiles" />
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row justify-content-center">
                            <div class="col-6">
                                <div id="FechaInhabiles">
                                    <asp:GridView ID="TbFechas" runat="server" AllowPaging="true" PageSize="6" OnPageIndexChanging="TbFechas_PageIndexChanging" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false"
                                        CssClass="gridview" CellPadding="5" Width="100%" OnRowDeleting="TbFechas_RowDeleting">
                                        <AlternatingRowStyle BackColor="#f9c3f7" />
                                        <RowStyle BackColor="#f9def8" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="100%">
                                                <ItemTemplate>
                                                    <asp:Label runat='server' ID='lblIdFecha' CssClass="lblUsuario" Text='<%# Bind("fechas_inhabiles", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ShowDeleteButton="true" HeaderText="Opciones" HeaderStyle-Width="100%" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        
        
        
        <div runat="server" id="divGrupos" class="px-5" visible="false">
            
            <!--Tabla De Grupos-->
            <div runat="server" id="divGvGrupos" >
                <div>
                    <button type="button" class="btnEnviarCorreo" data-toggle="modal" data-target="#modalCrearGrupo">
                            Crear Grupo
                        </button>
                </div>
                <div>
                <asp:GridView ID="gvGrupos" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="7" PagerSettings-Mode="NumericFirstLast"
                    CssClass="gridview" CellPadding="5" Width="100%" OnPageIndexChanging="gvGrupos_PageIndexChanging" OnDataBound="gvGrupos_DataBound">
                    <AlternatingRowStyle BackColor="#f9c3f7" />
                    <RowStyle BackColor="#f9def8" />
                    <PagerStyle BackColor="#CCCCCC" />
                    <SortedAscendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedDescendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedAscendingCellStyle CssClass="Seleccionada" />
                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                    <PagerStyle CssClass="Paginador" />

                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblIdGrupo' CssClass="lblUsuario" Text='<%# Bind("ID_Grupo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre Grupo" HeaderStyle-Width="20%" SortExpression="Nombre_Grupo">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblNombre' CssClass="lblEnviarCorreo" Text='<%# Bind("Nombre_Grupo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripcion Grupo" SortExpression="Descripcion_grupo">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblDescripción' CssClass="lblEnviarCorreo" Text='<%# Bind("Descripcion_Grupo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Número de Clientes" SortExpression="cantidadCliente">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblcantidadCliente' CssClass="lblEnviarCorreo" Text='<%# Bind("cantidadCliente") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Opciones">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkEditG" ToolTip="Editar" OnClick="gvGrupos_RowEditing" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-pencil-square-o' aria-hidden='true'></i>" />
		                        <asp:LinkButton ID="linkDeleteG" ToolTip="Eliminar" OnClick="gvGrupos_RowDeleting" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-trash' aria-hidden='true'></i>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:CommandField ShowEditButton="true" ShowDeleteButton="true" HeaderText="Opciones" HeaderStyle-Width="10%"/>--%>
                    </Columns>
                </asp:GridView>
                </div>
            </div>
            <!--Tabla Editar/Agregar Clienes A Grupo-->
            
            <div runat="server" id="divContenidosGrupos" visible="false">
                <div id="divBotoneraGrupos">
                    <div class="btnEnviarCorreo"><asp:CheckBox runat="server" ID="chkTodosCliGrup" Text="Todos" OnCheckedChanged="chkTodosCliGrup_CheckedChanged" AutoPostBack="true"/></div>
                    <asp:Button runat="server" ID="btnLimpiarClientesGrupo" CssClass="btnAgrClienteG" Text="Limpiar Selección" OnClick="btnLimpiarClientesGrupo_Click"/>
                    <asp:Button runat="server" ID="btnGuardarGrupo" CssClass="btnEnviarCorreo" AutoPostBack="true" Text="Guardar grupo" OnClick="btnGuardarGrupo_Click"></asp:Button>
                    
                    <asp:Button runat="server" ID="btnCancelarGrupo" CssClass="btnEnviarCorreo" AutoPostBack="true" Text="Cancelar" OnClick="btnCancelarGrupo_Click1"></asp:Button>
                    <asp:Button runat="server" ID="btnBuscarCliGrup" Text="Buscar" OnClick="btnBuscarCliGrup_Click" style="background-position: 20px center; background-image: url('Imagenes/imgBusqueda_opt.png'); background-repeat: no-repeat; width: 140px;"  CssClass="btnAgrClienteG floatDerecha btnBuscar"  />
                </div>
                <div runat="server" id="divClientesGrupos" class="divContenido2">
                    <asp:Label runat="server" ID="lblIDGrupoGvGrupo" Visible="false"></asp:Label>
                    <asp:Label runat="server" ID="lblNombreGrupoGvGrupo" Visible="false"></asp:Label>
                    <asp:GridView ID="gvCliGrupo" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" PagerSettings-Mode="NumericFirstLast"
                        CssClass="gridview gvCliGrupo" CellPadding="5" Width="100%" OnRowDataBound="gvCliGrupo_RowDataBound" OnPageIndexChanging="gvCliGrupo_PageIndexChanging" OnSorting="gvCliGrupo_Sorting" AllowSorting="true">

                        <AlternatingRowStyle BackColor="#f9c3f7" />
                        <RowStyle BackColor="#f9def8" />
                        <PagerStyle BackColor="#CCCCCC" />
                        <SortedAscendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                        <SortedDescendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                        <SortedAscendingCellStyle CssClass="Seleccionada" />
                        <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                        <PagerStyle CssClass="Paginador" />

                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCliente" ToolTip="Seleccionar" runat="server" OnCheckedChanged="chkCliente_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:Templatefield HeaderText="indice" Visible ="true">
                                 <ItemTemplate>
                                        <asp:Label runat='server' ID='lblindice' CssClass="lblUsuario" ></asp:Label>
                                    </ItemTemplate>
                            </asp:Templatefield>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblIdCliente' CssClass="lblUsuario" Text='<%# Bind("ID_CLIENTE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblNombre' CssClass="lblEnviarCorreo" Text='<%# Bind("Nombre") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apellido Paterno" SortExpression="Apellido_paterno">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblApellidoP' CssClass="lblEnviarCorreo" Text='<%# Bind("Apellido_paterno") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apellido Materno" SortExpression="Apellido_materno">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblApellidoM' CssClass="lblEnviarCorreo" Text='<%# Bind("Apellido_materno") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Género" SortExpression="Sexo">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblSexo' CssClass="lblEnviarCorreo" Text='<%# Bind("Sexo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="ESTADO">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblEstado' CssClass="lblEnviarCorreo" Text='<%# Bind("ESTADO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Municipio" SortExpression="MUNICIPIO">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblMunicipio' CssClass="lblEnviarCorreo" Text='<%# Bind("MUNICIPIO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edad" SortExpression="EDAD">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblEdad' CssClass="lblEnviarCorreo" Text='<%# Bind("EDAD") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Ingreso" SortExpression="FECHA_INGRESO">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblFechaI' CssClass="lblEnviarCorreo" Text='<%# Bind("FECHA_INGRESO", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tel casa" SortExpression="tel_casa_fijo">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblTelFijo' CssClass="lblEnviarCorreo" Text='<%# Bind("tel_casa_fijo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Celular" SortExpression="Celular">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblCelular' CssClass="lblEnviarCorreo" Text='<%# Bind("Celular") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha nacimiento" SortExpression="FECHA_NACIMIENTO">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblFechaN' CssClass="lblEnviarCorreo" Text='<%# Bind("FECHA_NACIMIENTO", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email" SortExpression="EMAIL">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblEmail' CssClass="lblEnviarCorreo" Text='<%# Bind("EMAIL") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estatus" SortExpression="Estatus">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblEstatus' CssClass="lblEnviarCorreo" Text='<%# Bind("ESTATUS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pais" SortExpression="Pais" Visible="true">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblPais' CssClass="lblEnviarCorreo" Text='<%# Bind("PAIS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EnvCorreo" SortExpression="Enviar_Correo" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblEnvCorreo' CssClass="lblEnviarCorreo" Text='<%# Bind("Enviar_Correo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <!--Modal Crear Grupo-->
            <div class="modal fade" id="modalCrearGrupo" tabindex="-1" role="dialog" aria-labelledby="modalCrearGrupoLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content text-center divCorreo2">
                        <div class="modal-header justify-content-center divCorreoTitulo">
                            <h5 class="modal-title">Crear Grupo</h5>
                        </div>
                        <div class="modal-body container">
                            <div class="row">
                                <div class="col">
                                    <div class="">
                                        <label>Nombre del Grupo: </label>
                                        <input type="text" runat="server" id="nombreGrupo" autocomplete="off" />
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col">
                                    <div class="">
                                        <label>Descripcion del Grupo: </label>
                                        <textarea runat="server" id="descripcionGrupo" rows="1" cols="50" name="comment" autocomplete="off"></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer justify-content-center">
                            <asp:Button runat="server" ID="btnCrearGrupo" CssClass="btnEnviarCorreo" Text="Guardar Grupo" OnClick="btnCrearGrupo_Click" />
                            <button type="button" class="btnEnviarCorreo" data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="divMedico" class="divContenido" visible="false" runat="server">
            <div id="divMedicos1">
                <asp:Button runat="server" ID="btnAgregarMedico" Text="Agregar Médico" OnClick="btnAgregarMedico_Click" CssClass="btnAgrUsuarioG" />

                <asp:Button runat="server" ID="btnCancelaMedico" Text="Cancelar" OnClick="btnCancelaMedico_Click" CssClass="btnAgrUsuarioG" />
                <asp:Button runat="server" ID="btnGuardaMedico" Text="Guardar" OnClick="btnGuardaMedico_Click" CssClass="btnAgrUsuarioG" />
                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtBusqMedicos" placeholder="Médico" CssClass="txtBusquedaG" OnTextChanged="txtBusqMed_TextChanged" AutoPostBack="true"></asp:TextBox>
                <asp:Image runat="server" ID="imgBusqMed" ImageUrl="~/Imagenes/imgBusqueda.png" CssClass="imgBusquedaG" />
            </div>
            <div id="divTablaMedicos" class="divTablaMedicos" runat="server">
                <asp:GridView ID="gvMedicos" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="7" PagerSettings-Mode="NumericFirstLast"
                    CssClass="gridview" CellPadding="5" Width="100%" OnPageIndexChanging="gvMedicos_PageIndexChanging" OnDataBound="gvMedicos_DataBound">
                    <AlternatingRowStyle BackColor="#f9c3f7" />
                    <RowStyle BackColor="#f9def8" />
                    <PagerStyle BackColor="#CCCCCC" />
                    <SortedAscendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedDescendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedAscendingCellStyle CssClass="Seleccionada" />
                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                    <PagerStyle CssClass="Paginador" />

                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblIdUsuario' CssClass="lblUsuario" Text='<%# Bind("ID_USUARIO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Prefijo" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblPrefijo' CssClass="lblGerentes" Text='<%# Bind("Prefijo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblNombre' CssClass="lblGerentes" Text='<%# Bind("Nombre") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido Paterno" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblApellidoP' CssClass="lblGerentes" Text='<%# Bind("Apellido_paterno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido Materno" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblApellidoM' CssClass="lblGerentes" Text='<%# Bind("Apellido_materno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Título" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <div style="overflow: hidden; white-space: nowrap; text-overflow: ellipsis;">
                                    <asp:Label runat='server' ID='lblTitulo' CssClass="lblGerentes" Text='<%# Bind("Titulo") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Especialidades" HeaderStyle-Width="20%" Visible="false">
                            <ItemTemplate>
                                    <asp:Label runat='server' ID='lblEspecialidad' CssClass="lblGerentes" Text='<%# Bind("Especialidades") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cédula Profesional" HeaderStyle-Width="20%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblCedulaProf' CssClass="lblGerentes" Text='<%# Bind("Cedula_profesional") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cédula de especialidad" HeaderStyle-Width="20%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblCedulaEspec' CssClass="lblGerentes" Text='<%# Bind("Cedula_Especialidad") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Instagram" HeaderStyle-Width="20%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblInstagram' CssClass="lblGerentes" Text='<%# Bind("Instagram") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Facebook" HeaderStyle-Width="20%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblFacebook' CssClass="lblGerentes" Text='<%# Bind("Facebook") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Correo" HeaderStyle-Width="20%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblCorreo' CssClass="lblGerentes" Text='<%# Bind("Correo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sitio web" HeaderStyle-Width="20%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblSitioWeb' CssClass="lblGerentes" Text='<%# Bind("Sitio_web") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Número celular" HeaderStyle-Width="20%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblNumCelular' CssClass="lblGerentes" Text='<%# Bind("Numero_celular") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Número fijo" HeaderStyle-Width="20%" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblNumFijo' CssClass="lblGerentes" Text='<%# Bind("Numero_fijo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Opciones">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkEditMedico" ToolTip="Editar" OnClick="gvMedicos_RowEditing" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-pencil-square-o' aria-hidden='true'></i>" />
		                        <asp:LinkButton ID="linkDeleteMedico" ToolTip="Eliminar" OnClick="gvMedicos_RowDeleting" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-trash' aria-hidden='true'></i>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
            
            <div id="divFormularioMedico" class="FContenido divFormularioMedico" visible="false" runat="server">
                <asp:Label runat="server" ID="lblIDmedico" Visible="false"></asp:Label>
                <div class="divCorreoTitulo"><h5>Datos generales</h5></div>
                <table class="tblParametros">
                    <tr>
                        <td><label>Prefijo:</label></td>
                        <td>
                            <asp:DropDownList ID="ddlPrefijoMedico" runat="server">
                                <asp:ListItem Text="Dr." Value="Dr."></asp:ListItem>
                                <asp:ListItem Text="Dra." Value="Dra."></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td><label>Nombre:</label></td>
                        <td><asp:TextBox onkeypress="return DisableEnterKey(event);" Enabled="false" runat="server" ID="txtNombreMedico" CssClass="ParamInput"></asp:TextBox></td>
                        <td><asp:TextBox onkeypress="return DisableEnterKey(event);" Enabled="false" runat="server" ID="txtApellidoPMedico" CssClass="ParamInput"></asp:TextBox></td>
                        <td><asp:TextBox onkeypress="return DisableEnterKey(event);" Enabled="false" runat="server" ID="txtApellidoMMedico" CssClass="ParamInput"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><label>Título:</label></td>
                        <td colspan="3"><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtTitulodMedico" CssClass="ParamInput"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><label>Cédula profesional:</label></td>
                        <td colspan="3"><asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtCedulaProfMedico" CssClass="ParamInput"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><label>Cédula de especialidad:</label></td>
                        <td colspan="3">
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtCedulaEspMedico" CssClass="ParamInput"></asp:TextBox>
                        </td>
                    </tr>                    
                    <tr>
                        <td><label>Especialidades:</label></td>
                        <td colspan="3"><asp:TextBox runat="server" ID="txtEspecialidaddMedico" CssClass="ParamInput"></asp:TextBox></td>
                    </tr>                    
                </table>
                <div class="divCorreoTitulo" style="border-radius:unset;"><h5>Datos de contacto</h5></div>
                <table class="tblParametros">
                    <tr>
                        <td><label>Instagram:</label></td>
                        <td colspan="3"><asp:TextBox runat="server" ID="txtInstagramMedico" CssClass="ParamInput"></asp:TextBox></td>
                    </tr> 
                    <tr>
                        <td><label>Facebook:</label></td>
                        <td colspan="3"><asp:TextBox runat="server" ID="txtFacebookMedico" CssClass="ParamInput"></asp:TextBox></td>
                    </tr> 
                    <tr>
                        <td><label>Correo:</label></td>
                        <td colspan="3"><asp:TextBox runat="server" ID="txtCorreoMedico" CssClass="ParamInput"></asp:TextBox></td>
                    </tr> 
                    <tr>
                        <td><label>Sitio web:</label></td>
                        <td colspan="3"><asp:TextBox runat="server" ID="txtSitioMedico" CssClass="ParamInput"></asp:TextBox></td>
                    </tr> 
                    <tr>
                        <td><label>Números de contacto:</label></td>
                        <td><asp:TextBox runat="server" ID="txtFijoMedico" ToolTip="Teléfono fijo" placeholder="Teléfono fijo" CssClass="ParamInput"></asp:TextBox></td>
                        <td></td>
                        <td><asp:TextBox runat="server" ID="txtCelularMedico" ToolTip="Teléfono celular" placeholder="Teléfono celular" CssClass="ParamInput"></asp:TextBox></td>
                    </tr> 
                </table>
            </div>
        </div>

    </div>
</asp:Content>