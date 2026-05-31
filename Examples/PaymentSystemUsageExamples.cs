using System;
using System.Collections.Generic;
using RetailECommerce.Services.Facades;
using RetailECommerce.Services.Observers;
using RetailECommerce.Services.Strategy.Payment;

namespace RetailECommerce.Examples
{
    /// <summary>
    /// Practical usage examples demonstrating all payment refactoring patterns.
    /// Run these examples to understand how the architecture works.
    /// </summary>
    public class PaymentSystemUsageExamples
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Payment System Design Patterns Examples ===\n");

            Example1_BasicCheckout();
            Console.WriteLine("\n" + new string('-', 60) + "\n");

            Example2_TaxCalculation();
            Console.WriteLine("\n" + new string('-', 60) + "\n");

            Example3_OrderStateTransitions();
            Console.WriteLine("\n" + new string('-', 60) + "\n");

            Example4_ObserverPattern();
            Console.WriteLine("\n" + new string('-', 60) + "\n");

            Example5_FacadePattern();
            Console.WriteLine("\n" + new string('-', 60) + "\n");

            Example6_ErrorHandling();
            Console.WriteLine("\n" + new string('-', 60) + "\n");

            Example7_CustomObserver();
        }

        /// <summary>
        /// Example 1: Basic checkout with facade
        /// Shows the simplest usage - one line to process entire checkout
        /// </summary>
        public static void Example1_BasicCheckout()
        {
            Console.WriteLine("EXAMPLE 1: Basic Checkout with Facade");
            Console.WriteLine("=====================================\n");

            var facade = new CheckoutFacade();
            
            var result = facade.ProcessCheckout(
                paymentMethod: "card",
                subtotal: 100m,
                orderId: 1001,
                userId: 42);

            Console.WriteLine($"\nCheckout Result:");
            Console.WriteLine($"  Success: {result.IsSuccessful}");
            Console.WriteLine($"  Message: {result.Message}");
            Console.WriteLine($"  Total Amount: {result.TotalAmount:C}");
            Console.WriteLine($"  Order Status: {result.GetOrderStatus()}");
            Console.WriteLine($"  Transaction ID: {result.GetTransactionId()}");
        }

        /// <summary>
        /// Example 2: Tax calculation with different rates
        /// Shows how tax logic is segregated and easily configurable
        /// </summary>
        public static void Example2_TaxCalculation()
        {
            Console.WriteLine("EXAMPLE 2: Tax Calculation Segregation");
            Console.WriteLine("=====================================\n");

            var facade = new CheckoutFacade();
            
            // Default 8% tax
            decimal total1 = facade.CalculateTotal(100m);
            Console.WriteLine($"Subtotal: $100.00");
            Console.WriteLine($"With 8% tax: {total1:C}");

            // Custom 10% tax (e.g., different region)
            facade.SetTaxCalculator(new StandardTaxCalculator(0.10m));
            decimal total2 = facade.CalculateTotal(100m);
            Console.WriteLine($"With 10% tax: {total2:C}");

            // No tax (e.g., tax-free zone)
            facade.SetTaxCalculator(new StandardTaxCalculator(0m));
            decimal total3 = facade.CalculateTotal(100m);
            Console.WriteLine($"With 0% tax: {total3:C}");
        }

        /// <summary>
        /// Example 3: Order state transitions
        /// Shows the state machine from Pending to Success/Failed
        /// </summary>
        public static void Example3_OrderStateTransitions()
        {
            Console.WriteLine("EXAMPLE 3: Order State Transitions (State Pattern)");
            Console.WriteLine("==================================================\n");

            var order1 = new OrderState(orderId: 2001);
            Console.WriteLine($"Order #{order1.OrderId} created");
            Console.WriteLine($"  Initial Status: {order1.CurrentStatus}");
            Console.WriteLine($"  Created At: {order1.CreatedAt:yyyy-MM-dd HH:mm:ss}");

            // Transition to successful
            order1.TransitionToPaymentSuccess();
            Console.WriteLine($"\nAfter successful payment:");
            Console.WriteLine($"  Status: {order1.CurrentStatus}");
            Console.WriteLine($"  Updated At: {order1.LastUpdatedAt:yyyy-MM-dd HH:mm:ss}");

            // Show that invalid transitions are prevented
            var order2 = new OrderState(orderId: 2002);
            order2.TransitionToPaymentFailure("Card declined");
            Console.WriteLine($"\nOrder #{order2.OrderId} with failed payment:");
            Console.WriteLine($"  Status: {order2.CurrentStatus}");
            Console.WriteLine($"  Failure Reason: {order2.FailureReason}");

            // Demonstrate invalid transition prevention
            try
            {
                order1.TransitionToPaymentFailure("Cannot fail - already succeeded!");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\nInvalid transition prevented: {ex.Message}");
            }
        }

        /// <summary>
        /// Example 4: Observer pattern for automatic notifications
        /// Shows how observers are notified on payment success/failure
        /// </summary>
        public static void Example4_ObserverPattern()
        {
            Console.WriteLine("EXAMPLE 4: Observer Pattern (Auto Notifications)");
            Console.WriteLine("===============================================\n");

            var context = new CheckoutContext();
            
            // Subscribe observers
            var dashboardObserver = new DashboardDataObserver();
            var receiptObserver = new ReceiptNotificationObserver();
            
            context.Subscribe(dashboardObserver);
            context.Subscribe(receiptObserver);

            Console.WriteLine("Observers subscribed:");
            Console.WriteLine("  - DashboardDataObserver");
            Console.WriteLine("  - ReceiptNotificationObserver");

            // Set payment strategy and execute
            context.SetPaymentStrategy(new CardPayment());
            
            var cartItems = new Dictionary<string, object>
            {
                { "Laptop", 999.99m },
                { "Mouse", 29.99m }
            };

            Console.WriteLine("\nProcessing payment...");
            var result = context.ExecutePayment(100m, orderId: 3001, userId: 100, cartItems);

            Console.WriteLine("\nAll observers were automatically notified!");
        }

        /// <summary>
        /// Example 5: Facade pattern simplifying complex operations
        /// Shows how facade encapsulates multiple patterns
        /// </summary>
        public static void Example5_FacadePattern()
        {
            Console.WriteLine("EXAMPLE 5: Facade Pattern (Simplified Orchestration)");
            Console.WriteLine("====================================================\n");

            Console.WriteLine("Before Facade (Complex):");
            Console.WriteLine(@"
var context = new CheckoutContext();
context.SetTaxCalculator(new StandardTaxCalculator(0.08m));
context.SetPaymentStrategy(new QRPayment());
context.Subscribe(new DashboardDataObserver());
context.Subscribe(new ReceiptNotificationObserver());
var result = context.ExecutePayment(418.99m, 4001, 200, cartItems);
// ... manual error handling, result processing
");

            Console.WriteLine("\nAfter Facade (Simple):");
            Console.WriteLine(@"
var facade = new CheckoutFacade();
var result = facade.ProcessCheckout(
    paymentMethod: 'qr',
    subtotal: 418.99m,
    orderId: 4001,
    userId: 200,
    cartItems);
// Everything handled automatically!
");

            // Demonstrate
            var facade = new CheckoutFacade();
            var cartData = new Dictionary<string, object>
            {
                { "Keyboard", 89.99m },
                { "Monitor", 329.00m }
            };

            var result = facade.ProcessCheckout("qr", 418.99m, 4001, 200, cartData);
            Console.WriteLine("\nResult from facade:");
            Console.WriteLine($"  Success: {result.IsSuccessful}");
            Console.WriteLine($"  Total: {result.TotalAmount:C}");
        }

        /// <summary>
        /// Example 6: Error handling and validation
        /// Shows how facade handles various error scenarios
        /// </summary>
        public static void Example6_ErrorHandling()
        {
            Console.WriteLine("EXAMPLE 6: Error Handling & Validation");
            Console.WriteLine("=====================================\n");

            var facade = new CheckoutFacade();

            // Invalid payment method
            Console.WriteLine("Test 1: Invalid payment method");
            var result1 = facade.ProcessCheckout("bitcoin", 100m, 5001, 300);
            Console.WriteLine($"  Result: {result1.Message}");

            // Invalid subtotal
            Console.WriteLine("\nTest 2: Invalid subtotal (zero)");
            var result2 = facade.ProcessCheckout("card", 0m, 5002, 300);
            Console.WriteLine($"  Result: {result2.Message}");

            // Invalid subtotal (negative)
            Console.WriteLine("\nTest 3: Invalid subtotal (negative)");
            var result3 = facade.ProcessCheckout("card", -100m, 5003, 300);
            Console.WriteLine($"  Result: {result3.Message}");

            // Valid case
            Console.WriteLine("\nTest 4: Valid payment");
            var result4 = facade.ProcessCheckout("cod", 100m, 5004, 300);
            Console.WriteLine($"  Result: {result4.Message}");
        }

        /// <summary>
        /// Example 7: Adding custom observers
        /// Shows how to extend the system with new observers
        /// </summary>
        public static void Example7_CustomObserver()
        {
            Console.WriteLine("EXAMPLE 7: Custom Observer Extension");
            Console.WriteLine("===================================\n");

            // Create a custom observer
            var loyaltyObserver = new CustomLoyaltyPointsObserver();
            
            var facade = new CheckoutFacade();
            facade.AddObserver(loyaltyObserver);

            Console.WriteLine("Custom observer added: LoyaltyPointsObserver");
            Console.WriteLine("\nProcessing payment with custom observer...");

            var cartData = new Dictionary<string, object>
            {
                { "Gaming PC", 1299.99m }
            };

            var result = facade.ProcessCheckout("card", 1299.99m, 6001, 400, cartData);

            Console.WriteLine("\nThe custom observer was automatically called!");
            Console.WriteLine("(See [Loyalty] output above)");
        }
    }

    /// <summary>
    /// Custom observer implementation example
    /// Shows how easy it is to add new functionality
    /// </summary>
    public class CustomLoyaltyPointsObserver : IPaymentObserver
    {
        public void OnPaymentSuccess(PaymentEventData eventData)
        {
            // Award loyalty points (1 point per dollar spent)
            int points = (int)eventData.Amount;
            Console.WriteLine($"\n[Loyalty] User {eventData.UserId} earned {points} loyalty points!");
            Console.WriteLine($"  Total spent: {eventData.Amount:C}");
            Console.WriteLine($"  Points multiplier: 1x (standard member)");
        }

        public void OnPaymentFailure(PaymentEventData eventData)
        {
            Console.WriteLine($"\n[Loyalty] Payment failed - no points awarded");
        }
    }
}
