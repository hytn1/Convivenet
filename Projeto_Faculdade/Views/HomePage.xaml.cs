using Projeto_Faculdade.Models;

namespace Projeto_Faculdade.Views
{
    public partial class HomePage : ContentPage
    {
        private Usuario usuario;

        public HomePage(Usuario usuario)
        {
            InitializeComponent();

            this.usuario = usuario;

            CarregarUsuario();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Busca novamente os dados atualizados no banco
            Usuario? usuarioAtualizado =
                App.Banco.BuscarUsuarioPorId(usuario.Id);

            if (usuarioAtualizado != null)
            {
                usuario = usuarioAtualizado;
                CarregarUsuario();
            }
        }

        private void CarregarUsuario()
        {
            NomeUsuarioLabel.Text = $"Olá, {usuario.Nome}!";

            // Mostra a foto do usuário
            if (!string.IsNullOrWhiteSpace(usuario.FotoPerfil) &&
                File.Exists(usuario.FotoPerfil))
            {
                FotoUsuarioImage.Source =
                    ImageSource.FromFile(usuario.FotoPerfil);

                FotoUsuarioImage.IsVisible = true;
                InicialLabel.IsVisible = false;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(usuario.Nome))
                {
                    InicialLabel.Text =
                        usuario.Nome.Substring(0, 1).ToUpper();
                }

                InicialLabel.IsVisible = true;
                FotoUsuarioImage.IsVisible = false;
            }
        }

        private async void OnJogarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AtividadesPage(usuario));
        }

        private async void OnPerfilClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(
                new PerfilPage(usuario));
        }

        private async void OnSobreClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(
                new SobrePage(usuario));
        }

        private async void OnSairClicked(object sender, EventArgs e)
        {
            bool sair = await DisplayAlert(
                "Sair",
                "Deseja realmente sair?",
                "Sim",
                "Não");

            if (sair)
            {
                await Navigation.PopToRootAsync();
            }
        }
    }
}