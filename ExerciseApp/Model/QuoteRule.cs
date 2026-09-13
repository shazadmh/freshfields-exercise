using System;

namespace ExerciseApp.Model
{
    public record QuoteRule(Func<QuoteRequest, bool> Matches, decimal Price);
}
