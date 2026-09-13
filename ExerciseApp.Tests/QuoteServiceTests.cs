using System;
using ExerciseApp.Model;
using ExerciseApp.Service;
using Xunit;

namespace ExerciseApp.Tests
{
    public class QuoteServiceTests
    {
        private readonly QuoteService _qs = new QuoteService();

        // ── Original test ────────────────────────────────────────────────────

        [Fact]
        public void WhenDetailsAreProvided_AQuoteIsProduced()
        {
            var result = _qs.PerformQuote(new QuoteRequest
            {
                DateOfBirth = DateTime.Today.AddYears(-30),
                InsuranceType = InsuranceType.FullyComprehensive,
                Make = "Ford",
                Model = "Focus"
            });
            Assert.Equal(200, result);
        }

        // ── Age boundary tests ───────────────────────────────────────────────

        [Fact]
        public void WhenApplicantIsExactly17_AQuoteIsProduced()
        {
            var result = _qs.PerformQuote(new QuoteRequest
            {
                DateOfBirth = DateTime.Today.AddYears(-17),
                InsuranceType = InsuranceType.FullyComprehensive,
                Make = "Ford",
                Model = "Focus"
            });
            Assert.Equal(200, result);
        }

        [Fact]
        public void WhenApplicantIsExactly80_AQuoteIsProduced()
        {
            var result = _qs.PerformQuote(new QuoteRequest
            {
                DateOfBirth = DateTime.Today.AddYears(-80),
                InsuranceType = InsuranceType.FullyComprehensive,
                Make = "Ford",
                Model = "Focus"
            });
            Assert.Equal(200, result);
        }

        [Fact]
        public void WhenApplicantIsUnder17_AnExceptionIsThrown()
        {
            // AddDays(1) means they haven't had their 17th birthday yet
            var dob = DateTime.Today.AddYears(-17).AddDays(1);
            Assert.Throws<InvalidOperationException>(() =>
                _qs.PerformQuote(new QuoteRequest
                {
                    DateOfBirth = dob,
                    InsuranceType = InsuranceType.FullyComprehensive,
                    Make = "Ford",
                    Model = "Focus"
                }));
        }

        [Fact]
        public void WhenApplicantIsOver80_AnExceptionIsThrown()
        {
            var dob = DateTime.Today.AddYears(-81);
            Assert.Throws<InvalidOperationException>(() =>
                _qs.PerformQuote(new QuoteRequest
                {
                    DateOfBirth = dob,
                    InsuranceType = InsuranceType.FullyComprehensive,
                    Make = "Ford",
                    Model = "Focus"
                }));
        }

        // ── Pricing rule tests ───────────────────────────────────────────────

        [Fact]
        public void BmwX5FullyComprehensive_Returns500()
        {
            var result = _qs.PerformQuote(new QuoteRequest
            {
                DateOfBirth = DateTime.Today.AddYears(-30),
                InsuranceType = InsuranceType.FullyComprehensive,
                Make = "BMW",
                Model = "X5"
            });
            Assert.Equal(500, result);
        }

        [Fact]
        public void BmwOtherModelFullyComprehensive_Returns400()
        {
            var result = _qs.PerformQuote(new QuoteRequest
            {
                DateOfBirth = DateTime.Today.AddYears(-30),
                InsuranceType = InsuranceType.FullyComprehensive,
                Make = "BMW",
                Model = "3 Series"
            });
            Assert.Equal(400, result);
        }

        [Fact]
        public void BmwX5ThirdPartyFireAndTheft_Returns510()
        {
            var result = _qs.PerformQuote(new QuoteRequest
            {
                DateOfBirth = DateTime.Today.AddYears(-30),
                InsuranceType = InsuranceType.ThirdPartyFireAndTheft,
                Make = "BMW",
                Model = "X5"
            });
            Assert.Equal(510, result);
        }

        [Fact]
        public void AudiThirdPartyOnly_Returns250()
        {
            var result = _qs.PerformQuote(new QuoteRequest
            {
                DateOfBirth = DateTime.Today.AddYears(-30),
                InsuranceType = InsuranceType.ThirdPartyOnly,
                Make = "Audi",
                Model = "A3"
            });
            Assert.Equal(250, result);
        }

        [Fact]
        public void FordThirdPartyOnly_Returns180()
        {
            var result = _qs.PerformQuote(new QuoteRequest
            {
                DateOfBirth = DateTime.Today.AddYears(-30),
                InsuranceType = InsuranceType.ThirdPartyOnly,
                Make = "Ford",
                Model = "Fiesta"
            });
            Assert.Equal(180, result);
        }
    }
}
