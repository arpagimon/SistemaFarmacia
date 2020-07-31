<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFarmacia.Master" AutoEventWireup="true" CodeBehind="ConsultarCliente.aspx.cs" Inherits="SistemaFarmacia.ConsultarCliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Css/ConsultaCliente.css" type="text/css" rel="stylesheet" />
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

        <div runat="server" id="divmensaje2"  class="divMensaje" visible="false">
            <div id="MCabecera2">
                <label id="Preguntar" class="verificacion">Mensaje</label>
            </div>
            <div id="MContenido2" >
                <asp:Label runat="server" ID="LABELPREGUNTA" CssClass="lblMensaje">¿ESTA SEGURO QUE DESEA ELIMINAR EL REGISTRO?</asp:Label>
                <br />
                <asp:TextBox runat="server" ID="recupera"  Visible="false"  AutoComplete="off"/>
                <asp:Button runat="server"  ID="BCancelar"  CssClass="MBoton" Text="Cancelar" OnClick="BCancelar_Click"  Visible="false" />
                <asp:Button runat="server" OnClick="BAceptar_Click" CssClass="MBoton" Visible="false" Text="Aceptar" ID="BAceptar" />    
            </div>
        </div>

        <div runat="server" id="divFormularioG" class="FormGerente">
            <div id="FCabecera" class="FCabecera">
                <asp:Label runat="server" id="FTitulo" CssClass="FTitulo">Agregar usuario</asp:Label>
            </div>
            <asp:Panel runat="server" ID="panelMsj">
                <div id="FContenido" class="FContenido">
                    <br />
                    <label class="FGEtiqueta">Nombre: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtNombre" CssClass="FGColumna2 FGInput" AutoComplete="off" ></asp:TextBox>
                    <label class="FGEtiqueta">Apellido Paterno: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtApellidoP" CssClass="FGColumna2 FGInput" AutoComplete="off"></asp:TextBox>
                    <br />
                    <div class="divApePat">
                        <label class="FGEtiqueta">Apellido Materno: </label>
                        <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtApellidoM" CssClass="FGColumna2 FGInput" AutoComplete="off"></asp:TextBox>
                    </div>
                    <label class="FGEtiqueta">Estado:</label>                 
                    <asp:DropDownList OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="FGColumna2 FGInput" ID="ddlEstado" >
                    </asp:DropDownList>
                    <label class="FGEtiqueta">Municipio: </label>
                    <%--<asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtMunicipio" CssClass="FGColumna2 FGInput"></asp:TextBox>--%>
                    <asp:DropDownList runat="server" CssClass="FGColumna2 FGInput" ID="ddlMunicipio" Enabled="false">
                    </asp:DropDownList>
                    <br />
                    <label class="FGEtiqueta">Fecha Nacimiento: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFechaN" CssClass="FGColumna2 FGInput" TextMode="Date" AutoComplete="off"></asp:TextBox>
                    <label class="FGEtiqueta">Edad: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtEdad" CssClass="FGColumna2 FGInput" AutoComplete="off" TextMode="Number"></asp:TextBox>
                    <br />
                    <label class="FGEtiqueta">Fecha ingreso: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFechaI" CssClass="FGColumna2 FGInput" TextMode="Date" AutoComplete="off"></asp:TextBox>
                    <label class="FGEtiqueta">Medio: </label>
                    <asp:DropDownList runat="server" ID="ddlMedio" CssClass="FGColumna2 FGInput" ></asp:DropDownList>
                    <%--<asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtMedio" CssClass="FGColumna2 FGInput" ></asp:TextBox>--%>
                    <br />
                    <label class="FGEtiqueta">Telefono fijo: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtTelFijo" CssClass="FGColumna2 FGInput" AutoComplete="off" TextMode="Phone"></asp:TextBox>
                    <label class="FGEtiqueta">Extensión: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtExtension" CssClass="FGColumna2 FGInput" AutoComplete="off"></asp:TextBox>
                    <br />
                    <label class="FGEtiqueta">Celular: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtCelular" CssClass="FGColumna2 FGInput" AutoComplete="off" TextMode="Phone"></asp:TextBox>
                    <label class="FGEtiqueta">Email: </label>
                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtEmail" CssClass="FGColumna2 FGInput" AutoComplete="off" TextMode="Email"></asp:TextBox>
                    <br />
                    <div runat="server" id="divObservacionesNota">
                        <label class="FGEtiqueta">Observaciones: </label>
                        <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtObservaciones" CssClass="FGColumna2 FGInput" AutoComplete="off" TextMode="MultiLine"></asp:TextBox>
                        <label class="FGEtiqueta">Nota: </label>
                        <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtNota" CssClass="FGColumna2 FGInput" AutoComplete="off" TextMode="MultiLine"></asp:TextBox>
                        <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtIdCliente" CssClass="FGColumna2 FGInput" Visible="false"></asp:TextBox>
                        <br />

                    <label class="FGEtiqueta">Estatus: </label>
                    <asp:DropDownList runat="server" ID="ddlEstatus" CssClass="FGColumna2 FGInput" >
                        <asp:ListItem Text="Activo" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Inactivo" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    
                    </div>
                    <br />
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
                <%--<asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtBusquedaC" placeholder="Nombre/Apellido/Email/Observación" CssClass="txtBusquedaG" OnTextChanged="txtBusquedaC_TextChanged" AutoPostBack="true"></asp:TextBox>--%>
                <%--<asp:Button runat="server" ID="btnVerTodos" Text="Ver Todos" OnClick="btnVerTodos_Click" CssClass="btnAgrClienteG floatDerecha btnVerTodos" />--%>
                <asp:DropDownList runat="server" ID="ddlEstatusGeneral" OnSelectedIndexChanged="ddlEstatusGeneral_SelectedIndexChanged" AutoPostBack="true" CssClass="ddlEstatusGeneral floatDerecha">
                    <asp:ListItem Value="-1">Todos</asp:ListItem>
                    <asp:ListItem Value="0">Inactivos</asp:ListItem>
                    <asp:ListItem Value="1">Activos</asp:ListItem>
                </asp:DropDownList>
                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" OnClick="btnBuscar_Click" CssClass="btnAgrClienteG floatDerecha" />
                <asp:Image runat="server" ID="imgBusquedaG" ImageUrl="~/Imagenes/imgBusqueda.png" CssClass="imgBusquedaG" />
            </div>
            <div id="divGerentes2">
                <asp:GridView ID="gvGerentes" runat="server" HeaderStyle-BackColor="#5b9bd5" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" PagerSettings-Mode="NumericFirstLast"
                    CssClass="gridview" CellPadding="5" Width="100%" OnRowDataBound="gvGerentes_RowDataBound" OnRowEditing="gvGerentes_RowEditing" OnRowCancelingEdit="gvGerentes_RowCancelingEdit"
                    OnRowUpdated="gvGerentes_RowUpdated" OnRowDeleted="gvGerentes_RowDeleted" OnRowDeleting="gvGerentes_RowDeleting" OnPageIndexChanging="gvGerentes_PageIndexChanging" OnSorting="gvGerentes_Sorting" AllowSorting="true" >
                    <AlternatingRowStyle BackColor="#f2f2f2" />
                    <RowStyle BackColor="#FFFFFF" />
                    <PagerStyle BackColor="#CCCCCC" />
                    <SortedAscendingHeaderStyle ForeColor ="WhiteSmoke" CssClass="Seleccionada"/>
                    <SortedDescendingHeaderStyle ForeColor="WhiteSmoke" CssClass="Seleccionada"/>
                    <SortedAscendingCellStyle CssClass="Seleccionada" />
                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                    <PagerStyle CssClass="Paginador" />

                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false" >
                            
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblIdCliente' CssClass="lblUsuario" Text='<%# Bind("ID_CLIENTE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre" HeaderStyle-Width="15%" SortExpression="Nombre">
                            <ItemTemplate>
                                <asp:Label runat='server' ID='lblNombre' CssClass="lblGerentes" Text='<%# Bind("Nombre") %>' ></asp:Label>
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
                                <asp:Label runat='server' ID='lblFechaI' CssClass="lblGerentes" Text='<%# Bind("FECHA_INGRESO") %>' ></asp:Label>
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
                        
                        <asp:CommandField ShowEditButton="true"  ShowDeleteButton="false" HeaderText="Opciones" HeaderStyle-Width="15%" ShowCancelButton="true" />

                    </Columns>
                </asp:GridView>
            </div>
    


    <%--<div>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Style="height: 26px" Text="Button" />
        <asp:TextBox ID="CONSULTA" runat="server"></asp:TextBox>
        <asp:Button ID="buscar01" runat="server" Text="buscar"  OnClick="buscardatos"/>

    </div>

    <asp:GridView ID="tablaPrincipal" runat="server" AutoGenerateColumns="true" AllowPaging="true" PageSize="50">
        <%--  <Columns>
                <asp:BoundField DataField="nombre" HeaderText="nombre" />
                <asp:BoundField DataField="appellidop" HeaderText="apellidoPaterno" />
                <asp:BoundField DataField="apellidom" HeaderText="apellidoMaterno" />
                <asp:BoundField DataField="nombre" HeaderText="nombre" />
                <asp:BoundField DataField="nombre" HeaderText="nombre" />
                <asp:BoundField DataField="nombre" HeaderText="nombre" />
                <asp:BoundField DataField="nombre" HeaderText="nombre" />
                <asp:BoundField DataField="nombre" HeaderText="nombre" />
                <asp:BoundField DataField="nombre" HeaderText="nombre" />
                <asp:BoundField DataField="nombre" HeaderText="nombre" />
                <asp:BoundField DataField="nombre" HeaderText="nombre" />
                <asp:BoundField DataField="nombre" HeaderText="nombre" />
                <asp:BoundField DataField="nombre" HeaderText="nombre" />
                <asp:BoundField DataField="nombre" HeaderText="nombre" />
            </Columns>
    </asp:GridView>
    
        <div>
            <asp:Button ID="GUARDARD" runat="server" Text="GUARDAR" OnClick="GUARDARD_Click" />
        <asp:Label ID="Label1" runat="server" Text="nombre"></asp:Label> 
        <asp:TextBox ID="Cnombre" runat="server"></asp:TextBox>
         <br />
        <asp:Label ID="Label2" runat="server" Text="apellido paterno"></asp:Label>
          <asp:TextBox ID="cappellidopaterno" runat="server"></asp:TextBox>
         <br />
           <asp:Label ID="Label3" runat="server" Text="apellido materno"></asp:Label>
          <asp:TextBox ID="cappelidomaterno" runat="server"></asp:TextBox>
         <br />
           <asp:Label ID="Label4" runat="server" Text="edad"></asp:Label>
          <asp:TextBox ID="cedad" runat="server"></asp:TextBox>
         <br />
           <asp:Label ID="Label5" runat="server" Text="fecha_nacimiento"></asp:Label>
          <asp:TextBox ID="cfechanacimiento" runat="server"></asp:TextBox>
         <br />
           <asp:Label ID="Label6" runat="server" Text="fecha_ingreso"></asp:Label>
          <asp:TextBox ID="cfechaingreso" runat="server"></asp:TextBox>
         <br />
           <asp:Label ID="Label7" runat="server" Text="municipio"></asp:Label>
          <asp:TextBox ID="cmunicipio" runat="server"></asp:TextBox>
         <br />
           <asp:Label ID="Label8" runat="server" Text="calle"></asp:Label>
          <asp:TextBox ID="ccalle" runat="server"></asp:TextBox>
         <br />
           <asp:Label ID="Label9" runat="server" Text="no_int"></asp:Label>
          <asp:TextBox ID="cnoint" runat="server"></asp:TextBox>
         <br />
           <asp:Label ID="Label10" runat="server" Text="no_ext"></asp:Label>
          <asp:TextBox ID="cnoext" runat="server"></asp:TextBox>
         <br />
           <asp:Label ID="Label11" runat="server" Text="tel casa"></asp:Label>
          <asp:TextBox ID="Ctelcasafija" runat="server"></asp:TextBox>
         <br />
           <asp:Label ID="Label12" runat="server" Text="extension"></asp:Label>
          <asp:TextBox ID="cextension" runat="server"></asp:TextBox>
         <br />
           <asp:Label ID="Label13" runat="server" Text="celular"></asp:Label>
          <asp:TextBox ID="ccelular" runat="server"></asp:TextBox>
         <br />
           <asp:Label ID="Label14" runat="server" Text="email"></asp:Label>
          <asp:TextBox ID="cemail" runat="server"></asp:TextBox>
           
       



--%>

        </div>


</asp:Content>
