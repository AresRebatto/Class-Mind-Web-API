class Materia
{
    public int MateriaId { get; set; }
    public string Nome { get; set; }
    public virtual ICollection<Lezione> Lezioni { get; set; }
    public virtual ICollection<Interrogazione> Interrogazioni { get; set; }
}