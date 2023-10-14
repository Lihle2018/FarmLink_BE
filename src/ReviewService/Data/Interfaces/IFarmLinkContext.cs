using MongoDB.Driver;
using ReviewService.Models;

namespace ReviewService.Data.Interfaces
{
    public interface IFarmLinkContext
    {
        IMongoCollection<Comment> Comments { get; }
        IMongoCollection<Rating> Ratings { get; }
    }
}
