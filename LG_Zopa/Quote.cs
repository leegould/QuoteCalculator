namespace LG_Zopa
{
    public class Quote
    {
        public int RequestedAmount { get; set; }
        public decimal RatePercent { get; set; }

        public decimal MonthlyPayment { get; set; }

        public decimal TotalRepayment { get; set; }
    }
}
