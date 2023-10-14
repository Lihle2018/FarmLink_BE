namespace PromotionsService.Enumarations
{
    public enum PromotionTargetAudience
    {
        /// <summary>
        /// The promotion is applicable to all customers.
        /// </summary>
        AllCustomers,

        /// <summary>
        /// The promotion is exclusively for new customers.
        /// </summary>
        NewCustomers,

        /// <summary>
        /// The promotion is exclusively for returning customers.
        /// </summary>
        ReturningCustomers,

        /// <summary>
        /// The promotion is exclusively for premium members or subscribers.
        /// </summary>
        PremiumMembers,

        /// <summary>
        /// The promotion is customized for a specific segment of customers.
        /// </summary>
        SpecificSegment,

        /// <summary>
        /// The promotion targets customers based on their purchase history.
        /// </summary>
        PurchaseHistory,

        /// <summary>
        /// The promotion targets customers based on their location or geography.
        /// </summary>
        GeographicalLocation,

        /// <summary>
        /// The promotion targets customers based on their age group.
        /// </summary>
        AgeGroup,

        /// <summary>
        /// The promotion targets customers based on their gender.
        /// </summary>
        Gender,

        /// <summary>
        /// The promotion targets customers based on their interests or preferences.
        /// </summary>
        Interests,

        /// <summary>
        /// The promotion targets customers who have abandoned their shopping carts.
        /// </summary>
        CartAbandoners,

        /// <summary>
        /// The promotion targets customers celebrating birthdays or anniversaries.
        /// </summary>
        SpecialOccasions,

        /// <summary>
        /// The promotion targets customers who have referred others.
        /// </summary>
        ReferralSource,

        /// <summary>
        /// The promotion targets customers who engage with specific marketing channels.
        /// </summary>
        MarketingChannel,

        /// <summary>
        /// The promotion targets customers who have subscribed to newsletters or updates.
        /// </summary>
        NewsletterSubscribers,

        /// <summary>
        /// The promotion targets customers who have provided feedback or reviews.
        /// </summary>
        FeedbackProviders,

        /// <summary>
        /// The promotion is customized for a specific user group.
        /// </summary>
        CustomUserGroup
    }

}
