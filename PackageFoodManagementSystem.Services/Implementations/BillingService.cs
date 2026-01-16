using PackageFoodManagementSystem.Repository.Interfaces;

using PackageFoodManagementSystem.Repository.Models;

using PackageFoodManagementSystem.Services.Interfaces;

using System;

namespace PackageFoodManagementSystem.Services.Implementations

{

    public class BillingService : IBillingService

    {

        private readonly IBillRepository _billRepository;

        private readonly IPaymentRepository _paymentRepository;

        public BillingService(IBillRepository billRepository, IPaymentRepository paymentRepository)

        {

            _billRepository = billRepository;

            _paymentRepository = paymentRepository;

        }

        public Bill GenerateBill(int orderId)

        {

            var bill = new Bill

            {

                OrderID = orderId,

                BillDate = DateTime.Now,

                BillingStatus = "Generated"

            };

            _billRepository.AddBill(bill);

            _billRepository.Save();

            return bill;

        }

        public void MakePayment(Payment payment)

        {

            payment.PaymentDate = DateTime.Now;

            payment.PaymentStatus = "Success";

            _paymentRepository.AddPayment(payment);

            _paymentRepository.Save();

        }

    }

}
