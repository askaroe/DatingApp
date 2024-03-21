using API.DTOs;
using API.Entities;

namespace API.Interfaces;
public interface ILikesRepository
{
    Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
    Task<AppUser> GetUserWithLikes(int userId);
    Task<IEnumerable<LikeDTO>> GetUserLikes(string predicate, int userId);
}
