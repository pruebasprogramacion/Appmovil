using Wsinvmovil.Views;
namespace Wsinvmovil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        // Método recomendado en .NET MAUI moderno
        protected override Window CreateWindow(IActivationState? activationState)
        {
            Page paginaInicial;

            if (Preferences.ContainsKey("EmpresaNombre"))
            {
                paginaInicial = new LoginPage();
            }
            else
            {
                paginaInicial = new SetupPage();
            }

            return new Window(paginaInicial);
        }
    }
}
