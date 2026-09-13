using System;
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
        public QuoteController()
        {

        }

        [HttpGet]
        public QuoteDetail Get()
        {
            return _quoteService.GetQuoteDetail();
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
