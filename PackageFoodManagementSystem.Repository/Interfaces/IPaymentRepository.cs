using System;
using System.Collections.Generic;
using System.Text;
using PackageFoodManagementSystem.Repository.Models;


namespace PackageFoodManagementSystem.Repository.Interfaces
{
    public interface IPaymentRepository
    {
        void AddPayment(Payment payment);
        Payment GetPaymentByBillId(int billId);

        void Save();
    }
}
