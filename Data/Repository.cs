using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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

        public async Task<int> IdValidarAcceso(string id,string ip)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Validar_Acceso_Ingresar", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        cmd.Parameters.Add(new SqlParameter("@ip", ip));
                        int response = -1;
                        await sql.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response = reader.GetOrdinal("result");
                            }
                        }
                        return response;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}
