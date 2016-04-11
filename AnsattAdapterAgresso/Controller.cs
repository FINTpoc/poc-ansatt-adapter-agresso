using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AnsattAdapterAgresso.AgressoUserAdministrationServiceReference;

namespace AnsattAdapterAgresso
{
    public class Controller : IDisposable
    {
        private readonly string _agressoClient;
        private readonly string _agressoPassword;
        private readonly string _agressoUsername;
        private UserAdministrationV200702SoapClient _client;
        
        public Controller()
        {
            _agressoClient = ConfigurationManager.AppSettings["AgressoUserAdministrationClient"];
            _agressoUsername = ConfigurationManager.AppSettings["AgressoUserAdministrationUsername"];
            _agressoPassword = ConfigurationManager.AppSettings["AgressoUserAdministrationPassword"];
        }

        public UserAdministrationV200702SoapClient GetClient()
        {
            return _client ?? new UserAdministrationV200702SoapClient();
        }

        public WSCredentials GetWsCredentials()
        {
            return new WSCredentials { Client = _agressoClient, Username = _agressoUsername, Password = _agressoPassword };
        }

        public string GetAgressoClient()
        {
            return _agressoClient;
        }

        public void Dispose()
        {
            if (_client == null)
            {
                throw new InvalidOperationException("The DisposableCommunicationObjectToken structure was created with the default constructor.");
            }
            try
            {
                _client.Close();
            }
            catch (CommunicationException)
            {
                _client.Abort();
            }
            catch (TimeoutException)
            {
                _client.Abort();
            }
            catch (Exception)
            {
                _client.Abort();
                throw;
            }
        }
    }
}
