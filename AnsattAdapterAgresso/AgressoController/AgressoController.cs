using System;
using System.Configuration;
using System.ServiceModel;
using AnsattAdapterAgresso.AgressoUserAdministrationServiceReference;

namespace AnsattAdapterAgresso.AgressoController
{
    public class AgressoController : IDisposable
    {
        private readonly string _agressoClient;
        private readonly string _agressoPassword;
        private readonly string _agressoUsername;
        private UserAdministrationV200702SoapClient _client;
        
        public AgressoController()
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

        public DateTime GetAgressoDateMin()
        {
            return new DateTime(1753, 1, 1);
        }

        public DateTime GetAgressoDateMax()
        {
            return new DateTime(9999, 12, 31);
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
