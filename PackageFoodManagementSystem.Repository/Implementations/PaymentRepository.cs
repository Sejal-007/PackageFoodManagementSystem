using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Interfaces;

using PackageFoodManagementSystem.Repository.Models;

using System.Linq;

namespace PackageFoodManagementSystem.Repository.Implementations

{

    public class PaymentRepository : IPaymentRepository

    {

        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)

        {

            _context = context;

        }

        public void AddPayment(Payment payment)

        {

            _context.Payment.Add(payment);

        }

        public Payment GetPaymentByBillId(int billId)

        {

            return _context.Payment.FirstOrDefault(p => p.BillID == billId);

        }

        public void Save()

        {

            _context.SaveChanges();

        }

    }

}
