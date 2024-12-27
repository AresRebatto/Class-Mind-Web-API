# ClassMind API

Di seguito sono riportati tutti gli endpoint e il formato di richiesta per ciascuno di essi.

## EndPoint

### Ottenere tutti gli studenti
```http
GET /studenti
```
Permette di ottenere tutta la lista di studenti. Non richiede parametri particolari.

#### Possibili Risposte
| Status Code | Descrizione                                |
|-------------|--------------------------------------------|
| `200 OK`    | Restituisce un elenco di studenti con nome e cognome |

---

### Inserire degli studenti
```http
POST /inserisci-studenti
```
Permette di inserire una lista di nuovi studenti e vuole nel body della richiesta una lista di oggetti che poi vengono deserializzati mediante un record DTO.

#### Parametri

| Parameter  | Type     | Description                                 |
| :--------- | :------- | :------------------------------------------ |
| `nome`     | `string` | Il nome dello studente da inserire          |
| `cognome`  | `string` | Il cognome dello studente da inserire       |

#### Possibili Risposte
| Status Code | Descrizione                                                                 |
|-------------|-----------------------------------------------------------------------------|
| `200 OK`    | Gli studenti sono stati inseriti correttamente                              |
| `400 Bad Request` | Uno o più studenti sono già presenti nel database                      |

---

### Modificare uno studente
```http
POST /modifica-studente/{studente_id:int}
```
Permette di modificare il nome o il cognome di uno studente specificato tramite il suo ID.

#### Parametri

| Parameter      | Type     | Description                                        |
| :------------- | :------- | :------------------------------------------------- |
| `studente_id`  | `int`    | L'ID dello studente da modificare                  |
| `nome`         | `string` | Il nuovo nome dello studente                       |
| `cognome`      | `string` | Il nuovo cognome dello studente                    |

#### Possibili Risposte
| Status Code | Descrizione                                                             |
|-------------|-------------------------------------------------------------------------|
| `200 OK`    | Lo studente è stato modificato correttamente                             |
| `404 Not Found` | L'ID fornito non corrisponde a nessuno studente                     |

---

### Aggiungere materie
```http
POST /aggiungi-materie
```
Permette di aggiungere nuove materie al database. Richiede una lista di nomi delle materie nel body della richiesta.

#### Parametri

| Parameter  | Type     | Description                      |
| :--------- | :------- | :------------------------------- |
| `nomi`     | `array`  | Una lista di nomi delle materie   |

#### Possibili Risposte
| Status Code | Descrizione                                                        |
|-------------|--------------------------------------------------------------------|
| `200 OK`    | Le materie sono state aggiunte correttamente e vengono restituite  |

---

### Ottenere tutte le materie
```http
GET /get-materie
```
Permette di ottenere tutte le materie presenti nel database, incluse le lezioni e le interrogazioni associate.

#### Possibili Risposte
| Status Code | Descrizione                                                  |
|-------------|--------------------------------------------------------------|
| `200 OK`    | Restituisce l'elenco delle materie con lezioni e interrogazioni |

---

### Cancellare una materia
```http
GET /cancella-materia/{materia_id:int}
```
Permette di cancellare una materia specificata tramite il suo ID, eliminando anche le lezioni e le interrogazioni associate.

#### Parametri

| Parameter     | Type     | Description                              |
| :------------ | :------- | :--------------------------------------- |
| `materia_id`  | `int`    | L'ID della materia da cancellare         |

#### Possibili Risposte
| Status Code | Descrizione                                                             |
|-------------|-------------------------------------------------------------------------|
| `200 OK`    | La materia è stata cancellata con successo                              |
| `404 Not Found` | L'ID fornito non corrisponde a nessuna materia                     |

---

### Aggiungere l'orario di una materia
```http
POST /aggiungi-orario/{id_materia:int}
```
Permette di aggiungere o aggiornare i giorni della settimana in cui si tiene una materia specificata tramite il suo ID.

#### Parametri

| Parameter         | Type     | Description                                              |
| :---------------- | :------- | :------------------------------------------------------- |
| `id_materia`      | `int`    | L'ID della materia                                       |
| `giorniSettimana` | `array`  | Una lista di stringhe rappresentanti i giorni della settimana (e.g., "lun", "mar") |

#### Possibili Risposte
| Status Code | Descrizione                                                         |
|-------------|---------------------------------------------------------------------|
| `200 OK`    | L'orario è stato aggiornato correttamente                            |
| `400 Bad Request` | I giorni non sono in un formato accettabile o la materia non esiste |

---

### Ottenere le lezioni di una materia
```http
GET /get-lezioni/{id_materia:int}
```
Permette di ottenere tutte le lezioni di una materia specificata tramite il suo ID.

#### Parametri

| Parameter     | Type     | Description                              |
| :------------ | :------- | :--------------------------------------- |
| `id_materia`  | `int`    | L'ID della materia                       |

#### Possibili Risposte
| Status Code | Descrizione                                                       |
|-------------|-------------------------------------------------------------------|
| `200 OK`    | Restituisce l'elenco delle lezioni della materia specificata      |
| `404 Not Found` | L'ID fornito non corrisponde a nessuna materia                |

---

### Generare interrogazioni programmate
```http
GET /genera-programmate/{idMateria}/{interrogati_giornalmente}
```
Genera un piano di interrogazioni per una materia specificata tramite il suo ID, in base al numero di studenti interrogati giornalmente.

#### Parametri

| Parameter                | Type     | Description                                                |
| :----------------------- | :------- | :--------------------------------------------------------- |
| `idMateria`              | `int`    | L'ID della materia                                         |
| `interrogati_giornalmente` | `int`  | Numero di studenti interrogati giornalmente                |

#### Possibili Risposte
| Status Code | Descrizione                                                                |
|-------------|----------------------------------------------------------------------------|
| `200 OK`    | Le interrogazioni sono state generate con successo                        |
| `400 Bad Request` | Il numero di studenti interrogati giornalmente supera gli studenti disponibili |
| `404 Not Found` | La materia non esiste o non ha lezioni                               |

---

### Ottenere le interrogazioni programmate
```http
GET /get-programmate/{id_materia:int}
```
Permette di ottenere tutte le interrogazioni programmate per una materia specificata tramite il suo ID.

#### Parametri

| Parameter     | Type     | Description                              |
| :------------ | :------- | :--------------------------------------- |
| `id_materia`  | `int`    | L'ID della materia                       |

#### Possibili Risposte
| Status Code | Descrizione                                                                |
|-------------|----------------------------------------------------------------------------|
| `200 OK`    | Restituisce l'elenco delle interrogazioni programmate                      |
| `404 Not Found` | L'ID fornito non corrisponde a nessuna materia                        |

