using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClassMindContext>(options=> 
    options.UseSqlite(builder.Configuration.GetConnectionString("DbPath")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/studenti", (ClassMindContext context)=> context.Studenti.Select(studente => new {nome = studente.Nome, cognome = studente.Cognome}))
.WithName("lista-studenti")
.WithOpenApi();



app.MapPost("/inserisci-studenti", (ClassMindContext context, List<StudenteDTO> studenti) =>{
    if(IsStudentAlreadyInDB(studenti, context)){
        return Results.BadRequest("Impossibile inserire tutti gli studenti: uno di quelli specificati e' gia' presente nel database");
    }
    
    context.Studenti.AddRange(studenti
        .Select(st => new Studente(){
            Nome = st.Nome, Cognome = st.Cognome
        }));
    context.SaveChangesAsync();
    return Results.Ok();
})
.WithName("inserisci-studenti")
.WithOpenApi();


app.MapPost("/modifica-studente/{studente_id:int}", (ClassMindContext context, StudenteDTO studente, int studente_id)=>{
    if(!context.Studenti.Where(studente => studente.StudenteId == studente_id).Any())
        return Results.NotFound("L'ID fornito non corrisponde a nessuno studente");

    context.Studenti.Where(studente => studente.StudenteId == studente_id).ToList()[0].Nome = studente.Nome;
    context.Studenti.Where(studente => studente.StudenteId == studente_id).ToList()[0].Cognome = studente.Cognome;

    context.SaveChanges();
    
    return Results.Ok();
})
.WithName("modifica-studente")
.WithOpenApi();
app.MapPost("/aggiungi-materie", (ClassMindContext context, List<String> nomi)=>{
    context.Materie.AddRange(
        nomi.Select(nome => new Materia(){Nome = nome})
    );

    context.SaveChanges();
    return context.Materie.Select(materia => new {nome=materia.Nome, lezioni = "Non impostate"});
})
.WithName("aggiungi-materie")
.WithOpenApi();

app.MapGet("/get-materie", (ClassMindContext context)=>{
    return context.Materie
                    .Include(materia => materia.Lezioni)
                    .Include(materia => materia.Interrogazioni)
                    .Select(materia=> new{
                        id=materia.MateriaId, 
                        nome=materia.Nome, 
                        lezioni = materia.Lezioni
                                            .Select(lezione => lezione.GiornoSettimana), 
                        interrogazioni= materia.Interrogazioni.Select(interrogazione => 
                            new {
                                nome=$"{interrogazione.Studente.Nome} {interrogazione.Studente.Cognome}",
                                data = interrogazione.Data
                            }
                        )
                    });
})
.WithName("get-materie")
.WithOpenApi();

app.MapGet("/cancella-materia/{materia_id:int}", (ClassMindContext context, int materia_id)=>{
    if(!context.Materie.Where(materia => materia.MateriaId == materia_id).Any())
        return Results.NotFound("Materia non trovata");
    context.Lezioni.RemoveRange(context.Lezioni.Where(lezione => lezione.Materia.MateriaId == materia_id));
    context.Interrogazioni.RemoveRange(context.Interrogazioni.Where(interrogazione => interrogazione.Materia.MateriaId == materia_id));
    
    context.Materie.RemoveRange(context.Materie.Where(materia => materia.MateriaId == materia_id));
    
    context.SaveChanges();

    return Results.Ok();
})
.WithName("cancella-materia")
.WithOpenApi();



app.MapPost("/aggiungi-orario/{id_materia:int}", (ClassMindContext context, int id_materia, List<string> giorniSettimana)=>{
    
    List<string> giorniAmmissibili = new List<String>{"lun", "mar", "mer", "gio", "ven", "sab"};
    
    //Se la materia ha gia' delle lezioni, le cancella prima di generare le nuove
    if(LaMateriaHaLezioni(context, id_materia)){
        context.Lezioni.RemoveRange(context.Lezioni.Where(lezione => lezione.Materia.MateriaId == id_materia));
    }


    //Verifica che i giorni della settimana siano dati nel formato come quello nella lista sopra
    if (giorniSettimana.Select(giorni => giorni.ToLower()).Except(giorniAmmissibili).Any()){
        return Results.BadRequest("I giorni non sono stati dati in un formato accettato");
    }

    var giorniInMinuscolo = giorniSettimana.Select(g => g.ToLower()).ToList();

    // Si salva la materia specificata e controlla che non sia null
    var materia = TrovaMateria(id_materia, context);
    if (materia == null)
    {
        return Results.BadRequest("Materia non trovata");
    }

    var nuoveLezioni = giorniInMinuscolo
        .Select(g => new Lezione { GiornoSettimana = g, Materia = materia })
        .ToList();
    
    context.Lezioni.AddRange(nuoveLezioni);
    context.SaveChanges();
    
    
    return Results.Ok();
})
.WithName("aggiungi-orari")
.WithOpenApi();



app.MapGet("/get-lezioni/{id_materia:int}", (ClassMindContext context, int id_materia)=>{
    if(!context.Materie.Where(materia => materia.MateriaId == id_materia).Any())
        return Results.NotFound("Non esiste nessuna materia con l'ID specificato");
    return Results.Ok(context.Lezioni
                    .Where(lezione => lezione.Materia.MateriaId == id_materia)
                    .Select(lezione => new {giorno = lezione.GiornoSettimana, materia = lezione.Materia.Nome}));
})
.WithName("get-lezioni")
.WithOpenApi();



app.MapGet(
    "/genera-programmate/{idMateria}/{interrogati_giornalmente}", 
    (ClassMindContext context, int idMateria, int interrogati_giornalmente)=>
    {
        if(context.Studenti.Count() < interrogati_giornalmente){
            return Results.BadRequest("Il numero di studenti interrogati giornalmente sono di più degli studenti effettivamente presenti nel database");
        }

        //Verifica se ci sono gia' delle interrogazioni programmate per quella materia
        List<Interrogazione> interrogazioni_materia = context.Interrogazioni.Where(interrogazione => interrogazione.Materia.MateriaId == idMateria).ToList();
        if(interrogazioni_materia.Any()){
            context.Interrogazioni.RemoveRange(interrogazioni_materia);
        }


        int todayWeekDay = (int)DateTime.Today.DayOfWeek;
        Materia materia = TrovaMateria(idMateria, context);

        //Estrapola le lezioni 
        List<Lezione> lezioni = context.Lezioni.Where(lezione => lezione.Materia.MateriaId == materia.MateriaId).ToList();
        List<int> giorniInterrogazione = new List<int>();
        
        //Converte tutte le giornate d'interrogazione in numeri(Lun = 1, Mar = 2...)
        if(materia != null && lezioni != null){
            giorniInterrogazione= lezioni
                .Select(
                    lezione=> NumeroGiornoSettimana(lezione.GiornoSettimana)
                ).ToList();
        }else{
            return Results.NotFound("La materia inserita non esiste o non ha lezioni");
        }

        giorniInterrogazione.Sort();

        List<Studente> lista_studenti = context.Studenti.AsEnumerable().OrderBy(_ => Guid.NewGuid()).ToList();
        var interrogazioni = NumeroGiorniInterrogazione(lista_studenti, interrogati_giornalmente);
        int counterGiorno = 0;
        int counterGiorniInterrogazione = 0;

        for(int i = 0; i < interrogazioni.nGiorni; i++){

            //In caso un giorno ci fossero meno studenti da interrogare
            if(interrogazioni.divisibile == false && i == interrogazioni.nGiorni - 1){
                for(int j = i*interrogati_giornalmente; j < lista_studenti.Count(); j++){
                    context.Interrogazioni.Add(new Interrogazione(){
                        Data= ProssimaInterrogazione(todayWeekDay, giorniInterrogazione, counterGiorno, counterGiorniInterrogazione), 
                        Studente = lista_studenti[j],Materia = TrovaMateria(idMateria, context)
                    });
                }
            }else{

                //Inserisce il numero di studenti da interrogare
                for(int j = 0; j < interrogati_giornalmente; j++){
                    context.Interrogazioni.Add(new Interrogazione(){
                        Data= ProssimaInterrogazione(todayWeekDay, giorniInterrogazione, counterGiorno, counterGiorniInterrogazione), 
                        Studente = lista_studenti[i*2+j],Materia = TrovaMateria(idMateria, context)
                    });
                }
            }

            //Fa ripetere i giorni della settimana
            counterGiorno++;
            if(counterGiorno == giorniInterrogazione.Count()){
                counterGiorno = 0;
                counterGiorniInterrogazione++;
            }
        }

        context.SaveChanges();
        return Results.Ok();
    }
)
.WithName("genera-programmate")
.WithOpenApi();



app.MapGet("/get-programmate/{id_materia:int}", (ClassMindContext context, int id_materia)=>{
 return context.Interrogazioni
                .Where(interrogazione => interrogazione.Materia.MateriaId == id_materia)
                .Select(interrogazione => new {
                                                materia = interrogazione.Materia.Nome, 
                                                studente = $"{interrogazione.Studente.Nome} {interrogazione.Studente.Cognome}",
                                                data = interrogazione.Data
                                            });
})
.WithName("get-programmate").WithOpenApi();

app.Run();


//Metodi d'estrazione
(int nGiorni, bool divisibile) NumeroGiorniInterrogazione(List<Studente> lista_studenti, int interrogati_giornalmente) => (lista_studenti.Count()%interrogati_giornalmente == 0? lista_studenti.Count()/interrogati_giornalmente:lista_studenti.Count()/interrogati_giornalmente+1, lista_studenti.Count()%interrogati_giornalmente == 0? true:false);
bool IsStudentAlreadyInDB(List<StudenteDTO> studenti, ClassMindContext context) => studenti.Select(studente => studente.Nome).Intersect(context.Studenti.Select(st => st.Nome)).ToList().Any() && studenti.Select(studente => studente.Cognome).Intersect(context.Studenti.Select(st => st.Cognome)).ToList().Any();
int NumeroGiornoSettimana(string g){
    return g switch
    {
        "lun"=> 1,
        "mar"=> 2,
        "mer"=> 3,
        "gio"=> 4,
        "ven"=> 5,
        "sab"=> 6
    };
}
Materia TrovaMateria(int id_materia, ClassMindContext context) => context.Materie.Where(m => m.MateriaId == id_materia).ToList()[0];
string ProssimaInterrogazione(int todayWeekDay, List<int> giorniInterrogazione, int counterGiorno, int numeroInterrogazioniFatte) => DateTime.Today.AddDays((todayWeekDay < giorniInterrogazione[counterGiorno]?7+(giorniInterrogazione[counterGiorno]-todayWeekDay):7-(todayWeekDay-giorniInterrogazione[counterGiorno]))+(7*numeroInterrogazioniFatte)).ToString("dd/MM/yyyy");
bool LaMateriaHaLezioni(ClassMindContext context, int id_materia) =>context.Lezioni.Where(lezione => lezione.Materia.MateriaId == id_materia) != null;
    