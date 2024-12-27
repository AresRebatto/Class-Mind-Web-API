using Microsoft.EntityFrameworkCore;

class ClassMindContext: DbContext
{
    public ClassMindContext(DbContextOptions<ClassMindContext> options):base(options){}
    public DbSet<Interrogazione> Interrogazioni { get; set; }
    public DbSet<Lezione> Lezioni { get; set; }
    public DbSet<Materia> Materie { get; set; }
    public DbSet<Studente> Studenti { get; set; }


}