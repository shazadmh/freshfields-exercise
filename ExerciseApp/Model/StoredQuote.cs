using System;

namespace ExerciseApp.Model
{
    public class StoredQuote
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Make { get; set; }
        public string Model { get; set; }
        public InsuranceType InsuranceType { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal Quote { get; set; }
    }
}
