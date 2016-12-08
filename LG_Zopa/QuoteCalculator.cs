using System;
using System.Collections.Generic;
using System.Linq;

namespace LG_Zopa
{   
    public class QuoteCalculator
    {
        private const int TotalNumberOfPayments = 36;
        private const int MinimumLoanAmount = 1000;
        private const int MaximumLoanAmount = 15000;

        private readonly IList<Lender> market;

        public QuoteCalculator(IList<Lender> market)
        {
            this.market = market;
        }

        /// <summary>
        /// Find a Quote calculation
        /// </summary>
        /// <param name="loanAmount"></param>
        /// <returns>The rate</returns>
        /// <remarks>
        /// using formula:
        /// MonthlyPayment = Principal * (EffectiveInterestRate / (1 - (1 + EffectiveInterestRate) ^-totalNumberOfPayments) )
        /// </remarks>
        public Quote FindBestQuote(int loanAmount)
        {
            if (loanAmount < MinimumLoanAmount || loanAmount > MaximumLoanAmount || loanAmount % 100 > 0)
            {
                throw new ArgumentException();
            }

            if (market.Sum(x => x.Amount) < loanAmount)
            {
                return null;
            }

            decimal averageRate = 0;
            var amountNeeded = loanAmount;
            foreach (var lender in market.OrderBy(x => x.Rate))
            {
                var lentAmount = lender.Amount;
                if (amountNeeded < lender.Amount)
                {
                    lentAmount = amountNeeded;
                }

                averageRate += lender.Rate*((decimal) lentAmount/loanAmount);
                
                amountNeeded -= lentAmount;
            }

            var effectiveInterestRate = GetEffectiveInterestRate(averageRate);
            
            var monthlyPayments = loanAmount *
                                  (effectiveInterestRate /
                                   (1 - (decimal) Math.Pow((double) (1 + effectiveInterestRate), -TotalNumberOfPayments)));

            return new Quote
            {
                RequestedAmount = loanAmount,
                RatePercent = decimal.Round(averageRate * 100, 1),
                MonthlyPayment = decimal.Round(monthlyPayments, 2),
                TotalRepayment = decimal.Round(monthlyPayments * TotalNumberOfPayments, 2)
            };
        }

        private static decimal GetEffectiveInterestRate(decimal rate)
        {
            return rate/12;
        }
    }
}
