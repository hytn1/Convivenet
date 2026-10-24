using Projeto_Faculdade.Models;

namespace Projeto_Faculdade.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void EntrarButton_Clicked(object sender, EventArgs e)
        {
            string nome = NomeEntry.Text?.Trim() ?? "";
            string senha = SenhaEntry.Text ?? "";

            if (string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(senha))
            {
                await DisplayAlert(
                    "Atenção",
                    "Preencha seu nome e sua senha.",
                    "OK"
                );

                return;
            }

            Usuario? usuario = App.Banco.FazerLogin(nome, senha);

            if (usuario != null)
            {
                await DisplayAlert(
                    "Login realizado!",
                    $"Olá, {usuario.Nome}! Vamos começar?",
                    "Continuar"
                );

                await Navigation.PushAsync(new HomePage(usuario));
            }
            else
            {
                await DisplayAlert(
                    "Ops!",
                    "Nome ou senha incorretos.",
                    "Tentar novamente"
                );
            }
        }

        private async void CriarConta_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CadastroPage());
        }
    }
}