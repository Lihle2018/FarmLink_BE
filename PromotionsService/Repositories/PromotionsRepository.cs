using FarmLink.Shared.Enumarations;
using MongoDB.Driver;
using PromotionsService.Data.Interfaces;
using PromotionsService.Enumarations;
using PromotionsService.Models;
using PromotionsService.Models.RequestModels;
using PromotionsService.Repositories.Interfaces;

namespace PromotionsService.Repositories
{
    public class PromotionsRepository : IPromotionsRepository
    {
        private readonly IFarmLinkContext _context;
        public PromotionsRepository(IFarmLinkContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Promotion> CreatePromotionAsync(PromotionRequestModel Request)
        {
            var promotion = new Promotion(Request);
            await _context.Promotions.InsertOneAsync(promotion);
            return promotion;
        }

        public async Task<long> DeletePromotionAsync(string promotionId)
        {
            var result = await _context.Promotions.DeleteOneAsync(promotionId);
            return result.DeletedCount;
        }

        public async Task<IEnumerable<Promotion>> GetActivePromotionsAsync()
        {
            var result = await _context.Promotions.Find(p => p.State == State.Active).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Promotion>> GetPromotionsAsync()
        {
            var result = await _context.Promotions.Find(p => true).ToListAsync();
            return result;
        }

        public async Task<Promotion> GetPromotionByIdAsync(string promotionId)
        {
            var result = await _context.Promotions.Find(p => p.Id == promotionId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<Promotion>> GetPromotionsByTargetAudienceAsync(PromotionTargetAudience targetAudience)
        {
            var result = await _context.Promotions.Find(p => p.TargetAudience==targetAudience).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Promotion>> GetPromotionsByTypeAsync(PromotionType type)
        {
            var result = await _context.Promotions.Find(p => p.Type == type).ToListAsync();
            return result;
        }

        public async Task<Promotion> UpdatePromotionAsync(PromotionRequestModel promotion)
        {
            var filter = Builders<Promotion>.Filter.Eq(p => p.Id, promotion.Id);

            var update = Builders<Promotion>.Update
                .Set(p => p.Name, promotion.Name)
                .Set(p => p.Description, promotion.Description)
                .Set(p => p.Code, promotion.Code)
                .Set(p => p.DiscountAmount, promotion.DiscountAmount)
                .Set(p => p.StartDate, promotion.StartDate)
                .Set(p => p.EndDate, promotion.EndDate)
                .Set(p => p.Type, promotion.Type)
                .Set(p => p.State, promotion.State)
                .Set(p => p.CreatedBy, promotion.CreatedBy)
                .Set(p => p.CreatedDate, promotion.CreatedDate)
                .Set(p => p.ModifiedBy, promotion.ModifiedBy)
                .Set(p => p.ModifiedDate, DateTime.Now)
                .Set(p => p.TargetAudience, promotion.TargetAudience);

            var options = new FindOneAndUpdateOptions<Promotion>
            {
                ReturnDocument = ReturnDocument.After
            };

            return await _context.Promotions.FindOneAndUpdateAsync(filter, update, options);
        }
    }
}
