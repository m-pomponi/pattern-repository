# pattern-repository
Il progetto è un'applicazione .NET Core che fornisce un'infrastruttura solida per la gestione dei dati attraverso un Generic Repository e gestisce in modo personalizzato le eccezioni attraverso un middleware.

Caratteristiche principali:

  Generic Repository:

  Il progetto include un Generic Repository che offre un'astrazione per l'accesso ai dati attraverso l'ORM (Object-Relational Mapping) di Entity Framework Core o un altro sistema di persistenza.
  Il repository supporta operazioni CRUD (Create, Read, Update, Delete) generiche per diverse entità di dati.
  È possibile estendere il repository per aggiungere metodi personalizzati specifici dell'applicazione per soddisfare le esigenze di accesso ai dati.

  Custom Exception Middleware:
  
  Il progetto include un middleware personalizzato per la gestione delle eccezioni HTTP.
  Il middleware cattura eccezioni non gestite durante la richiesta HTTP e genera risposte JSON strutturate in modo personalizzato per gli errori.
  Le risposte di errore possono includere informazioni dettagliate sull'errore, come il tipo di errore, il messaggio, il codice di stato HTTP appropriato, ecc.
  
  Gestione del Logger con Serilog:
  
  Il progetto utilizza Serilog come sistema di logging per registrare eventi, informazioni di debug e errori.
  Serilog è configurato per scrivere i log su file di log.
