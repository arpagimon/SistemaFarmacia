<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFarmacia.Master" AutoEventWireup="true" CodeBehind="Administracion.aspx.cs" Inherits="SistemaFarmacia.Administracion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Css/Administración.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">
        window.onload = function () {
            $("#nuevoUsuario").hide();
            $("#divPerfiles").hide();
        };

        function abrePreview() {
            window.open('PreviewCorreo.aspx', 'Correo previo', 'location=no, toolbar=no, Top=0, Left=0');
        }

        function mostrarmensajejs() {
            try {
                $("#<%=divConfirmacionCorreo.ClientID%>").hide();
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
                <label cla
                    ss="FGEtiqueta">Apellido Paterno: </label>
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
                                </asp:CheckBoxList>
                                <asp:CheckBoxList runat="server"  id="chkAdmin" CssClass="chbFG" RepeatColumns="2">
                                    <asp:ListItem Value="312">Envio selectivo de correo</asp:ListItem>
                                    <asp:ListItem Value="313">Reactivar clientes</asp:ListItem>
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
                <asp:GridView ID="gvDestinoCorreo" runat="server" HeaderStyle-BackColor="#5b9bd5" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" PagerSettings-Mode="NumericFirstLast"
                    CssClass="gridview2" CellPadding="5" Width="100%" OnRowDataBound="gvDestinoCorreo_RowDataBound" OnPageIndexChanging="gvDestinoCorreo_PageIndexChanging" OnSorting="gvDestinoCorreo_Sorting" AllowSorting="true">
                    <AlternatingRowStyle BackColor="#f2f2f2" />
                    <RowStyle BackColor="#FFFFFF" />
                    <PagerStyle BackColor="#CCCCCC" />
                    <SortedAscendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedDescendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada" />
                    <SortedAscendingCellStyle CssClass="Seleccionada" />
                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                    <PagerStyle CssClass="Paginador" />

                    <Columns>
                        <%--<asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSeleccionado" ToolTip="Seleccionar" runat="server" OnCheckedChanged="chkSeleccionado_CheckedChanged" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
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
                        <%--<asp:TemplateField HeaderText="Estado" SortExpression="ESTADO">
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
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Email" SortExpression="EMAIL">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblEmail' CssClass="lblEnviarCorreo" Text='<%# Bind("EMAIL") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Observaciones" SortExpression="OBSERVACIONES" Visible="false">
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
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
                <br />
                <asp:Button runat="server" ID="btnConfcorreoCancelar" CssClass="MBoton" Text="Cancelar" OnClick="btnConfcorreoCancelar_Click" />
                <asp:Button runat="server" ID="btnConfCorreoOK" CssClass="MBoton" Text="Aceptar" OnClick="btnConfCorreoOK_Click" OnClientClick="return mostrarmensajejs();"/>
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
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TextBox1" CssClass="form-control FGInput FGCliente" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Apellido Paterno</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TextBox2" CssClass="form-control FGInput FGCliente" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Apellido Materno</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TextBox3" CssClass="form-control FGInput FGCliente" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-center">
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Fecha Ingreso</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFechaI" CssClass="form-control FGInput FGCliente" TextMode="Date" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Fecha Nacimiento</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFechaN" CssClass="form-control FGInput FGCliente" TextMode="Date" AutoComplete="off" AutoPostBack="true"></asp:TextBox>
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
        <asp:Button runat="server" ID ="btnEnvioCorreo" OnClick="btnEnvioCorreo_Click" Text="Envio selectivo de correo" Visible="true" />
        <br />
        <asp:Button runat="server" ID ="btnOpcionClientes" OnClick="btnOpcionClientes_Click" Text="Reactivar clientes" />
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
                <button runat="server" id="btnPreview" visible="false" class="btnAgrUsuarioG" onclick="abrePreview()">Preview</button>
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

        <div runat="server" id="divEnvioCorreo" class="divContenido2" visible="false">
            <div id="divEnvioCorreo1">
                <div class="btnEnviarCorreo"><asp:CheckBox runat="server" ID="chkECSelectTodo" Text="Todos" OnCheckedChanged="chkECSelectTodo_CheckedChanged" AutoPostBack="true"/></div>
                <asp:Button runat="server" ID="btnEnviarCorreo" CssClass="btnEnviarCorreo" AutoPostBack="true" Text="Enviar correos" OnClick="btnEnviarCorreo_Click" Visible="true"></asp:Button>
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" style="background-position: 20px center; background-image: url('Imagenes/imgBusqueda_opt.png'); background-repeat: no-repeat; width: 140px;"  CssClass="btnAgrClienteG floatDerecha btnBuscar"  />
            </div>
            <div id="divEnvioCorreo2">
                <asp:GridView ID="gvClientesCorreo" runat="server" HeaderStyle-BackColor="#5b9bd5" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" PagerSettings-Mode="NumericFirstLast"
                    CssClass="gridview2" CellPadding="5" Width="100%" OnRowDataBound="gvClientesCorreo_RowDataBound" OnPageIndexChanging="gvClientesCorreo_PageIndexChanging" OnSorting="gvClientesCorreo_Sorting" AllowSorting="true">
                    <AlternatingRowStyle BackColor="#f2f2f2" />
                    <RowStyle BackColor="#FFFFFF" />
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
                    <asp:GridView ID="gvClientes" runat="server" HeaderStyle-BackColor="#5b9bd5" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="7" PagerSettings-Mode="NumericFirstLast"
                    CssClass="gridview" CellPadding="5" Width="100%" OnRowDataBound="gvClientes_RowDataBound">
                    <AlternatingRowStyle BackColor="#f2f2f2" />
                    <RowStyle BackColor="#FFFFFF" />
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
                    <asp:TemplateField HeaderText="Requiere Factura" HeaderStyle-Width="20%" SortExpression="Req_Factura">
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
                    </asp:TemplateField>                    
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
    </div>
</asp:Content>