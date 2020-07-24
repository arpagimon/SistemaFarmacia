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

        public void cerrar_conexion()
        {
            if (connMySql.State != System.Data.ConnectionState.Closed)
            {
                connMySql.Close();
            }
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

        public String ConsultaPermisos(string usuario)
        {
            String resultado = "";
            try
            {
                connMySql.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("select permisos from empleado join perfil on empleado.id_perfil = perfil.id_perfil where usuario= '" + usuario + "'", connMySql);
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

                MySqlCommand mySqlCommand = new MySqlCommand("Select contraseña from farmacia.empleado where usuario='" + usuario + "' and contraseña = '" + contrasena + "'and estatus='1' ", connMySql);
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

                MySqlCommand mySqlCommand = new MySqlCommand("update  farmacia.empleado  set contraseña ='" + contrasena + "' where usuario ='" + usuario + "' and nombre='" + nombre + "' and apellido_paterno='" + apellido + "' and apellido_materno='" + apellidoM + "' AND ESTATUS = '1'", connMySql);
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

        public DataSet TraerClientes(String condicion)
        {
            return EjecutaQueryDS("select ID_CLIENTE,NOMBRE,APELLIDO_PATERNO,APELLIDO_MATERNO, EDAD, DATE_FORMAT(FECHA_NACIMIENTO, '%d/%m/%Y') as FECHA_NACIMIENTO, DATE_FORMAT(FECHA_INGRESO, '%d/%m/%Y') as FECHA_INGRESO, MUNICIPIO, tel_casa_fijo,extension, celular, EMAIL,OBSERVACIONES, NOTA, MEDIO from farmacia.cliente where estatus = 1 " + (condicion.Trim().Length > 0 ? " and " + condicion : "") + " order by nombre asc, apellido_paterno asc, apellido_materno asc");
        }

        public String GuardaCliente(String NOMBRE, String APELLIDO_PATERNO, String APELLIDO_MATERNO, String EDAD, String FECHA_NACIMIENTO, String FECHA_INGRESO, String MUNICIPIO, String tel_casa_fijo, String extension, String celular, String EMAIL, String OBSERVACIONES, String NOTA, String MEDIO)
        {
            return EjecutaQueryInsert("insert into cliente(NOMBRE, APELLIDO_PATERNO, APELLIDO_MATERNO, EDAD, FECHA_NACIMIENTO, FECHA_INGRESO, MUNICIPIO, tel_casa_fijo, extension, celular, EMAIL, OBSERVACIONES, NOTA, MEDIO, estatus) " +
                "values('" + NOMBRE + "','" + APELLIDO_PATERNO + "','" + APELLIDO_MATERNO + "','" + EDAD + "','" + FECHA_NACIMIENTO + "','" + FECHA_INGRESO + "','" + MUNICIPIO + "','" + tel_casa_fijo + "','" + extension + "','" + celular + "','" + EMAIL + "','" + OBSERVACIONES + "','" + NOTA + "','" + MEDIO + "','1')");
        }
        public String ActualizaCliente(String id_cliente, String NOMBRE, String APELLIDO_PATERNO, String APELLIDO_MATERNO, String EDAD, String FECHA_NACIMIENTO, String FECHA_INGRESO, String MUNICIPIO, String tel_casa_fijo, String extension, String celular, String EMAIL, String OBSERVACIONES, String NOTA, String MEDIO)
        {
            return EjecutaQueryInsert("update cliente set NOMBRE='" + NOMBRE + "', APELLIDO_PATERNO='" + APELLIDO_PATERNO + "', APELLIDO_MATERNO='" + APELLIDO_MATERNO + "', EDAD='" + EDAD + "', FECHA_NACIMIENTO='" + FECHA_NACIMIENTO + "', FECHA_INGRESO='" + FECHA_INGRESO + "', MUNICIPIO='" + MUNICIPIO + "', tel_casa_fijo='" + tel_casa_fijo + "', extension='" + extension + "', celular='" + celular + "', EMAIL = '" + EMAIL + "', OBSERVACIONES = '" + OBSERVACIONES + "', NOTA = '" + NOTA + "', MEDIO = '" + MEDIO + "' where id_cliente = '" + id_cliente + "'");
        }

        public String EliminaCliente(String Id_Cliente)
        {
            return EjecutaQueryInsert("update farmacia.cliente set estatus = 0 where id_cliente = " + Id_Cliente);
        }

        public DataSet LLENAR_TABLA()
        {
            return EjecutaQueryDS("select * from  farmacia.cliente c join farmacia.contacto_cliente cl on  c.ID_CLIENTE=cl.ID_CLIENTE");
        }

        public DataSet consultarLlENAR(string nombre)
        {

            return EjecutaQueryDS("select * from  farmacia.cliente c join farmacia.contacto_cliente cl on  c.ID_CLIENTE=cl.ID_CLIENTE  where c.nombre like '%" + nombre + "%'");
        }

        public void insertar(string nombre, string apellido_paterno, string apellido_marteno, string edad, string fecha_nacimiento, string fecha_ingreso, string municipio, string calle, string no_int, string no_ext, string tel_casa_fijo, string extension, string celular, string email)
        {

            try
            {
                connMySql.Open();
                MySqlCommand comandoInserta = connMySql.CreateCommand();
                comandoInserta.CommandType = CommandType.Text;
                comandoInserta.CommandText = "insert into farmacia.cliente(nombre,apellido_paterno, apellido_marteno,edad, fecha_nacimiento, fecha_ingreso) values ('" + nombre + "','" + apellido_paterno + "','" + apellido_marteno + "'," + edad + ", '" + fecha_nacimiento + "','" + fecha_ingreso + "', )";
                comandoInserta.ExecuteNonQuery();
                MySqlCommand comandoInserta2 = connMySql.CreateCommand();
                comandoInserta2.CommandType = CommandType.Text;
                comandoInserta2.CommandText = "insert into farmacia.contacto_cliente(municipio, calle, no_int , no_ext, tel_casa_fijo,extension, celular, email ) values ('" + municipio + "','" + calle + "', '" + no_int + "', '" + no_ext + "', '" + tel_casa_fijo + "', '" + extension + "','" + celular + "','" + email + "')";
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

        #endregion

        #region Pablo
        #region Adminstracion
        #region Usuario
        public DataSet TraerUsuarios(String condicion)
        {
            return EjecutaQueryDS("select id_usuario, nombre, apellido_paterno, apellido_materno, usuario,descripcion as Perfil from empleado left join perfil on empleado.id_perfil = perfil.id_perfil  where empleado.estatus = 1 " + (condicion.Trim().Length > 0 ? " and " + condicion : "") + " order by nombre asc, apellido_paterno asc, apellido_materno asc");
        }

        public String GuardaUsuario(String Nombre, String ApellidoP, String ApellidoM, String Usuario, String Perfil, String Contrasenia)
        {
            return EjecutaQueryInsert("Insert into Farmacia.empleado (nombre, apellido_paterno, apellido_materno, usuario, contraseña, id_perfil, estatus) values('" + Nombre + "','" + ApellidoP + "','" + ApellidoM + "','" + Usuario + "','" + Contrasenia + "','" + Perfil + "','1')");
        }

        public String ActualizaUsuario(String id_usuario, String Nombre, String ApellidoP, String ApellidoM, String Usuario, String Perfil, String Contrasenia, bool CambiaContraseña)
        {
            return EjecutaQueryInsert("Update Farmacia.empleado set nombre = '" + Nombre + "', apellido_paterno = '" + ApellidoP + "', apellido_materno='" + ApellidoM + "', usuario='" + Usuario + "', id_perfil='" + Perfil + "' " + (CambiaContraseña ? ", contraseña = '" + Contrasenia + "'" : "") + " where id_usuario = '" + id_usuario + "'");
        }
        public DataSet TraerPerfilesddl(String condicion)
        {
            return EjecutaQueryDS("select id_Perfil, descripcion from farmacia.perfil  " + (condicion.Trim().Length > 0 ? " where " + condicion : "") + " order by descripcion asc");
        }

        public String EliminaUsuario(String Id_Usuario)
        {
            return EjecutaQueryInsert("update farmacia.empleado set estatus = 0 where id_usuario= " + Id_Usuario);
        }
        #endregion


        #region Perfiles
        public DataSet TraerPerfiles(String condicion)
        {
            return EjecutaQueryDS("SELECT id_perfil, descripcion, permisos FROM farmacia.PERFIL where estatus = 1 " + (condicion.Trim().Length > 0 ? " and " + condicion : "") + " order by descripcion asc");
        }

        public String Guardaperfiles(String Descripcion, String Permiso)
        {
            return EjecutaQueryInsert("Insert into Farmacia.perfil (descripcion, permisos, fechaAlta,estatus ) values('" + Descripcion + "','" + Permiso + "',sysdate(),'1')");
        }

        public String ActualizaPerfil(String id_perfil, String Descripcion, String Permisos)
        {
            return EjecutaQueryInsert("Update Farmacia.perfil set descripcion = '" + Descripcion + "', permisos = '" + Permisos + "' where id_perfil = '" + id_perfil + "'");
        }
        public String EliminaPerfil(String Id_perfil)
        {
            //No funca
            return EjecutaQueryInsert("update farmacia.perfil set estatus = 0 where id_perfil= " + Id_perfil);
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

                MySqlCommand mySqlCommand = new MySqlCommand("Select nombre, valor from farmacia.configuracion ", connMySql);
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
                    mySqlCommand.CommandText = "select sysdate() > date_add(STR_TO_DATE(valor, '%d/%m/%Y'), interval 20 day)  from configuracion where nombre = 'ULTIMOENVIO'";
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

        public DataSet TraerClientesDelMesSiguiente(String condicion)
        {
            return EjecutaQueryDS("select ID_CLIENTE,NOMBRE,APELLIDO_PATERNO,APELLIDO_MATERNO, EDAD, DATE_FORMAT(FECHA_NACIMIENTO, '%d/%m/%Y') as FECHA_NACIMIENTO, DATE_FORMAT(FECHA_INGRESO, '%d/%m/%Y') as FECHA_INGRESO, MUNICIPIO, tel_casa_fijo,extension, celular, EMAIL,OBSERVACIONES, NOTA, MEDIO from farmacia.cliente where estatus = 1 and date_format(fecha_nacimiento,'%m') = date_format(sysdate(),'%m')+1 " + (condicion.Trim().Length > 0 ? " and " + condicion : "") );
        }

        public DataSet TraerClientesDelMes(String condicion, int mes)
        {
            return EjecutaQueryDS("select ID_CLIENTE,NOMBRE,APELLIDO_PATERNO,APELLIDO_MATERNO, EDAD, DATE_FORMAT(FECHA_NACIMIENTO, '%d/%m/%Y') as FECHA_NACIMIENTO, " +
                " DATE_FORMAT(FECHA_INGRESO, '%d/%m/%Y') as FECHA_INGRESO, MUNICIPIO, tel_casa_fijo,extension, celular, EMAIL,OBSERVACIONES, NOTA, MEDIO " +
                " from farmacia.cliente where estatus = 1 and date_format(fecha_nacimiento,'%m') = '" + mes.ToString().PadLeft(2,'0') + "' " + (condicion.Trim().Length > 0 ? " and " + condicion : ""));
        }


        public String ActualizaUltimoCorreo()
        {
            return EjecutaQueryInsert("update configuracion set valor = date_format(sysdate(),'%d/%m/%Y') where nombre = 'ULTIMOENVIO'");
        }



        public String ActualizaDatosCorreo(DatosCorreo datoscorreo)
        {
            string resultado = "OK";
            try
            {

                EjecutaQueryInsert("update configuracion set valor = '" + datoscorreo.SMTP_CORREO + "' where nombre = 'SMTP_CORREO'");
                EjecutaQueryInsert("update configuracion set valor = '" + datoscorreo.SMTP_PASS + "' where nombre = 'SMTP_PASS'");

                EjecutaQueryInsert("update configuracion set valor = '" + datoscorreo.SMTP_MENSAJE + "' where nombre = 'SMTP_MENSAJE'");
                EjecutaQueryInsert("update configuracion set valor = '" + datoscorreo.SMTP_IMAGEN + "' where nombre = 'IMAGENCORREO'");

                EjecutaQueryInsert("update configuracion set valor = '" + datoscorreo.SMTP_SUJETO + "' where nombre = 'SMTP_SUJETO'");
                EjecutaQueryInsert("update configuracion set valor = '" + datoscorreo.SMTP_SSL + "' where nombre = 'SMTP_SSL'");

                EjecutaQueryInsert("update configuracion set valor = '" + datoscorreo.SMTP_HOST + "' where nombre = 'SMTP_HOST'");
                EjecutaQueryInsert("update configuracion set valor = '" + datoscorreo.SMTP_PUERTO + "' where nombre = 'SMTP_PUERTO'");

                EjecutaQueryInsert("update configuracion set valor = '" + datoscorreo.UltimoEnvio + "' where nombre = 'ULTIMOENVIO'");
                EjecutaQueryInsert("update configuracion set valor = '" + datoscorreo.DiasAntes + "' where nombre = 'DIASANTESDEENVIO'");

                EjecutaQueryInsert("update configuracion set valor = '" + datoscorreo.PRUEBAS + "' where nombre = 'PRUEBAS'");
                EjecutaQueryInsert("update configuracion set valor = '" + datoscorreo.SMTP_CORREO_PRUEBA + "' where nombre = 'SMTP_CORREO_PRUEBA'");
            }
            catch(Exception ex)
            {
                resultado = ex.InnerException.Message;
            }

            return resultado;
        }

        #endregion
        #endregion

    }
}