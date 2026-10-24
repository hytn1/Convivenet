
using Projeto_Faculdade.Models;

namespace Projeto_Faculdade.Views;

public partial class PerfilPage : ContentPage
{
    private Usuario usuario;

    public PerfilPage(Usuario usuario)
    {
        InitializeComponent();

        this.usuario = usuario;

        CarregarDados();
    }

    private void CarregarDados()
    {
        NomeUsuarioLabel.Text = usuario.Nome;
        PontosLabel.Text = usuario.Pontos.ToString();
        NivelLabel.Text = usuario.Nivel.ToString();

        if (!string.IsNullOrWhiteSpace(usuario.FotoPerfil) &&
            File.Exists(usuario.FotoPerfil))
        {
            FotoPerfilImage.Source =
                ImageSource.FromFile(usuario.FotoPerfil);
        }

        CarregarProgresso();
    }

    private void CarregarProgresso()
    {
        const int totalModulos = 5;

        var modulos = App.Banco.BuscarModulos();

        var progressoUsuario =
            App.Banco.BuscarProgressoUsuario(usuario.Id);

        int totalConcluidos = modulos.Count(modulo =>
            progressoUsuario.Any(progresso =>
                progresso.ModuloId == modulo.Id &&
                progresso.Concluido));

        double percentual = (double)totalConcluidos / totalModulos;

        ProgressoModulosLabel.Text =
            $"{totalConcluidos} de {totalModulos} módulos";

        ProgressoBar.Progress = Math.Clamp(percentual, 0, 1);

        if (totalConcluidos == 0)
        {
            MensagemProgressoLabel.Text =
                "Comece sua jornada e descubra os módulos!";
        }
        else if (totalConcluidos < totalModulos)
        {
            MensagemProgressoLabel.Text =
                $"Você já concluiu {totalConcluidos} módulo(s). " +
                "Continue aprendendo e avançando!";
        }
        else
        {
            MensagemProgressoLabel.Text =
                "Parabéns! Você concluiu todos os módulos e aprendeu muito sobre convivência!";
        }
    }

    private async void VoltarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void AdicionarFotoClicked(object sender, EventArgs e)
    {
        try
        {
            FileResult? foto = await MediaPicker.Default.PickPhotoAsync();

            if (foto == null)
                return;

            string extensao = Path.GetExtension(foto.FileName);

            if (string.IsNullOrWhiteSpace(extensao))
                extensao = ".jpg";

            string nomeArquivo = $"foto_{Guid.NewGuid()}{extensao}";

            string caminhoFoto = Path.Combine(
                FileSystem.AppDataDirectory,
                nomeArquivo
            );

            using (Stream stream = await foto.OpenReadAsync())
            using (FileStream arquivo = File.Create(caminhoFoto))
            {
                await stream.CopyToAsync(arquivo);
            }

            bool atualizado = App.Banco.AtualizarFotoPerfil(
                usuario.Id,
                caminhoFoto
            );

            if (!atualizado)
            {
                await DisplayAlert(
                    "Erro",
                    "Não foi possível salvar a foto.",
                    "OK");

                return;
            }

            usuario.FotoPerfil = caminhoFoto;

            FotoPerfilImage.Source = null;
            FotoPerfilImage.Source =
                ImageSource.FromFile(caminhoFoto);

            await DisplayAlert(
                "Foto atualizada",
                "Sua foto de perfil foi alterada com sucesso!",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Erro",
                $"Não foi possível alterar a foto.\n\n{ex.Message}",
                "OK");
        }
    }
}
