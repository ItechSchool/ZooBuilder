namespace ZooBuilderBackend;

public class RevenueCalculation
{
    private List<FoodStand> _foodStands = new List<FoodStand>();

    public int GetDailyRevenue(int totalVisitors)
    {
        int revenue = 0;
        int visitorsLeft = totalVisitors;
        _foodStands.Sort((stand, foodStand) => stand.MaxRevenuePerDay.CompareTo(foodStand.MaxRevenuePerDay));
        foreach (var stand in _foodStands)
        {
            int visitors = Math.Max(stand.MaxVisitorsPerDay, visitorsLeft);
            visitorsLeft -= visitors;
            revenue += (visitors / stand.MaxVisitorsPerDay) * stand.MaxRevenuePerDay;
        }

        return revenue;
    }
}

struct FoodStand
{
    public int MaxVisitorsPerDay;
    public int MaxRevenuePerDay;
}