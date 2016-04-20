using System.Linq;
using AnsattAdapterAgresso.AgressoUserAdministrationServiceReference;

namespace AnsattAdapterAgresso.AgressoController
{
    public class AnsattRessursController : AgressoController
    {

        public void OppdaterEpostTilRessurs(string ressursnummer, string epostadresse)
        {
            var ressurs = HentRessurs(ressursnummer);
            ressurs.Addresses.Select(a =>
            {
                a.EMailList.Clear();
                a.EMailList.Add(epostadresse);
                return a;
            }).ToList();
            GetClient().ModifyResource(ressurs, GetWsCredentials());
        }

        public Resource HentRessurs(string ressursnummer)
        {
            var resources = HentRessurser(ressursnummer);
            return resources.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ressursnummer">Kan benytte *-søk. Feks. "9*".</param>
        /// <returns></returns>
        public Resource[] HentRessurser(string ressursnummer)
        {
            return GetClient()
                .GetResources(GetAgressoClient(), ressursnummer, GetAgressoDateMin(), GetAgressoDateMax(),
                    GetWsCredentials());
        }

        public Resource[] HentAlleRessurser()
        {
            const string ressursnummer = "*";
            return HentRessurser(ressursnummer);
        }
    }
}