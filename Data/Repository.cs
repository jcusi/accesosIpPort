using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace accesosIp.Data
{
    public class Repository
    {
        private readonly string _connectionstring;
        public Repository(IConfiguration configuration)
        {
            _connectionstring = configuration.GetConnectionString("ConnectionString");
        }

        public async Task<Tuple<int,int,string>> IdValidarAcceso(string id,string ip,string port, string navegador)
        {
            Tuple<int,int, string> tuple = new Tuple<int, int, string>(-10,-1,"");
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionstring))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    using (SqlCommand cmd = new SqlCommand("sp_Validar_Acceso_Ingresar", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));
                        cmd.Parameters.Add(new SqlParameter("@Ip", ip));
                        cmd.Parameters.Add(new SqlParameter("@Port", port));
                        cmd.Parameters.Add(new SqlParameter("@Navegador", navegador));
                        cmd.Parameters.Add("@result",SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@acceso",SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@portbase", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        tuple = new Tuple<int, int,string>(Convert.ToInt32(cmd.Parameters["@result"].Value), Convert.ToInt32(cmd.Parameters["@acceso"].Value), cmd.Parameters["@portbase"].Value.ToString());
                        return tuple;
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
           
        }
        public async Task<Tuple<int, string>> acceso_Actualizar(string id, string ip, string port, string navegador)
        {
            Tuple<int, string> tuple = new Tuple<int, string>(-10, "");
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionstring))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    using (SqlCommand cmd = new SqlCommand("sp_Session_Actualizar", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));
                        cmd.Parameters.Add(new SqlParameter("@Ip", ip));
                        cmd.Parameters.Add(new SqlParameter("@Port", port));
                        cmd.Parameters.Add(new SqlParameter("@Navegador", navegador));
                        cmd.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@acceso", SqlDbType.VarChar, 36).Direction = ParameterDirection.Output;

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        tuple = new Tuple<int, string>(Convert.ToInt32(cmd.Parameters["@result"].Value), cmd.Parameters["@acceso"].Value.ToString());
                        return tuple;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<Tuple<int, string>> accesoInactivo_Actualizar(string id, string ip, string port, string navegador)
        {
            Tuple<int, string> tuple = new Tuple<int, string>(-10, "");
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionstring))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    using (SqlCommand cmd = new SqlCommand("sp_SessionInactiva_Actualizar", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IdAcceso", id));
                        cmd.Parameters.Add(new SqlParameter("@Ip", ip));
                        cmd.Parameters.Add(new SqlParameter("@Port", port));
                        cmd.Parameters.Add(new SqlParameter("@Navegador", navegador));
                        cmd.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@acceso", SqlDbType.VarChar, 36).Direction = ParameterDirection.Output;

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        tuple = new Tuple<int, string>(Convert.ToInt32(cmd.Parameters["@result"].Value), cmd.Parameters["@acceso"].Value.ToString());
                        return tuple;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public Tuple<int, string> sesionActiva_validar(int id, int idAcceso)
        {
            Tuple<int, string> tuple = new Tuple<int, string>(-10, "");
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionstring))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    using (SqlCommand cmd = new SqlCommand("sp_Session_Actualizar", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));
                        cmd.Parameters.Add(new SqlParameter("@idAcceso", idAcceso));
                        cmd.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@acceso", SqlDbType.VarChar, 36).Direction = ParameterDirection.Output;
                         sql.OpenAsync();
                         cmd.ExecuteNonQueryAsync();

                        tuple = new Tuple<int, string>(Convert.ToInt32(cmd.Parameters["@result"].Value), cmd.Parameters["@acceso"].Value.ToString());
                        return tuple;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
