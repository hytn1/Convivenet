using Projeto_Faculdade.Models;

namespace Projeto_Faculdade.Views;

public partial class SobrePage : ContentPage
{
    private Usuario usuario;

    public SobrePage(Usuario usuario)
    {
        InitializeComponent();
        this.usuario = usuario;
    }

    private async void OnInicioClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HomePage(usuario));
    }

    private async void OnAtividadesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AtividadesPage(usuario));
    }
}