using System.Security.Cryptography;

namespace Projeto_Faculdade.Services
{
    public static class SenhaHelper
    {
        private const int TamanhoSalt = 16;
        private const int TamanhoHash = 32;
        private const int Iteracoes = 100000;

        public static string CriarHash(string senha)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(TamanhoSalt);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                senha,
                salt,
                Iteracoes,
                HashAlgorithmName.SHA256,
                TamanhoHash
            );

            return $"{Iteracoes}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public static bool VerificarSenha(string senha, string senhaHash)
        {
            string[] partes = senhaHash.Split('.');

            if (partes.Length != 3)
                return false;

            int iteracoes = int.Parse(partes[0]);

            byte[] salt = Convert.FromBase64String(partes[1]);
            byte[] hashOriginal = Convert.FromBase64String(partes[2]);

            byte[] hashDigitado = Rfc2898DeriveBytes.Pbkdf2(
                senha,
                salt,
                iteracoes,
                HashAlgorithmName.SHA256,
                hashOriginal.Length
            );

            return CryptographicOperations.FixedTimeEquals(
                hashOriginal,
                hashDigitado
            );
        }
    }
}