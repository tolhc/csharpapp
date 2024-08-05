namespace CSharpApp.Core.Interfaces;

public interface IPostsService
{
    Task<PostRecord?> GetPostById(int id, CancellationToken cancellationToken);
    Task<ReadOnlyCollection<PostRecord>> GetAllPosts(CancellationToken cancellationToken);
    Task<PostRecord?> CreatePost(PostRecord postRecord, CancellationToken cancellationToken);
    Task DeletePostById(int id, CancellationToken cancellationToken);
}