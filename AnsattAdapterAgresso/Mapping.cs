﻿using AnsattAdapterAgresso.AgressoUserAdministrationServiceReference;

namespace AnsattAdapterAgresso
{
    public class Mapping
    {

        public static Ansatt AgressoRessursTilFkAnsatt(Resource ressurs)
        {
            var ansatt = new Ansatt()
            {
                identifikatorer = new Identifikator[]
                {
                    new Identifikator() { identifikatortype = "ressursnummer", identifikatorverdi = ressurs.ResourceId }
                },
                navn = new Personnavn() { etternavn = ressurs.Surname, fornavn = ressurs.FirstName },
                fulltNavn = ressurs.Name,
            };
            return ansatt;
        }
    }
}