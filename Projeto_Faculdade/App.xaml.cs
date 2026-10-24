using Projeto_Faculdade.Data;
using Projeto_Faculdade.Views;

namespace Projeto_Faculdade
{
    public partial class App : Application
    {
        public static BancoDados Banco { get; private set; }

        public App()
        {
            InitializeComponent();

            Banco = new BancoDados();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new LoginPage()));
        }
    }
}