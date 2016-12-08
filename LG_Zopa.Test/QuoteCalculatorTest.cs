using System;
using System.Collections.Generic;
using Xunit;

namespace LG_Zopa.Test
{
    public class QuoteCalculatorTest
    {
        [Fact]
        public void FindBestQuote_LoanAmountNotIncrementOfOneHundred_ThrowsException()
        {
            // Arrange
            var market = new List<Lender>();
            var rateCalculator = new QuoteCalculator(market);

            // Act
            int loanAmount = 1020;

            // Assert
            Assert.Throws<ArgumentException>(() => rateCalculator.FindBestQuote(loanAmount));
        }

        [Fact]
        public void FindBestQuote_LoanAmountTooSmall_ThrowsException()
        {
            // Arrange
            var market = new List<Lender>();
            var rateCalculator = new QuoteCalculator(market);

            // Act
            int loanAmount = 900;

            // Assert
            Assert.Throws<ArgumentException>(() => rateCalculator.FindBestQuote(loanAmount));
        }

        [Fact]
        public void FindBestQuote_LoanAmountTooLarge_ThrowsException()
        {
            // Arrange
            var market = new List<Lender>();
            var rateCalculator = new QuoteCalculator(market);

            // Act
            int loanAmount = 15100;

            // Assert
            Assert.Throws<ArgumentException>(() => rateCalculator.FindBestQuote(loanAmount));
        }

        [Fact]
        public void FindBestQuote_NotSufficientLenders_ReturnsNull()
        {
            // Arrange
            var market = new List<Lender>
            {
                new Lender { Amount = 100, Name = "TestLender1", Rate = 0.07M }
            };
            var rateCalculator = new QuoteCalculator(market);

            // Act
            int loanAmount = 1000;
            var result = rateCalculator.FindBestQuote(loanAmount);

            // Assert
            Assert.Equal(null, result);
        }

        [Fact]
        public void FindBestQuote_SingleLender_ReturnsRate()
        {
            // Arrange
            var market = new List<Lender>
            {
                new Lender { Amount = 1000, Name = "TestLender1", Rate = 0.07M }
            };
            var rateCalculator = new QuoteCalculator(market);

            // Act
            int loanAmount = 1000;
            var result = rateCalculator.FindBestQuote(loanAmount);

            // Assert
            var expected = new Quote
            {
                RequestedAmount = 1000,
                TotalRepayment = 1111.58M,
                MonthlyPayment = 30.88M,
                RatePercent = 7.0M
            };
            AssertAllPropertiesEqual(expected, result);
        }

        [Fact]
        public void FindBestQuote_MultipleLenders_ReturnsRate()
        {
            // Arrange
            var market = new List<Lender>
            {
                new Lender { Amount = 400, Name = "TestLender1", Rate = 0.06M },
                new Lender { Amount = 400, Name = "TestLender2", Rate = 0.07M },
                new Lender { Amount = 400, Name = "TestLender3", Rate = 0.08M }
            };
            var rateCalculator = new QuoteCalculator(market);

            // Act
            int loanAmount = 1200;
            var result = rateCalculator.FindBestQuote(loanAmount);

            // Assert
            var expected = new Quote
            {
                RequestedAmount = 1200,
                TotalRepayment = 1333.89M,
                MonthlyPayment = 37.05M,
                RatePercent = 7.0M
            };
            AssertAllPropertiesEqual(expected, result);
        }

        [Fact]
        public void FindBestQuote_MultipleLendersDifferentRatios_ReturnsRate()
        {
            // Arrange
            var market = new List<Lender>
            {
                new Lender { Amount = 200, Name = "TestLender1", Rate = 0.06M },
                new Lender { Amount = 200, Name = "TestLender2", Rate = 0.07M },
                new Lender { Amount = 600, Name = "TestLender3", Rate = 0.08M }
            };
            var rateCalculator = new QuoteCalculator(market);

            // Act
            int loanAmount = 1000;
            var result = rateCalculator.FindBestQuote(loanAmount);

            // Assert
            var expected = new Quote
            {
                RequestedAmount = 1000,
                TotalRepayment = 1118.17M,
                MonthlyPayment = 31.06M,
                RatePercent = 7.4M
            };
            AssertAllPropertiesEqual(expected, result);
        }

        private void AssertAllPropertiesEqual(Quote expected, Quote actual)
        {
            Assert.Equal(expected.RequestedAmount, actual.RequestedAmount);
			Assert.Equal(expected.MonthlyPayment, actual.MonthlyPayment);
			Assert.Equal(expected.RatePercent, actual.RatePercent);
			Assert.Equal(expected.TotalRepayment, actual.TotalRepayment);
        }
    }
}
