namespace CSharpApp.Core.Dtos;

public record TodoRecord(
    [property: JsonPropertyName("userId")] int UserId,
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("completed")] bool Completed
);