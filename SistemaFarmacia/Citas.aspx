<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFarmacia.Master" AutoEventWireup="true" CodeBehind="Citas.aspx.cs" Inherits="SistemaFarmacia.Citas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" href="Css/Calendario.css" />
    <link rel="stylesheet" href="FullCalendar/main.css" />
    <script type="text/javascript" src="FullCalendar/main.js"></script>
    <script type="text/javascript" src="FullCalendar/locales-all.js"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script type="text/javascript">



        function cargaCalendario(eventos, eventosBloqueos, tipoVista, eventosChico, fechaSeleccionada) {
            //alert(fechaSeleccionada);
            document.addEventListener('DOMContentLoaded', function () {
                var calendarEl = document.getElementById('calendar');

                var calendar = new FullCalendar.Calendar(calendarEl, {
                    //Tipo de vista inicial
                    initialView: tipoVista,
                    //defaultDate: fechaSeleccionada,
                    //Idiona
                    locale: 'es',
                    //Elementos de la cabecera
                    headerToolbar: {
                        left: 'customPrevButton,customNextButton customDayButton',
                        center: 'title',
                        right: 'timeGridWeek,timeGridDay'
                    },
                    //Horarios y dias laborales
                    //businessHours: {
                    //    daysOfWeek: [1, 2, 3, 4, 5, 6]
                    //},
                    allDaySlot: false,
                    editable: false,
                    dayMaxEventRows: true, // for all non-TimeGrid views
                    views: {
                        timeGrid: {
                            dayMaxEventRows: 6 // adjust to 6 only for timeGridWeek/timeGridDay
                        }
                    },
                    customButtons: {
                        customPrevButton: {

                            text: '<',
                            click: function () {
                                calendar.prev();
                                var date = calendar.getDate();
                                var fecha1 = date.toISOString().slice(0, 10);

                                calendarCh.changeView('dayGridMonth', fecha1);
                            }
                        }
                        ,
                        customNextButton: {
                            text: '>',
                            click: function () {
                                calendar.next();

                                var date = calendar.getDate();
                                var fecha1 = date.toISOString().slice(0, 10);

                                calendarCh.changeView('dayGridMonth', fecha1);

                            }
                        },
                        customDayButton: {
                            text: 'Hoy',

                            click: function () {
                                calendar.today();
                                var date = calendar.getDate();
                                var fecha1 = date.toISOString().slice(0, 10);

                                calendarCh.changeView('dayGridMonth', fecha1);
                            }
                        }
                    },

                    eventSources: [
                        {
                            events: eventos,

                            color: '#cdf0ea',
                            textColor: 'black',
                            borderColor: '#8db0aa'
                        },
                        {
                            events: eventosBloqueos,
                            color: '#ffc3c3',
                            textColor: 'black',
                            borderColor: '#8db0aa'
                        }],

                    windowResize: function (arg) {
                        calendar.render();
                    },
                    //Evento disparado en click a un dia
                    dateClick: function (info) {
                        __doPostBack("MostartDatosCitaNueva", info.dateStr);
                    },

                    eventClick: function (info) {
                        //alert('ID: ' + info.event._def.publicId);
                        __doPostBack("MostartDatosCita", info.event._def.publicId);
                    }
                });
                calendar.render();
                calendar.gotoDate(fechaSeleccionada);



                var calendarChico = document.getElementById('calendarioChico');
                var calendarCh = new FullCalendar.Calendar(calendarChico, {
                    initialView: 'dayGridMonth',
                    aspectRatio: 0.25,
                    height: 500,
                    locale: 'es',
                    headerToolbar: {
                        left: 'title',
                        right: 'prev,next'
                    },
                    dateClick: function (info) {

                        var fecha = info.dateStr;
                        //alert('a day has been clicked!' + info.dateStr);


                        calendar.changeView('timeGridDay', fecha);

                    },
                    events: eventosChico,


                    windowResize: function (arg) {
                        calendar.render();
                    }

                });
                calendarCh.render();
                calendarCh.gotoDate(fechaSeleccionada);
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
                <asp:Button runat="server" ID="btnContCitas" CssClass="MBoton" Text="Continuar" OnClick="btnContCitas_Click" />
                <asp:Button runat="server" ID="btnCerrarMensaje" CssClass="MBoton" Text="Cerrar" OnClick="btnCerrarMensaje_Click" />
                <asp:Button runat="server" ID="btnOkSalir" CssClass="MBoton" Text="Aceptar" OnClick="btnOkSalir_Click" Visible="false" />
                <asp:Button runat="server" ID="btnOKClienteGuardado" CssClass="MBoton" Text="Cerrar" OnClick="btnOKClienteGuardado_Click" />
            </div>
        </div>

        <div runat="server" id="divFormularioCita" class="FormGerente">
            <div id="FCabecera" class="FCabecera">
                <asp:Button Text="Datos de cita" runat="server" CssClass="btnPestana" ID="btnPestanaDatosCita" OnClick="btnPestanaDatosCita_Click" />
                <asp:Button Text="Resultado de cita" runat="server" CssClass="btnPestana" ID="btnPestanaResulCita" OnClick="btnPestanaResulCita_Click" />
                <asp:Button Text="Historial clínico" runat="server" CssClass="btnPestana" ID="btnPestanaHistorial" OnClick="btnPestanaHistorial_Click" />
                <asp:Button Text="Nota de evolución" runat="server" CssClass="btnPestana" ID="btnPestanaNotaEvol" OnClick="btnPestanaNotaEvol_Click" />
                <%--<asp:Label runat="server" ID="FTitulo" CssClass="FTitulo">Datos de cita</asp:Label>--%>
            </div>
            <asp:Panel runat="server" ID="panelMsj">
                <div id="divFContenidoDatoCita" class="FContenido" runat="server">
                    <asp:TextBox runat="server" ID="TxtIDCita" Visible="false"></asp:TextBox>
                    <asp:TextBox runat="server" ID="TxtIDCliente" Visible="false"></asp:TextBox>
                    <div runat="server" id="divSeleccionCliente" visible="false" class="divSeleccionCliente">
                        <asp:Button runat="server" ID="btnBuscarCliente" CssClass="MBoton2" Text="Cliente registrado" OnClick="btnBuscarCliente_Click" />
                        <asp:Button runat="server" ID="bntNuevoCliente" CssClass="MBoton2" Text="Cliente nuevo" OnClick="bntNuevoCliente_Click" />
                        <asp:Button runat="server" ID="btnOpcionInhabilitar" CssClass="MBoton2" Text="Inhabilitar horario" OnClick="btnOpcionInhabilitar_Click" />
                    </div>
                    <div runat="server" id="divDatosCliente" class="ajustediv2">
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
                                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtFechaN" CssClass="form-control InpGrande" TextMode="Date" AutoComplete="off" Enabled="false"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-8 px-2">
                                <div class="form-group">
                                    <label>Email</label>
                                    <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtEmail" CssClass="form-control InpGrande" AutoComplete="off" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row justify-content-center ajustediv2">
                        <div class="col-8 px-2">
                            <div class="form-group">
                                <label>Doctor</label>
                                <asp:DropDownList runat="server" CssClass="form-control " ID="ddlDoctorCita">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Fecha</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFechaCita" CssClass="form-control InpGrande" AutoComplete="off" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Hora inicio</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtHoraInicio" CssClass="form-control InpMediano" AutoComplete="off" TextMode="Time"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-4 px-2">
                            <div class="form-group">
                                <label>Hora fin</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtHoraFin" CssClass="form-control InpMediano" AutoComplete="off" TextMode="Time"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row justify-content-center ajustediv">
                        <div class="col-12 px-2">
                            <div class="form-group">
                                <label>Nota</label>
                                <asp:TextBox runat="server" CssClass="form-control " ID="txtNotasCita" TextMode="MultiLine">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-12 px-2">
                            <asp:Label runat="server" ID="lblErrorFormCita" CssClass="lblError"></asp:Label>
                        </div>
                        <div class="col px-2 text-center FBotonera">
                            <asp:Button runat="server" ID="btnCancelarCita" CssClass="FGBoton" Text="Cancelar cita" OnClick="btnCancelarCita_Click" />
                            <asp:Button runat="server" ID="btnModificarCita" CssClass="FGBoton" Text="Modificar cita" OnClick="btnModificarCita_Click" />
                            <asp:Button runat="server" ID="btnAgendarCita" CssClass="FGBoton" Text="Agendar cita" OnClick="btnAgendarCita_Click" />
                            <asp:Button runat="server" ID="btnInhabiliarHorario" CssClass="FGBoton" Text="Inhabilitar horario" OnClick="btnInhabiliarHorario_Click" />
                            <asp:Button runat="server" ID="btnCerrar" CssClass="FGBoton" Text="Cerrar" OnClick="btnCerrar_Click" />
                        </div>
                    </div>
                </div>
                <div id="Div3" class="sombra" runat="server" visible="false">
                    <div runat="server" id="divmensaje2" class="divMensaje FormGerente" visible="false">
                        <div id="MCabecera2" class="row justify-content-center FCabecera ajustediv3">

                            <div class="col-12 px-2">
                                <label id="Preguntar" class="verificacion ">¡Alerta!</label>
                            </div>
                        </div>
                        <div id="MContenido2 justify-content-center divcentrar">
                            <asp:Label runat="server" ID="lblPregunta" CssClass="lblMensaje lblMensajeB0">¿Está seguro que desea eliminar el registro ? </asp:Label><br />
                            <asp:Label runat="server" ID="lblpregunta2" CssClass="lblMensaje lblMensajeT0"></asp:Label>
                            <br />
                            <asp:TextBox runat="server" ID="recupera" Visible="false" AutoComplete="off" />
                            <asp:TextBox runat="server" ID="recuperaId" Visible="false" AutoComplete="off" />
                            <div class="divcentrar">
                                <asp:Button runat="server" ID="eliminarCancelar" CssClass="MBoton" Text="Cancelar" OnClick="eliminarCancelar_Click" Visible="true" />
                                <asp:Button runat="server" CssClass="MBoton" Visible="true" Text="Eliminar" ID="eliminarAceptar" OnClick="eliminarAceptar_Click" />
                            </div>
                        </div>
                    </div>


                    <div runat="server" id="div4" class="divMensaje FormGerente" visible="false">
                        <div id="MCabeceraEDITAR" class="row  justify-content-center FCabecera">


                            <div class="col-12 px-2">
                                <label id="PreguntarEdicion" class="verificacion">EDITAR</label>
                            </div>
                        </div>
                        <div id="MContenidoActualizar" class="divcentrar">
                            <asp:Label runat="server" ID="Label1" CssClass="lblMensaje lblMensajeB0"> </asp:Label><br />
                            <asp:Label runat="server" ID="Label2" CssClass="lblMensaje lblMensajeT0"></asp:Label>
                            <br />
                            <div class="row justify-content-center">

                                <div class="col-6 px-2">
                                    <div class="form-group">
                                        <label>Medicamento</label>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="MedicamentoACt" TextMode="MultiLine"></asp:TextBox>

                                    </div>
                                </div>



                                <div class="col-6 px-2">
                                    <div class="form-group">
                                        <label>Indicaciones</label>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="IndicacionesAct" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                                
                            </div>
                            <div class="row justify-content-center">

                                <div class="col-4 px-2">
                                    <div class="form-group">
                                        <label>Dosis</label>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="DosisAct" TextMode="MultiLine" Rows="1"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4 px-2">
                                    <label>Frecuencia</label>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="FrecuenciaAct" TextMode="MultiLine" Rows="1"></asp:TextBox>
                                </div>
                                <div class="col-4 px-2">
                                    <label>Periodo</label>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="PeriodoACt" TextMode="MultiLine" Rows="1"></asp:TextBox>
                                </div>


                            </div>

                            <asp:TextBox runat="server" CssClass="form-control" ID="medicamentoref" TextMode="MultiLine" Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" CssClass="form-control" ID="indicacionRef" TextMode="MultiLine" Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" CssClass="form-control" ID="dosisref" TextMode="MultiLine" Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" CssClass="form-control" ID="frecuenciaref" TextMode="MultiLine" Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" CssClass="form-control" ID="periodoref" TextMode="MultiLine" Visible="false"></asp:TextBox>
                            <asp:Button runat="server" ID="btnCancelaraAct" CssClass="MBoton" Text="Cancelar" OnClick="btnCancelaraAct_Click" Visible="true" />
                            <asp:Button runat="server" CssClass="MBoton" Visible="true" Text="actualizar" ID="btnActualizar" OnClick="btnActualizar_Click" />

                        </div>
                    </div>

                    <div runat="server" id="div5" class="divMensaje FormGerente" visible="false">

                        <div id="MCabeceraexito" class="row  justify-content-center FCabecera">
                        </div>
                        <div id="MContenidoexito" class=" justify-content-center divcentrar">
                            <asp:Label runat="server" ID="Label3" CssClass="lblMensaje lblMensajeB0">Guardado Exitoso!</asp:Label><br />
                            <asp:Label runat="server" ID="Label4" CssClass="lblMensaje lblMensajeT0"></asp:Label>
                            <br />

                            <asp:Button runat="server" CssClass="MBoton" Visible="true" Text="Aceptar" ID="btnAceptar" OnClick="btnAceptar_Click" />
                        </div>

                    </div>

                    <div runat="server" id="div6" class="divMensaje FormGerente" visible="false">

                        <div id="advertencia1" class="row  justify-content-center FCabecera">
                            <label id="PreguntaReceta" class="verificacion ">¡Alerta!</label>
                        </div>
                        <div id="MCadvertencia2" class=" justify-content-center divcentrar">
                            <asp:Label runat="server" ID="Label5" CssClass="lblMensaje lblMensajeB0">La receta no esta definida</asp:Label><br />
                            <asp:Label runat="server" ID="Label6" CssClass="lblMensaje lblMensajeT0"></asp:Label>
                            <br />

                            <asp:Button runat="server" CssClass="MBoton" Visible="true" Text="Aceptar" ID="btnReceta" OnClick="btnAceptar_Click" />
                        </div>

                    </div>

                    <div runat="server" id="divProxCita" class="divMensaje FormGerente" visible="false">

                        <div id="cabeceraProxCita" class="row  justify-content-center FCabecera">
                            <label id="tituloProxCita" class="verificacion ">Agendar Próxima Cita</label>
                        </div>
                        <div id="MsjProxCita" class="divcentrar">
                            <div class="row justify-content-start">
                                <div class="col-4 px-2">
                                    <div class="form-group">
                                        <label>Fecha</label>
                                        <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFechaProxCitaAg" CssClass="form-control" AutoComplete="off" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-4 px-2">
                                    <div class="form-group">
                                        <label>Hora inicio</label>
                                        <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtHoraIProxCitaAg" CssClass="form-control" AutoComplete="off" TextMode="Time"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-4 px-2">
                                    <div class="form-group">
                                        <label>Hora fin</label>
                                        <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtHoraFProxCitaAg" CssClass="form-control" AutoComplete="off" TextMode="Time"></asp:TextBox>
                                    </div>
                                </div>                        
                            </div>
                            <div class="row justify-content-center ajustediv">
                                <div class="col-12 px-2">
                                    <div class="form-group">
                                        <label>Nota</label>
                                        <asp:TextBox runat="server" CssClass="form-control " ID="txtNotaCitaAg" TextMode="MultiLine">
                                        </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row justify-content-start ajustediv">
                                <div class="col-12 px-2">
                                    <asp:label ID="MsjErrorCitaProx" runat="server"></asp:label>
                                </div>
                            </div>
                        </div>
                        <div id="PieProxCita" class="modal-footer">
                            <asp:Button runat="server" CssClass="MBoton" Visible="true" Text="Cancelar" ID="btnCancelaProxCita" OnClick="btnCancelaProxCita_Click" />
                            <asp:Button runat="server" CssClass="MBoton" Visible="true" Text="Aceptar" ID="btnAgendaCitaProx" OnClick="btnAgendaCitaProx_Click" />
                        </div>
                    </div>

                    <div runat="server" id="divMsjOKcitaProx" class="divMensaje FormGerente" visible="false">
                        <div id="MsjCabeceraCita" class="row justify-content-center FCabecera">
                            <label id="MsjTituloCita" class="FTitulo">¡Alerta!</label>
                        </div>
                        <div id="MsjContenidoCita" class=" justify-content-center divcentrar">

                            <asp:Label runat="server" ID="lblMsjCitaProx" CssClass="lblMensaje" Text="Cita Agendada Exitosamente"></asp:Label>
                            <br />
                            <asp:Button runat="server" ID="btnOkCitaProx" CssClass="MBoton" Text="Aceptar" OnClick="btnOkCitaProx_Click" />
                        </div>
                    </div>
                </div>


                <div id="divFContenidoResulCita" class="FContenido ajustediv" runat="server" visible="false">
                    <div class="row justify-content-start px-2">
                        <label>Objetivo</label>
                    </div>
                    <div class="row justify-content-start px-2">
                        <asp:TextBox runat="server" ID="txtObjetivo" TextMode="MultiLine" CssClass="form-control txtExtGrande" Rows="1"></asp:TextBox>
                    </div>
                    <div class="row justify-content-start px-2">
                        <label>Sintomas</label>
                    </div>
                    <div class="row justify-content-start px-2">
                        <asp:TextBox runat="server" ID="txtSintomas" TextMode="MultiLine" CssClass="form-control txtExtGrande" Rows="1"></asp:TextBox>
                    </div>
                    <div class="row justify-content-start px-2">
                        <label>Receta</label>
                    </div>

                    <div class="row justify-content-center">
                        <div class="col-12 px-2">
                            <asp:GridView ID="GridViewReceta" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="6" PagerSettings-Mode="NumericFirstLast"
                                CssClass="gridview2" CellPadding="5" class="" Width="100%" OnRowDeleting="linkeliminar_Click" OnRowEditing="GridViewReceta_RowEditing">
                                <AlternatingRowStyle BackColor="#f9c3f7" />
                                <RowStyle BackColor="#f9def8" />
                                <PagerStyle BackColor="#CCCCCC" />
                                <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                                <PagerStyle CssClass="Paginador" />
                                <Columns>
                                    <asp:TemplateField HeaderText="MEDICAMENTO" HeaderStyle-Width="0%" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label runat='server' ID='medicamentoId' CssClass="lblUsuario" Text='<%# Bind("medicamento") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Indicaciones">
                                        <ItemTemplate>
                                            <asp:Label runat='server' ID='indicacionesId' CssClass="lblEnviarCorreo" Text='<%# Bind("indicaciones") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Dosis'>
                                        <ItemTemplate>
                                            <asp:Label runat='server' ID='dosisId' CssClass="lblEnviarCorreo" Text='<%# Bind("dosis") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Frecuencia'>
                                        <ItemTemplate>
                                            <asp:Label runat='server' ID='frecuenciaId' CssClass="lblEnviarCorreo" Text='<%# Bind("frecuencia") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Periodo'>
                                        <ItemTemplate>
                                            <asp:Label runat='server' ID='periodoId' CssClass="lblEnviarCorreo" Text='<%# Bind("periodo") %>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Opciones'>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="linkeditar" ToolTip="Editar" OnClick="linkeditar_Click" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-pencil-square-o' aria-hidden='true'></i>" />

                                            <asp:LinkButton ID="linkeliminar" ToolTip="Eliminar" OnClick="linkeliminar_Click" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-trash' aria-hidden='true'></i>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-6 px-2">
                            <label>Medicamento</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="TextBoxMedicamento" TextMode="MultiLine"  Rows="1"></asp:TextBox>
                        </div>
                        <div class="col-6 px-2">
                            <label>Indicaciones</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="TextBoxIndicaciones" TextMode="MultiLine" Rows="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-4 px-2">
                            <label>Dosis</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="TextBoxDosis" TextMode="MultiLine" Rows="1"></asp:TextBox>
                        </div>
                        <div class="col-4 px-2">
                            <label>Frecuencia</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="TextFrecuencia" TextMode="MultiLine" Rows="1"></asp:TextBox>
                        </div>
                        <div class="col-4 px-2">
                            <label>Periodo</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="TextPeriodo" TextMode="MultiLine" Rows="1"></asp:TextBox>
                        </div>
                    </div>
                  <div class="row justify-content-center">
                        <div class="col-12 px-2 divcentrar">
                            <div class="form-group">
                                <asp:Button runat="server" OnClick="plusClick_Click" ID="plusClick" Class="btn-circle btnbrodercircle" Style="background-position: 6px center; background-image: url('Imagenes/plus2.png'); background-repeat: no-repeat;" />
                            </div>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2">
                        <label>Indicaciones generales</label>
                    </div>
                    <div class="row justify-content-start px-2">
                        <asp:TextBox runat="server" ID="txtIndicaciones" TextMode="MultiLine" CssClass="form-control txtExtGrande"></asp:TextBox>
                    </div>
                    <div class="row justify-content-start px-2">
                        <label>Adjuntar archivos</label>
                    </div>

                    <div class="row justify-content-start px-2">
                        <div class="col-6 px-2">
                            <asp:FileUpload runat="server" ID="FUploadResultCita" CssClass="txtExtGrande ParamInput" accept=".pdf" />
                        </div>
                        <div class="col-3 px-2 divcentrar">
                            <asp:Button runat="server" OnClick="plusClickArcivo_Click" ID="plusClickArcivo" Class="btn-circle btnbrodercircle" Style="background-position: 6px center; background-image: url('Imagenes/plus2.png'); background-repeat: no-repeat;" />
                        </div>
                        <div class="col-3 px-2">
                            <asp:LinkButton runat="server" OnClick="btnVerArchivos_Click" ID="btnVerArchivos" CssClass="btnVeArchivos FGBoton" Text="Mostrar archivos <i class='fa fa-arrow-circle-right' aria-hidden='true'></i>" ToolTip="Mostrar Archivos" />
                        </div>
                    </div>

                    <div class="col-12 px-2">
                        <asp:Label runat="server" ID="lblResulCitaError" CssClass="lblError"></asp:Label>
                    </div>
                    <div class="row justify-content-center" id="divGridArchivos" visible="false" runat="server">
                        <div class="col-12 px-2">
                            <asp:GridView ID="GridVieArchivo" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="6" PagerSettings-Mode="NumericFirstLast"
                                CssClass="gridview2" CellPadding="5" class="" Width="100%" OnRowEditing="GridViewReceta_RowEditing">
                                <AlternatingRowStyle BackColor="#f9c3f7" />
                                <RowStyle BackColor="#f9def8" />
                                <PagerStyle BackColor="#CCCCCC" />
                                <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                                <PagerStyle CssClass="Paginador" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Archivo" HeaderStyle-Width="0%" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label runat='server' ID='archivoId' CssClass="lblUsuario" Text='<%# Bind("archivo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="id_cita" HeaderStyle-Width="0%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat='server' ID='citaIdGrid' CssClass="lblEnviarCorreo" Text='<%# Bind("Id_Cita") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='ruta' HeaderStyle-Width="0%" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat='server' ID='rutaId' CssClass="lblEnviarCorreo" Text='<%# Bind("ruta_archivo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Opciones'>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="linkdescargar" ToolTip="descargar" OnClick="linkdescargar_Click" CssClass="linkOpcion" runat="server" Text="<i class='fa fa-cloud-download' aria-hidden='true'></i>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <div class="row justify-content-center">
                        <div class="col px-2 text-center FBotonera">
                            <asp:Button runat="server" ID="btnResulCitaGuardar" CssClass="FGBoton" Text="Guardar" OnClick="btnResulCitaGuardar_Click" />
                            <asp:Button runat="server" ID="btnResulCitaCerrar" CssClass="FGBoton" Text="Cerrar" OnClick="btnCerrar_Click" />
                        </div>
                    </div>
                </div>


                <div id="divFContenidoHistoriaC" class="FContenido ajustediv" runat="server" visible="false">
                    <div class="row justify-content-start px-2 Htitle">
                        <div class="col-11">
                            <label>Escala de Fitzpatrick</label>
                        </div>
                        <div class="col-1 text-right">
                            <asp:LinkButton Text="<i class='fa fa-angle-up' aria-hidden='true'></i>" ID="lbtnFitzpatrick" OnClick="lbtnMostrarEscalaF_Click" ToolTip="Ocultar" runat="server"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="row align-content-center px-2" id="divEscalaFitzpatrick" runat="server">
                        <asp:RadioButtonList runat="server" ID="rbtnFitzpatrick" CssClass="rbFitzpatrick" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1" Text="<p>I</p><img src='Imagenes/Fitzpatrick1.png' alt='I' />"></asp:ListItem>
                            <asp:ListItem Value="2" Text="<p>II</p><img src='Imagenes/Fitzpatrick2.png' alt='II' />"></asp:ListItem>
                            <asp:ListItem Value="3" Text="<p>III</p><img src='Imagenes/Fitzpatrick3.png' alt='III' />"></asp:ListItem>
                            <asp:ListItem Value="4" Text="<p>IV</p><img src='Imagenes/Fitzpatrick4.png' alt='IV' />"></asp:ListItem>
                            <asp:ListItem Value="5" Text="<p>V</p><img src='Imagenes/Fitzpatrick5.png' alt='V' />"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="row justify-content-start px-2 Htitle">                        
                        <label>Topografía</label>
                    </div>
                    <div class="row justify-content-start px-2 Hsubtitle">
                        <div class="col-11">
                            <label>Localización (segmento)</label>
                        </div>
                        <div class="col-1 text-right">
                            <asp:LinkButton Text="<i class='fa fa-angle-down' aria-hidden='true'></i>" ID="lbtnLocalizacionH" OnClick="lbtnMostrarLocalizacionH_Click" ToolTip="Mostrar" runat="server"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2" id="divLocalizacionH" visible="false" runat="server">
                        <div class="col-6 px-2">
                            <label>Cabeza y cuello</label>
                            <asp:TextBox runat="server" ID="txtCabezaCuello" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-6 px-2">
                            <label>Tronco</label>
                            <asp:TextBox runat="server" ID="txtTronco" CssClass="form-control"></asp:TextBox>
                        </div>                       
                        <div class="col-6 px-2">
                            <label>Extremidades superiores</label>
                            <asp:TextBox runat="server" ID="txtExtremidadesSup" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-6 px-2">
                            <label>Extremidades inferiores</label>
                            <asp:TextBox runat="server" ID="txtExtremidadesInf" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2 Hsubtitle">
                        <div class="col-11">
                            <label>Extensión</label>
                        </div>
                        <div class="col-1 text-right">
                            <asp:LinkButton Text="<i class='fa fa-angle-down' aria-hidden='true'></i>" ID="lbtnExtensionH" OnClick="lbtnMostrarExtensionH_Click" ToolTip="Mostrar" runat="server"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2" id="divExtensionH" visible="false" runat="server">                        
                        <div class="col-6 px-2">
                            <label>Localizada (1 segmento)</label>
                            <asp:TextBox runat="server" ID="txtLocalizada" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-6 px-2">
                            <label>Diseminada (2 o 3 segmentos)</label>
                            <asp:TextBox runat="server" ID="txtDiseminada" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-6 px-2">
                            <label>Generalizada (4 segmentos)</label>
                            <asp:TextBox runat="server" ID="txtGeneralizada" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2 Hsubtitle">
                        <div class="col-11">
                            <label>Predominio</label>
                        </div>
                        <div class="col-1 text-right">
                            <asp:LinkButton Text="<i class='fa fa-angle-down' aria-hidden='true'></i>" ID="lbtnPredominioH" OnClick="lbtnMostrarPredominioH_Click" ToolTip="Mostrar" runat="server"></asp:LinkButton>
                        </div>
                    </div>
                    <div class="row align-content-center px-2" id="divPredominioH" visible="false" runat="server">
                        <asp:RadioButtonList runat="server" ID="rbtnPredominio" CssClass="rbPredominio" RepeatDirection="Horizontal">
                            <asp:ListItem Value="a" Text="<p>a</p><img src='Imagenes/predominioA.png' alt='a' />"></asp:ListItem>
                            <asp:ListItem Value="b" Text="<p>b</p><img src='Imagenes/predominioB.png' alt='b' />"></asp:ListItem>
                            <asp:ListItem Value="c" Text="<p>c</p><img src='Imagenes/predominioC.png' alt='c' />"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="row justify-content-start px-2 Htitle">
                            <div class="col-11">
                                <label>Morfología</label>
                            </div>
                            <div class="col-1 text-right">
                                <asp:LinkButton Text="<i class='fa fa-angle-down' aria-hidden='true'></i>" ID="lbtnMorfologiaH" OnClick="lbtnMostrarMorfologiaH_Click" ToolTip="Mostrar" runat="server"></asp:LinkButton>
                            </div>
                    </div>
                    <div class="row px-2" id="divMorfologiaH" visible="false" runat="server">                        
                        <div class="col-12">
                            <asp:TextBox runat="server" ID="txtMorfologia" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2 Htitle">
                            <div class="col-11">
                                <label>Interrogatorio</label>
                            </div>
                            <div class="col-1 text-right">
                                <asp:LinkButton Text="<i class='fa fa-angle-down' aria-hidden='true'></i>" ID="lbtnInterrogatorioH" OnClick="lbtnInterrogatorioH_Click" ToolTip="Mostrar" runat="server"></asp:LinkButton>
                            </div>
                    </div>
                    <div class="row px-2" id="divInterrogatorioH" visible="false" runat="server">                        
                        <div class="col-12">
                            <asp:TextBox runat="server" ID="txtInterrogatorio" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2 Htitle">
                            <div class="col-11">
                                <label>Resto de piel y anexos</label>
                            </div>
                            <div class="col-1 text-right">
                                <asp:LinkButton Text="<i class='fa fa-angle-down' aria-hidden='true'></i>" ID="lbtnAnexosH" OnClick="lbtnAnexosH_Click" ToolTip="Mostrar" runat="server"></asp:LinkButton>
                            </div>     
                    </div>
                    <div class="row px-2" id="divAnexosH" visible="false" runat="server">                        
                        <div class="col-12">
                            <asp:TextBox runat="server" ID="txtRestoAnexos" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                        </div>     
                    </div>
                    <div class="row justify-content-start px-2 Htitle">
                            <div class="col-11">
                                <label>Sintomatología</label>
                            </div>                            
                            <div class="col-1 text-right">
                                <asp:LinkButton Text="<i class='fa fa-angle-down' aria-hidden='true'></i>" ID="lbtnSintomatologiaH" OnClick="lbtnSintomatologiaH_Click" ToolTip="Mostrar" runat="server"></asp:LinkButton>
                            </div>                        
                    </div>
                    <div class="row px-2" id="divSintomatologiaH" visible="false" runat="server">
                        <div class="col-12">
                            <asp:TextBox runat="server" ID="txtSintomatologia" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2 Htitle">
                            <div class="col-11">
                                <label>Tratamientos anteriores</label>
                            </div>                            
                            <div class="col-1 text-right">
                                <asp:LinkButton Text="<i class='fa fa-angle-down' aria-hidden='true'></i>" ID="lbtnTratamientosH" OnClick="lbtnTratamientosH_Click" ToolTip="Mostrar" runat="server"></asp:LinkButton>
                            </div>
                    </div>
                    <div class="row px-2" id="divTratamientosH" visible="false" runat="server">
                        <div class="col-12">
                            <asp:TextBox runat="server" ID="txtTratamientosA" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2 Htitle">
                            <div class="col-11">
                                <label>Antecedentes Importantes</label>
                            </div>                            
                            <div class="col-1 text-right">
                                <asp:LinkButton Text="<i class='fa fa-angle-down' aria-hidden='true'></i>" ID="lbtnAntecedentesH" OnClick="lbtnAntecedentesH_Click" ToolTip="Mostrar" runat="server"></asp:LinkButton>
                            </div>
                    </div>
                    <div class="row px-2" id="divAntecedentesH" visible="false" runat="server">
                        <div class="col-12">
                            <asp:TextBox runat="server" ID="txtAntecedentes" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                        </div>  
                    </div>
                    <div class="row justify-content-start px-2 Htitle">
                            <div class="col-11">
                                <label>Diagnóstico</label>
                            </div>                            
                            <div class="col-1 text-right">
                                <asp:LinkButton Text="<i class='fa fa-angle-down' aria-hidden='true'></i>" ID="lbtnDiagnosticoH" OnClick="lbtnDiagnosticoH_Click" ToolTip="Mostrar" runat="server"></asp:LinkButton>
                            </div>
                    </div>
                    <div class="row px-2" id="divDiagnosticoH" visible="false" runat="server">                        
                        <div class="col-12">
                            <asp:TextBox runat="server" ID="txtDiagnostico" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                        </div>    
                    </div>
                    <div class="row justify-content-start px-2 Htitle">
                            <div class="col-11">
                                <label>Tratamiento</label>
                            </div>                            
                            <div class="col-1 text-right">
                                <asp:LinkButton Text="<i class='fa fa-angle-down' aria-hidden='true'></i>" ID="lbtnTratamientoH" OnClick="lbtnTratamientoH_Click" ToolTip="Mostrar" runat="server"></asp:LinkButton>
                            </div>
                    </div>
                    <div class="row px-2" id="divTratamientoH" visible="false" runat="server">                        
                        <div class="col-12">
                            <asp:TextBox runat="server" ID="txtTratamientoH" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                        </div> 
                    </div>
                    <div class="row justify-content-center">
                        <div class="col px-2 text-center FBotonera">
                            <asp:Button runat="server" ID="btnGuardaHistoria" CssClass="FGBoton" Text="Guardar" OnClick="btnGuardaHistoria_Click" />
                            <asp:Button runat="server" ID="btnCierraHistoria" CssClass="FGBoton" Text="Cerrar" OnClick="btnCerrar_Click" />
                        </div>
                    </div>
                </div>

                <div id="divFContenidoNotaEvol" class="FContenido ajustediv" visible="false" runat="server">
                    <div class="row justify-content-start px-2 Htitle">
                        <label>Exploración Física Actual</label>
                    </div>
                    <div class="row justify-content-start px-2">
                        <div class="col-4 px-2">
                            <label>T./A.</label>
                            <asp:TextBox runat="server" ID="txtTAnota" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-4 px-2">
                            <label>F.C.</label>
                            <asp:TextBox runat="server" ID="txtFCnota" CssClass="form-control"></asp:TextBox>
                        </div>                       
                        <div class="col-4 px-2">
                            <label>F.R.</label>
                            <asp:TextBox runat="server" ID="txtFRnota" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2">                    
                        <div class="col-4 px-2">
                            <label>Temperatura</label>
                            <asp:TextBox runat="server" ID="txtTempNota" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-4 px-2">
                            <label>Peso</label>
                            <asp:TextBox runat="server" ID="txtPesoNota" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-4 px-2">
                            <label>Talla</label>
                            <asp:TextBox runat="server" ID="txtTallaNota" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2">
                        <div class="col">
                            <label>Evolución</label>                        
                            <asp:TextBox runat="server" ID="txtEvolucionNota" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2">
                        <div class="col">
                            <label>Diagnóstico</label>                        
                            <asp:TextBox runat="server" ID="txtDiagnosticoNota" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2">
                        <div class="col">
                            <label>Tratamiento</label>                        
                            <asp:TextBox runat="server" ID="txtTratamientoNota" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row justify-content-start px-2 Htitle">
                        <label>Próxima cita</label>
                    </div>
                    <div class="row justify-content-start">
                        <div class="col-3 px-2">
                            <div class="form-group">
                                <label>Fecha</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFechaCitaProx" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-3 px-2">
                            <div class="form-group">
                                <label>Hora inicio</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtHoraICitaProx" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-3 px-2">
                            <div class="form-group">
                                <label>Hora fin</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtHoraFCitaProx" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-3 px-2">
                            <asp:Button runat="server" ID="btnAgendarProxCita" CssClass="FGBoton btnProxCita" Text="Agendar cita" OnClick="btnAgendarProxCita_Click" />
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col px-2 text-center FBotonera">
                            <asp:Button runat="server" ID="btnGuardaNotaEvolucion" CssClass="FGBoton" Text="Guardar" OnClick="btnGuardaNotaEvolucion_Click" />
                            <asp:Button runat="server" ID="btnCierraNotaEvolucion" CssClass="FGBoton" Text="Cerrar" OnClick="btnCerrar_Click" />
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
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtBusqCliFechaN" CssClass="form-control InpGrande" TextMode="Date" AutoComplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-8 px-2">
                            <div class="form-group">
                                <label class="FGEtiqueta">Email</label>
                                <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="TxtBusqCliEmail" CssClass="form-control FGInput FGCliente InpGrande" AutoComplete="off"></asp:TextBox>
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

                    <br />
                    <div class="row justify-content-center">
                        <div class="col px-2 text-center FBotonera">
                            <asp:Button runat="server" ID="btnFormCliBuscar" CssClass="FGBoton FCCBoton" Text="Buscar" OnClick="btnFormCliBuscar_Click" />
                            <asp:Button runat="server" ID="btnFormCliLimpiar" CssClass="FGBoton FCCBoton" Text="Limpiar" OnClick="btnFormCliLimpiar_Click" />
                            <asp:Button runat="server" ID="btnFormCliCerrar" CssClass="FGBoton FCCBoton" Text="Cerrar" OnClick="btnFormCliCerrar_Click" />
                        </div>
                    </div>
                </div>
                <div id="divClienteResultados" class="FContenido FCCContenido" runat="server" visible="false">
                    <h5>Selecciona un cliente</h5>
                    <asp:GridView ID="gvClientes" runat="server" HeaderStyle-BackColor="#9d6da2" ShowHeader="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="6" PagerSettings-Mode="NumericFirstLast"
                        CssClass="gridview2" CellPadding="5" Width="100%" OnRowDataBound="gvClientes_RowDataBound" OnPageIndexChanging="gvClientes_PageIndexChanging">
                        <AlternatingRowStyle BackColor="#f9c3f7" />
                        <RowStyle BackColor="#f9def8" />
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
                    <div class="col-3 px-2">
                        <div class="form-group">
                            <label>Género</label>
                            <asp:DropDownList runat="server" ID="ddlSexo" CssClass="form-control ddlSexo">
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
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliFecIngreso" CssClass="form-control InpGrande" TextMode="Date" AutoComplete="off"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-3 px-2">
                        <div class="form-group">
                            <label>Fecha Nacimiento</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliFecNac" CssClass="form-control InpGrande" TextMode="Date" AutoComplete="off" OnTextChanged="txtFormCliFecNac_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-3 px-2">
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

                    <div class="col-3 px-2">
                        <div class="form-group">
                            <label>Enviar Email</label>
                            <asp:DropDownList runat="server" CssClass="form-control InpMediano" ID="ddlFormCliEnviarCorreo">
                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-6 px-2">
                        <div class="form-group">
                            <label>Email</label>
                            <asp:TextBox onkeypress="return DisableEnterKey(event);" runat="server" ID="txtFormCliEmail" CssClass="form-control InpExtGrande" AutoComplete="off" TextMode="Email"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-3 px-2">
                        <div class="form-group">
                            <label>Factura:</label>
                            <asp:DropDownList runat="server" CssClass="form-control InpChico ddlFormCliFactura" OnSelectedIndexChanged="ddlFormCliFactura_SelectedIndexChanged" AutoPostBack="true" ID="ddlFormCliFactura">
                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Si"></asp:ListItem>
                            </asp:DropDownList>
                            <div id="divBtnDatosF" class="divBtnDatosF" runat="server" visible="false">
                                <asp:LinkButton ID="btnAddDatosF" ToolTip="Datos de facturación" OnClick="btnAddDatosF_Click" CssClass="btnAgrClienteG AddDatosF" runat="server" Text="<i class='fa fa-file-text-o' aria-hidden='true'></i><i class='fa fa-plus' aria-hidden='true'></i>" />
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
                        <hr style="color: #e0e0e0" />
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
    <div class="justify-content-start row divContenedorPrincipal ">
        <div id="divIzquierdo" class="col-4">
            <h4 class="hDoctor">Doctor</h4>
            <asp:DropDownList runat="server" ID="ddlDoctor" OnSelectedIndexChanged="ddlDoctor_SelectedIndexChanged" CssClass="ddlDoctor" AutoPostBack="true"></asp:DropDownList>
            <div id="calendarioChico" class="divCalendarioChico"></div>
        </div>
        <div id='calendar' class="divCalendario col-8"></div>
    </div>
</asp:Content>
