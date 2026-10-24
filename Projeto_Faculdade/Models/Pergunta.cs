using SQLite;

namespace Projeto_Faculdade.Models
{
    public class Pergunta
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ModuloId { get; set; }

        public string Texto { get; set; } = "";

        public string AlternativaA { get; set; } = "";

        public string AlternativaB { get; set; } = "";

        public string AlternativaC { get; set; } = "";

        public string AlternativaD { get; set; } = "";

        public string RespostaCorreta { get; set; } = "";

        public int Pontos { get; set; } = 10;
    }
}