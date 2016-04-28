# Adapter for Agresso mot ansatt-felleskomponent

Dette adapteret er en del av en POC utarbeidet av et [felles fylkeskommunalt prosjekt FINT](https://github.com/FINTprosjektet/). Adapteret er ansvarlig for kommunikasjonen mellom 
[felleskomponenten "Ansatt"](https://github.com/FINTprosjektet/ansatt-konsumentgrensesnitt-poc) og Agresso. 

## Overordnet beskrivelse

Apdapteret tar i mot forespørsler fra felleskomponenten, gjør oppslag i Agresso, og sender svar tilbake til komponenten.

Adapteret kan utføre følgende funksjoner:

1. Hente alle ressurser i Agresso
2. Hent ansatt ut fra ressursnummer i Agresso
3. Lagre e-postadresse på en gitt ressursen i Agresso

All kommunikasjon mot felleskomponenten foregår ved bruk av en [felles ansatt-modell](https://github.com/FINTprosjektet/ansatt-modell-poc) 
basert på en [modell utarbeidet i samarbeidsprosjektet Veikart hos Difi](https://www.difi.no/fagomrader-og-tjenester/digitalisering-og-samordning/skate/veikart).

## Avhengigheter

### For å kjøre løsningn trengs

1. Fellskomponenten [ansatt-konsumergrensesnitt-poc](https://github.com/FINTprosjektet/ansatt-konsumentgrensesnitt-poc)
2. WebService mot Agresso: UserAdministration med utvidelse for Resource

### Oppdatere modellene

1. For å oppdatere ansatt.cs trengs xsd-modellen for ansatt [ansatt-modell-poc](https://github.com/FINTprosjektet/ansatt-modell-poc)
2. For å oppdatere event.cs trengs xsd-modellen for event [event-modell-poc](https://github.com/FINTprosjektet/event-modell-poc)
