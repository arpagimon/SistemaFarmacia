using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;
using SistemaFarmacia.Clases;

namespace SistemaFarmacia
{

    public class Conexion
    {
        MySqlConnection connMySql = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["condb"].ConnectionString);

        String esquema = System.Configuration.ConfigurationManager.AppSettings["Esquema"].ToString();

        public void cerrar_conexion()
        {
            if (connMySql.State != System.Data.ConnectionState.Closed)
            {
                connMySql.Close();
            }
        }

        public String EjecutaQueryString(string cmd)
        {
            String respuesta = "";

            try
            {
                connMySql.Open();
                MySqlCommand mysqlcommand = new MySqlCommand(cmd, connMySql);
                MySqlDataReader datareader = mysqlcommand.ExecuteReader();

                while (datareader.Read())
                {
                    try
                    {
                        respuesta = datareader.GetInt16(0).ToString();
                    }
                    catch
                    {
                        respuesta = datareader.GetString(0);
                    }
                }
                datareader.Dispose();
                datareader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cerrar_conexion();
            }

            return respuesta;
        }


        public DataSet EjecutaQueryDS(string cmd)
        {
            DataSet dataset = new DataSet();

            try
            {
                connMySql.Open();
                MySqlDataAdapter mySqlData = new MySqlDataAdapter(cmd, connMySql);
                mySqlData.Fill(dataset);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cerrar_conexion();
            }

            return dataset;
        }

        public String EjecutaQueryInsert(string cmd)
        {
            String resultado = "";

            try
            {
                connMySql.Open();
                MySqlCommand comando = new MySqlCommand(cmd, connMySql);
                comando.CommandType = CommandType.Text;
                int insertado = comando.ExecuteNonQuery();

                if (insertado > 0)
                {
                    resultado = "OK";
                }
                else
                {
                    resultado = "Error al ejecutar, intentalo nuevamente.";
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
            }
            finally
            {
                cerrar_conexion();
            }

            return resultado;
        }



        #region Gus

        public String traerIdEstado(String Estado)
        {
            return EjecutaQueryString("select ID from " + esquema + ".estados where Estado = '" + Estado + "'");
        }

        public DataSet traerEstado()
        {
            return EjecutaQueryDS("select id, estado from " + esquema + ".estados order by estado");
        }

        public DataSet traerPaises()
        {
            return EjecutaQueryDS("select id_pais, pais from " + esquema + ".paises order by pais");
        }

        public DataSet traerMunicipio(String Estado)
        {
            return EjecutaQueryDS("select municipios.id ID, MUNICIPIO from " + esquema + ".estados inner join " + esquema + ".estadosmunicipios on estados.id = estadosmunicipios.estados_id "
                                                             + " inner join " + esquema + ".municipios on municipios.id = estadosmunicipios.MUNICIPIOS_ID "
                                                             + " where estadosmunicipios.estados_id = '" + Estado + "' order by municipio");
        }



        public String ConsultaPermisos(string usuario)
        {
            String resultado = "";
            try
            {
                connMySql.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("select permisos from " + esquema + ".empleado join " + esquema + ".perfil on empleado.id_perfil = perfil.id_perfil where usuario= '" + usuario + "'", connMySql);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (mySqlDataReader.HasRows)
                {
                    if (mySqlDataReader.Read())
                    {
                        resultado = mySqlDataReader.GetString(0);
                    }
                }
                mySqlDataReader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cerrar_conexion();
            }
            return resultado;
        }



        public bool ConsultaUsuario(string usuario, string contrasena)
        {
            bool resultado = false;
            try
            {
                connMySql.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("Select contraseña from " + esquema + ".empleado where usuario='" + usuario + "' and contraseña = '" + contrasena + "'and estatus='1' ", connMySql);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (mySqlDataReader.HasRows)
                {
                    if (mySqlDataReader.Read())
                    {
                        resultado = true;
                    }
                }
                mySqlDataReader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cerrar_conexion();
            }
            return resultado;
        }
        //METODO RECUPERA CONTRASEÑA

        public Boolean ConsultaContraseña(string usuario, string nombre, string apellido, string apellidoM, string contrasena)
        {
            Boolean resultado = false;

            try
            {
                connMySql.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("update  " + esquema + ".empleado  set contraseña ='" + contrasena + "' where usuario ='" + usuario + "' and nombre='" + nombre + "' and apellido_paterno='" + apellido + "' and apellido_materno='" + apellidoM + "' AND ESTATUS = '1'", connMySql);
                int i = mySqlCommand.ExecuteNonQuery();


                resultado = i > 0 ? true : false;

            }
            catch (Exception ex)
            {
            }
            finally
            {
                cerrar_conexion();
            }

            return resultado;
        }

        public String ConsultaDoctor(String usuario)
        {
            return EjecutaQueryString("select medico from " + esquema + ".empleado where usuario = '" + usuario + "'");
        }

        #endregion

        #region Chayo

        public DataSet TraerClienteCorreo(String ID_Cliente)
        {
            return EjecutaQueryDS("select ID_CLIENTE,NOMBRE,APELLIDO_PATERNO,APELLIDO_MATERNO, (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) AS EDAD, FECHA_NACIMIENTO, FECHA_INGRESO, ESTADO, MUNICIPIO, tel_casa_fijo,extension, celular, EMAIL, OBSERVACIONES, NOTA, MEDIO, CASE WHEN ESTATUS = 1 THEN 'Activo' else 'Inactivo' end ESTATUS, PAIS, Enviar_Correo, CASE WHEN REQ_FACTURA = 1 THEN 'Sí' else 'No' end REQ_FACTURA, Sexo from " + esquema + ".cliente where ID_CLIENTE = '" + ID_Cliente + "' and Enviar_Correo='1'");
        }


        public DataSet TraerClientes(String condicion, String Estatus)
        {
            return EjecutaQueryDS("select ID_CLIENTE, NOMBRE, APELLIDO_PATERNO, APELLIDO_MATERNO, (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) AS EDAD, FECHA_NACIMIENTO, FECHA_INGRESO, ESTADO, MUNICIPIO, tel_casa_fijo, extension, celular, EMAIL, OBSERVACIONES, NOTA, MEDIO, CASE WHEN ESTATUS = 1 THEN 'Activo' else 'Inactivo' end ESTATUS, PAIS, Enviar_Correo, CASE WHEN REQ_FACTURA = 1 THEN 'Sí' else 'No' end REQ_FACTURA, RFC, ENTIDAD, calle_dirf, noInterior_dirf, noExterior_dirf, Colonia_dirf, CP_dirf, Estado_dirf, municipio_dirf, pais_dirf, NOMBRAZON_FACTURA, Sexo from " + esquema + ".cliente " + (condicion.Trim().Length > 0 ? " where " + condicion + " and estatus " + (Estatus == "1,0" ? " in ('1','0')" : " and estatus = '" + Estatus + "'") : " where estatus " + (Estatus == "1,0" ? " in ('1','0') " : " = '" + Estatus + "'")) + " order by ID_CLIENTE desc");
        }

        public String GuardaCliente(String NOMBRE, String APELLIDO_PATERNO, String APELLIDO_MATERNO, String EDAD, String FECHA_NACIMIENTO, String FECHA_INGRESO, String MUNICIPIO, String tel_casa_fijo, String extension, String celular, String EMAIL, String OBSERVACIONES, String NOTA, String MEDIO, String Estatus, String Estado, String Pais, String Enviar_Correo, String ID_Empleado, String Req_Factura, String RFC, String Entidad, String CalleF, String NIntF, String NExtF, String ColoniaF, String CPF, String EstadoF, String MunicipioF, String PaisF, String NomRazon, String Sexo)
        {
            return EjecutaQueryInsert("insert into " + esquema + ".cliente(NOMBRE, APELLIDO_PATERNO, APELLIDO_MATERNO, EDAD,FECHA_NACIMIENTO, FECHA_INGRESO, MUNICIPIO, tel_casa_fijo, extension, celular, EMAIL, OBSERVACIONES, NOTA, MEDIO, ESTATUS, Estado, Pais, Enviar_Correo, Fecha_creacion, ID_Empleado_Creacion, Fecha_Modificacion, Id_Empleado_Modificacion, req_factura, rfc, entidad, calle_dirf, noInterior_dirf, noExterior_dirf, Colonia_dirf, CP_dirf, Estado_dirf, municipio_dirf, pais_dirf, NombRazon_factura, Sexo) " +
                "values('" + NOMBRE + "','" + APELLIDO_PATERNO + "','" + APELLIDO_MATERNO + "'," + (((EDAD == null || EDAD == "") ? "null" : "'" + EDAD + "'")) + "," + ((FECHA_NACIMIENTO == null || FECHA_NACIMIENTO == "") ? "null" : "'" + FECHA_NACIMIENTO + "'") + "," + ((FECHA_INGRESO == null || FECHA_INGRESO == "") ? "null" : "'" + FECHA_INGRESO + "'") + ",'" + MUNICIPIO + "','" + tel_casa_fijo + "','" + extension + "','" + celular + "','" + EMAIL + "','" + OBSERVACIONES + "','" + NOTA + "','" + MEDIO + "','" + Estatus + "', '" + Estado + "','" + Pais + "','" + Enviar_Correo + "',sysdate(), " + ID_Empleado + ", sysdate(), " + ID_Empleado + ",'" + Req_Factura + "','" + RFC + "','" + Entidad + "','" + CalleF + "','" + NIntF + "','" + NExtF + "','" + ColoniaF + "','" + CPF + "','" + EstadoF + "','" + MunicipioF + "','" + PaisF + "','" + NomRazon + "','" + Sexo + "')");

            //return EjecutaQueryInsert("insert into cliente(NOMBRE, APELLIDO_PATERNO, APELLIDO_MATERNO, EDAD, FECHA_NACIMIENTO, FECHA_INGRESO, MUNICIPIO, tel_casa_fijo, extension, celular, EMAIL, OBSERVACIONES, NOTA, MEDIO, estatus) " +
            //"values('" + NOMBRE + "','" + APELLIDO_PATERNO + "','" + APELLIDO_MATERNO + "','" + EDAD + "','" + FECHA_NACIMIENTO + "','" + FECHA_INGRESO + "','" + MUNICIPIO + "','" + tel_casa_fijo + "','" + extension + "','" + celular + "','" + EMAIL + "','" + OBSERVACIONES + "','" + NOTA + "','" + MEDIO + "','1')");
        }
        public String ActualizaCliente(String id_cliente, String NOMBRE, String APELLIDO_PATERNO, String APELLIDO_MATERNO, String EDAD, String FECHA_NACIMIENTO, String FECHA_INGRESO, String MUNICIPIO, String tel_casa_fijo, String extension, String celular, String EMAIL, String OBSERVACIONES, String NOTA, String MEDIO, String ESTATUS, String Estado, String Pais, String Enviar_Correo, String ID_Empleado, String R_Factura, String RFC, String Entidad, String CalleF, String NIntF, String NExtF, String ColoniaF, String CPF, String EstadoF, String MunicipioF, String PaisF, String NomRazon, String Sexo)
        {
            return EjecutaQueryInsert("update " + esquema + ".cliente set NOMBRE='" + NOMBRE + "', APELLIDO_PATERNO='" + APELLIDO_PATERNO + "', APELLIDO_MATERNO='" + APELLIDO_MATERNO + "', EDAD=" + (((EDAD == null || EDAD == "") ? "null" : "'" + EDAD + "'")) + ", FECHA_NACIMIENTO=" + ((FECHA_NACIMIENTO == null || FECHA_NACIMIENTO == "") ? "null" : "'" + FECHA_NACIMIENTO + "'") + ", FECHA_INGRESO=" + ((FECHA_INGRESO == null || FECHA_INGRESO == "") ? "null" : "'" + FECHA_INGRESO + "'") + ", MUNICIPIO='" + MUNICIPIO + "', tel_casa_fijo='" + tel_casa_fijo + "', extension='" + extension + "', celular='" + celular + "', EMAIL = '" + EMAIL + "', OBSERVACIONES = '" + OBSERVACIONES + "', NOTA = '" + NOTA + "', MEDIO = '" + MEDIO + "', ESTATUS = '" + ESTATUS + "', ESTADO = '" + Estado + "', PAIS ='" + Pais + "', Enviar_Correo = '" + Enviar_Correo + "', Fecha_Modificacion = sysdate(), Id_Empleado_Modificacion = " + ID_Empleado + ", req_factura = '" + R_Factura + "', rfc = '" + RFC + "', entidad = '" + Entidad + "', calle_dirf = '" + CalleF + "', noInterior_dirf = '" + NIntF + "' ,noExterior_dirf = '" + NExtF + "',Colonia_dirf = '" + ColoniaF + "',CP_dirf = '" + CPF + "',Estado_dirf = '" + EstadoF + "',municipio_dirf = '" + MunicipioF + "',pais_dirf = '" + PaisF + "', NombRazon_factura = '" + NomRazon + "', Sexo ='" + Sexo + "'  where id_cliente = '" + id_cliente + "'");
        }

        public String EliminaCliente(String Id_Cliente)
        {
            return EjecutaQueryInsert("update " + esquema + ".cliente set estatus = 3 where id_cliente = " + Id_Cliente);
        }

        public DataSet LLENAR_TABLA()
        {
            return EjecutaQueryDS("select * from  " + esquema + ".cliente c join " + esquema + ".contacto_cliente cl on  c.ID_CLIENTE=cl.ID_CLIENTE");
        }

        public DataSet consultarLlENAR(string nombre)
        {

            return EjecutaQueryDS("select * from  " + esquema + ".cliente c join " + esquema + ".contacto_cliente cl on  c.ID_CLIENTE=cl.ID_CLIENTE  where c.nombre like '%" + nombre + "%'");
        }

        public void insertar(string nombre, string apellido_paterno, string apellido_marteno, string edad, string fecha_nacimiento, string fecha_ingreso, string municipio, string calle, string no_int, string no_ext, string tel_casa_fijo, string extension, string celular, string email)
        {

            try
            {
                connMySql.Open();
                MySqlCommand comandoInserta = connMySql.CreateCommand();
                comandoInserta.CommandType = CommandType.Text;
                comandoInserta.CommandText = "insert into " + esquema + ".cliente(nombre,apellido_paterno, apellido_marteno,edad, fecha_nacimiento, fecha_ingreso) values ('" + nombre + "','" + apellido_paterno + "','" + apellido_marteno + "'," + edad + ", '" + fecha_nacimiento + "','" + fecha_ingreso + "', )";
                comandoInserta.ExecuteNonQuery();
                MySqlCommand comandoInserta2 = connMySql.CreateCommand();
                comandoInserta2.CommandType = CommandType.Text;
                comandoInserta2.CommandText = "insert into " + esquema + ".contacto_cliente(municipio, calle, no_int , no_ext, tel_casa_fijo,extension, celular, email ) values ('" + municipio + "','" + calle + "', '" + no_int + "', '" + no_ext + "', '" + tel_casa_fijo + "', '" + extension + "','" + celular + "','" + email + "')";
                comandoInserta2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {


            }

            finally
            {
                cerrar_conexion();
            }
        }
        public DataSet consultarmedios()
        {
            return EjecutaQueryDS("select medio from " + esquema + ".info_medio");
        }
        #endregion

        #region Pablo
        #region Adminstracion
        #region Usuario
        public DataSet TraerUsuarios(String condicion)
        {
            return EjecutaQueryDS("select id_usuario, nombre, apellido_paterno, apellido_materno, usuario,descripcion as Perfil, medico from " + esquema + ".empleado left join " + esquema + ".perfil on empleado.id_perfil = perfil.id_perfil  where empleado.estatus = 1 " + (condicion.Trim().Length > 0 ? " and " + condicion : "") + " order by nombre asc, apellido_paterno asc, apellido_materno asc");
        }

        public String GuardaUsuario(String Nombre, String ApellidoP, String ApellidoM, String Usuario, String Perfil, String Contrasenia, String Email, String Pregunta, String Respuesta, String doctor)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".empleado (nombre, apellido_paterno, apellido_materno, usuario, contraseña, id_perfil, estatus, email, pregunta, respuesta, medico) values('" + Nombre + "','" + ApellidoP + "','" + ApellidoM + "','" + Usuario + "','" + Contrasenia + "','" + Perfil + "','1','" + Email + "','" + Pregunta + "','" + Respuesta + "','" + doctor + "')");
        }

        public String ActualizaUsuario(String id_usuario, String Nombre, String ApellidoP, String ApellidoM, String Usuario, String Perfil, String Contrasenia, bool CambiaContraseña, bool CambiaRespuesta, String Email, String Pregunta, String Respuesta, String doctor)
        {
            return EjecutaQueryInsert("Update " + esquema + ".empleado set nombre = '" + Nombre + "', apellido_paterno = '" + ApellidoP + "', apellido_materno='" + ApellidoM + "', usuario='" + Usuario + "', id_perfil='" + Perfil + "' " + (CambiaContraseña ? ", contraseña = '" + Contrasenia + "'" : "") + ", email = '" + Email + "', pregunta = '" + Pregunta + "'" + (CambiaRespuesta ? ", respuesta = '" + Respuesta + "'" : "") + ", medico = '" + doctor + "' where id_usuario = '" + id_usuario + "'");
        }
        public DataSet TraerPerfilesddl(String condicion)
        {
            return EjecutaQueryDS("select id_Perfil, descripcion from " + esquema + ".perfil  " + (condicion.Trim().Length > 0 ? " where " + condicion : "") + " order by descripcion asc");
        }

        public String EliminaUsuario(String Id_Usuario)
        {
            return EjecutaQueryInsert("update " + esquema + ".empleado set estatus = 0 where id_usuario= " + Id_Usuario);
        }

        //public String EjecutaQueryString2(string cmd)
        //{
        //    String respuesta = "";

        //    try
        //    {
        //        connMySql.Open();
        //        MySqlCommand mysqlcommand = new MySqlCommand(cmd, connMySql);
        //        MySqlDataReader datareader = mysqlcommand.ExecuteReader();

        //        while (datareader.Read())
        //        {
        //            respuesta = datareader.GetValue(0).ToString();
        //        }
        //        datareader.Dispose();
        //        datareader.Close();

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        cerrar_conexion();
        //    }

        //    return respuesta;
        //}


        public String TraerPreguntaS(String id_usuario, String usuario, String email)
        {
            String resultado = "";
            if (id_usuario != "" && usuario == "" && email == "")
            {
                resultado = EjecutaQueryString("select pregunta from " + esquema + ".empleado where id_usuario = '" + id_usuario + "'");
            }
            else if (usuario != "" && id_usuario == "" && email == "")
            {
                resultado = EjecutaQueryString("select pregunta from " + esquema + ".empleado where usuario = '" + usuario + "'");
            }
            else if (email != "" && usuario == "" && id_usuario == "")
            {
                resultado = EjecutaQueryString("select pregunta from " + esquema + ".empleado where email = '" + email + "'");
            }
            else if (usuario != "" && email != "" && id_usuario == "")
            {
                resultado = EjecutaQueryString("select pregunta from " + esquema + ".empleado where email = '" + email + "' and usuario = '" + usuario + "'");
            }
            return resultado;
        }


        //Método para evaluar respuesta a pregunta segura
        public Boolean EvaluaRespuestaS(string usuario, string correo, string respuesta)
        {
            return (EjecutaQueryString("select id_usuario from  " + esquema + ".empleado  where " + (usuario.Trim().Length > 0 ? "usuario = '" + usuario + "'" : "email = '" + correo + "'") + " and respuesta = '" + respuesta + "'").Length > 0 ? true : false);
        }

        //Método para cambiar la contraseña
        public Boolean CambiarContraseña(string usuario, string email, string contrasena)
        {
            Boolean resultado = false;
            if (usuario == "" && email != "")
            {
                resultado = (EjecutaQueryInsert("update  " + esquema + ".empleado  set contraseña = '" + contrasena + "' where email = '" + email + "' AND ESTATUS = '1'") == "OK" ? true : false);
            }
            else if (email == "" && usuario != "")
            {
                resultado = (EjecutaQueryInsert("update  " + esquema + ".empleado  set contraseña ='" + contrasena + "' where usuario ='" + usuario + "' AND ESTATUS = '1'") == "OK" ? true : false);
            }
            else
            {
                resultado = (EjecutaQueryInsert("update  " + esquema + ".empleado  set contraseña ='" + contrasena + "' where usuario ='" + usuario + "' and email='" + email + "' AND ESTATUS = '1'") == "OK" ? true : false);
            }
            return resultado;
        }


        #endregion


        #region Perfiles
        public DataSet TraerPerfiles(String condicion)
        {
            return EjecutaQueryDS("SELECT id_perfil, descripcion, permisos FROM " + esquema + ".perfil where estatus = 1 " + (condicion.Trim().Length > 0 ? " and " + condicion : "") + " order by descripcion asc");
        }

        public String Guardaperfiles(String Descripcion, String Permiso)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".perfil (descripcion, permisos, fechaAlta,estatus ) values('" + Descripcion + "','" + Permiso + "',sysdate(),'1')");
        }

        public String ActualizaPerfil(String id_perfil, String Descripcion, String Permisos)
        {
            return EjecutaQueryInsert("Update " + esquema + ".perfil set descripcion = '" + Descripcion + "', permisos = '" + Permisos + "' where id_perfil = '" + id_perfil + "'");
        }
        public String EliminaPerfil(String Id_perfil)
        {
            //No funca
            return EjecutaQueryInsert("update " + esquema + ".perfil set estatus = 0 where id_perfil= " + Id_perfil);
        }

        #endregion

        #endregion

        #region Correo
        public DatosCorreo ConsultaDatosCorreo()
        {
            DatosCorreo resultado = new DatosCorreo();
            try
            {
                connMySql.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("Select nombre, valor from " + esquema + ".configuracion ", connMySql);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        switch (mySqlDataReader.GetString(0))
                        {
                            case "SMTP_SSL":
                                resultado.SMTP_SSL = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_CORREO":
                                resultado.SMTP_CORREO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_PASS":
                                resultado.SMTP_PASS = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_HOST":
                                resultado.SMTP_HOST = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_PUERTO":
                                resultado.SMTP_PUERTO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_SUJETO":
                                resultado.SMTP_SUJETO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_MENSAJE":
                                resultado.SMTP_MENSAJE = mySqlDataReader.GetString(1);
                                break;
                            case "ULTIMOENVIO":
                                resultado.UltimoEnvio = mySqlDataReader.GetString(1);
                                break;
                            case "DIASANTESDEENVIO":
                                resultado.DiasAntes = mySqlDataReader.GetString(1);
                                break;
                            case "IMAGENCORREO":
                                resultado.SMTP_IMAGEN = mySqlDataReader.GetString(1);
                                break;
                            case "PRUEBAS":
                                resultado.PRUEBAS = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_CORREO_PRUEBA":
                                resultado.SMTP_CORREO_PRUEBA = mySqlDataReader.GetString(1);
                                break;
                            case "ENV_CORREO_A":
                                resultado.ENV_ESTADO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_FIRMA":
                                resultado.SMTP_FIRMA = mySqlDataReader.GetString(1);
                                break;
                            default:
                                break;
                        }
                    }
                }
                mySqlDataReader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cerrar_conexion();
            }
            return resultado;
        }



        public bool ValidaDiaCorreo(string DiasAntes)
        {
            bool resultado = false;
            try
            {
                connMySql.Open();
                //Se valida que sea mayor al minimo de días antes de inicio de mes para mandar correo
                MySqlCommand mySqlCommand = new MySqlCommand("SELECT SYSDATE() >= date_add(last_day(SYSDATE()), interval -" + DiasAntes + "+1 day) FROM DUAL", connMySql);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (mySqlDataReader.HasRows)
                {
                    if (mySqlDataReader.Read())
                    {
                        if (mySqlDataReader.GetBoolean(0))
                        {
                            resultado = true;
                        }
                    }
                }

                mySqlDataReader.Close();

                if (resultado)
                {
                    //Se valida que la fecha de ultimo correo + 20 dias sea menor al día actual, para asegurar que no se manda 2 veces la promoción
                    resultado = false;
                    mySqlCommand.CommandText = "select sysdate() > date_add(STR_TO_DATE(valor, '%d/%m/%Y'), interval 20 day)  from " + esquema + ".configuracion where nombre = 'ULTIMOENVIO'";
                    mySqlDataReader = mySqlCommand.ExecuteReader();

                    if (mySqlDataReader.HasRows)
                    {
                        if (mySqlDataReader.Read())
                        {
                            if (mySqlDataReader.GetBoolean(0))
                            {
                                resultado = true;
                            }
                        }
                    }
                }
                mySqlDataReader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cerrar_conexion();
            }
            return resultado;
        }

        public DataSet TraerClientesDelMesSiguiente(String condicion, String Estatus)
        {
            return EjecutaQueryDS("select ID_CLIENTE,NOMBRE,APELLIDO_PATERNO,APELLIDO_MATERNO, (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) AS EDAD, FECHA_NACIMIENTO, FECHA_INGRESO, ESTADO, MUNICIPIO, tel_casa_fijo,extension, celular, EMAIL,OBSERVACIONES, NOTA, MEDIO, Enviar_Correo, Sexo " +
                " from " + esquema + ".cliente " +
                " where estatus   " + (Estatus == "1,0" ? " in ('1','0')" : " = '" + Estatus + "' ") + " and Date_format(fecha_nacimiento,'%m') =  (case when date_format(sysdate(),'%m') = 12 then 1  else date_format(sysdate(),'%m')+1 end) " + (condicion.Trim().Length > 0 ? " and " + condicion : ""));
        }

        public DataSet TraerClientesDelMes(String condicion, int mes, String estatus)
        {
            return EjecutaQueryDS("select ID_CLIENTE,NOMBRE,APELLIDO_PATERNO,APELLIDO_MATERNO, (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) AS EDAD, FECHA_NACIMIENTO, " +
                " FECHA_INGRESO, ESTADO, MUNICIPIO, tel_casa_fijo, extension, celular, EMAIL, OBSERVACIONES, NOTA, MEDIO, CHECK_EMAIL, Enviar_Correo, Sexo " +
                " from " + esquema + ".cliente where estatus " + (estatus == "1,0" ? " in ('1','0')" : " = '" + estatus + "' ") + " and date_format(fecha_nacimiento,'%m') = '" + mes.ToString().PadLeft(2, '0') + "' " + (condicion.Trim().Length > 0 ? " and " + condicion : ""));
        }


        public String ActualizaUltimoCorreo()
        {
            return EjecutaQueryInsert("update " + esquema + ".configuracion set valor = date_format(sysdate(),'%d/%m/%Y') where nombre = 'ULTIMOENVIO'");
        }

        public String ActualizaCorreoEnvCliente(String IDS_Cliente)
        {
            return EjecutaQueryInsert("update " + esquema + ".cliente set CHECK_EMAIL = date_format(sysdate(),'%Y') where ID_Cliente in (" + IDS_Cliente + ")");
        }


        public String TraerEnvioCorreo()
        {
            return EjecutaQueryString("Select valor from " + esquema + ".configuracion where nombre = 'ENV_CORREO_A'");
        }

        public String ActualizaDatosCorreo(DatosCorreo datoscorreo)
        {
            string resultado = "OK";
            try
            {

                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_CORREO + "' where nombre = 'SMTP_CORREO'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_PASS + "' where nombre = 'SMTP_PASS'");


                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_MENSAJE + "' where nombre = 'SMTP_MENSAJE'");
                if (datoscorreo.SMTP_IMAGEN.Length > 0)
                {
                    EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_IMAGEN + "' where nombre = 'IMAGENCORREO'");
                }

                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_SUJETO + "' where nombre = 'SMTP_SUJETO'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_SSL + "' where nombre = 'SMTP_SSL'");

                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_HOST + "' where nombre = 'SMTP_HOST'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_PUERTO + "' where nombre = 'SMTP_PUERTO'");

                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.UltimoEnvio + "' where nombre = 'ULTIMOENVIO'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.DiasAntes + "' where nombre = 'DIASANTESDEENVIO'");

                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.PRUEBAS + "' where nombre = 'PRUEBAS'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_CORREO_PRUEBA + "' where nombre = 'SMTP_CORREO_PRUEBA'");

                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.ENV_ESTADO + "' where nombre = 'ENV_CORREO_A'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_FIRMA + "' where nombre = 'SMTP_FIRMA'");
            }
            catch (Exception ex)
            {
                resultado = ex.InnerException.Message;
            }

            return resultado;
        }

        public String ActualizaTecCorreo(DatosCorreo datoscorreo)
        {
            string resultado = "OK";
            try
            {
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_HOST + "' where nombre = 'SMTP_HOST'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_PUERTO + "' where nombre = 'SMTP_PUERTO'");

                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_SSL + "' where nombre = 'SMTP_SSL'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_CORREO + "' where nombre = 'SMTP_CORREO'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_PASS + "' where nombre = 'SMTP_PASS'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = DATE_FORMAT(sysdate(),'%d-%m-%Y %H:%i:%s') where nombre = 'fechaCorreoTec'");
            }
            catch (Exception ex)
            {
                resultado = ex.InnerException.Message;
            }

            return resultado;
        }

        public String ActualizaConfCorreo(DatosCorreo datoscorreo)
        {
            string resultado = "OK";
            try
            {
                //EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.UltimoEnvio + "' where nombre = 'ULTIMOENVIO'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.DiasAntes + "' where nombre = 'DIASANTESDEENVIO'");

                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_CORREO_PRUEBA + "' where nombre = 'SMTP_CORREO_PRUEBA'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.PRUEBAS + "' where nombre = 'PRUEBAS'");

                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.ENV_ESTADO + "' where nombre = 'ENV_CORREO_A'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = DATE_FORMAT(sysdate(),'%d-%m-%Y %H:%i:%s') where nombre = 'fechaCorreoConf'");
            }
            catch (Exception ex)
            {
                resultado = ex.InnerException.Message;
            }

            return resultado;
        }

        public String ActualizaContCorreo(DatosCorreo datoscorreo)
        {
            string resultado = "OK";
            try
            {
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_SUJETO + "' where nombre = 'SMTP_SUJETO'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_MENSAJE + "' where nombre = 'SMTP_MENSAJE'");
                if (datoscorreo.SMTP_IMAGEN.Length > 0)
                {
                    EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_IMAGEN + "' where nombre = 'IMAGENCORREO'");
                }
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_FIRMA + "' where nombre = 'SMTP_FIRMA'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = DATE_FORMAT(sysdate(),'%d-%m-%Y %H:%i:%s') where nombre = 'fechaCorreoCont'");
            }
            catch (Exception ex)
            {
                resultado = ex.InnerException.Message;
            }

            return resultado;
        }

        #endregion
        #endregion







        public string ConsultarNombreUsuario(string usuario)
        {
            return EjecutaQueryString("select concat(nombre, ' ', apellido_paterno, ' ', apellido_materno) from " + esquema + ".empleado where usuario = '" + usuario + "'");
        }

        public Boolean GuardaBitacora(String empleado, List<String> ID_Cliente, DatosCorreo dCorreo)
        {
            Boolean inserto = (EjecutaQueryInsert("Insert into " + esquema + ".BitacoraCorreo(fecha_hora,empleado_envia) values(sysdate(), " + traerIDEmpleado(empleado) + " )") == "OK" ? true : false);
            if (inserto)
            {
                foreach (String ID in ID_Cliente)
                {
                    EjecutaQueryInsert("Insert into " + esquema + ".BitacoraDestinos(ID_Entrada,ID_Cliente) values((select last_insert_id()), " + ID + ")");
                }
                EjecutaQueryInsert("Insert into " + esquema + ".BitacoraEstructura(ID_Entrada, Asunto, Mensaje, imagen, firma) values((select last_insert_id()), '" + dCorreo.SMTP_SUJETO + "','" + dCorreo.SMTP_MENSAJE + "','" + dCorreo.SMTP_IMAGEN + "','" + dCorreo.SMTP_FIRMA + "')");
            }
            return true;
        }


        public String traerIDEmpleado(String empleado)
        {
            return EjecutaQueryString("select id_usuario from " + esquema + ".empleado where usuario = '" + empleado + "'");
        }

        public String guardaError(String Error, String Modulo, String Evento)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".LogErrores(Modulo,Evento,msjError,Fecha) values('" + Modulo + "','" + Evento + "','" + Error + "',sysdate())");
        }



        public DataSet TraerClEliminados(String Condicion)
        {
            return EjecutaQueryDS("select ID_CLIENTE,NOMBRE,APELLIDO_PATERNO,APELLIDO_MATERNO, (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) AS EDAD, FECHA_NACIMIENTO, FECHA_INGRESO, ESTADO, MUNICIPIO, tel_casa_fijo,extension, celular, EMAIL, OBSERVACIONES, NOTA, MEDIO, CASE WHEN ESTATUS = 1 THEN 'Activo' else 'Inactivo' end ESTATUS, PAIS, Enviar_Correo, CASE WHEN REQ_FACTURA = 1 THEN 'Sí' else 'No' end REQ_FACTURA, RFC, ENTIDAD, DIR_FACTURA, NOMBRAZON_FACTURA, Sexo from " + esquema + ".cliente where estatus = 3 " + (Condicion.Length > 0 ? " and " + Condicion : "") + " order by ID_CLIENTE desc");
        }

        public String ReactivaCliente(String Id_Cliente, String ID_Empleado)
        {
            return EjecutaQueryInsert("update " + esquema + ".cliente set estatus = 1, Fecha_Modificacion = sysdate(), Id_Empleado_Modificacion = " + ID_Empleado + " where id_cliente = " + Id_Cliente);
        }




        public DataSet traerCitasDoctor(String Mes, String Doctor)
        {
            //return EjecutaQueryDS("Select ID_Cita, Citas.ID_CLIENTE, DATE_FORMAT(hora_inicio,'%Y-%m-%d %H:%i:%s') hora_inicio, DATE_FORMAT(hora_fin,'%Y-%m-%d %H:%i:%s') hora_fin, Citas.nota, nombre, apellido_paterno, apellido_materno, tipo from " + esquema + ".Citas left join " + esquema + ".cliente on Citas.ID_CLIENTE = Cliente.ID_CLIENTE where estatus_cita = '1' and ID_usuario=" + Doctor + " ");
            return EjecutaQueryDS("Select ID_Cita, Citas.ID_CLIENTE, DATE_FORMAT(hora_inicio,'%Y-%m-%d %H:%i:%s') hora_inicio, DATE_FORMAT(hora_fin,'%Y-%m-%d %H:%i:%s') hora_fin, DATE_FORMAT(Citas.Fecha,'%Y-%m-%d') Fechas, Citas.nota, nombre, apellido_paterno, apellido_materno, tipo from " + esquema + ".Citas left join " + esquema + ".cliente on Citas.ID_CLIENTE = Cliente.ID_CLIENTE where estatus_cita = '1' and ID_usuario=" + Doctor + " ");

        }

        public DataSet traerDatosCitas(String ID_Cita)
        {
            return EjecutaQueryDS("Select ID_Cita,Citas.ID_CLIENTE, DATE_FORMAT(fecha,'%Y-%m-%d %H:%i:%s') fecha_cita, DATE_FORMAT(hora_inicio,'%Y-%m-%d %H:%i:%s') hora_inicio,  DATE_FORMAT(hora_fin,'%Y-%m-%d %H:%i:%s') hora_fin, Citas.nota, nombre, apellido_paterno, apellido_materno, EMAIL, DATE_FORMAT(FECHA_NACIMIENTO,'%Y-%m-%d %H:%i:%s') Fecha_Nacimiento, ID_usuario, Citas.nota, tipo from " + esquema + ".Citas left join " + esquema + ".cliente on Citas.ID_CLIENTE = Cliente.ID_CLIENTE where estatus_cita = '1' and ID_Cita =" + ID_Cita);
        }

        public DataSet consultaMedico()
        {
            return EjecutaQueryDS("select id_usuario, concat(nombre, ' ', apellido_paterno, ' ', apellido_materno) Nombre from " + esquema + ".empleado  where estatus = 1 and medico = 1");
        }

        public String cancelarCita(String Id_Cita, String id_EmpleadoCancela)
        {
            return EjecutaQueryInsert("update " + esquema + ".Citas set estatus_cita= '0', fecha_Modifica = sysdate(), ID_Empleado_Modifica='" + id_EmpleadoCancela + "' where id_cita = " + Id_Cita);
        }

        public String ActualizaCita(String Id_Cita, String id_EmpleadoActualiza, String Fecha_Cita, String Hora_inicio, String Hora_fin)
        {
            return EjecutaQueryInsert("update " + esquema + ".Citas set fecha_Modifica = sysdate(), ID_Empleado_Modifica='" + id_EmpleadoActualiza + "', Fecha = '" + Fecha_Cita + " 00:00:00', hora_inicio='" + Fecha_Cita + " " + Hora_inicio + "', hora_fin='" + Fecha_Cita + " " + Hora_fin + "' where id_cita = " + Id_Cita);
        }

        public String AgregarCita(String id_EmpleadoCrea, String Fecha_Cita, String Hora_inicio, String Hora_fin, String ID_Cliente, String Nota, String ID_Doctor, String tipo)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".Citas(fecha_Modifica, ID_Empleado_Modifica, ID_Empleado_Crea, fecha_Crea, Fecha, hora_inicio, hora_fin, ID_Cliente, nota, ID_usuario, tipo)   values(sysdate(), '" + id_EmpleadoCrea + "', '" + id_EmpleadoCrea + "', sysdate(), '" + Fecha_Cita + " 00:00:00', '" + Fecha_Cita + " " + Hora_inicio + "', '" + Fecha_Cita + " " + Hora_fin + "'," + ID_Cliente + ",'" + Nota + "'," + ID_Doctor + ",'" + tipo + "') ");
        }

        public String traeUltimoID()
        {
            return EjecutaQueryString("select last_insert_id()");
        }

        public String validaCitaColisionada(String ID_Doctor, String Fecha, String Hora_Inicio, String Hora_Fin, String ID_Cita)
        {
            return EjecutaQueryString("Select concat(DATE_FORMAT(hora_inicio,'%d-%m-%Y'), ' de las ', DATE_FORMAT(hora_inicio,'%H:%i:%s'), ' a las ', DATE_FORMAT(hora_fin,'%H:%i:%s')) from " + esquema + ".citas where ((hora_inicio <= '" + Fecha + " " + Hora_Inicio + "' and hora_fin <= '" + Fecha + " " + Hora_Fin + "' and hora_fin > '" + Fecha + " " + Hora_Inicio + "' ) or " +
                "(hora_inicio >= '" + Fecha + " " + Hora_Inicio + "' and hora_fin >= '" + Fecha + " " + Hora_Fin + "' and hora_inicio < '" + Fecha + " " + Hora_Fin + "') or " +
                "(hora_inicio >= '" + Fecha + " " + Hora_Inicio + "' and hora_fin <= '" + Fecha + " " + Hora_Fin + "')) and ID_usuario = " + ID_Doctor + " and estatus_cita = 1 " + (ID_Cita != "" ? "and ID_Cita != " + ID_Cita : ""));
        }




        public DatosCorreo ConsultaDatosCorreoSelec()
        {
            DatosCorreo resultado = new DatosCorreo();
            try
            {
                connMySql.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("Select nombre, valor from " + esquema + ".configuracion ", connMySql);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        switch (mySqlDataReader.GetString(0))
                        {
                            case "SMTP_SSL":
                                resultado.SMTP_SSL = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_CORREO":
                                resultado.SMTP_CORREO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_PASS":
                                resultado.SMTP_PASS = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_HOST":
                                resultado.SMTP_HOST = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_PUERTO":
                                resultado.SMTP_PUERTO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_SUJETO_S":
                                resultado.SMTP_SUJETO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_MSJ_S":
                                resultado.SMTP_MENSAJE = mySqlDataReader.GetString(1);
                                break;
                            case "ULTIMOENVIO":
                                resultado.UltimoEnvio = mySqlDataReader.GetString(1);
                                break;
                            case "DIASANTESDEENVIO":
                                resultado.DiasAntes = mySqlDataReader.GetString(1);
                                break;
                            case "IMGCORREO_S":
                                resultado.SMTP_IMAGEN = mySqlDataReader.GetString(1);
                                break;
                            case "PRUEBAS":
                                resultado.PRUEBAS = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_CORREO_PRUEBA":
                                resultado.SMTP_CORREO_PRUEBA = mySqlDataReader.GetString(1);
                                break;
                            case "ENV_CORREO_A":
                                resultado.ENV_ESTADO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_FIRMA_S":
                                resultado.SMTP_FIRMA = mySqlDataReader.GetString(1);
                                break;
                            default:
                                break;
                        }
                    }
                }
                mySqlDataReader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cerrar_conexion();
            }
            return resultado;
        }

        public String ActualizaContCorreoSelec(DatosCorreo datoscorreo)
        {
            string resultado = "OK";
            try
            {
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_SUJETO + "' where nombre = 'SMTP_SUJETO_S'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_MENSAJE + "' where nombre = 'SMTP_MSJ_S'");
                if (datoscorreo.SMTP_IMAGEN.Length > 0)
                {
                    EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_IMAGEN + "' where nombre = 'IMGCORREO_S'");
                }
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_FIRMA + "' where nombre = 'SMTP_FIRMA_S'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = DATE_FORMAT(sysdate(),'%d-%m-%Y %H:%i:%s') where nombre = 'fechaCorreoCont'");
            }
            catch (Exception ex)
            {
                resultado = ex.InnerException.Message;
            }

            return resultado;
        }


        public string GuardaDiasInhabilesSemana(string diasSemana)
        {
            return EjecutaQueryInsert("UPDATE " + esquema + ".configuracion SET VALOR = '" + diasSemana + "' WHERE NOMBRE='no_Inhabiles_Semana'");
        }

        public string GuardaHorarioLaboralA(string hora_Apertura)
        {
            return EjecutaQueryInsert("UPDATE " + esquema + ".configuracion SET VALOR = '" + hora_Apertura + "' WHERE NOMBRE='hora_Apertura'");
        }
        public string GuardaHorarioLaboralC(string hora_Cierre)
        {
            return EjecutaQueryInsert("UPDATE " + esquema + ".configuracion SET VALOR = '" + hora_Cierre + "' WHERE NOMBRE='hora_Cierre'");
        }
        public string GuardaFechasInhabiles(string FechasInhabiles)
        {
            return EjecutaQueryInsert("INSERT " + esquema + ".fechas_inhabiles(fechas_inhabiles)  values ('" + FechasInhabiles + "')");
        }
        public DataSet TraerFechaInhabiles()
        {
            return EjecutaQueryDS("SELECT fechas_inhabiles FROM " + esquema + ".fechas_inhabiles");
        }
        public string TraerHrApertura()
        {
            return EjecutaQueryString("SELECT VALOR FROM " + esquema + ".configuracion WHERE NOMBRE = 'hora_Apertura'");
        }
        public string TraerHrCierre()
        {
            return EjecutaQueryString("SELECT VALOR FROM " + esquema + ".configuracion WHERE NOMBRE = 'hora_Cierre'");
        }
        public string TraerDiasSemana()
        {
            return EjecutaQueryString("SELECT VALOR FROM " + esquema + ".configuracion WHERE NOMBRE = 'no_Inhabiles_Semana'");
        }
        public String EliminaFecha(String Fecha)
        {
            //return EjecutaQueryInsert("delete from " + esquema + ".fechas_inhabiles where fechas_inhabiles = '" + Fecha + "'");
            return EjecutaQueryInsert("delete from " + esquema + ".fechas_inhabiles where fechas_inhabiles = str_to_date('" + Fecha + "','%d/%m/%Y')");
        }





        public bool ValidaCitaCorreo(String diasAntes)
        {
            //return int.Parse(EjecutaQueryString("Select COUNT(*) from " + esquema + ".citas where estatus_cita = 1 and Fecha =  DATE_SUB(CURDATE(), INTERVAL -1 DAY) and (CorreoEnviado is null or CorreoEnviado = '0')")) > 0 ;
            return int.Parse(EjecutaQueryString("Select COUNT(*) from " + esquema + ".citas where estatus_cita = 1 and Fecha =  DATE_SUB(CURDATE(), INTERVAL - " + diasAntes + " DAY) and (CorreoEnviado is null or CorreoEnviado = '0')")) > 0;
        }


        public DataSet TraerClientesConCitaM(String diasAntes)
        {
            //return EjecutaQueryDS("Select ID_Cita, DATE_FORMAT(hora_inicio,'%H:%i:%s') hora_inicio, DATE_FORMAT(hora_fin, '%H:%i:%s') hora_fin, DATE_FORMAT(fecha, '%Y-%m-%d') fecha, cliente.ID_CLIENTE," +
            //    " cliente.nombre, cliente.apellido_paterno, cliente.apellido_materno, cliente.EMAIL " +
            //    " from " + esquema + ".Citas inner join " + esquema + ".cliente on Citas.ID_CLIENTE = Cliente.ID_CLIENTE" +
            //" where estatus_cita = 1 and citas.Fecha = (select DATE_SUB(CURDATE(), INTERVAL - 1 DAY)) and (CorreoEnviado is null or CorreoEnviado = '0'); ");
            return EjecutaQueryDS("Select ID_Cita, DATE_FORMAT(hora_inicio,'%H:%i:%s') hora_inicio, DATE_FORMAT(hora_fin, '%H:%i:%s') hora_fin, DATE_FORMAT(fecha, '%Y-%m-%d') fecha, cliente.ID_CLIENTE," +
                " cliente.nombre, cliente.apellido_paterno, cliente.apellido_materno, cliente.EMAIL " +
                " from " + esquema + ".Citas inner join " + esquema + ".cliente on Citas.ID_CLIENTE = Cliente.ID_CLIENTE" +
                " where estatus_cita = 1 and citas.Fecha = (select DATE_SUB(CURDATE(), INTERVAL - " + diasAntes + " DAY)) and (CorreoEnviado is null or CorreoEnviado = '0'); ");
        }

        public String ActualizaRecordatorio(int id_cita)
        {
            return EjecutaQueryInsert("update " + esquema + ".citas set CorreoEnviado = '1' where ID_Cita = " + id_cita);
        }


        public DatosCorreo ConsultaDatosCorreoRec()
        {
            DatosCorreo resultado = new DatosCorreo();
            try
            {
                connMySql.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("Select nombre, valor from " + esquema + ".configuracion ", connMySql);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        switch (mySqlDataReader.GetString(0))
                        {
                            case "SMTP_SSL":
                                resultado.SMTP_SSL = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_CORREO":
                                resultado.SMTP_CORREO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_HOST":
                                resultado.SMTP_HOST = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_PUERTO":
                                resultado.SMTP_PUERTO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_SUJETO_R":
                                resultado.SMTP_SUJETO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_MSJ_R":
                                resultado.SMTP_MENSAJE = mySqlDataReader.GetString(1);
                                break;
                            case "DIASANTESENVREC":
                                if (mySqlDataReader.GetString(1).Length > 0)
                                {
                                    resultado.DiasAntes = mySqlDataReader.GetString(1);
                                }
                                else
                                {
                                    resultado.DiasAntes = "0";
                                }
                                break;
                            case "IMGCORREO_R":
                                resultado.SMTP_IMAGEN = mySqlDataReader.GetString(1);
                                break;
                            case "PRUEBAS":
                                resultado.PRUEBAS = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_CORREO_PRUEBA":
                                resultado.SMTP_CORREO_PRUEBA = mySqlDataReader.GetString(1);
                                break;
                            case "ENV_CORREO_A":
                                resultado.ENV_ESTADO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_FIRMA_R":
                                resultado.SMTP_FIRMA = mySqlDataReader.GetString(1);
                                break;
                            default:
                                break;
                        }
                    }
                }
                mySqlDataReader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cerrar_conexion();
            }
            return resultado;
        }


        public String ActualizaContCorreoRec(DatosCorreo datoscorreo, String diasAntes)
        {
            string resultado = "OK";
            try
            {
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_SUJETO + "' where nombre = 'SMTP_SUJETO_R'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_MENSAJE + "' where nombre = 'SMTP_MSJ_R'");
                if (datoscorreo.SMTP_IMAGEN.Length > 0)
                {
                    EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_IMAGEN + "' where nombre = 'IMGCORREO_R'");
                }
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + datoscorreo.SMTP_FIRMA + "' where nombre = 'SMTP_FIRMA_R'");
                EjecutaQueryInsert("update " + esquema + ".configuracion set valor = '" + diasAntes + "' where nombre = 'DIASANTESENVREC'");

            }
            catch (Exception ex)
            {
                resultado = ex.InnerException.Message;
            }

            return resultado;
        }




        public DataSet TraerTipoCorreo(String condicion)
        {
            return EjecutaQueryDS("Select ID_tipo, Nombre_tipo, Sujeto, Mensaje, ImgCorreo, Firma from " + esquema + ".tipo_correo where estatus = 1 " + (condicion.Trim().Length > 0 ? " and " + condicion : "") + " order by Nombre_tipo asc");
        }

        public String GuardaTipoCorreo(String Nombre, String Sujeto, String Mensaje, String ImgCorreo, String Firma)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".tipo_correo (Nombre_tipo,Sujeto,Mensaje,ImgCorreo,Firma,estatus) values ('" + Nombre + "','" + Sujeto + "','" + Mensaje + "','" + ImgCorreo + "','" + Firma + "','1')");
        }

        public String ActualizaTipoCorreo(String id_tipo, String Nombre, String Sujeto, String Mensaje, bool CambiaImg, String ImgCorreo, String Firma)
        {
            return EjecutaQueryInsert("Update " + esquema + ".tipo_correo set Nombre_tipo = '" + Nombre + "',Sujeto = '" + Sujeto + "',Mensaje = '" + Mensaje + "' " + (CambiaImg ? ", ImgCorreo = '" + ImgCorreo + "'" : "") + ",Firma = '" + Firma + "' where id_tipo=" + id_tipo);
        }

        public String EliminaTipoCorreo(String id_tipo)
        {
            return EjecutaQueryInsert("update " + esquema + ".tipo_correo set estatus = 0 where id_tipo= " + id_tipo);
        }

        public DatosCorreo ConsultaDatosTipoCorreo(String Asunto, String Mensaje, String Firma, String Imagen)
        {
            DatosCorreo resultado = new DatosCorreo();
            try
            {
                connMySql.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("Select nombre, valor from " + esquema + ".configuracion ", connMySql);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        switch (mySqlDataReader.GetString(0))
                        {
                            case "SMTP_CORREO":
                                resultado.SMTP_CORREO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_CORREO_PRUEBA":
                                resultado.SMTP_CORREO_PRUEBA = mySqlDataReader.GetString(1);
                                break;
                            case "ENV_CORREO_A":
                                resultado.ENV_ESTADO = mySqlDataReader.GetString(1);
                                break;
                            default:
                                break;
                        }
                    }
                }
                mySqlDataReader.Close();
                resultado.SMTP_SUJETO = Asunto;
                resultado.SMTP_MENSAJE = Mensaje;
                resultado.SMTP_IMAGEN = Imagen;
                resultado.SMTP_FIRMA = Firma;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cerrar_conexion();
            }
            return resultado;
        }



        public DataSet TraerTipoCorreoDdl()
        {
            return EjecutaQueryDS("Select ID_tipo, Nombre_tipo from " + esquema + ".tipo_correo where estatus = 1 order by Nombre_tipo asc");
        }
        public DataSet TraerGpoClddl()
        {
            return EjecutaQueryDS("Select ID_Grupo, Nombre_Grupo from " + esquema + ".Grupos where Estatus_Grupo='1' and cantidadCliente > 0 order by Nombre_grupo asc");
        }
        public DatosCorreo ConsultaDatosTipoMail(String id_tipo)
        {
            DatosCorreo resultado = new DatosCorreo();
            try
            {
                connMySql.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("Select nombre, valor from " + esquema + ".configuracion ", connMySql);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        switch (mySqlDataReader.GetString(0))
                        {
                            case "SMTP_SSL":
                                resultado.SMTP_SSL = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_CORREO":
                                resultado.SMTP_CORREO = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_PASS":
                                resultado.SMTP_PASS = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_HOST":
                                resultado.SMTP_HOST = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_PUERTO":
                                resultado.SMTP_PUERTO = mySqlDataReader.GetString(1);
                                break;
                            case "PRUEBAS":
                                resultado.PRUEBAS = mySqlDataReader.GetString(1);
                                break;
                            case "SMTP_CORREO_PRUEBA":
                                resultado.SMTP_CORREO_PRUEBA = mySqlDataReader.GetString(1);
                                break;
                            case "ENV_CORREO_A":
                                resultado.ENV_ESTADO = mySqlDataReader.GetString(1);
                                break;
                            default:
                                break;
                        }
                    }
                }
                mySqlDataReader.Close();
                //Traer datos de tipo correo
                mySqlCommand = new MySqlCommand("Select Sujeto, Mensaje, ImgCorreo, Firma, Nombre_tipo from " + esquema + ".tipo_correo where Id_tipo = " + id_tipo + ";", connMySql);
                mySqlDataReader = mySqlCommand.ExecuteReader();

                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        resultado.SMTP_SUJETO = mySqlDataReader.GetString(0);
                        resultado.SMTP_MENSAJE = mySqlDataReader.GetString(1);
                        resultado.SMTP_IMAGEN = mySqlDataReader.GetString(2);
                        resultado.SMTP_FIRMA = mySqlDataReader.GetString(3);
                    }
                }
                mySqlDataReader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cerrar_conexion();
            }
            return resultado;
        }




        public DataSet TraerClGpo(String id_grupo)
        {
            return EjecutaQueryDS("Select cliente.ID_CLIENTE, cliente.NOMBRE, cliente.APELLIDO_PATERNO, cliente.APELLIDO_MATERNO, cliente.EMAIL from " + esquema + ".clientesGrupo " +
                " left join " + esquema + ".cliente on cliente.ID_CLIENTE = clientesGrupo.ID_CLIENTE where clientesGrupo.ID_Grupo = " + id_grupo + " ");
        }

        //public DataSet traerGrupos()
        //{
        //    return EjecutaQueryDS("select nombre_grupo from " + esquema + ".grupos order by Nombre_Grupo");
        //}
        //public string traerIdsClientesGrupo(string seleccion)
        //{
        //    return EjecutaQueryString("select clientes_grupo from " + esquema + ".grupos_clientes where nombre_grupo = '" + seleccion + "';");
        //}

        //public string GuardarCliEnGrupo(string id, string seleccion)
        //{
        //    return EjecutaQueryInsert("UPDATE " + esquema + ".cliente SET GRUPO = '" + seleccion + "' where ID_CLIENTE = '" + id + "';");

        //    //string Ids = traerIdsClientesGrupo(seleccion);
        //    //return EjecutaQueryInsert("UPDATE "+esquema+".grupos_clientes SET clientes_grupo = '"+id+","+Ids+"' WHERE nombre_grupo = '"+seleccion+"';");
        //}

        //////////
        public DataSet TraerGrupos()
        {
            return EjecutaQueryDS("SELECT ID_Grupo, Nombre_Grupo, Descripcion_Grupo, cantidadCliente from " + esquema + ".grupos where Estatus_Grupo = '1' order by Nombre_Grupo");
        }

        public string GuardarNuevoGrupo(string nombre, string descripcion, String ID_empleado, String cantidadClientes)
        {
            return EjecutaQueryInsert("INSERT " + esquema + ".grupos (Nombre_Grupo, Descripcion_Grupo, ID_Usuario_Crea, Fecha_Crea, ID_Usuario_modifica, Fecha_Modifica, Estatus_Grupo, cantidadCliente)  VALUES ('" + nombre + "','" + descripcion + "'," + ID_empleado + ",sysdate()," + ID_empleado + ",sysdate(),'1'," + cantidadClientes + ")");
        }

        public DataSet TraerCliGrupos(string idGrupo)
        {
            return EjecutaQueryDS("select ID_Cliente from " + esquema + ".clientesgrupo WHERE ID_Grupo = " + idGrupo);
        }

        public String actualizaGrupoModificacion(String ID_Grupo, String ID_Empleado, String CantidadClientes)
        {
            return EjecutaQueryString("update " + esquema + ".grupos set ID_Usuario_modifica = " + ID_Empleado + ", Fecha_Modifica = sysdate(), cantidadCliente = " + CantidadClientes + " where ID_Grupo = " + ID_Grupo);
        }

        public string GuardarIDCLienteGrupo(string ID_Grupo, string id_cliente)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".clientesGrupo(ID_Grupo, ID_Cliente) values(" + ID_Grupo + ", " + id_cliente + ")");
        }

        public String EliminaGrupoClientes(String ID_Grupo, String ID_Empleado)
        {
            return EjecutaQueryString("update " + esquema + ".grupos set Estatus_Grupo = '0', ID_Usuario_modifica = " + ID_Empleado + ", Fecha_Modifica = sysdate() where ID_Grupo = " + ID_Grupo);
        }

        public String limpiaGrupoClientes(String ID_Grupo)
        {
            return EjecutaQueryString("delete from " + esquema + ".clientesGrupo where ID_Grupo = " + ID_Grupo);
        }






        #region CitasRececta
        public string GuardarReceta(String idCita, String medicina, String indicacion, String dosis, String frecuencia, String periodo)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".receta(Medicamento,Indicaciones,Dosis, id_cita,frecuencia, periodo)values( '" + medicina + "','" + indicacion + "','" + dosis + "'," + idCita + ",'" + frecuencia + "','" + periodo + "')");
        }

        public DataSet TraerReceta(String idCita)
        {
            return EjecutaQueryDS("select Medicamento, indicaciones, dosis, frecuencia, periodo  from  " + esquema + ".receta  where Id_Cita=" + idCita);
        }
        public string GuardarResultadoCita(String idCita, String objetivo, String sintomas, String indicacionesgenerales)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".resultadocita(Id_Cita,Objetivo,Sintomas,indicaciones_generales)values(" + idCita + ",'" + objetivo + "','" + sintomas + "','" + indicacionesgenerales + "')");
        }
        public string ActualizaResultadoCita(String idCita, String objetivo, String sintomas, String indicacionesgenerales)
        {
            return EjecutaQueryInsert("update " + esquema + ".resultadocita set Objetivo = '" + objetivo + "', Sintomas = '" + sintomas + "', indicaciones_generales = '" + indicacionesgenerales + "' where Id_Cita= " + idCita );
        }

        public DataSet TraerResultadoCita(String Id_Cita)
        {
            return EjecutaQueryDS("select objetivo, sintomas, indicaciones_generales  from  " + esquema + ".resultadocita  where Id_Cita=" + Id_Cita);
        }

        public String BorrarMedicamento(String medicamentoFinal, String idCitaFinal)
        {
            return EjecutaQueryString("delete from " + esquema + ".receta where Id_Cita= " + idCitaFinal + " and medicamento='" + medicamentoFinal + "'");
        }

        public String ActualizaReceta(String idcita, String medicamentoNuevo, String indicacionNuevo, String dosisNuevo, String frecuenciaNuevo, String periodoNuevo, String medicamentoViejo, String indicacionViejo, String dosisViejo, String frecuenciaViejo, String periodoViejo)
        {
            return EjecutaQueryString("update " + esquema + ".receta  set  medicamento='" + medicamentoNuevo + "', indicaciones= '" + indicacionNuevo + "', dosis ='" + dosisNuevo + "',frecuencia='" + frecuenciaNuevo + "',periodo ='" + periodoNuevo + "' where  Id_Cita =" + idcita + " and  medicamento='" + medicamentoViejo + "'  and  indicaciones ='" + indicacionViejo + "'  and  dosis ='" + dosisViejo + "'");
        }

        public string GuardarExpediente(String idCita, String nombreArchivoT, String rutaT)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".archivo (Id_Cita, archivo, ruta_archivo )values(" + idCita + ",'" + nombreArchivoT + "','" + rutaT + "')");
        }

        public DataSet traerExpediente(String Id_Cita)
        {
            return EjecutaQueryDS("select Id_Cita, archivo, ruta_archivo from  " + esquema + ".archivo  where Id_Cita=" + Id_Cita);
        }

        public DataSet TraerResultadoCitaCompara(String idCita)
        {
            return EjecutaQueryDS("select objetivo, sintomas, indicaciones_generales  from  " + esquema + ".resultadocita  where Id_Cita=" + idCita);
        }
        #endregion

        public string GuardarHistoriaClinica(String id_empleado, String Fitzpatrick, String Loc_cabeza, String Loc_tronco, String Loc_extinf, String Loc_extsup, String Ext_localizada, String Ext_Diseminada, String Ext_Generalizada, String Predominio, String Morfologia, String Interrogatorio, String Resto_anexos, String Sintomatologia, String Tratamientos_anterior, String Antecedentes, String Diagnostico, String Tratamiento, String ID_Cliente)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".Historia_Clinica (ID_Empleado_Crea,Fecha_creacion,ID_Empleado_Modifica,Fecha_modificacion,Fitzpatrick,Loc_cabeza,Loc_tronco,Loc_extinf,Loc_extsup,Ext_localizada,Ext_Diseminada,Ext_Generalizada,Predominio,Morfologia,Interrogatorio,Resto_anexos,Sintomatologia,Tratamientos_anterior,Antecedentes,Diagnostico,Tratamiento,ID_Cliente)" +
                " values (" + id_empleado + ",sysdate()," + id_empleado + ",sysdate(),'" + Fitzpatrick + "','" + Loc_cabeza + "','" + Loc_tronco + "','" + Loc_extinf + "','" + Loc_extsup + "','" + Ext_localizada + "','" + Ext_Diseminada + "','" + Ext_Generalizada + "','" + Predominio + "','" + Morfologia + "','" + Interrogatorio + "','" + Resto_anexos + "','" + Sintomatologia + "','" + Tratamientos_anterior + "','" + Antecedentes + "','" + Diagnostico + "','" + Tratamiento + "'," + ID_Cliente + ");");
        }

        public DataSet TraerHistoriaC(String id_cliente)
        {
            return EjecutaQueryDS("select Fitzpatrick,Loc_cabeza,Loc_tronco,Loc_extinf,Loc_extsup,Ext_localizada,Ext_Diseminada,Ext_Generalizada,Predominio,Morfologia,Interrogatorio,Resto_anexos,Sintomatologia,Tratamientos_anterior,Antecedentes,Diagnostico,Tratamiento  from  " + esquema + ".Historia_Clinica  where ID_Cliente=" + id_cliente);
        }

        public String ActualizaHistoriaC(String id_empleado, String Fitzpatrick, String Loc_cabeza, String Loc_tronco, String Loc_extinf, String Loc_extsup, String Ext_localizada, String Ext_Diseminada, String Ext_Generalizada, String Predominio, String Morfologia, String Interrogatorio, String Resto_anexos, String Sintomatologia, String Tratamientos_anterior, String Antecedentes, String Diagnostico, String Tratamiento, String ID_Cliente)
        {
            return EjecutaQueryString("Update " + esquema + ".Historia_Clinica set ID_Empleado_Modifica=" + id_empleado + ",Fecha_modificacion=sysdate(),Fitzpatrick='" + Fitzpatrick + "',Loc_cabeza='" + Loc_cabeza + "',Loc_tronco='" + Loc_tronco + "',Loc_extinf='" + Loc_extinf + "',Loc_extsup='" + Loc_extsup + "',Ext_localizada='" + Ext_localizada + "',Ext_Diseminada='" + Ext_Diseminada + "',Ext_Generalizada='" + Ext_Generalizada + "',Predominio='" + Predominio + "',Morfologia='" + Morfologia + "'," +
                "Interrogatorio='" + Interrogatorio + "',Resto_anexos='" + Resto_anexos + "',Sintomatologia='" + Sintomatologia + "',Tratamientos_anterior='" + Tratamientos_anterior + "',Antecedentes='" + Antecedentes + "',Diagnostico='" + Diagnostico + "',Tratamiento='" + Tratamiento + "' where ID_Cliente=" + ID_Cliente);
        }

        public bool ValidaExistenciaHistoria(String id_cliente)
        {
            bool resultado = false;
            try
            {
                connMySql.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("Select ID_Historia from " + esquema + ".Historia_Clinica where ID_Cliente=" + id_cliente + ";", connMySql);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (mySqlDataReader.HasRows)
                {
                    if (mySqlDataReader.Read())
                    {
                        resultado = true;
                    }
                }
                mySqlDataReader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cerrar_conexion();
            }
            return resultado;
        }

        public string GuardarNotaEvolucion(String id_empleado, String ta, String fc, String fr, String temperatura, String peso, String talla, String evolucion, String diagnostico, String tratamiento, String id_cita)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".Notas_evolucion (ID_Empleado_Crea,Fecha_creacion,ID_Empleado_Modifica,Fecha_modificacion,TA,FC,FR,Temperatura,Peso,Talla,Evolucion,Diagnostico,Tratamiento,ID_Cita)" +
                " values (" + id_empleado + ",sysdate()," + id_empleado + ",sysdate(),'" + ta + "','" + fc + "','" + fr + "','" + temperatura + "','" + peso + "','" + talla + "','" + evolucion + "','" + diagnostico + "','" + tratamiento + "'," + id_cita + ");");
        }

        public DataSet TraerNotaEvolucion(String id_cita)
        {
            return EjecutaQueryDS("Select TA,FC,FR,Temperatura,Peso,Talla,Evolucion,Diagnostico,Tratamiento from " + esquema + ".Notas_evolucion where ID_Cita=" + id_cita);
        }

        public String ActualizaNotaEvolucion(String id_empleado, String ta, String fc, String fr, String temperatura, String peso, String talla, String evolucion, String diagnostico, String tratamiento, String id_cita)
        {
            return EjecutaQueryString("Update " + esquema + ".Notas_evolucion set ID_Empleado_Modifica=" + id_empleado + ",Fecha_modificacion=sysdate(),TA='" + ta + "',FC='" + fc + "',FR='" + fr + "',Temperatura='" + temperatura + "'," +
                "Peso='" + peso + "',Talla='" + talla + "',Evolucion='" + evolucion + "',Diagnostico='" + diagnostico + "',Tratamiento='" + tratamiento + "' where ID_Cita=" + id_cita);
        }
        public DataSet TraerProxCita(String id_cliente, String fecha_cita)
        {
            return EjecutaQueryDS("Select MIN(date_format(fecha,'%Y-%m-%d')) fecha,DATE_FORMAT(hora_inicio,'%H:%i:%s') hora_inicio, DATE_FORMAT(hora_fin,'%H:%i:%s') hora_fin from " + esquema + ".citas where id_cliente=" + id_cliente + " and Fecha>'" + fecha_cita + "' and estatus_cita=1;");
        }
        public bool ValidaExistenciaEvolucion(String id_cita)
        {
            bool resultado = false;
            try
            {
                connMySql.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("Select ID_Nota from " + esquema + ".Notas_evolucion where ID_Cita=" + id_cita + ";", connMySql);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (mySqlDataReader.HasRows)
                {
                    if (mySqlDataReader.Read())
                    {
                        resultado = true;
                    }
                }
                mySqlDataReader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cerrar_conexion();
            }
            return resultado;
        }

        public bool ValidaExistenciaDatosMedico(String id_empleado)
        {
            bool resultado = false;
            try
            {
                connMySql.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("Select ID_Datos from " + esquema + ".Datos_Medico where id_usuario=" + id_empleado + ";", connMySql);
                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                if (mySqlDataReader.HasRows)
                {
                    if (mySqlDataReader.Read())
                    {
                        resultado = true;
                    }
                }
                mySqlDataReader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                cerrar_conexion();
            }
            return resultado;
        }
        public DataSet TraerMedicos(String condicion)
        {
            return EjecutaQueryDS("select empleado.id_usuario, empleado.nombre, empleado.apellido_paterno, empleado.apellido_materno, datos.prefijo, datos.titulo, " +
                "datos.especialidades, datos.Cedula_profesional, datos.Cedula_especialidad, datos.instagram, datos.facebook, datos.correo, datos.sitio_web, " +
                "datos.numero_celular, datos.numero_fijo from " + esquema + ".empleado left join " + esquema + ".datos_medico as datos on datos.id_usuario = empleado.id_usuario " +
                "where empleado.medico = '1' and empleado.estatus = 1 " + (condicion.Trim().Length > 0 ? " and " + condicion : "") + " order by empleado.nombre asc, " +
                "empleado.apellido_paterno asc, empleado.apellido_materno asc;");
        }
        public string GuardarDatos_Medico(String id_medico, String empleado, String prefijo, String titulo, String especialidades, String cedula_prof, String cedula_esp, String instagram, String facebook, String correo, String sitio_web, String num_cel, String num_fijo)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".datos_medico (ID_usuario, ID_Empleado_Crea, Fecha_creacion, ID_Empleado_Modifica, Fecha_modificacion, " +
                "prefijo, titulo, especialidades, Cedula_profesional, Cedula_especialidad, instagram, facebook, correo, sitio_web, numero_celular, numero_fijo) " +
                "values (" + id_medico + "," + empleado + ",sysdate()," + empleado + ",sysdate(),'" + prefijo + "','" + titulo + "','" + especialidades + "','" +
                cedula_prof + "','" + cedula_esp + "','" + instagram + "','" + facebook + "','" + correo + "','" + sitio_web + "','" + num_cel + "','" + num_fijo + "');");
        }
        public String ActualizaDatosMedico(String id_medico, String id_empleado, String prefijo, String titulo, String especialidades, String cedula_prof, String cedula_esp, String instagram, String facebook, String correo, String sitio_web, String num_cel, String num_fijo)
        {
            return EjecutaQueryInsert("Update " + esquema + ".datos_medico set ID_Empleado_Modifica=" + id_empleado + ",Fecha_modificacion=sysdate(), prefijo = '" + prefijo + "', " +
                "titulo = '" + titulo + "', especialidades = '" + especialidades + "', Cedula_profesional = '" + cedula_prof + "', Cedula_especialidad = '" + cedula_esp + "', " +
                "instagram = '" + instagram + "', facebook = '" + facebook + "', correo = '" + correo + "', sitio_web = '" + sitio_web + "', numero_celular = '" + num_cel + "', " +
                "numero_fijo = '" + num_fijo + "' where id_usuario=" + id_medico);
        }
        public String AgregarMedico(String id_medico)
        {
            return EjecutaQueryInsert("Update " + esquema + ".empleado set medico = '1' where id_usuario = " + id_medico);
        }
        public String EliminaMedico(String id_medico)
        {
            return EjecutaQueryInsert("update " + esquema + ".empleado set medico = '0' where id_usuario= " + id_medico);
        }

        public DataSet TraerNotasEvolucionCliente(String id_cliente)
        {
            return EjecutaQueryDS("Select ID_Nota, TA, FC, FR, Temperatura, Peso, Talla, Evolucion, Diagnostico, Tratamiento, DATE_FORMAT(notas_evolucion.Fecha_creacion,'%d-%m-%Y') as Fecha, " +
                "citas.ID_Cita from " + esquema + ".notas_evolucion inner join " + esquema + ".citas on notas_evolucion.ID_Cita = citas.ID_Cita where citas.id_cliente = " + id_cliente);
        }


        public DataSet TraerInfoDoctor(String doctorid)
        {
            return EjecutaQueryDS("select prefijo, titulo, especialidades, instagram, facebook, Sitio_web, Numero_celular, Cedula_profesional, Cedula_especialidad, correo, numero_fijo from " + esquema + ".datos_medico  where ID_Datos=" + doctorid);
        }
    }
}