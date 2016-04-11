using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnsattAdapterAgresso.AgressoUserAdministrationServiceReference;

namespace AnsattAdapterAgresso
{
    public class AnsattAgressoController : Controller
    {
        

        public Resource HentAnsatt(string ressursnummer)
        {
            var dateFromIn = new DateTime(1753,1,1);
            var dateToIn = new DateTime(9999, 12, 31);
            var resources = GetClient().GetResources(GetAgressoClient(), ressursnummer, dateFromIn, dateToIn, GetWsCredentials());
            return resources.FirstOrDefault();
        }

        
    }
}