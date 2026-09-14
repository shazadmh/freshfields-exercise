using System.Collections.Generic;
using ExerciseApp.Model;

namespace ExerciseApp.Service
{
    public class InMemoryQuoteStorageService : IQuoteStorageService
    {
        private readonly List<StoredQuote> _store = new List<StoredQuote>();

        public void Save(StoredQuote quote) => _store.Add(quote);

        public IEnumerable<StoredQuote> GetAll() => _store.AsReadOnly();
    }
}
