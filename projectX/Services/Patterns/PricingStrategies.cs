namespace projectX.Services.Patterns
{
    public interface IPricingStrategy
    {
        double CalculatePrice(double basePrice, int quantity, string userType);
    }

    public class StandardPricingStrategy : IPricingStrategy
    {
        public double CalculatePrice(double basePrice, int quantity, string userType)
        {
            LoggerService.Instance.LogInformation($"Standard pricing applied for {quantity} tickets");
            return basePrice * quantity;
        }
    }

    public class StudentPricingStrategy : IPricingStrategy
    {
        public double CalculatePrice(double basePrice, int quantity, string userType)
        {
            double discount = 0.2;
            double total = (basePrice * (1 - discount)) * quantity;
            LoggerService.Instance.LogInformation($"Student pricing applied: {discount:P0} discount for {quantity} tickets");
            return total;
        }
    }

    public class SeniorPricingStrategy : IPricingStrategy
    {
        public double CalculatePrice(double basePrice, int quantity, string userType)
        {
            double discount = 0.3;
            double total = (basePrice * (1 - discount)) * quantity;
            LoggerService.Instance.LogInformation($"Senior pricing applied: {discount:P0} discount for {quantity} tickets");
            return total;
        }
    }

    public class PremiumPricingStrategy : IPricingStrategy
    {
        public double CalculatePrice(double basePrice, int quantity, string userType)
        {
            double premiumMultiplier = 1.5;
            double total = (basePrice * premiumMultiplier) * quantity;
            LoggerService.Instance.LogInformation($"Premium pricing applied: {premiumMultiplier}x multiplier for {quantity} tickets");
            return total;
        }
    }

    public class PricingContext
    {
        private IPricingStrategy _strategy;

        public PricingContext(IPricingStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IPricingStrategy strategy)
        {
            _strategy = strategy;
        }

        public double ExecuteStrategy(double basePrice, int quantity, string userType)
        {
            return _strategy.CalculatePrice(basePrice, quantity, userType);
        }
    }
}