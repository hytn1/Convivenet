using SQLite;

namespace Projeto_Faculdade.Models
{
    public class ProgressoModulo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int ModuloId { get; set; }

        public bool Concluido { get; set; }
    }
}