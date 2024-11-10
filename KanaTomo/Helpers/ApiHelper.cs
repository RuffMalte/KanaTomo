namespace KanaTomo.Helpers;

public static class ApiHelper
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task<string> GetAsync(string endpoint)
    {
        var response = await client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}