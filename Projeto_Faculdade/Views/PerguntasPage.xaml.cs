using Projeto_Faculdade.Models;

namespace Projeto_Faculdade.Views;

public partial class PerguntasPage : ContentPage
{
    private Usuario usuario;
    private Modulo modulo;

    private List<Pergunta> perguntas = new List<Pergunta>();

    private int perguntaAtual = 0;
    private int pontosObtidos = 0;

    public PerguntasPage(Usuario usuario, Modulo modulo)
    {
        InitializeComponent();

        this.usuario = usuario;
        this.modulo = modulo;

        TituloModuloLabel.Text =
            $"Módulo {modulo.Ordem} - {modulo.Titulo}";

        VerificarModuloConcluido();
    }

    private async void VerificarModuloConcluido()
    {
        if (App.Banco.ModuloEstaConcluido(usuario.Id, modulo.Id))
        {
            await DisplayAlert(
                "Módulo concluído",
                "Você já respondeu a todas as perguntas deste módulo.",
                "OK"
            );

            await Navigation.PopAsync();
            return;
        }

        CarregarPerguntas();
    }
    private void CarregarPerguntas()
    {
        perguntas = App.Banco.BuscarPerguntasDoModulo(modulo.Id);

        if (perguntas.Count == 0)
        {
            PerguntaLabel.Text =
                "Não existem perguntas cadastradas para este módulo.";

            AlternativaAButton.IsVisible = false;
            AlternativaBButton.IsVisible = false;
            AlternativaCButton.IsVisible = false;
            AlternativaDButton.IsVisible = false;

            return;
        }

        MostrarPergunta();
    }

    private void MostrarPergunta()
    {
        Pergunta pergunta = perguntas[perguntaAtual];

        PerguntaNumeroLabel.Text =
            $"Pergunta {perguntaAtual + 1} de {perguntas.Count}";

        PerguntaLabel.Text = pergunta.Texto;

        AlternativaAButton.Text =
            $"A) {pergunta.AlternativaA}";

        AlternativaBButton.Text =
            $"B) {pergunta.AlternativaB}";

        AlternativaCButton.Text =
            $"C) {pergunta.AlternativaC}";

        AlternativaDButton.Text =
            $"D) {pergunta.AlternativaD}";

        AlternativaAButton.IsEnabled = true;
        AlternativaBButton.IsEnabled = true;
        AlternativaCButton.IsEnabled = true;
        AlternativaDButton.IsEnabled = true;

        StatusLabel.Text = "";
        ProximaButton.IsVisible = false;
    }

    private async void AlternativaA_Clicked(
        object sender,
        EventArgs e)
    {
        await Responder("A");
    }

    private async void AlternativaB_Clicked(
        object sender,
        EventArgs e)
    {
        await Responder("B");
    }

    private async void AlternativaC_Clicked(
        object sender,
        EventArgs e)
    {
        await Responder("C");
    }

    private async void AlternativaD_Clicked(
        object sender,
        EventArgs e)
    {
        await Responder("D");
    }

    private async Task Responder(string resposta)
    {
        if (perguntaAtual >= perguntas.Count)
            return;

        Pergunta pergunta = perguntas[perguntaAtual];

        // Impede que o usuário responda novamente
        AlternativaAButton.IsEnabled = false;
        AlternativaBButton.IsEnabled = false;
        AlternativaCButton.IsEnabled = false;
        AlternativaDButton.IsEnabled = false;

        if (resposta == pergunta.RespostaCorreta)
        {
            pontosObtidos += pergunta.Pontos;

            StatusLabel.Text =
                $"Resposta correta! +{pergunta.Pontos} pontos";

            StatusLabel.TextColor =
                Color.FromArgb("#129E91");
        }
        else
        {
            StatusLabel.Text =
                $"Resposta incorreta. A resposta correta era {pergunta.RespostaCorreta}.";

            StatusLabel.TextColor =
                Color.FromArgb("#FF3F64");
        }

        // Se ainda existem perguntas, mostra o botão para continuar
        if (perguntaAtual < perguntas.Count - 1)
        {
            ProximaButton.Text = "Próxima pergunta";
            ProximaButton.IsVisible = true;
        }
        else
        {
            ProximaButton.Text = "Concluir módulo";
            ProximaButton.IsVisible = true;
        }
    }

    private async void ProximaButton_Clicked(
        object sender,
        EventArgs e)
    {
        // Se ainda não chegou na última pergunta
        if (perguntaAtual < perguntas.Count - 1)
        {
            perguntaAtual++;

            MostrarPergunta();

            return;
        }

        // Adiciona os pontos ao usuário
        if (pontosObtidos > 0)
        {
            App.Banco.AdicionarPontos(
                usuario.Id,
                pontosObtidos
            );
        }

        bool concluido = App.Banco.ConcluirModulo(
    usuario.Id,
    modulo.Id
);

        if (!concluido)
        {
            await DisplayAlert(
                "Erro",
                "Não foi possível registrar a conclusão do módulo.",
                "OK"
            );

            return;
        }

        // Atualiza o objeto do usuário
        Usuario? usuarioAtualizado =
            App.Banco.BuscarUsuarioPorId(usuario.Id);

        if (usuarioAtualizado != null)
        {
            usuario = usuarioAtualizado;
        }

        await DisplayAlert(
            "Módulo concluído!",
            $"Você terminou o módulo {modulo.Titulo}.\n\n" +
            $"Pontos conquistados: {pontosObtidos}\n" +
            $"Total de pontos: {usuario.Pontos}\n" +
            $"Nível: {usuario.Nivel}",
            "Continuar"
        );

        await Navigation.PopAsync();
    }

    private async void VoltarClicked(
        object sender,
        EventArgs e)
    {
        await Navigation.PopAsync();
    }
}