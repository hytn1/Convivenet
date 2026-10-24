
using Projeto_Faculdade.Models;

namespace Projeto_Faculdade.Views;

public partial class TextoModuloPage : ContentPage
{
    private Usuario usuario;
    private Modulo modulo;

    public TextoModuloPage(Usuario usuario, Modulo modulo)
    {
        InitializeComponent();

        this.usuario = usuario;
        this.modulo = modulo;

        TituloModuloLabel.Text = $"Módulo {modulo.Ordem}";
        TituloTextoLabel.Text = modulo.Titulo;
        TextoEducativoLabel.Text = modulo.Texto;

        ImagemModulo.Source = modulo.Imagem;
    }

    private async void ContinuarClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(
            new PerguntasPage(usuario, modulo)
        );
    }

    private async void VoltarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
