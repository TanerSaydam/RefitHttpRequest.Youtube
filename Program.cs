using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services
    .AddRefitGeneratedClient<IPostClient>()
    .ConfigureHttpClient(c =>
            c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com"));
var app = builder.Build();

//app.MapGet("/", async (IHttpClientFactory httpClientFactory) =>
//{
//    var client = httpClientFactory.CreateClient();
//    var message = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");
//    var posts = await message.Content.ReadFromJsonAsync<List<PostDto>>();
//    return Results.Ok(posts);
//});

app.MapGet("/", async (IPostClient postClient) =>
{
    var posts = await postClient.GetPosts();
    return Results.Ok(posts);
});

app.MapGet("/{id}", async (int id, IPostClient postClient) =>
{
    var post = await postClient.GetPost(id);
    return Results.Ok(post);
});

app.Run();

interface IPostClient
{
    [Get("/posts")]
    Task<List<PostDto>> GetPosts();


    [Get("/posts/{id}")]
    Task<PostDto> GetPost(int id);
}

record PostDto(
    int UserId,
    int Id,
    string Title,
    string Body);