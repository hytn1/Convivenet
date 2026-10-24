using SQLite;

namespace Projeto_Faculdade.Models
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nome { get; set; } = "";

        public string SenhaHash { get; set; } = "";

        public int Pontos { get; set; }

        public int Nivel { get; set; }

        public string FotoPerfil { get; set; } = "";
    }
}