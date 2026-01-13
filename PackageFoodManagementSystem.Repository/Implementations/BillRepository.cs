using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Interfaces;

using PackageFoodManagementSystem.Repository.Models;

using System.Linq;

namespace PackageFoodManagementSystem.Repository.Implementations

{

    public class BillRepository : IBillRepository

    {

        private readonly ApplicationDbContext _context;

        public BillRepository(ApplicationDbContext context)

        {

            _context = context;

        }

        public Bill GetBillByOrderId(int orderId)

        {

            return _context.Bills.FirstOrDefault(b => b.OrderID == orderId);

        }

        public void AddBill(Bill bill)

        {

            _context.Bills.Add(bill);

        }

        public void UpdateBill(Bill bill)

        {

            _context.Bills.Update(bill);

        }

        public void Save()

        {

            _context.SaveChanges();

        }

    }

}
