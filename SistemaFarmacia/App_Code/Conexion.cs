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
        #endregion

        #region Chayo

        public DataSet TraerClienteCorreo(String ID_Cliente)
        {
            return EjecutaQueryDS("select ID_CLIENTE,NOMBRE,APELLIDO_PATERNO,APELLIDO_MATERNO, (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) AS EDAD, FECHA_NACIMIENTO, FECHA_INGRESO, ESTADO, MUNICIPIO, tel_casa_fijo,extension, celular, EMAIL, OBSERVACIONES, NOTA, MEDIO, CASE WHEN ESTATUS = 1 THEN 'Activo' else 'Inactivo' end ESTATUS, PAIS, Enviar_Correo, CASE WHEN REQ_FACTURA = 1 THEN 'Sí' else 'No' end REQ_FACTURA from " + esquema + ".cliente where ID_CLIENTE = '" + ID_Cliente + "' and Enviar_Correo='1'");
        }


        public DataSet TraerClientes(String condicion, String Estatus)
        {
            return EjecutaQueryDS("select ID_CLIENTE,NOMBRE,APELLIDO_PATERNO,APELLIDO_MATERNO, (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) AS EDAD, FECHA_NACIMIENTO, FECHA_INGRESO, ESTADO, MUNICIPIO, tel_casa_fijo,extension, celular, EMAIL, OBSERVACIONES, NOTA, MEDIO, CASE WHEN ESTATUS = 1 THEN 'Activo' else 'Inactivo' end ESTATUS, PAIS, Enviar_Correo, CASE WHEN REQ_FACTURA = 1 THEN 'Sí' else 'No' end REQ_FACTURA, RFC, ENTIDAD, calle_dirf ,noInterior_dirf ,noExterior_dirf ,Colonia_dirf ,CP_dirf ,Estado_dirf ,municipio_dirf ,pais_dirf, NOMBRAZON_FACTURA from " + esquema + ".cliente " + (condicion.Trim().Length > 0 ? " where " + condicion + " and estatus " + (Estatus == "1,0" ? " in ('1','0')" : " and estatus = '" + Estatus + "'") : " where estatus " + (Estatus =="1,0"? " in ('1','0') ": " = '"+Estatus+"'")) + " order by ID_CLIENTE desc");
        }

        public String GuardaCliente(String NOMBRE, String APELLIDO_PATERNO, String APELLIDO_MATERNO, String EDAD, String FECHA_NACIMIENTO, String FECHA_INGRESO, String MUNICIPIO, String tel_casa_fijo, String extension, String celular, String EMAIL, String OBSERVACIONES, String NOTA, String MEDIO, String Estatus, String Estado, String Pais, String Enviar_Correo, String ID_Empleado, String Req_Factura, String RFC, String Entidad, String CalleF, String NIntF, String NExtF, String ColoniaF, String CPF, String EstadoF, String MunicipioF, String PaisF, String NomRazon)
        {
            return EjecutaQueryInsert("insert into " + esquema + ".cliente(NOMBRE, APELLIDO_PATERNO, APELLIDO_MATERNO, EDAD,FECHA_NACIMIENTO, FECHA_INGRESO, MUNICIPIO, tel_casa_fijo, extension, celular, EMAIL, OBSERVACIONES, NOTA, MEDIO, ESTATUS, Estado, Pais, Enviar_Correo, Fecha_creacion, ID_Empleado_Creacion, Fecha_Modificacion, Id_Empleado_Modificacion, req_factura, rfc, entidad, calle_dirf ,noInterior_dirf ,noExterior_dirf ,Colonia_dirf ,CP_dirf ,Estado_dirf ,municipio_dirf ,pais_dirf,NombRazon_factura) " +
                "values('" + NOMBRE + "','" + APELLIDO_PATERNO + "','" + APELLIDO_MATERNO + "'," + (((EDAD == null || EDAD == "") ? "null" : "'" + EDAD + "'")) + "," + ((FECHA_NACIMIENTO == null || FECHA_NACIMIENTO == "") ? "null" : "'" + FECHA_NACIMIENTO + "'") + "," + ((FECHA_INGRESO == null || FECHA_INGRESO == "") ? "null" : "'" + FECHA_INGRESO + "'") + ",'" + MUNICIPIO + "','" + tel_casa_fijo + "','" + extension + "','" + celular + "','" + EMAIL + "','" + OBSERVACIONES + "','" + NOTA + "','" + MEDIO + "','" + Estatus + "', '" + Estado + "','" + Pais + "','" + Enviar_Correo + "',sysdate(), "+ ID_Empleado + ", sysdate(), " + ID_Empleado + ",'" + Req_Factura + "','" + RFC + "','" + Entidad + "','" + CalleF + "','" + NIntF + "','" + NExtF + "','" + ColoniaF + "','" + CPF + "','" + EstadoF + "','" + MunicipioF + "','" + PaisF + "','" + NomRazon + "')");
            
            //return EjecutaQueryInsert("insert into cliente(NOMBRE, APELLIDO_PATERNO, APELLIDO_MATERNO, EDAD, FECHA_NACIMIENTO, FECHA_INGRESO, MUNICIPIO, tel_casa_fijo, extension, celular, EMAIL, OBSERVACIONES, NOTA, MEDIO, estatus) " +
            //"values('" + NOMBRE + "','" + APELLIDO_PATERNO + "','" + APELLIDO_MATERNO + "','" + EDAD + "','" + FECHA_NACIMIENTO + "','" + FECHA_INGRESO + "','" + MUNICIPIO + "','" + tel_casa_fijo + "','" + extension + "','" + celular + "','" + EMAIL + "','" + OBSERVACIONES + "','" + NOTA + "','" + MEDIO + "','1')");
        }
        public String ActualizaCliente(String id_cliente, String NOMBRE, String APELLIDO_PATERNO, String APELLIDO_MATERNO, String EDAD, String FECHA_NACIMIENTO, String FECHA_INGRESO, String MUNICIPIO, String tel_casa_fijo, String extension, String celular, String EMAIL, String OBSERVACIONES, String NOTA, String MEDIO, String ESTATUS, String Estado, String Pais, String Enviar_Correo, String ID_Empleado, String R_Factura, String RFC, String Entidad, String CalleF, String NIntF, String NExtF, String ColoniaF, String CPF, String EstadoF, String MunicipioF, String PaisF, String NomRazon)
        {
            return EjecutaQueryInsert("update " + esquema + ".cliente set NOMBRE='" + NOMBRE + "', APELLIDO_PATERNO='" + APELLIDO_PATERNO + "', APELLIDO_MATERNO='" + APELLIDO_MATERNO + "', EDAD=" + (((EDAD == null || EDAD == "") ? "null" : "'" + EDAD + "'")) + ", FECHA_NACIMIENTO=" + ((FECHA_NACIMIENTO == null || FECHA_NACIMIENTO == "") ? "null" : "'" + FECHA_NACIMIENTO + "'") + ", FECHA_INGRESO=" + ((FECHA_INGRESO == null || FECHA_INGRESO == "") ? "null" : "'" + FECHA_INGRESO + "'") + ", MUNICIPIO='" + MUNICIPIO + "', tel_casa_fijo='" + tel_casa_fijo + "', extension='" + extension + "', celular='" + celular + "', EMAIL = '" + EMAIL + "', OBSERVACIONES = '" + OBSERVACIONES + "', NOTA = '" + NOTA + "', MEDIO = '" + MEDIO + "', ESTATUS = '" + ESTATUS + "', ESTADO = '" + Estado + "', PAIS ='" + Pais + "', Enviar_Correo = '" + Enviar_Correo + "', Fecha_Modificacion = sysdate(), Id_Empleado_Modificacion = " + ID_Empleado + ", req_factura = '" + R_Factura + "', rfc = '" + RFC + "', entidad = '" + Entidad + "', calle_dirf = '" + CalleF + "', noInterior_dirf = '" + NIntF + "' ,noExterior_dirf = '" + NExtF + "',Colonia_dirf = '" + ColoniaF + "',CP_dirf = '" + CPF + "',Estado_dirf = '" + EstadoF + "',municipio_dirf = '" + MunicipioF + "',pais_dirf = '" + PaisF + "', NombRazon_factura = '" + NomRazon + "'  where id_cliente = '" + id_cliente + "'");
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
            return EjecutaQueryDS("select id_usuario, nombre, apellido_paterno, apellido_materno, usuario,descripcion as Perfil from " + esquema + ".empleado left join " + esquema + ".perfil on empleado.id_perfil = perfil.id_perfil  where empleado.estatus = 1 " + (condicion.Trim().Length > 0 ? " and " + condicion : "") + " order by nombre asc, apellido_paterno asc, apellido_materno asc");
        }

        //public String GuardaUsuario(String Nombre, String ApellidoP, String ApellidoM, String Usuario, String Perfil, String Contrasenia)
        //{
        //    return EjecutaQueryInsert("Insert into " + esquema + ".empleado (nombre, apellido_paterno, apellido_materno, usuario, contraseña, id_perfil, estatus) values('" + Nombre + "','" + ApellidoP + "','" + ApellidoM + "','" + Usuario + "','" + Contrasenia + "','" + Perfil + "','1')");
        //}
        public String GuardaUsuario(String Nombre, String ApellidoP, String ApellidoM, String Usuario, String Perfil, String Contrasenia, String Email, String Pregunta, String Respuesta)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".empleado (nombre, apellido_paterno, apellido_materno, usuario, contraseña, id_perfil, estatus, email, pregunta, respuesta) values('" + Nombre + "','" + ApellidoP + "','" + ApellidoM + "','" + Usuario + "','" + Contrasenia + "','" + Perfil + "','1','" + Email + "','" + Pregunta + "','" + Respuesta + "')");
        }
        //public String ActualizaUsuario(String id_usuario, String Nombre, String ApellidoP, String ApellidoM, String Usuario, String Perfil, String Contrasenia, bool CambiaContraseña)
        //{
        //    return EjecutaQueryInsert("Update " + esquema + ".empleado set nombre = '" + Nombre + "', apellido_paterno = '" + ApellidoP + "', apellido_materno='" + ApellidoM + "', usuario='" + Usuario + "', id_perfil='" + Perfil + "' " + (CambiaContraseña ? ", contraseña = '" + Contrasenia + "'" : "") + " where id_usuario = '" + id_usuario + "'");
        //}
        public String ActualizaUsuario(String id_usuario, String Nombre, String ApellidoP, String ApellidoM, String Usuario, String Perfil, String Contrasenia, bool CambiaContraseña, bool CambiaRespuesta, String Email, String Pregunta, String Respuesta)
        {
            return EjecutaQueryInsert("Update " + esquema + ".empleado set nombre = '" + Nombre + "', apellido_paterno = '" + ApellidoP + "', apellido_materno='" + ApellidoM + "', usuario='" + Usuario + "', id_perfil='" + Perfil + "' " + (CambiaContraseña ? ", contraseña = '" + Contrasenia + "'" : "") + ", email = '" + Email + "', pregunta = '" + Pregunta + "'" + (CambiaRespuesta ? ", respuesta = '" + Respuesta + "'" : "") + " where id_usuario = '" + id_usuario + "'");
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
                    while(mySqlDataReader.Read())
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
            return EjecutaQueryDS("select ID_CLIENTE,NOMBRE,APELLIDO_PATERNO,APELLIDO_MATERNO, (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) AS EDAD, FECHA_NACIMIENTO, FECHA_INGRESO, ESTADO, MUNICIPIO, tel_casa_fijo,extension, celular, EMAIL,OBSERVACIONES, NOTA, MEDIO,Enviar_Correo " +
                " from " + esquema + ".cliente "+
                " where estatus   " + (Estatus == "1,0" ? " in ('1','0')" : " = '" + Estatus + "' ") + " and Date_format(fecha_nacimiento,'%m') =  (case when date_format(sysdate(),'%m') = 12 then 1  else date_format(sysdate(),'%m')+1 end) " + (condicion.Trim().Length > 0 ? " and " + condicion : "") );
        }

        public DataSet TraerClientesDelMes(String condicion, int mes, String estatus)
        {
            return EjecutaQueryDS("select ID_CLIENTE,NOMBRE,APELLIDO_PATERNO,APELLIDO_MATERNO, (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) AS EDAD, FECHA_NACIMIENTO, " +
                " FECHA_INGRESO, ESTADO, MUNICIPIO, tel_casa_fijo,extension, celular, EMAIL,OBSERVACIONES, NOTA, MEDIO, CHECK_EMAIL,Enviar_Correo " +
                " from " + esquema + ".cliente where estatus " + (estatus == "1,0" ? " in ('1','0')" : " = '" + estatus + "' ") + " and date_format(fecha_nacimiento,'%m') = '" + mes.ToString().PadLeft(2,'0') + "' " + (condicion.Trim().Length > 0 ? " and " + condicion : ""));
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
            catch(Exception ex)
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
                foreach(String ID in ID_Cliente) { 
                    EjecutaQueryInsert("Insert into " + esquema + ".BitacoraDestinos(ID_Entrada,ID_Cliente) values((select last_insert_id()), "+ID+")");
                }
                EjecutaQueryInsert("Insert into " + esquema + ".BitacoraEstructura(ID_Entrada, Asunto, Mensaje, imagen, firma) values((select last_insert_id()), '" + dCorreo.SMTP_SUJETO + "','" + dCorreo.SMTP_MENSAJE + "','" + dCorreo.SMTP_IMAGEN + "','" + dCorreo.SMTP_FIRMA + "')");
            }
            return true;
        }


        public String traerIDEmpleado(String empleado)
        {
            return EjecutaQueryString("select id_usuario from " + esquema + ".empleado where usuario = '" + empleado + "'");
        }

        public String guardaError(String Error,String Modulo, String Evento)
        {
            return EjecutaQueryInsert("Insert into " + esquema + ".LogErrores(Modulo,Evento,msjError,Fecha) values('"+Modulo+"','"+Evento+"','"+Error+"',sysdate())");
        }



        public DataSet TraerClEliminados(String Condicion)
        {
            return EjecutaQueryDS("select ID_CLIENTE,NOMBRE,APELLIDO_PATERNO,APELLIDO_MATERNO, (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) AS EDAD, FECHA_NACIMIENTO, FECHA_INGRESO, ESTADO, MUNICIPIO, tel_casa_fijo,extension, celular, EMAIL, OBSERVACIONES, NOTA, MEDIO, CASE WHEN ESTATUS = 1 THEN 'Activo' else 'Inactivo' end ESTATUS, PAIS, Enviar_Correo, CASE WHEN REQ_FACTURA = 1 THEN 'Sí' else 'No' end REQ_FACTURA, RFC, ENTIDAD, DIR_FACTURA, NOMBRAZON_FACTURA from " + esquema + ".cliente where estatus = 3 " + (Condicion.Length > 0 ? " and " + Condicion: "" ) + " order by ID_CLIENTE desc");
        }

        public String ReactivaCliente(String Id_Cliente, String ID_Empleado)
        {
            return EjecutaQueryInsert("update " + esquema + ".cliente set estatus = 1, Fecha_Modificacion = sysdate(), Id_Empleado_Modificacion = " + ID_Empleado + " where id_cliente = " + Id_Cliente);
        }

        public DataSet traerCitasDoctor(String Mes, String Doctor)
        {
            return EjecutaQueryDS("Select ID_Cita,Citas.ID_CLIENTE, DATE_FORMAT(hora_inicio,'%Y-%m-%d %H:%i:%s') hora_inicio,  DATE_FORMAT(hora_fin,'%Y-%m-%d %H:%i:%s') hora_fin, Citas.nota, nombre, apellido_paterno, apellido_materno from " + esquema + ".Citas left join " + esquema + ".cliente on Citas.ID_CLIENTE = Cliente.ID_CLIENTE where ID_usuario=" + Doctor + " and DATE_FORMAT(hora_inicio,'%m') = '" + Mes.PadLeft(2, '0') + "'");
        }
    }
}