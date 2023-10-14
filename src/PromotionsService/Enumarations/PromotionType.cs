namespace PromotionsService.Enumarations
{
    public enum PromotionType
    {
        /// <summary>
        /// Discount a percentage off the product price.
        /// </summary>
        PercentageDiscount,

        /// <summary>
        /// Discount a fixed amount from the product price.
        /// </summary>
        FixedAmountDiscount,

        /// <summary>
        /// Buy one, get one free or with a discount.
        /// </summary>
        BuyOneGetOne,

        /// <summary>
        /// Special price for bundled products.
        /// </summary>
        BundleDiscount,

        /// <summary>
        /// Free shipping for orders that meet specific criteria.
        /// </summary>
        FreeShipping,

        /// <summary>
        /// Cashback or rewards on purchases.
        /// </summary>
        Cashback,

        /// <summary>
        /// Gift with purchase.
        /// </summary>
        GiftWithPurchase,

        /// <summary>
        /// Flash sale with limited-time discounts.
        /// </summary>
        FlashSale,

        /// <summary>
        /// Seasonal or holiday-specific promotions.
        /// </summary>
        SeasonalSale,

        /// <summary>
        /// Clearance or closeout sale.
        /// </summary>
        ClearanceSale,

        /// <summary>
        /// Membership or loyalty program benefits.
        /// </summary>
        MembershipBenefits,

        /// <summary>
        /// Referral rewards for customer referrals.
        /// </summary>
        ReferralRewards,

        /// <summary>
        /// Special promotions for specific events or occasions.
        /// </summary>
        EventPromotion,

        /// <summary>
        /// Custom or undefined promotion type.
        /// </summary>
        Custom,

        /// <summary>
        /// No promotion type specified.
        /// </summary>
        None
    }
}
