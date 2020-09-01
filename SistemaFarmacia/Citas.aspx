﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFarmacia.Master" AutoEventWireup="true" CodeBehind="Citas.aspx.cs" Inherits="SistemaFarmacia.Citas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link rel="stylesheet" href="Css/Calendario.css" />
    <link rel="stylesheet" href="FullCalendar/main.css" />
    <script type="text/javascript" src="FullCalendar/main.js"></script>
    <script type="text/javascript" src="FullCalendar/locales-all.js"></script>
    <script type="text/javascript">

        function cargaCalendario(eventos) {
            document.addEventListener('DOMContentLoaded', function () {
                var calendarEl = document.getElementById('calendar');

                var calendar = new FullCalendar.Calendar(calendarEl, {
                    //Tipo de vista inicial
                    initialView: 'timeGridWeek',
                    //Idiona
                    locale: 'es',
                    //Elementos de la cabecera
                    headerToolbar: {
                        left: 'prev,next today',
                        center: 'title',
                        right: 'timeGridWeek,timeGridDay'
                    },
                    //Horarios y dias laborales
                    businessHours: {
                        daysOfWeek: [1, 2, 3, 4, 5, 6]
                    },
                    editable: false,
                    dayMaxEventRows: true, // for all non-TimeGrid views
                    views: {
                        timeGrid: {
                            dayMaxEventRows: 6 // adjust to 6 only for timeGridWeek/timeGridDay
                        }
                    },
                    eventSources: [
                    {
                        events: eventos,
                    
                        color: '#cdf0ea',
                        textColor: 'black',
                    }],

                    windowResize: function (arg) {
                        calendar.render();
                    },
                    //Evento disparado en click a un dia
                    dateClick: function (info) {
                        __doPostBack("MostartDatosCitaNueva", info.dateStr);
                    },

                    eventClick: function(info) {
                        //alert('ID: ' + info.event._def.publicId);
                        __doPostBack("MostartDatosCita", info.event._def.publicId);
                    }
                });
                calendar.render();




                var calendarChico = document.getElementById('calendarioChico');
                var calendarCh = new FullCalendar.Calendar(calendarChico, {
                    initialView: 'dayGridMonth',
                    aspectRatio: 0.25,
                    height: 500,
                    locale: 'es',
                    headerToolbar: {
                        left: 'title',
                        right:  'prev,next'
                    },

                    windowResize: function (arg) {
                        calendar.render();
                    }

                });
                calendarCh.render();

            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceMensaje" runat="server">
    <div id="sombraMensaje" class="sombra" runat="server" visible="false">
        <div runat="server" id="divMensaje" class="FormGerente" visible="false">
            <div id="MCabecera" class="FCabecera">
                <label id="MTitulo" class="FTitulo">¡Alerta!</label>
            </div>
            <div id="MContenido">

                <asp:Label runat="server" ID="lblMensaje" CssClass="lblMensaje"></asp:Label>
                <br />
                <asp:Button runat="server" ID="btnCancCancelarCita" CssClass="MBoton" Text="Cancelar" OnClick="btnCancCancelarCita_Click" />
                <asp:Button runat="server" ID="btnConfCancelarCita" CssClass="MBoton" Text="Confirmar" OnClick="btnConfCancelarCita_Click" />
                <asp:Button runat="server" ID="btnCancModifCita" CssClass="MBoton" Text="Cancelar" OnClick="btnCancModifCita_Click" />
                <asp:Button runat="server" ID="btnConfModifCita" CssClass="MBoton" Text="Confirmar" OnClick="btnConfModifCita_Click" />
                <asp:Button runat="server" ID="btnCerrarMensaje" CssClass="MBoton" Text="Cerrar" OnClick="btnCerrarMensaje_Click" />
                <asp:Button runat="server" ID="btnOkSalir" CssClass="MBoton" Text="Aceptar" OnClick="btnOkSalir_Click" Visible="false" />
            </div>
        </div>

        <div runat="server" id="divFormularioCita" class="FormGerente" >
            <div id="FCabecera" class="FCabecera">
                <asp:Label runat="server" ID="FTitulo" CssClass="FTitulo">Datos de cita</asp:Label>
            </div>
            <asp:Panel runat="server" ID="panelMsj">
                <div id="FContenido" class="FContenido">
                    <asp:TextBox runat="server" ID="TxtIDCita" Visible="false" ></asp:TextBox>
                    <asp:TextBox runat="server" ID="TxtIDCliente" Visible="false" ></asp:TextBox>
                    <div runat="server" id="divSeleccionCliente" visible="false" class="divSeleccionCliente">
                        <asp:Button runat="server" ID="btnBuscarCliente" CssClass="MBoton" Text="Cliente registrado" OnClick="btnBuscarCliente_Click" />
                        <asp:Button runat="server" ID="bntNuevoCliente" CssClass="MBoton" Text="Cliente nuevo" OnClick="bntNuevoCliente_Click"/>
                    </div>
                    <div runat="server" id="divDatosCliente">
                        <div class="row justify-content-start">
                            <div class="col-4 px-2">
                                <div class="form-group">
                                    <label>Nombre</label>
                                    <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtNombre" CssClass="form-control " AutoComplete="off" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-4 px-2">
                                <div class="form-group">
                                    <label>Apellido Paterno</label>
                                    <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtApellidoP" CssClass="form-control" AutoComplete="off" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-4 px-2">
                                <div class="form-group">
                                    <label>Apellido Materno</label>
                                    <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtApellidoM" CssClass="form-control " AutoComplete="off" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row justify-content-center">
                            <div class="col-4 px-2">
                                <div class="form-group">
                                    <label>Fecha Nacimiento</label>
                                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFechaN" CssClass="form-control InpGrande" TextMode="Date" AutoComplete="off" Enabled="false" ></asp:TextBox>
                                </div>
                            </div>
                        
                            <div class="col-8 px-2">
                                <div class="form-group">
                                    <label>Email</label>
                                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtEmail" CssClass="form-control InpGrande" AutoComplete="off" Enabled="false" ></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Doctor</label>
                                <asp:DropDownList runat="server" CssClass="form-control " ID="ddlDoctorCita">
                                </asp:DropDownList>
                            </div>
                        </div>
                        
                        <div class="col px-2">
                            <div class="form-group">
                                <label>Fecha</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFechaCita" CssClass="form-control InpGrande" AutoComplete="off" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col px-2">
                            <div class="form-group">
                                <label>Hora inicio</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtHoraInicio" CssClass="form-control InpMediano" AutoComplete="off" TextMode="Time"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col px-2">
                            <div class="form-group">
                                <label>Hora fin</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtHoraFin" CssClass="form-control InpMediano" AutoComplete="off" TextMode="Time"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-12 px-2">
                            <div class="form-group">
                                <label>Nota</label>
                                <asp:TextBox runat="server" CssClass="form-control " ID="txtNotasCita" TextMode="MultiLine">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col px-2 text-center FBotonera">
                            <asp:Button runat="server" ID="btnCancelarCita" CssClass="FGBoton" Text="Cancelar cita" OnClick="btnCancelarCita_Click" />
                            <asp:Button runat="server" ID="btnModificarCita" CssClass="FGBoton" Text="Modificar cita" OnClick="btnModificarCita_Click" />
                            <asp:Button runat="server" ID="btnAgendarCita" CssClass="FGBoton" Text="Agendar cita" OnClick="btnAgendarCita_Click" />
                            <asp:Button runat="server" ID="btnCerrar" CssClass="FGBoton" Text="Cerrar" OnClick="btnCerrar_Click" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>

        <div runat="server" id="divFormBusqCliente" class="FormClienteCorreo FormGerente" visible="false">
            <div id="divFormCliCabecera" class="FCabecera">
                <label id="lblFormCliTitulo" class="FTitulo">Buscar cliente</label>
            </div>
            <asp:Panel runat="server" ID="panel1">
                <div id="FormCliContenido" class="FContenido FCCContenido" runat="server">
                    <div class="row justify-content-start">
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Nombre</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtBusqCliNombre" CssClass="form-control " AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Apellido Paterno</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtBusqCliApellidoP" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Apellido Materno</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtBusqCliApellidoM" CssClass="form-control " AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-center">
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Fecha Nacimiento</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtBusqCliFechaN" CssClass="form-control InpGrande" TextMode="Date" AutoComplete="off" ></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-8 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Email</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtBusqCliEmail" CssClass="form-control FGInput FGCliente InpGrande" AutoComplete="off" ></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-center">
                        <div class="col-6 px-2">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblBusqCliError" CssClass="lblError"></asp:Label>
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
                <div id="divClienteResultados" class="FContenido FCCContenido" runat="server" visible="false">
                    <h5>Selecciona un cliente</h5>
                    <asp:GridView ID="gvClientes" runat="server" HeaderStyle-BackColor="#5b9bd5" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="6" PagerSettings-Mode="NumericFirstLast"
                    CssClass="gridview2" CellPadding="5" Width="100%" OnRowDataBound="gvClientes_RowDataBound" OnPageIndexChanging="gvClientes_PageIndexChanging" >
                        <AlternatingRowStyle BackColor="#f2f2f2" />
                        <RowStyle BackColor="#FFFFFF" />
                        <PagerStyle BackColor="#CCCCCC" />
                        <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                        <PagerStyle CssClass="Paginador" />
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="0%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat='server' ID='lblIdCliente' CssClass="lblUsuario" Text='<%# Bind("ID_CLIENTE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnSeleccionar" ToolTip="Seleccionar" runat="server" Text="Seleccionar" OnClick="btnSeleccionar_Click" AutoPostBack="true" />
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
                        
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </div>


        <div runat="server" id="divFormularioCliente" class="FormGerente" visible="false">
            <div id="FormCliCabecera" class="FCabecera">
                <asp:Label runat="server" ID="lblFormCliCabecera" CssClass="FTitulo">Agregar usuario</asp:Label>
            </div>
            <div id="divFormCliContenido" class="FContenido">
                <div class="row justify-content-start">
                    <div class="col-4 px-2">
                        <div class="form-group">
                            <label>Nombre</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="txtFormCliNombre" CssClass="form-control " AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-4 px-2">
                        <div class="form-group">
                            <label>Apellido Paterno</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="txtFormCliApePat" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-4 px-2">
                        <div class="form-group">
                            <label>Apellido Materno</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="txtFormCliApeMat" CssClass="form-control " AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row justify-content-center">
                    <div class="col-4 px-2">
                        <div class="form-group">
                            <label>Fecha Ingreso</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliFecIngreso" CssClass="form-control InpGrande" TextMode="Date" AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-4 px-2">
                        <div class="form-group">
                            <label>Fecha Nacimiento</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliFecNac" CssClass="form-control InpGrande" TextMode="Date" AutoComplete="off" OnTextChanged="txtFormCliFecNac_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Edad</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFormCliEdad" CssClass="form-control-edad" AutoComplete="off" TextMode="Number" Enabled="false"></asp:TextBox>
                            </div>
                    </div>
                </div>

                <div class="row justify-content-start">
                    <div class="col-3 px-2">
                        <div class="form-group">
                            <label>Telefono Fijo</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliTelFijo" CssClass="form-control InpMediano" AutoComplete="off" TextMode="Phone"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-2 px-2">
                        <div class="form-group">
                            <label>Ext.</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliExtension" CssClass="form-control InpExt" AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-3 px-2">
                        <div class="form-group">
                            <label>Celular</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliCelular" CssClass="form-control InpMediano " AutoComplete="off" TextMode="Phone"></asp:TextBox>
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
                            <asp:DropDownList runat="server" CssClass="form-control InpMediano" ID="ddlFormCliEnviarCorreo">
                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col px-2">
                        <div class="form-group">
                            <label>Email</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliEmail" CssClass="form-control InpExtGrande" AutoComplete="off" TextMode="Email"></asp:TextBox>
                        </div>
                    </div>
                        
                    <div class="col px-2">
                        <div class="form-group">
                            <label>Factura:</label>
                            <asp:DropDownList runat="server" CssClass="form-control InpChico ddlFormCliFactura" OnSelectedIndexChanged="ddlFormCliFactura_SelectedIndexChanged" AutoPostBack="true" ID="ddlFormCliFactura">
                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                            </asp:DropDownList>
                            <div id="divBtnDatosF" class="divBtnDatosF" runat="server" visible="false">
                                <asp:LinkButton ID="btnAddDatosF" ToolTip="Añadir datos de facturación" OnClick="btnAddDatosF_Click" CssClass="btnAgrClienteG AddDatosF" runat="server" Text="<i class='fa fa-file-text-o' aria-hidden='true'></i><i class='fa fa-plus' aria-hidden='true'></i>" />
                            </div>
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
                            <div runat="server" id="divMunicipio" class="divCelda">
                                <label class="FGEtiqueta">Municipio: </label>
                                <asp:DropDownList runat="server" CssClass="form-control " ID="ddlFormCliMunicipio" Enabled="false">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div runat="server" id="divPais" visible="false">
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

                <div class="row justify-content-start">
                    <div class="col px-2">
                        <div class="form-group" runat="server" id="divObservacionesNota">
                            <label for="exampleFormControlTextarea1">Observaciones</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtFormCliObservaciones" CssClass="form-control" AutoComplete="off" TextMode="MultiLine">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row justify-content-start">
                    <div class="col-12 px-2">
                        <div class="form-group">
                            <label for="exampleFormControlTextarea1">Notas</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtFormCliNota" CssClass="form-control" AutoComplete="off" TextMode="MultiLine"></asp:TextBox>
                            <%--<asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliNota" CssClass="txtObsNota" Visible="false"></asp:TextBox>--%>
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

                <div class="row justify-content-center">
                    <div class="col px-2 text-center FBotonera">
                        <asp:Button runat="server" ID="FormCliCancelar" CssClass="FGBoton" Text="Cancelar" OnClick="FormCliCancelar_Click" />
                        <asp:Button runat="server" ID="FormCliAgregar" CssClass="FGBoton" Text="Agregar" OnClick="FormCliAgregar_Click" />
                    </div>
                </div>
                <br />
            </div>
        </div>

        <div id="divMensajeDF" class="FormGerente" runat="server" visible="false">
            <div id="MDFCabecera" class="FCabecera">
                <label id="MDFTitulo">Cliente - Datos de Facturación</label>
            </div>
            <div id="MDFContenido">
                <div class="row">
                    <div class="col-12 px-2">
                            <div class="form-group">
                                <label>Nombre o Razón Social</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtNombRDFa" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                            </div>
                    </div>

                    <div class="col-4 px-2">
                        <div class="form-group">
                            <label>RFC</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtRfcDFa" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                                
                        </div>
                    </div>
                    <div class="col-4 px-2">
                        <div class="form-group">
                            <label>Entidad</label>
                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlEntidadDFa">
                                <asp:ListItem Value="1" Text="Persona moral"></asp:ListItem>
                                <asp:ListItem Value="0" Text="Persona física"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12 px-2">
                        <hr style="color:#e0e0e0" />
                        <h5>Dirección Fiscal</h5>
                    </div>
                    <div class="col-4 px-2">
                        <div class="form-group">
                            <label class="FGEtiqueta">Calle</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtCalleDFa" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>   
                    <div class="col-3 px-2">
                        <div class="form-group">
                            <label class="FGEtiqueta">No. Exterior</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtNoExtDFa" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>                         
                    <div class="col-3 px-2">
                        <div class="form-group">
                            <label class="FGEtiqueta">No. Interior</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtNoIntDFa" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-4 px-2">
                        <div class="form-group">
                            <label class="FGEtiqueta">Colonia</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtColoniaDFa" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-4 px-2">
                        <div class="form-group">
                            <label class="FGEtiqueta">Estado</label>
                            <asp:DropDownList OnSelectedIndexChanged="ddlEstadoDFa_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control " ID="ddlEstadoDFa">
                        </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-4 px-2" id="divMunicipioDFa" runat="server">
                        <div class="form-group">
                            <label class="FGEtiqueta">Municipio</label>
                            <asp:DropDownList runat="server" CssClass="form-control " ID="ddlMunicipioDFa" Enabled="false">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-4 px-2" id="divPaisDFa" runat="server" visible="false">
                        <div class="form-group">
                            <label class="FGEtiqueta">País</label>
                            <asp:DropDownList runat="server" CssClass="form-control InpGrande" ID="ddlPaisDFa">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-3 px-2">
                        <div class="form-group">
                            <label class="FGEtiqueta">Código Postal</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtCPdfA" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row justify-content-center">
                        <div class="col-6 px-2">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblErrorDF" CssClass="lblError"></asp:Label>
                            </div>
                        </div>
                    </div>
                <div class="row justify-content-center">
                    <div class="col px-2 text-center FBotonera">
                        <asp:Button runat="server" ID="btnCancelaDF" CssClass="FGBoton" Text="Cancelar" OnClick="btnCancelaDF_Click" />
                        <asp:Button runat="server" ID="btnLimpiaDF" CssClass="FGBoton" Text="Limpiar" OnClick="btnLimpiaDF_Click" />
                        <asp:Button runat="server" ID="btnGuardaDF" CssClass="FGBoton" Text="Aceptar" OnClick="btnGuardaDF_Click" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceBody" runat="server">    
    <div class="divContenedorPrincipal row justify-content-start">
        <div id="divIzquierdo" class="col-4">
            <h4 class="hDoctor">Doctor</h4>
            <asp:DropDownList runat="server" ID="ddlDoctor" OnSelectedIndexChanged="ddlDoctor_SelectedIndexChanged" CssClass="ddlDoctor"></asp:DropDownList>
            <div id="calendarioChico" class="divCalendarioChico"></div>
        </div>
        <div id='calendar' class="divCalendario col-8"></div>
    </div>
</asp:Content>
