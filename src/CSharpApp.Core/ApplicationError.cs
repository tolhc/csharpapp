using System.Net;

namespace CSharpApp.Core;

public record ApplicationError(string Description, HttpStatusCode StatusCode);