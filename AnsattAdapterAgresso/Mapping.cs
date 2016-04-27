using System.Linq;
using AnsattAdapterAgresso.AgressoUserAdministrationServiceReference;

namespace AnsattAdapterAgresso
{
    public class Mapping
    {

        public static Ansatt AgressoRessursTilFkAnsatt(Resource ressurs)
        {
            string epostadresse = null;
            var adress = ressurs.Addresses?.FirstOrDefault();
            if (adress != null)
            {
                epostadresse = adress.EMailList?.FirstOrDefault();
            }

            var ansatt = new Ansatt()
            {
                identifikatorer = new Identifikator[]
                {
                    new Identifikator() { identifikatortype = "ressursnummer", identifikatorverdi = ressurs.ResourceId }
                },
                navn = new Personnavn() { etternavn = ressurs.Surname, fornavn = ressurs.FirstName },
                fulltNavn = ressurs.Name,
                kontaktinformasjon = new Kontaktinformasjon() { epostadresse = epostadresse}
            };
            return ansatt;
        }
    }
}