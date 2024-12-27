class Studente
{
    public int StudenteId { get; set; }
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public virtual ICollection<Interrogazione> Interrogazioni { get; set; }
}