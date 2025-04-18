using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Maui.Controls;

public class GoogleSheetsService
{
    private static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
    private const string ApplicationName = "WsInventarioApp";
    private const string SpreadsheetId = "12_k3zHMlQlJ-p46VzaqL42aUsQmdk3RU3njbta-cZMk";
    private const string SheetRange = "A:B";
    private const string ColumnDRange = "D:D";
    private const string ResourceName = "MauiApp1.Resources.Raw.wsinventarioapp6280e2388278.json";

    private  Task<GoogleCredential> GetCredentialsFromEmbeddedResource()
    {
        var assembly = Assembly.GetExecutingAssembly();

        using Stream? stream = assembly.GetManifestResourceStream(ResourceName);
        if (stream == null)
        {
            throw new FileNotFoundException($"No se encontró el recurso incrustado: {ResourceName}");
        }
        GoogleCredential cred = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
        return Task.FromResult(cred);
    }

    public async Task<string[]> BuscarValoresPorCeldaD(string valorBuscado)
    {
        try
        {
            GoogleCredential credential = await GetCredentialsFromEmbeddedResource();

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var requestD = service.Spreadsheets.Values.Get(SpreadsheetId, ColumnDRange);
            var responseD = await requestD.ExecuteAsync();
            var valuesD = responseD.Values;

            if (valuesD == null || valuesD.Count == 0)
                return new string[] { "Error: No se encontraron datos en la columna D" };

            int rowIndex = -1;
            for (int i = 0; i < valuesD.Count; i++)
            {
                if (valuesD[i].Count > 0 && (valuesD[i][0]?.ToString() ?? "") == valorBuscado)
                {
                    rowIndex = i;
                    break;
                }
            }

            if (rowIndex == -1)
                return new string[] { "Error: Valor no encontrado en la columna D" };

            string rowRange = $"A{rowIndex + 1}:B{rowIndex + 1}";
            var requestABC = service.Spreadsheets.Values.Get(SpreadsheetId, rowRange);
            var responseABC = await requestABC.ExecuteAsync();
            var valuesABC = responseABC.Values;

            if (valuesABC == null || valuesABC.Count == 0)
                return new string[] { "Error: No se encontraron datos en la fila correspondiente" };

            var fila = valuesABC[0];
            string valorA = fila.Count > 0 ? (fila[0]?.ToString() ?? "N/A") : "N/A";
            string valorB = fila.Count > 1 ? (fila[1]?.ToString() ?? "N/A") : "N/A";

            return new string[] { valorA, valorB };
        }
        catch (Exception ex)
        {
            return new string[] { $"Error: {ex.Message}" };
        }
    }

    public async Task<string?> EmpresaExisteEnGoogleSheets(string empresa)
    {
        try
        {
            GoogleCredential credential = await GetCredentialsFromEmbeddedResource();

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var request = service.Spreadsheets.Values.Get(SpreadsheetId, "C:D");
            var response = await request.ExecuteAsync();
            var values = response.Values;

            if (values != null)
            {
                foreach (var row in values)
                {
                    if (row.Count >= 2 && (row[1]?.ToString()?.Trim() ?? "") == empresa)
                        return row[0]?.ToString() ?? "";
                }
            }

            return null;
        }
        catch (Exception ex)
        {
           // await MostrarAlertaCompat("Error", $"Error al verificar empresa: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Muestra una alerta compatible con iOS y Android usando el nuevo enfoque sin Application.MainPage
    /// </summary>
    private static async Task MostrarAlertaCompat(string titulo, string mensaje)
    {
        if (Application.Current?.Windows.Count > 0 &&
            Application.Current.Windows[0].Page is Page currentPage)
        {
            await currentPage.DisplayAlert(titulo, mensaje, "OK");
        }
    }
}
