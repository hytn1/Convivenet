using Projeto_Faculdade.Models;

namespace Projeto_Faculdade.Views;

public partial class ModuloPage : ContentPage
{
    private Usuario usuario;
    private Modulo modulo;

    public ModuloPage(Usuario usuario, Modulo modulo)
    {
        InitializeComponent();

        this.usuario = usuario;
        this.modulo = modulo;

        CarregarModulo();
    }

    private void CarregarModulo()
    {
        NumeroModuloLabel.Text = $"Módulo {modulo.Ordem}";

        TituloModuloLabel.Text = modulo.Titulo;

        TituloPrincipalLabel.Text = modulo.Titulo;

        TextoModuloLabel.Text = modulo.Texto;
    }

    private async void ContinuarPerguntasClicked(
        object sender,
        EventArgs e)
    {
        await Navigation.PushAsync(
            new PerguntasPage(usuario, modulo)
        );
    }

    private async void VoltarClicked(
        object sender,
        EventArgs e)
    {
        await Navigation.PopAsync();
    }
}