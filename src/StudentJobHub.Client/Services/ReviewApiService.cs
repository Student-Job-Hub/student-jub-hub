using System.Net.Http.Json;
using StudentJobHub.Client.Models;

namespace StudentJobHub.Client.Services;

public class ReviewApiService
{
    private readonly HttpClient _httpClient;

    public ReviewApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ReviewModel>> GetUserReviewsAsync(string userId)
    {
        return await _httpClient.GetFromJsonAsync<List<ReviewModel>>($"api/reviews/user/{userId}")
            ?? new List<ReviewModel>();
    }

    public async Task<HttpResponseMessage> CreateAsync(CreateReviewModel model)
    {
        return await _httpClient.PostAsJsonAsync("api/reviews", model);
    }

    public async Task<HttpResponseMessage> DeleteAsync(int id)
    {
        return await _httpClient.DeleteAsync($"api/reviews/{id}");
    }
}
