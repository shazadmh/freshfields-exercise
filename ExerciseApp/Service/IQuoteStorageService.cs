using System.Collections.Generic;
using ExerciseApp.Model;

namespace ExerciseApp.Service
{
    public interface IQuoteStorageService
    {
        void Save(StoredQuote quote);
        IEnumerable<StoredQuote> GetAll();
    }
}
