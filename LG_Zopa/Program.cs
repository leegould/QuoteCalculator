using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LG_Zopa
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: quote.exe <filename> <loan_amount>");
                Console.WriteLine("\te.g. quote.exe market.csv 1000");
                return;
            }

            var csv = File.ReadAllLines(args[0]);
            var market = new List<Lender>();
            foreach (var line in csv.Skip(1)) // Skip header line
            {
                var values = line.Split(',');
                market.Add(new Lender
                {
                    Name = values[0],
                    Rate = decimal.Parse(values[1]),
                    Amount = int.Parse(values[2])
                });
            }
            
            var quoteCalculator = new QuoteCalculator(market);

            try
            {
                var quote = quoteCalculator.FindBestQuote(int.Parse(args[1]));

                if (quote == null)
                {
                    Console.WriteLine("it is not possible to provide a quote at this time!");
                    return;
                }

                Console.WriteLine($"Requested amount: £{quote.RequestedAmount}");
                Console.WriteLine($"Rate: {quote.RatePercent}");
                Console.WriteLine($"Monthly repayment: {quote.MonthlyPayment}");
                Console.WriteLine($"Total repayment: {quote.TotalRepayment}");

            }
            catch (ArgumentException)
            {
                Console.WriteLine("Loan amounts must be between 1000 and 15000 inclusive, and in multiples of 100.");
            }
        }
    }
}
