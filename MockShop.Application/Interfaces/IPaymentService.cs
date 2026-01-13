using System;
using System.Collections.Generic;
using System.Text;

namespace MockShop.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ProcessPaymentAsync(decimal amount, string cardNumber);
    }
}
