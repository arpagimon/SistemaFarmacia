<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFarmacia.Master" AutoEventWireup="true" CodeBehind="Citas.aspx.cs" Inherits="SistemaFarmacia.Citas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>--%>
    <%--<script type="text/javascript" src="js/modal.js"></script>--%>

    <%--<script type="text/javascript" >
        function createEvent(start, end, resource) {
            var modal = new DayPilot.Modal();
            modal.border = "10px solid #ccc";
            modal.closed = function () {
                dps.clearSelection();
                if (this.result == "OK") {
                    dps.commandCallBack('refresh');
                }
            }; modal.showUrl("New.aspx?start=" + start.toStringSortable() + "&end=" + end.toStringSortable() + "&r=" + resource);
        }
    </script>
    <style>
        .calendario{
            float:right;
        }
    </style>--%>
    
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
                    dateClick: function () {
                        alert('a day has been clicked!');
                    },

                    eventClick: function(info) {
                        alert('ID: ' + info.event._def.publicId);
                        $("#<%= sombraMensaje.ClientID %>").show();
                        $("#<%= btnAgendarCita.ClientID %>").hide();
                    }

                });
                calendar.render();
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceMensaje" runat="server">
    <div id="sombraMensaje" class="sombra" runat="server">
        <div runat="server" id="divFormularioCita" class="FormGerente" >
            <div id="FCabecera" class="FCabecera">
                <asp:Label runat="server" ID="FTitulo" CssClass="FTitulo">Datos de cita</asp:Label>
            </div>
            <asp:Panel runat="server" ID="panelMsj">
                <div id="FContenido" class="FContenido">
                    <div class="row justify-content-start">
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Nombre</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtNombre" CssClass="form-control " AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Apellido Paterno</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtApellidoP" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Apellido Materno</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" onkeyup="this.value = this.value.toUpperCase();" runat="server" ID="TxtApellidoM" CssClass="form-control " AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                    <div class="row justify-content-center">
                        
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Fecha Nacimiento</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFechaN" CssClass="form-control InpGrande" TextMode="Date" AutoComplete="off" ></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="col px-2">
                            <div class="form-group">
                                <label>Email</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtEmail" CssClass="form-control InpExtGrande" AutoComplete="off" TextMode="Email"></asp:TextBox>
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
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFechaCita" CssClass="form-control InpChico" AutoComplete="off" TextMode="Email"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col px-2">
                            <div class="form-group">
                                <label>Hora inicio</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtHoraInicio" CssClass="form-control InpChico" AutoComplete="off" TextMode="Time"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col px-2">
                            <div class="form-group">
                                <label>Hora fin</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtHoraFin" CssClass="form-control InpChico" AutoComplete="off" TextMode="Time"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col px-2 text-center FBotonera">
                            <asp:Button runat="server" ID="btnCancelarCita" CssClass="FGBoton" Text="Cancelar cita" OnClick="btnCancelarCita_Click" />
                            <asp:Button runat="server" ID="btnAgendarCita" CssClass="FGBoton" Text="Agendar cita" OnClick="btnAgendarCita_Click" />
                            <asp:Button runat="server" ID="btnCerrar" CssClass="FGBoton" Text="Cerrar" OnClick="btnCerrar_Click" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceBody" runat="server">    
    <div>
        
    </div>
    <div id='calendar' class="divCalendario"></div>
</asp:Content>
