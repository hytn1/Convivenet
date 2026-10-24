using Microsoft.Maui.Controls.Shapes;
using Projeto_Faculdade.Models;

namespace Projeto_Faculdade.Views;

public partial class AtividadesPage : ContentPage
{
    private Usuario usuario;

    public AtividadesPage(Usuario usuario)
    {
        InitializeComponent();

        this.usuario = usuario;

        CarregarModulos();
    }

    private void CarregarModulos()
    {
        List<Modulo> modulos = App.Banco.BuscarModulos();

        List<ProgressoModulo> progresso =
            App.Banco.BuscarProgressoUsuario(usuario.Id);

        ModulosLayout.Children.Clear();

        int modulosConcluidos = 0;

        foreach (Modulo modulo in modulos)
        {
            ProgressoModulo? progressoModulo =
                progresso.FirstOrDefault(
                    p => p.ModuloId == modulo.Id
                );

            bool concluido =
                progressoModulo != null &&
                progressoModulo.Concluido;

            if (concluido)
            {
                modulosConcluidos++;
            }

            Border card = CriarCardModulo(
                modulo,
                concluido
            );

            ModulosLayout.Children.Add(card);
        }

        ProgressoLabel.Text =
            $"{modulosConcluidos} / {modulos.Count} módulos";

        if (modulos.Count > 0)
        {
            ProgressoBar.Progress =
                (double)modulosConcluidos / modulos.Count;
        }
        else
        {
            ProgressoBar.Progress = 0;
        }
    }


    private Border CriarCardModulo(
        Modulo modulo,
        bool concluido)
    {
        string tituloStatus;
        string corStatus;
        string corFundoStatus;
        string textoDescricao;

        if (concluido)
        {
            tituloStatus = "CONCLUÍDO";
            corStatus = "#129E91";
            corFundoStatus = "#D5F7F1";

            textoDescricao =
                "Você já concluiu este módulo. Clique para revisar o conteúdo.";
        }
        else
        {
            tituloStatus = "DISPONÍVEL";
            corStatus = "#8B5CF6";
            corFundoStatus = "#EDE9FE";

            textoDescricao =
                "Leia o conteúdo educativo e depois responda às perguntas.";
        }

        Border card = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(corStatus),
            StrokeThickness = 3,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(22)
            },
            Padding = 20
        };


        TapGestureRecognizer toque =
            new TapGestureRecognizer();

        toque.Tapped += async (sender, e) =>
        {
            await ModuloClicked(modulo);
        };

        card.GestureRecognizers.Add(toque);


        VerticalStackLayout conteudo =
            new VerticalStackLayout
            {
                Spacing = 10
            };


        Border status =
            new Border
            {
                HorizontalOptions = LayoutOptions.Start,
                BackgroundColor =
                    Color.FromArgb(corFundoStatus),
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(20)
                },
                Padding = new Thickness(12, 6)
            };


        Label statusLabel =
            new Label
            {
                Text = tituloStatus,
                FontSize = 12,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb(corStatus)
            };


        status.Content = statusLabel;


        Label numero =
            new Label
            {
                Text = $"Módulo {modulo.Ordem}",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#6D28D9")
            };


        Label titulo =
            new Label
            {
                Text = modulo.Titulo,
                FontSize = 21,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#222052")
            };


        Label descricao =
            new Label
            {
                Text = textoDescricao,
                FontSize = 15,
                TextColor = Color.FromArgb("#5146E5"),
                LineBreakMode = LineBreakMode.WordWrap
            };


        conteudo.Children.Add(status);
        conteudo.Children.Add(numero);
        conteudo.Children.Add(titulo);
        conteudo.Children.Add(descricao);

        card.Content = conteudo;

        return card;
    }


    private async Task ModuloClicked(Modulo modulo)
    {
        await Navigation.PushAsync(
            new TextoModuloPage(usuario, modulo)
        );
    }


    private async void VoltarClicked(
        object sender,
        EventArgs e)
    {
        await Navigation.PopAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CarregarModulos();
    }
}