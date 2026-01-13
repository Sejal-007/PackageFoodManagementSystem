using System;
using System.Collections.Generic;
using System.Text;
using PackageFoodManagementSystem.Repository.Models;

namespace PackageFoodManagementSystem.Repository.Interfaces
{
    public interface IBillRepository
    {
        Bill GetBillByOrderId(int orderId);
        void AddBill(Bill bill);
        void UpdateBill(Bill bill);
        void Save();
    }
}
