namespace CSharpApp.Core.Dtos;

public record PostRecord(
    [property: JsonPropertyName("userId")] int UserId,
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("body")] string Body
);