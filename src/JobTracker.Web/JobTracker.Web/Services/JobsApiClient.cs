using System.Net.Http.Json;
using JobTracker.Shared;

namespace JobTracker.Web.Services;

public class JobsApiClient(HttpClient http)
{
    public async Task<List<JobApplicationDto>> GetAllAsync(string? search = null, JobApplicationStatus? status = null)
    {
        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(search))
            query.Add($"search={Uri.EscapeDataString(search)}");
        if (status.HasValue)
            query.Add($"status={Uri.EscapeDataString(status.Value.ToString())}");

        var url = "api/job-applications" + (query.Count > 0 ? "?" + string.Join("&", query) : "");
        return await http.GetFromJsonAsync<List<JobApplicationDto>>(url) ?? [];
    }

    public Task<JobApplicationDto?> GetAsync(int id) =>
        http.GetFromJsonAsync<JobApplicationDto>($"api/job-applications/{id}");

    public async Task<(JobApplicationDto? Item, string? Error)> CreateAsync(JobApplicationRequest request)
    {
        var response = await http.PostAsJsonAsync("api/job-applications", request);
        if (response.IsSuccessStatusCode)
            return (await response.Content.ReadFromJsonAsync<JobApplicationDto>(), null);
        return (null, await ReadErrorAsync(response));
    }

    public async Task<(JobApplicationDto? Item, string? Error)> UpdateAsync(int id, JobApplicationRequest request)
    {
        var response = await http.PutAsJsonAsync($"api/job-applications/{id}", request);
        if (response.IsSuccessStatusCode)
            return (await response.Content.ReadFromJsonAsync<JobApplicationDto>(), null);
        return (null, await ReadErrorAsync(response));
    }

    public async Task<string?> DeleteAsync(int id)
    {
        var response = await http.DeleteAsync($"api/job-applications/{id}");
        return response.IsSuccessStatusCode ? null : await ReadErrorAsync(response);
    }

    private static async Task<string> ReadErrorAsync(HttpResponseMessage response)
    {
        try
        {
            var payload = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            if (payload is not null && payload.TryGetValue("message", out var message) && !string.IsNullOrWhiteSpace(message))
                return message;
        }
        catch
        {
            // Fall back to raw/status text.
        }

        var body = await response.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(body) ? $"Request failed ({(int)response.StatusCode})." : body;
    }
}
