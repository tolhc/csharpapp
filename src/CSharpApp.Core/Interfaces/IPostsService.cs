namespace CSharpApp.Core.Interfaces;

public interface IPostsService
{
    Task<Result<PostRecord?, ApplicationError>> GetPostById(int id, CancellationToken cancellationToken);
    Task<Result<ReadOnlyCollection<PostRecord>, ApplicationError>> GetAllPosts(CancellationToken cancellationToken);
    Task<Result<PostRecord?, ApplicationError>> CreatePost(PostRecord postRecord, CancellationToken cancellationToken);
    Task<UnitResult<ApplicationError>> DeletePostById(int id, CancellationToken cancellationToken);
}