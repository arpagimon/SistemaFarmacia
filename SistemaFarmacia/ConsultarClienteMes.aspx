﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFarmacia.Master" AutoEventWireup="true" CodeBehind="ConsultarClienteMes.aspx.cs" Inherits="SistemaFarmacia.ConsultarClienteMes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Css/ConsultaCliente.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">
        function mostrarMensajeJS() {
            $("#<%=sombraJS.ClientID%>").show();
            $("#<%=btnOK.ClientID%>").hide();
        }
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
        
        <div runat="server" id="divFormularioG" class="FormGerente" visible="false">
            <div id="FCabecera" class="FCabecera">
                <label id="FTitulo" class="FTitulo">Buscar usuario</label>
            </div>
            <asp:Panel runat="server" ID="panelMsj">
                <div id="FContenido" class="FContenido">
                    <br />
                    <label class="FGEtiqueta">Nombre: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtNombre" CssClass="FGColumna2 FGInput"></asp:TextBox>
                    <label class="FGEtiqueta">Apellido Paterno: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtApellidoP" CssClass="FGColumna2 FGInput"></asp:TextBox>
                    <br />
                    <div class="divApePat">
                        <label class="FGEtiqueta">Apellido Materno: </label>
                        <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtApellidoM" CssClass="FGColumna2 FGInput"></asp:TextBox>
                    </div>
                    <label class="FGEtiqueta">Estado:</label>                 
                    <asp:DropDownList OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="FGColumna2 FGInput" ID="ddlEstado" >
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
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFechaN" CssClass="FGColumna2 FGInput" TextMode="Date"></asp:TextBox>
                    <label class="FGEtiqueta">Edad: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtEdad" CssClass="FGColumna2 FGInput"></asp:TextBox>
                    <asp:Label runat="server" id="lblA" Visible="false" CssClass="lblA"> a </asp:Label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtEdad2" CssClass="FGColumna2 FGInput txtEdad2" AutoComplete="off" TextMode="Number" Visible="false"></asp:TextBox>
                    <asp:CheckBox runat="server" ID="chkRango" OnCheckedChanged="chkRango_CheckedChanged" Visible="true" CssClass="chkRango" AutoPostBack="true"/>
                    <br />
                    <label class="FGEtiqueta">Fecha ingreso: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFechaI" CssClass="FGColumna2 FGInput" TextMode="Date"></asp:TextBox>
                    <label class="FGEtiqueta">Medio: </label>
                    <%--<asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtMedio" CssClass="FGColumna2 FGInput"></asp:TextBox>--%>
                    <asp:DropDownList runat="server" ID="ddlMedio" CssClass="FGColumna2 FGInput" ></asp:DropDownList>
                    <br />
                    <label class="FGEtiqueta">Telefono fijo: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtTelFijo" CssClass="FGColumna2 FGInput"></asp:TextBox>
                    <label class="FGEtiqueta">Extensión: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtExtension" CssClass="FGColumna2 FGInput"></asp:TextBox>
                    <br />
                    <label class="FGEtiqueta">Celular: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtCelular" CssClass="FGColumna2 FGInput"></asp:TextBox>
                    <label class="FGEtiqueta">Email: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtEmail" CssClass="FGColumna2 FGInput"></asp:TextBox>
                    <br />
                    <label class="FGEtiqueta">Observaciones: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtObservaciones" CssClass="FGColumna2 FGInput"></asp:TextBox>
                    <label class="FGEtiqueta">Nota: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtNota" CssClass="FGColumna2 FGInput"></asp:TextBox>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtIdCliente" CssClass="FGColumna2 FGInput" Visible="false"></asp:TextBox>
                    <br />
                    <asp:Label runat="server" ID="lblError" CssClass="lblError"></asp:Label>
                    <br />
                    <div id="FBotonera">
                        <asp:Button runat="server" ID="btnBuscarF" CssClass="FGBoton" Text="Buscar" OnClick="btnBuscarF_Click" Visible="false" />
                        <asp:Button runat="server" ID="btnLimpiarF" CssClass="FGBoton" Text="Limpiar" OnClick="btnLimpiarF_Click" Visible="false" />
                        <asp:Button runat="server" ID="btnCerrarF" CssClass="FGBoton" Text="Cerrar" OnClick="btnCerrarF_Click" Visible="false" />
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <div id="sombraJS" class="sombra" runat="server" style="display:none">
        <div runat="server" id="divMensajeJS" class="divMensaje divMensajeJS" >
            <div id="MCabeceraJS">
                <label id="MTituloJS">Mensaje</label>
            </div>
            <div id="MContenidoJS">
                <asp:Image runat="server" ID="CargaJS" ImageUrl="~/Imagenes/cargando-gif.gif" CssClass="CargaJS" />
                <asp:Label runat="server" ID="lblMensajeJS" CssClass="lblMensaje" Text="Enviando correos a los festejados del mes actual"></asp:Label>
                <br />
                <asp:Button runat="server" ID="btnOK" CssClass="MBoton" Text="Aceptar" OnClick="btnOK_Click" />
            </div>
        </div>

    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">

        <div runat="server" id="divGerentes" class="divContenido" visible="true">
            

            <div id="divGerentes1">
                <%--<div id="divEnviarCorreo">--%>
                    <asp:Button OnClientClick="mostrarMensajeJS()" runat="server" ID="btnEnviarCorreo" CssClass="btnAgrClienteG" AutoPostBack="true" Text="Reenviar Correos" OnClick="btnEnviarCorreo_Click" Visible="false"></asp:Button>
                    
                    <%--<asp:Image runat="server" ID="imgEnviarCorreo" ImageUrl="~/Imagenes/mail.png" CssClass="imgBusquedaG" />--%>
                <%--</div>--%>
                <%--<asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtBusquedaC" placeholder="Nombre/Apellido/Email/Observación" CssClass="txtBusquedaG" OnTextChanged="txtBusquedaC_TextChanged" AutoPostBack="true"></asp:TextBox>--%>
                <%--<asp:Button runat="server" ID="btnVerTodos" Text="Ver Todos" OnClick="btnVerTodos_Click" CssClass="btnAgrClienteG floatDerecha btnVerTodos" />--%>
                <asp:Button runat="server" ID="btnBuscar" OnClick="btnBuscar_Click" Text="Buscar" Style="background-position: 20px center; background-image: url('Imagenes/imgBusqueda_opt.png'); background-repeat: no-repeat; width: 140px;"  CssClass="btnAgrClienteG floatDerecha " />
                <%--<asp:Image runat="server" ID="imgBusquedaG" ImageUrl="~/Imagenes/imgBusqueda.png" CssClass="imgBusquedaG" />--%>
            </div>
            <div id="divGerentes2" style="overflow:auto;">
                <div  class="divMes">
                    <asp:ImageButton runat="server" ID="btnMenosMes" OnClick="btnMenosMes_Click" ImageUrl="~/Imagenes/left-arrow.png" CssClass="btnMenosMes"/>
                    <asp:Label runat="server" ID="lblMes" CssClass="lblMes"></asp:Label>
                    <asp:ImageButton runat="server" ID="btnMasMes" OnClick="btnMasMes_Click" ImageUrl="~/Imagenes/right-arrow.png" CssClass="btnMasMes"/>
                </div>
                <asp:GridView ID="gvGerentes" runat="server" HeaderStyle-BackColor="#5b9bd5" ShowHeader="true" AutoGenerateColumns="false"
                    CssClass="gridview" CellPadding="5" Width="100%" OnRowDataBound="gvGerentes_RowDataBound" AllowPaging ="true" PageSize="10" PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="gvGerentes_PageIndexChanging"
                    OnSorting="gvGerentes_Sorting" AllowSorting="true">
                    <AlternatingRowStyle BackColor="#f2f2f2" />
                    <PagerStyle BackColor="#CCCCCC" />
                    <RowStyle BackColor="#FFFFFF" />
                    <PagerStyle BackColor="#CCCCCC" />
                    <SortedAscendingHeaderStyle ForeColor ="WhiteSmoke" CssClass="Seleccionada"/>
                    <SortedDescendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada"/>
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
                                <asp:Label runat='server' ID='lblEstado' CssClass="lblGerentes" Text='<%# Bind("Estado") %>'></asp:Label>
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
                        <asp:TemplateField HeaderText="Ext" HeaderStyle-Width="20%" SortExpression="Extension">
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
                    </Columns>
                </asp:GridView>
            </div>
        </div>
</asp:Content>
