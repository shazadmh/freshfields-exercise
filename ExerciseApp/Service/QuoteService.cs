using System;
using System.Collections.Generic;
using System.Linq;
using ExerciseApp.Model;

namespace ExerciseApp.Service
{
    public class QuoteService
    {
        private static readonly List<QuoteRule> _rules = new()
        {
            new(r => r.InsuranceType == InsuranceType.FullyComprehensive && r.Make == "Ford", 200),
            new(r => r.InsuranceType == InsuranceType.FullyComprehensive && r.Make == "BMW" && r.Model == "X5", 500),
            new(r => r.InsuranceType == InsuranceType.FullyComprehensive && r.Make == "BMW", 400),
            new(r => r.InsuranceType == InsuranceType.FullyComprehensive, 300),
            new(r => r.InsuranceType == InsuranceType.ThirdPartyFireAndTheft && r.Make == "Ford", 180),
            new(r => r.InsuranceType == InsuranceType.ThirdPartyFireAndTheft && r.Make == "BMW" && r.Model == "X5", 510),
            new(r => r.InsuranceType == InsuranceType.ThirdPartyFireAndTheft && r.Make == "BMW", 400),
            new(r => r.InsuranceType == InsuranceType.ThirdPartyFireAndTheft, 300),
            new(r => r.InsuranceType == InsuranceType.ThirdPartyOnly && r.Make == "Ford", 180),
            new(r => r.InsuranceType == InsuranceType.ThirdPartyOnly && r.Make == "Audi", 250),
            new(r => r.InsuranceType == InsuranceType.ThirdPartyOnly, 300),
        };

        public QuoteDetail GetQuoteDetail()
        {
            var quoteDetail = new QuoteDetail();

            quoteDetail.Makes.Add("Ford");
            quoteDetail.Makes.Add("Audi");
            quoteDetail.Makes.Add("BMW");

            var modelSpec = new ModelSpec { Make = "Ford" };
            modelSpec.Models.AddRange(new[] { "Fiesta", "Focus", "Puma", "S Max" });
            quoteDetail.Models.Add(modelSpec);

            modelSpec = new ModelSpec { Make = "Audi" };
            modelSpec.Models.AddRange(new[] { "A3", "A4", "A5" });
            quoteDetail.Models.Add(modelSpec);

            modelSpec = new ModelSpec { Make = "BMW" };
            modelSpec.Models.AddRange(new[] { "X5", "3 Series", "5 Series" });
            quoteDetail.Models.Add(modelSpec);

            return quoteDetail;
        }

        public decimal PerformQuote(QuoteRequest request)
        {
            ValidateAge(request.DateOfBirth!.Value);
            var rule = _rules.First(r => r.Matches(request));
            return rule.Price;
        }

        private static void ValidateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;

            if (age < 17 || age > 80)
                throw new InvalidOperationException(
                    $"Applicants must be between 17 and 80 years old. Age provided: {age}.");
        }
    }
}
