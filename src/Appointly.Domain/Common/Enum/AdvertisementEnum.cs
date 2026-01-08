namespace Appointly.Domain.Common.Enum
{
    public enum FuelType
    {
        Petrol = 1,
        Diesel = 2,
        Hybrid = 3,
        Electric = 4 
    }

    public enum TransmissionType 
    { 
        Manual = 1, 
        Automatic = 2
    }

    public enum VehicleCondition 
    { 
        New = 1, 
        Used = 2, 
        Reconditioned = 3
    }

    public enum AdStatus 
    { 
        Pending = 1,
        Active = 2, 
        Rejected = 3,   
        Sold = 4, 
        Removed = 5 
    }

    public enum SortBy
    {
        CreatedDate,
        Year,
        Price,
        EngineCapacity,
        Title
    }

    public enum SortOrder
    {
        Asc,
        Desc
    }
}
