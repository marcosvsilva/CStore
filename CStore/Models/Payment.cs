using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CStore.Models
{
    public enum PaymentTypeList
    {
        Checks,
        DebitCard,
        CreditCards,
        MobilePayments,
        ElectronicBankTransfers
    }

    public enum PaymentStatusList
    {
        Pending,
        AwaitingPayment,
        Completed,
        Cancelled,
        Declined,
        Refunded
    }

    [Table("Payments")]
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public double Amount { get; set; }

        public string Provider { get; set; }

        public PaymentTypeList PaymentType { get; set; }

        public PaymentStatusList PaymentStatus { get; set; }
    }
}
