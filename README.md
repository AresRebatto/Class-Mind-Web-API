# ClassMind API

Di seguito sono riportati tutti gli endpoint e il formato di richiesta per ciascuno di essi.

E possibile testare ciascuno di essere tramite il REST Client Swagger oppure eseguire i 
singoli test predefiniti nel file [http](ClassMindAPI.http). Nel caso si decidesse però di 
utilizzare il file http, si consgilia di prestare attenzione alle chiamate che utilizzano la 
primary key per specificare un record: di fatto nel corso dei vari test, potrebbero non
esistere ed è quindi opportuno usare gli appositi metodi per verificare.

## EndPoint

### Ottenere tutti gli studenti
```http
GET /studenti
```

Permette di ottenere tutta la lista di studenti. Non richiede parametri particolari.

#### Possibili Risposte
| Status Code | Descrizione                                |
|-------------|--------------------------------------------|
| 200 OK      | Restituisce un elenco di studenti con nome e cognome |

---

### Inserire degli studenti

```http
POST /inserisci-studenti
```

Permette di inserire una lista di nuovi studenti e vuole nel body della richiesta una lista di oggetti che poi vengono deserializzati mediante un record DTO.

#### Parametri

| Parameter  | Type     | Description                                 |
| :--------- | :------- | :------------------------------------------ |
| nome       | string   | Il nome dello studente da inserire          |
| cognome    | string   | Il cognome dello studente da inserire       |

#### Possibili Risposte
| Status Code     | Descrizione                                                                 |
|-----------------|-----------------------------------------------------------------------------|
| 200 OK          | Gli studenti sono stati inseriti correttamente                              |
| 400 Bad Request | Uno o più studenti sono già presenti nel database                          |

---

### Modificare uno studente

```http
PUT /modifica-studente/{studente_id}
```

Permette di modificare le informazioni di uno studente esistente specificando l'ID.

#### Parametri

| Parameter      | Type     | Description                                 |
| :------------- | :------- | :------------------------------------------ |
| studente_id    | int      | L'ID dello studente da modificare           |
| nome           | string   | Il nuovo nome dello studente                |
| cognome        | string   | Il nuovo cognome dello studente             |

#### Possibili Risposte
| Status Code     | Descrizione                                                                 |
|-----------------|-----------------------------------------------------------------------------|
| 200 OK          | Lo studente è stato aggiornato correttamente                                |
| 404 Not Found   | Lo studente con l'ID specificato non esiste                                |

---
### Elimina uno studente

```http
DELETE /elimina-studente/{studente_id}
```

Permette di eliminare uno studente esistente specificando l'ID.

#### Parametri

| Parameter      | Type     | Description                                 |
| :------------- | :------- | :------------------------------------------ |
| studente_id    | int      | L'ID dello studente da modificare           |

#### Possibili Risposte
| Status Code     | Descrizione                                                                 |
|-----------------|-----------------------------------------------------------------------------|
| 200 OK          | Lo studente è stato eliminato correttamente                                |
| 404 Not Found   | Lo studente con l'ID specificato non esiste                                |

---

### Aggiungere nuove materie

```http
POST /aggiungi-materie
```

Permette di aggiungere nuove materie specificando una lista di nomi.

#### Parametri

| Parameter      | Type     | Description                                 |
| :------------- | :------- | :------------------------------------------ |
| nomi           | list     | Una lista di nomi delle materie da aggiungere |

#### Possibili Risposte
| Status Code     | Descrizione                                                                 |
|-----------------|-----------------------------------------------------------------------------|
| 200 OK          | Le materie sono state aggiunte correttamente                               |
| 400 Bad Request | Una o più materie sono già presenti nel database                           |

---
### Cancellare una materia

```http
DELETE /cancella-materia/{materia_id}
```
Permette di eliminare una materia specificando l'ID.

#### Parametri

| Parameter      | Type     | Description                                 |
| :------------- | :------- | :------------------------------------------ |
| materia_id     | int      | L'ID della materia da eliminare             |

#### Possibili Risposte
| Status Code     | Descrizione                                                                 |
|-----------------|-----------------------------------------------------------------------------|
| 200 OK          | La materia è stata eliminata correttamente                                 |
| 404 Not Found   | La materia con l'ID specificato non esiste                                 |

---

### Aggiungere l'orario per una materia

```http
POST /aggiungi-orario/{id_materia}
```
Permette di aggiungere un orario (giorni della settimana) a una materia specifica.

#### Parametri

| Parameter      | Type     | Description                                 |
| :------------- | :------- | :------------------------------------------ |
| id_materia     | int      | L'ID della materia                          |
| giorniSettimana| list     | Una lista di giorni (es: "lun", "mar")      |

#### Possibili Risposte
| Status Code     | Descrizione                                                                 |
|-----------------|-----------------------------------------------------------------------------|
| 200 OK          | L'orario è stato aggiunto correttamente                                    |
| 400 Bad Request | I giorni della settimana non sono in un formato accettato                 |

---
### Creare una nuova materia e aggiungere le lezioni

```http
POST /aggiungi-materia-orario
```

Permette di creare una nuova materia e di aggiungergli immediatamente le nuove lezioni, senza dover fare due chiamatte separata.

#### Parametri

| Parameter      | Type     | Description                                 |
| :------------- | :------- | :------------------------------------------ |
| NomeMateria     | int      | Il nome della materia che si vuole creare                      |
| GiorniSettimana| list     | Una lista di giorni (es: "lun", "mar")      |

#### Possibili Risposte
| Status Code     | Descrizione                                                                 |
|-----------------|-----------------------------------------------------------------------------|
| 200 OK          | L'orario e la materia sono stati aggiunti correttamente                                    |
| 400 Bad Request | I giorni della settimana non sono in un formato accettato o la materia è già presente              |

---
### Ottenere tutte le materie
```http
GET /get-materie
```

Ritorna tutte le materie presenti nel
database.


---
### Ottenere le lezioni
```http
GET /get-lezioni/{id_materia}
```

Permette di ottenere l'insieme delle lezioni di una
specifica materia.

### Parametri
| Parameter                | Type     | Description                                 |
| :----------------------- | :------- | :------------------------------------------ |
| id_materia                | int      | L'ID della materia                          |

#### Possibili Risposte
| Status Code     | Descrizione                                                                 |
|-----------------|-----------------------------------------------------------------------------|
| 200 OK          | Interrogazioni programmate generate con successo                           |
| 404 Not Found   | La materia o le lezioni specificate non esistono                           |
---
### Generare interrogazioni programmate

```http
GET /genera-programmate/{idMateria}/{interrogati_giornalmente}
```

Genera un piano di interrogazioni giornaliere per una materia specifica.

#### Parametri

| Parameter                | Type     | Description                                 |
| :----------------------- | :------- | :------------------------------------------ |
| idMateria                | int      | L'ID della materia                          |
| interrogati_giornalmente | int      | Numero di studenti interrogati giornalmente |

#### Possibili Risposte
| Status Code     | Descrizione                                                                 |
|-----------------|-----------------------------------------------------------------------------|
| 200 OK          | Interrogazioni programmate generate con successo                           |
| 400 Bad Request | Il numero di studenti giornalieri è superiore al numero totale di studenti |
| 404 Not Found   | La materia o le lezioni specificate non esistono                           |

---

### Ottenere interrogazioni programmate
```http
GET /get-programmate/{id_materia}
```

Restituisce l'elenco delle interrogazioni programmate per una materia specifica.

#### Parametri

| Parameter      | Type     | Description                                 |
| :------------- | :------- | :------------------------------------------ |
| id_materia     | int      | L'ID della materia                          |

#### Possibili Risposte
| Status Code     | Descrizione                                                                 |
|-----------------|-----------------------------------------------------------------------------|
| 200 OK          | Restituisce l'elenco delle interrogazioni programmate                      |

