using accesosIp.Extension;
using accesosIp.helper;
using accesosIp.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace accesosIp.Services
{
    public interface IDatabaseChangeNotificationService
    {
        void Config(int id);
    }

    public class SqlDependencyService : IDatabaseChangeNotificationService
    {
        private readonly IConfiguration configuration;
        private readonly IHubContext<ChatHub> chatHub;
        private readonly Helper _helper;
        private int _idAcceso;
        private readonly IActionContextAccessor _accesor;
        public SqlDependencyService(IConfiguration configuration,IHubContext<ChatHub> chathub, IActionContextAccessor accesor)
        {
            this.configuration = configuration;
            this.chatHub = chathub;
            _accesor = accesor;
            _helper = new Helper(_accesor);
        }

        public void Config(int id)
        {
            
                _idAcceso = id;
                session_cambios();
            
        }

        private void session_cambios()
        {
            try
            {
                string _connectionstring = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection sql = new SqlConnection(_connectionstring))
                {
                    sql.Open();
                    using (var cmd = new SqlCommand(@"Select sPort from Accesos.tAcceso where sIdAcceso = " + _idAcceso + " and nSession = 1", sql))
                    {
                        cmd.Notification = null;
                        SqlDependency dependency = new SqlDependency(cmd);
                        dependency.OnChange += Sesion_Cambio;
                        SqlDependency.Start(_connectionstring);
                        cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception)
            {

               
            }
           // _idAcceso = _idAcceso = _helper.DevolverUsuario();
            
        }
        private void Sesion_Cambio(object sender,SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                string mensaje = NotificarCambio(e);
                chatHub.Clients.All.SendAsync("ReceiveMessage", _idAcceso, mensaje);
            }
            session_cambios();
        }

        private string NotificarCambio(SqlNotificationEventArgs e)
        {
            switch (e.Info)
            {
                case SqlNotificationInfo.Update:
                    return "Esta sessión se acaba de cerrar";
                default :
                    return "Se ha iniciado una nueva Sesión";
            }
        }
    }
}
