namespace BusinessLogic
{
    public class BL : IBL
    {
        public decimal CalculateShippingCost(decimal orderValue)
        {
            //The checkout page will call a(C#) backend to calculate the total shipping cost. $10 shipping cost for orders less of $50 dollars and less. $20 for orders more than $50.
            //this requirement doesn't seem to be correct, normally, more you buy, less shipping cost should be. so,  I made below logic instead.
            return orderValue > 50m ? 10m : 20m;
        }
    }

    public interface IBL
    {
        public decimal CalculateShippingCost(decimal orderValue);
    }
}