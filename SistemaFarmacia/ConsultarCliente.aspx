<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFarmacia.Master" AutoEventWireup="true" CodeBehind="ConsultarCliente.aspx.cs" Inherits="SistemaFarmacia.ConsultarCliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Css/ConsultaCliente.css" type="text/css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceMensaje" runat="server">
    <div id="sombraMensaje" class="sombra" runat="server">
        <div runat="server" id="divMensaje" class="divMensaje" visible="false">
            <div id="MCabecera">
                <label id="MTitulo">¡Alerta!</label>
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
                <label id="Preguntar" class="verificacion">¡Alerta!</label>
            </div>
            <div id="MContenido2">
                <asp:Label runat="server" ID="lblPregunta" CssClass="lblMensaje lblMensajeB0">¿Está seguro que desea eliminar el registro del cliente </asp:Label><br /><asp:Label runat="server" ID="lblPregunta2" CssClass="lblMensaje lblMensajeT0"></asp:Label>
                <br />
                <asp:TextBox runat="server" ID="recupera" Visible="false" AutoComplete="off" />
                <asp:Button runat="server" ID="BCancelar" CssClass="MBoton" Text="Cancelar" OnClick="BCancelar_Click" Visible="false" />
                <asp:Button runat="server" OnClick="BAceptar_Click" CssClass="MBoton" Visible="false" Text="Eliminar" ID="BAceptar" />
            </div>
        </div>

        <div runat="server" id="divFormularioG" class="FormGerente">
            <div id="FCabecera" class="FCabecera">
                <asp:Label runat="server" ID="FTitulo" CssClass="FTitulo">Agregar usuario</asp:Label>
            </div>
            <asp:Panel runat="server" ID="panelMsj">
                <div id="FContenido" class="FContenido">
                    <br />
                    <label class="FGEtiqueta">Nombre: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtNombre" CssClass="FGColumna2 FGInput" AutoComplete="off"></asp:TextBox>
                    <label class="FGEtiqueta">Apellido Paterno: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtApellidoP" CssClass="FGColumna2 FGInput" AutoComplete="off"></asp:TextBox>
                    <br />
                    
                    <label class="FGEtiqueta">Apellido Materno: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtApellidoM" CssClass="FGColumna2 FGInput" AutoComplete="off"></asp:TextBox>
                    <label class="FGEtiqueta">Medio: </label>
                    <asp:DropDownList runat="server" ID="ddlMedio" CssClass="FGColumna2 FGInput"></asp:DropDownList>
                    
                    <label class="FGEtiqueta">Estado:</label>
                    <asp:DropDownList OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="FGColumna2 FGInput" ID="ddlEstado">
                    </asp:DropDownList>
                    <div runat="server" id="divMunicipio" class="divCelda">
                        <label class="FGEtiqueta">Municipio: </label>
                        <asp:DropDownList runat="server" CssClass="FGColumna2 FGInput" ID="ddlMunicipio" Enabled="false">
                        </asp:DropDownList>
                    </div>
                    <div runat="server" id="divPais" visible="false" class="divCelda">
                        <label class="FGEtiqueta" >Pais: </label>
                        <asp:DropDownList runat="server" CssClass="FGColumna2 FGInput" ID="ddlPais" >
                        </asp:DropDownList>
                    </div>
                    <br />
                    <label class="FGEtiqueta">Fecha Nacimiento: </label>
                    <%--<asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFechaN" CssClass="FGColumna2 FGInput" TextMode="Date" AutoComplete="off"></asp:TextBox>--%>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" OnTextChanged="TxtFechaN_TextChanged" AutoPostBack="true"  ID="TxtFechaN" CssClass="FGColumna2 FGInput" TextMode="Date" AutoComplete="off"></asp:TextBox>
                    <label class="FGEtiqueta">Edad: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtEdad" CssClass="FGColumna2 FGInput" AutoComplete="off" TextMode="Number" Enabled="false"></asp:TextBox>
                    <asp:Label runat="server" id="lblA" Visible="false" CssClass="lblA"> a </asp:Label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtEdad2" CssClass="FGColumna2 FGInput txtEdad2" AutoComplete="off" TextMode="Number" Visible="false"></asp:TextBox>
                    <asp:CheckBox runat="server" ID="chkRango" OnCheckedChanged="chkRango_CheckedChanged" Visible="false" CssClass="chkRango" AutoPostBack="true"/>
                    <br />
                    <label class="FGEtiqueta">Fecha ingreso: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFechaI" CssClass="FGColumna2 FGInput" TextMode="Date" AutoComplete="off"></asp:TextBox>
                    
                    <label class="FGEtiqueta">Celular: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtCelular" CssClass="FGColumna2 FGInput" AutoComplete="off" TextMode="Phone"></asp:TextBox>
                    
                    <br />
                    <label class="FGEtiqueta">Telefono fijo: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtTelFijo" CssClass="FGColumna2 FGInput" AutoComplete="off" TextMode="Phone"></asp:TextBox>
                    <label class="FGEtiqueta">Extensión: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtExtension" CssClass="FGColumna2 FGInput" AutoComplete="off"></asp:TextBox>
                    <br />
                    <label class="FGEtiqueta">Email: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtEmail" CssClass="FGColumna2 FGInput" AutoComplete="off" TextMode="Email"></asp:TextBox>
                    <label class="FGEtiqueta">Enviar correo:</label>
                    <asp:DropDownList runat="server" CssClass="FGColumna2 FGInput" ID="ddlEnviarCorreo" >
                        <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                        <asp:ListItem Value="0" Text="No"></asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <div runat="server" id="divObservacionesNota">
                        <label class="FGEtiqueta">Observaciones: </label>
                        <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtObservaciones" CssClass="FGColumna2 FGInput txtObsNota" AutoComplete="off" TextMode="MultiLine"></asp:TextBox>
                        
                        <label class="FGEtiqueta">Nota: </label>
                        <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtNota" CssClass="FGColumna2 FGInput txtObsNota" AutoComplete="off" TextMode="MultiLine"></asp:TextBox>
                        <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtIdCliente" CssClass="FGColumna2 FGInput" Visible="false"></asp:TextBox>
                        <br />

                        <label class="FGEtiqueta">Estatus: </label>
                        <asp:DropDownList runat="server" ID="ddlEstatus" CssClass="FGColumna2 FGInput">
                            <asp:ListItem Text="Activo" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Inactivo" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <label class="FGEtiqueta">Requiere factura:</label>
                        <asp:DropDownList runat="server" ID="ddlFactura" CssClass="FGColumna2 FGInput">
                            <asp:ListItem Text="Sí" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Label runat="server" ID="lblError" CssClass="lblError"></asp:Label>
                    <%--<br />--%>
                    
                    <div id="FBotonera">
                        <asp:Button runat="server" ID="btnBuscarF" CssClass="FGBoton" Text="Buscar" OnClick="btnBuscarF_Click" Visible="false" />
                        <asp:Button runat="server" ID="btnLimpiarF" CssClass="FGBoton" Text="Limpiar" OnClick="btnLimpiarF_Click" Visible="false" />
                        <asp:Button runat="server" ID="btnCerrarF" CssClass="FGBoton" Text="Cerrar" OnClick="btnCerrarF_Click" Visible="false" />
                        <asp:Button runat="server" ID="FGCancelar" CssClass="FGBoton" Text="Cancelar" OnClick="FGCancelar_Click" />
                        <asp:Button runat="server" ID="FGAgregar" CssClass="FGBoton" Text="Agregar" OnClick="FGAgregar_Click" />
                        <asp:Button runat="server" ID="FGActualizar" CssClass="FGBoton" Text="Actualizar" OnClick="FGActualizar_Click" />
                    </div>
                </div>
            </asp:Panel>
        </div>


    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">

    <div runat="server" id="divGerentes" class="divContenido" visible="true">
        <div id="divGerentes1">
            <asp:Button runat="server" ID="btnAgrClienteG" Text="Agregar cliente" OnClick="btnAgrClienteG_Click" CssClass="btnAgrClienteG" />
            <asp:DropDownList runat="server" ID="ddlEstatusGeneral" OnSelectedIndexChanged="ddlEstatusGeneral_SelectedIndexChanged" AutoPostBack="true" CssClass="ddlEstatusGeneral floatDerecha">
                <asp:ListItem Value="1,0">Todos</asp:ListItem>
                <asp:ListItem Value="0">Inactivos</asp:ListItem>
                <asp:ListItem Value="1">Activos</asp:ListItem>
            </asp:DropDownList>
            <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" style="background-position: 20px center; background-image: url('Imagenes/imgBusqueda_opt.png'); background-repeat: no-repeat; width: 140px;"  CssClass="btnAgrClienteG floatDerecha btnBuscar"  />
        </div>
        <div id="divGerentes2">
            <asp:GridView ID="gvGerentes" runat="server" HeaderStyle-BackColor="#5b9bd5" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" PagerSettings-Mode="NumericFirstLast"
                CssClass="gridview" CellPadding="5" Width="100%" OnRowDataBound="gvGerentes_RowDataBound" OnRowEditing="gvGerentes_RowEditing" OnRowCancelingEdit="gvGerentes_RowCancelingEdit"
                OnRowUpdated="gvGerentes_RowUpdated" OnRowDeleted="gvGerentes_RowDeleted" OnRowDeleting="gvGerentes_RowDeleting" OnPageIndexChanging="gvGerentes_PageIndexChanging" OnSorting="gvGerentes_Sorting" AllowSorting="true">
                <AlternatingRowStyle BackColor="#f2f2f2" />
                <RowStyle BackColor="#FFFFFF" />
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
                    <asp:TemplateField HeaderText="Nombre" HeaderStyle-Width="15%" SortExpression="Nombre">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblNombre' CssClass="lblGerentes" Text='<%# Bind("Nombre") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Apellido Paterno" HeaderStyle-Width="15%" SortExpression="Apellido_paterno">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblApellidoP' CssClass="lblGerentes" Text='<%# Bind("Apellido_paterno") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Apellido Materno" HeaderStyle-Width="15%" SortExpression="Apellido_materno">
                        <ItemTemplate>
                            <asp:Label runat='server' ID='lblApellidoM' CssClass="lblGerentes" Text='<%# Bind("Apellido_materno") %>'></asp:Label>
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
                    <asp:TemplateField HeaderText="Estatus" HeaderStyle-Width="20%" SortExpression="Estatus">
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
                    <asp:TemplateField HeaderText="Opciones">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkEditG" ToolTip="Editar" OnClick="gvGerentes_RowEditing" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-pencil-square-o' aria-hidden='true'></i>" />
                                <asp:LinkButton ID="linkConsultG" ToolTip="Ver detalle" OnClick="gvGerentes_RowConsult" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-eye' aria-hidden='true'></i>" />
		                        <asp:LinkButton ID="linkDeleteG" ToolTip="Eliminar" OnClick="gvGerentes_RowDeleting" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-trash' aria-hidden='true'></i>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    <%--<asp:CommandField ShowEditButton="true" ShowDeleteButton="true" HeaderText="Opciones" HeaderStyle-Width="15%" ShowCancelButton="true" />--%>

                </Columns>
            </asp:GridView>
        </div>
    </div>


</asp:Content>
