namespace Projeto_Faculdade.Views
{
    public partial class CadastroPage : ContentPage
    {
        public CadastroPage()
        {
            InitializeComponent();
        }

        private async void CadastrarButton_Clicked(object sender, EventArgs e)
        {
            string nome = NomeEntry.Text?.Trim() ?? "";
            string senha = SenhaEntry.Text ?? "";
            string confirmarSenha = ConfirmarSenhaEntry.Text ?? "";

            if (string.IsNullOrWhiteSpace(nome))
            {
                await DisplayAlert(
                    "Atenção",
                    "Digite seu nome.",
                    "OK");

                return;
            }

            if (string.IsNullOrWhiteSpace(senha))
            {
                await DisplayAlert(
                    "Atenção",
                    "Digite uma senha.",
                    "OK");

                return;
            }

            if (senha.Length < 6)
            {
                await DisplayAlert(
                    "Atenção",
                    "A senha precisa ter pelo menos 6 caracteres.",
                    "OK");

                return;
            }

            if (senha != confirmarSenha)
            {
                await DisplayAlert(
                    "Atenção",
                    "As senhas não são iguais.",
                    "OK");

                return;
            }

            bool cadastrado = App.Banco.CadastrarUsuario(nome, senha);

            if (cadastrado)
            {
                await DisplayAlert(
                    "Cadastro realizado!",
                    "Sua conta foi criada. Agora você pode fazer login.",
                    "OK");

                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert(
                    "Atenção",
                    "Esse nome de usuário já está cadastrado.",
                    "OK");
            }
        }

        private async void VoltarLogin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}