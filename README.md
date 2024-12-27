# ClassMind API

Di seguito sono riportati tutti gli endpoint e il formato di richiesta per ciascuno di essi.

## EndPoint

### Ottenere tutti gli studenti
```http
GET /studenti
```
Permette di ottenere tutta la lista di studenti. Non richiede parametri particolari.

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

---

### Aggiungere materie
```http
POST /aggiungi-materie
```
Permette di aggiungere una lista di nuove materie al database.

#### Parametri

| Parameter  | Type     | Description                      |
| :--------- | :------- | :------------------------------- |
| `nomi`     | `array`  | Lista di nomi delle materie      |

---

### Aggiungere orari a una materia
```http
POST /aggiungi-orario/{id_materia:int}
```
Permette di aggiungere orari di lezione a una materia specifica.

#### Parametri

| Parameter           | Type     | Description                                                  |
| :------------------ | :------- | :----------------------------------------------------------- |
| `id_materia`        | `int`    | ID della materia a cui aggiungere gli orari                 |
| `giorniSettimana`   | `array`  | Lista dei giorni in formato abbreviato (es. "lun", "mar") |

#### Note
- I giorni devono essere forniti in formato abbreviato come "lun", "mar", ecc.

---

### Ottenere lezioni di una materia
```http
GET /get-lezioni/{id_materia:int}
```
Permette di ottenere tutte le lezioni associate a una determinata materia.

#### Parametri

| Parameter    | Type     | Description                           |
| :----------- | :------- | :------------------------------------ |
| `id_materia` | `int`    | ID della materia di cui ottenere le lezioni |

#### Risposta
Ritorna una lista di oggetti contenenti:
- `giorno`: Il giorno della settimana
- `materia`: Il nome della materia

---

### Generare interrogazioni programmate
```http
GET /genera-programmate/{idMateria}/{interrogati_giornalmente}
```
Genera un programma di interrogazioni per una materia.

#### Parametri

| Parameter                  | Type     | Description                                                           |
| :------------------------- | :------- | :-------------------------------------------------------------------- |
| `idMateria`                | `int`    | ID della materia per cui generare le interrogazioni                  |
| `interrogati_giornalmente` | `int`    | Numero di studenti da interrogare ogni giorno                        |

#### Note
- Verifica che il numero di studenti nel database sia sufficiente per il parametro `interrogati_giornalmente`.
- Cancella eventuali interrogazioni precedenti per la stessa materia prima di aggiungerne di nuove.

---

### Ottenere interrogazioni programmate
```http
GET /get-programmate/{id_materia:int}
```
Permette di ottenere tutte le interrogazioni programmate per una materia specifica.

#### Parametri

| Parameter    | Type     | Description                                 |
| :----------- | :------- | :------------------------------------------ |
| `id_materia` | `int`    | ID della materia di cui ottenere le interrogazioni |

#### Risposta
Ritorna una lista di oggetti contenenti:
- `materia`: Nome della materia
- `studente`: Nome e cognome dello studente
- `data`: Data dell'interrogazione



### Aggiungere materie
```http
POST /aggiungi-materie
```
Permette di aggiungere nuove materie. Richiede una lista di nomi di materie nel body della richiesta.

| Parameter | Type     | Description                     |
| :-------- | :------- | :------------------------------ |
| `nomi`    | `array`  | Lista di nomi delle materie     |

### Aggiungere orari per una materia
```http
POST /aggiungi-orario/{id_materia:int}
```
Permette di aggiungere giorni della settimana per una materia specifica, utilizzando l'ID della materia.

| Parameter          | Type       | Description                                                 |
| :----------------- | :--------- | :---------------------------------------------------------- |
| `id_materia`       | `int`      | L'ID della materia                                           |
| `giorniSettimana`  | `array`    | Lista di giorni (in formato breve: `lun`, `mar`, ...)       |

**Risposte possibili:**
- `400 Bad Request` se i giorni non sono in un formato accettabile o la materia non esiste.
- `200 OK` con la lista di lezioni create.

### Generare interrogazioni programmate
```http
GET /genera-programmate/{idMateria}/{interrogati_giornalmente}
```
Permette di generare un programma di interrogazioni per una materia specifica, basato sul numero di studenti interrogati giornalmente.

| Parameter                 | Type     | Description                                    |
| :------------------------ | :------- | :--------------------------------------------- |
| `idMateria`               | `int`    | L'ID della materia                             |
| `interrogati_giornalmente`| `int`    | Numero di studenti interrogati ogni giorno     |


