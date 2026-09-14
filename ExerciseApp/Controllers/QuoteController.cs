using System;
using System.Collections.Generic;
using ExerciseApp.Model;
using ExerciseApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly QuoteService _quoteService = new QuoteService();
        private readonly IQuoteStorageService _storage;

        public QuoteController(IQuoteStorageService storage)
        {
            _storage = storage;
        }

        [HttpGet]
        public QuoteDetail Get()
        {
            return _quoteService.GetQuoteDetail();
        }

        [HttpGet("saved")]
        public IEnumerable<StoredQuote> GetSaved()
        {
            return _storage.GetAll();
        }

        [HttpPost]
        public QuoteResponse Post(QuoteRequest request)
        {
            var returnObject = new QuoteResponse() { QuoteRequestValid = false };
            if (TryValidateModel(request))
            {
                try
                {
                    returnObject.Quote = _quoteService.PerformQuote(request);
                    returnObject.QuoteRequestValid = true;
                    _storage.Save(new StoredQuote
                    {
                        Make = request.Make,
                        Model = request.Model,
                        InsuranceType = request.InsuranceType!.Value,
                        DateOfBirth = request.DateOfBirth!.Value,
                        Quote = returnObject.Quote
                    });
                }
                catch (InvalidOperationException ex)
                {
                    returnObject.Message = ex.Message;
                }
            }

            return returnObject;
        }
    }
}
