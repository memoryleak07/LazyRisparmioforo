using System.Collections.Immutable;

namespace LazyRisparmioforo.Domain.Constants;

public enum CategoryOther
{
    // ReSharper disable once InconsistentNaming
    OTHER_OTHER = 39
}

public static class CategoryConstants
{
    private static readonly ImmutableDictionary<string, int> LabelToId = new Dictionary<string, int>
    {
        { "BILLS_SUBSCRIPTIONS_BILLS", 0 },
        { "BILLS_SUBSCRIPTIONS_INTERNET_PHONE", 1 },
        { "BILLS_SUBSCRIPTIONS_OTHER", 2 },
        { "BILLS_SUBSCRIPTIONS_SUBSCRIPTIONS", 3 },
        { "CREDIT_CARDS_CREDIT_CARDS", 4 },
        { "EATING_OUT_COFFEE_SHOPS", 5 },
        { "EATING_OUT_OTHER", 6 },
        { "EATING_OUT_RESTAURANTS", 7 },
        { "EATING_OUT_TAKEAWAY_RESTAURANTS", 8 },
        { "HEALTH_WELLNESS_AID_EXPENSES", 9 },
        { "HEALTH_WELLNESS_DRUGS", 10 },
        { "HEALTH_WELLNESS_GYMS", 11 },
        { "HEALTH_WELLNESS_MEDICAL_EXPENSES", 12 },
        { "HEALTH_WELLNESS_OTHER", 13 },
        { "HEALTH_WELLNESS_WELLNESS_RELAX", 14 },
        { "HOUSING_FAMILY_APPLIANCES", 15 },
        { "HOUSING_FAMILY_CHILDHOOD", 16 },
        { "HOUSING_FAMILY_FURNITURE", 17 },
        { "HOUSING_FAMILY_GROCERIES", 18 },
        { "HOUSING_FAMILY_INSURANCES", 19 },
        { "HOUSING_FAMILY_MAINTENANCE_RENOVATION", 20 },
        { "HOUSING_FAMILY_OTHER", 21 },
        { "HOUSING_FAMILY_RENTS", 22 },
        { "HOUSING_FAMILY_SERVANTS", 23 },
        { "HOUSING_FAMILY_VETERINARY", 24 },
        { "LEISURE_BOOKS", 25 },
        { "LEISURE_CINEMA", 26 },
        { "LEISURE_CLUB_ASSOCIATIONS", 27 },
        { "LEISURE_GAMBLING", 28 },
        { "LEISURE_MAGAZINES_NEWSPAPERS", 29 },
        { "LEISURE_MOVIES_MUSICS", 30 },
        { "LEISURE_OTHER", 31 },
        { "LEISURE_SPORT_EVENTS", 32 },
        { "LEISURE_THEATERS_CONCERTS", 33 },
        { "LEISURE_VIDEOGAMES", 34 },
        { "MORTGAGES_LOANS_LOANS", 35 },
        { "MORTGAGES_LOANS_MORTGAGES", 36 },
        { "OTHER_CASH", 37 },
        { "OTHER_CHECKS", 38 },
        { "OTHER_OTHER", 39 },
        { "PROFITS_PROFITS", 40 },
        { "SHOPPING_ACCESSORIZE", 41 },
        { "SHOPPING_CLOTHING", 42 },
        { "SHOPPING_FOOTWEAR", 43 },
        { "SHOPPING_HI_TECH", 44 },
        { "SHOPPING_OTHER", 45 },
        { "SHOPPING_SPORT_ARTICLES", 46 },
        { "TAXES_SERVICES_BANK_FEES", 47 },
        { "TAXES_SERVICES_DEFAULT_PAYMENTS", 48 },
        { "TAXES_SERVICES_MONEY_ORDERS", 49 },
        { "TAXES_SERVICES_OTHER", 50 },
        { "TAXES_SERVICES_PROFESSIONAL_ACTIVITY", 51 },
        { "TAXES_SERVICES_PROFIT_DEDUCTION", 52 },
        { "TAXES_SERVICES_TAXES", 53 },
        { "TRANSFERS_BANK_TRANSFERS", 54 },
        { "TRANSFERS_GIFTS_DONATIONS", 55 },
        { "TRANSFERS_INVESTMENTS", 56 },
        { "TRANSFERS_OTHER", 57 },
        { "TRANSFERS_REFUNDS", 58 },
        { "TRANSFERS_RENT_INCOMES", 59 },
        { "TRANSFERS_SAVINGS", 60 },
        { "TRAVELS_TRANSPORTATION_BUSES", 61 },
        { "TRAVELS_TRANSPORTATION_CAR_RENTAL", 62 },
        { "TRAVELS_TRANSPORTATION_FLIGHTS", 63 },
        { "TRAVELS_TRANSPORTATION_FUEL", 64 },
        { "TRAVELS_TRANSPORTATION_HOTELS", 65 },
        { "TRAVELS_TRANSPORTATION_OTHER", 66 },
        { "TRAVELS_TRANSPORTATION_PARKING_URBAN_TRANSPORTS", 67 },
        { "TRAVELS_TRANSPORTATION_TAXIS", 68 },
        { "TRAVELS_TRANSPORTATION_TOLLS", 69 },
        { "TRAVELS_TRANSPORTATION_TRAINS", 70 },
        { "TRAVELS_TRANSPORTATION_TRAVELS_HOLIDAYS", 71 },
        { "TRAVELS_TRANSPORTATION_VEHICLE_MAINTENANCE", 72 },
        { "WAGES_PENSION", 73 },
        { "WAGES_PROFESSIONAL_COMPENSATION", 74 },
        { "WAGES_SALARY", 75 }
    }.ToImmutableDictionary();
    
    public static readonly ImmutableDictionary<int, string> IdToLabel = 
        LabelToId.ToImmutableDictionary(kvp => kvp.Value, kvp => kvp.Key);

    // public static readonly ImmutableDictionary<string, int> ConsolidatedLabelToId = new Dictionary<string, int>
    // {
    //     { "BILLS_SUBSCRIPTIONS", 0 },
    //     { "CREDIT_CARDS", 1 },
    //     { "EATING_OUT", 2 },
    //     { "HEALTH_WELLNESS", 3 },
    //     { "HOUSING_FAMILY", 4 },
    //     { "LEISURE", 5 },
    //     { "MORTGAGES_LOANS", 6 },
    //     { "SHOPPING", 7 },
    //     { "TAXES_SERVICES", 8 },
    //     { "TRANSFERS", 9 },
    //     { "TRAVELS_TRANSPORTATION", 10 },
    //     { "WAGES_INCOME", 11 },
    //     { "OTHER", 12 }
    // }.ToImmutableDictionary();
    //
    // public static readonly ImmutableDictionary<string, ImmutableDictionary<string, int>> CategoryToSubcategories = 
    //     new Dictionary<string, Dictionary<string, int>>
    // {
    //     {
    //         "BILLS", new Dictionary<string, int>
    //         {
    //             { "SUBSCRIPTIONS_BILLS", 0 },
    //             { "SUBSCRIPTIONS_INTERNET_PHONE", 1 },
    //             { "SUBSCRIPTIONS_OTHER", 2 },
    //             { "SUBSCRIPTIONS_SUBSCRIPTIONS", 3 }
    //         }
    //     },
    //     {
    //         "CREDIT_CARDS", new Dictionary<string, int>
    //         {
    //             { "CREDIT_CARDS", 4 }
    //         }
    //     },
    //     {
    //         "EATING_OUT", new Dictionary<string, int>
    //         {
    //             { "COFFEE_SHOPS", 5 },
    //             { "OTHER", 6 },
    //             { "RESTAURANTS", 7 },
    //             { "TAKEAWAY_RESTAURANTS", 8 }
    //         }
    //     },
    //     // Add more categories and subcategories as needed...
    // }.ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value.ToImmutableDictionary());
}