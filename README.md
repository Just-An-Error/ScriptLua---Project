# Lua Automation Platform

Piattaforma di automazione "self-hosted" ispirata a strumenti come IFTTT/Zapier, dove la logica di ogni regola non è scritta rigidamente nel backend, ma è uno script **Lua** scritto dall'utente, salvato a runtime ed eseguito dinamicamente — senza bisogno di ricompilare o ridistribuire l'applicazione.

## Indice

- [Il problema che risolve](#il-problema-che-risolve)
- [Come funziona](#come-funziona)
- [Stack tecnologico](#stack-tecnologico)
- [Architettura](#architettura)
- [Funzionalità implementate](#funzionalità-implementate)
- [Setup del progetto](#setup-del-progetto)
- [Esempio d'uso](#esempio-duso)
- [Sicurezza e limiti noti](#sicurezza-e-limiti-noti)
- [Roadmap futura](#roadmap-futura)

## Il problema che risolve

In molti sistemi, ogni cambio di logica di business richiede una modifica al codice, una ricompilazione e un nuovo deploy. Questo progetto esplora un approccio diverso: permettere a un utente di definire regole personalizzate tramite piccoli script, eseguiti in sicurezza dal backend, così la logica può evolvere senza toccare il codice sorgente dell'applicazione.

È lo stesso principio usato da strumenti reali come Redis, Nginx o i motori di scripting nei videogiochi, dove Lua è la scelta storica per la sua leggerezza e velocità di embedding.

## Come funziona

1. L'utente crea una **regola**: un nome, un tipo di evento a cui reagisce (`TriggerType`) e uno script Lua che ne definisce il comportamento.
2. Un evento esterno arriva al sistema tramite un endpoint dedicato (es. `{ "eventType": "nuovo_ordine", "data": { "importo": 150 } }`).
3. Il sistema individua tutte le regole attive collegate a quel tipo di evento e le esegue, passando i dati dell'evento come variabili disponibili nello script.
4. Ogni esecuzione — riuscita o fallita — viene salvata in uno storico, con input, output (o errore) e tempo di esecuzione.

## Stack tecnologico

**Backend**
- ASP.NET Core (C#) — API REST
- Entity Framework Core — accesso ai dati
- PostgreSQL / SQL Server — persistenza
- [NLua](https://github.com/NLua/NLua) — motore di scripting Lua embedded

**Frontend**
- Angular (standalone components, zoneless change detection)
- Angular Material
- Monaco Editor — editor di codice con syntax highlighting per Lua

## Architettura

Il backend segue una **Clean Architecture** semplificata, su tre progetti separati:

```
Api/              → Controller, Program.cs (composition root)
Application/      → Entità di dominio, DTO, interfacce (IService, IRepository), logica applicativa
Infrastructure/   → Implementazioni concrete: DbContext, migration, repository, motore Lua
```

Il principio cardine è la **direzione delle dipendenze**: `Application` non referenzia mai `Infrastructure`. Il cuore del progetto (le regole di business) dipende solo da contratti astratti (`IRuleRepository`, `ILuaExecutionService`), mai da dettagli tecnici concreti (Entity Framework, NLua). Questo permette, in teoria, di sostituire il motore di scripting o il database senza toccare la logica applicativa.

```
Api  ────────────►  Application  ◄────────────  Infrastructure
```

## Funzionalità implementate

- **CRUD completo delle regole** — creazione, lettura (singola e lista), aggiornamento, cancellazione
- **Motore di esecuzione Lua** — esegue script salvati nel database passando dati dinamici come variabili, con gestione degli errori tramite Result pattern (mai eccezioni non gestite verso il chiamante)
- **Trigger automatico da eventi** — un endpoint `/events` individua e attiva tutte le regole collegate a un dato tipo di evento
- **Storico esecuzioni** — ogni esecuzione (riuscita o fallita) viene tracciata con input, output, esito e durata
- **Editor Lua con syntax highlighting** — integrazione di Monaco Editor (lo stesso motore di VS Code) nel frontend
- **Test "a secco" delle regole** — possibilità di eseguire uno script con dati finti prima ancora di salvarlo, per iterare rapidamente
- **Sandboxing e limiti di sicurezza** — timeout sull'esecuzione ed esclusione delle funzioni Lua potenzialmente pericolose (`os`, `io`, `require`)
- **Versionamento delle regole** — ogni modifica al codice Lua di una regola esistente genera una versione storica, con possibilità di ripristino

## Setup del progetto

### Backend

```bash
cd LuaProject.Api
dotnet restore
dotnet ef database update
dotnet run
```

Configura la connection string in `appsettings.json` (o tramite variabili d'ambiente/user secrets, non committare credenziali reali).

### Frontend

```bash
cd lua-project-fe
npm install
npm start
```

L'app Angular si aspetta il backend raggiungibile all'URL configurato nei service (`environment.ts` o equivalente).

## Esempio d'uso

Creazione di una regola che applica uno sconto sopra una soglia:

**Script Lua della regola:**
```lua
if importo > 100 then
    return importo * 0.95
else
    return importo
end
```

**Evento in ingresso:**
```json
{
  "eventType": "nuovo_ordine",
  "data": { "importo": 150 }
}
```

**Risultato:** la regola viene individuata tramite il suo `TriggerType`, eseguita con `importo = 150`, e restituisce `142.5`. L'esecuzione viene salvata nello storico.

## Sicurezza e limiti noti

Questo progetto è stato costruito come esercizio di portfolio, con un'attenzione consapevole ai temi di sicurezza tipici dell'esecuzione di codice non fidato, ma con alcuni limiti dichiarati apertamente:

- **Il timeout non termina realmente il thread.** .NET non permette di forzare la terminazione di un thread in modo sicuro. L'implementazione attuale smette di *attendere* il completamento dello script oltre una soglia di tempo, restituendo un errore al chiamante, ma un thread bloccato (es. da un ciclo infinito) continua a occupare risorse in background finché il processo non viene riavviato.
- **Il sandboxing usa un approccio a blacklist**, disattivando esplicitamente le funzioni Lua note come pericolose (`os`, `io`, `require`, ecc.), piuttosto che una whitelist di funzioni esplicitamente permesse. Una whitelist sarebbe più robusta perché non dipende dal ricordarsi di bloccare ogni funzione rischiosa.
- **L'isolamento è a livello di thread, non di processo.** Per un sistema in produzione con utenti non fidati, la soluzione più solida sarebbe eseguire gli script in un processo separato e sacrificabile (es. containerizzato, con limiti di CPU/memoria imposti dal sistema operativo), così da poter terminare in sicurezza un'esecuzione fuori controllo senza impattare il resto del backend.

Questi limiti sono accettabili per lo scope dimostrativo del progetto, ma andrebbero risolti prima di un utilizzo in produzione con dati o utenti reali.

## Roadmap futura

- Notifiche in tempo reale (SignalR) quando una regola si attiva
- Analisi statica di base dello script prima dell'esecuzione, per segnalare pattern rischiosi (es. loop potenzialmente infiniti)
- Whitelist esplicita delle funzioni Lua permesse, in sostituzione della blacklist attuale
- Esecuzione degli script in processo separato per un isolamento più robusto
