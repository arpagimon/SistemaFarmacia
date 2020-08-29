<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFarmacia.Master" AutoEventWireup="true" CodeBehind="Citas.aspx.cs" Inherits="SistemaFarmacia.Citas" %>
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
                    editable: true,
                    dayMaxEventRows: true, // for all non-TimeGrid views
                    views: {
                        timeGrid: {
                            dayMaxEventRows: 6 // adjust to 6 only for timeGridWeek/timeGridDay
                        }
                    },
                    events: eventos,

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
