using SQLite;

namespace Projeto_Faculdade.Models
{
    public class Modulo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Titulo { get; set; } = "";

        public string Texto { get; set; } = "";

        public int Ordem { get; set; }

        public string Imagem { get; set; } = "";
    }
}