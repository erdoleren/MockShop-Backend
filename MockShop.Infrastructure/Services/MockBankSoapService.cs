using MockShop.Application.Interfaces;

namespace MockShop.Infrastructure.Services
{
    public class MockBankSoapService : IPaymentService
    {
        public async Task<bool> ProcessPaymentAsync(decimal amount, string cardNumber)
        {
            // Simulate a delay for processing payment
            await Task.Delay(2000);

            if (amount < 10000)
            {
                Console.WriteLine($"[SOAP Mock] {amount} TL ödeme onaylandı.");
                return true;
            }

            Console.WriteLine($"[SOAP Mock] {amount} TL limit yetersiz!");
            return false;
        }
    }
}
