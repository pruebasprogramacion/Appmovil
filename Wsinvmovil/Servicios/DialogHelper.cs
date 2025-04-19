public static class DialogHelper
{
    // 🔹 Alerta simple con botón único (por defecto: OK)
    public static async Task ShowAlert(string title, string message, string cancel = "OK")
    {
        var page = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (page != null)
            await page.DisplayAlert(title, message, cancel);
    }

    // 🔹 Diálogo de confirmación con dos botones
    public static async Task<bool> ShowConfirmation(string title, string message, string accept = "Sí", string cancel = "No")
    {
        var page = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (page != null)
            return await page.DisplayAlert(title, message, accept, cancel);

        return false; // Si no hay ventana activa, retornar falso por seguridad
    }
}
