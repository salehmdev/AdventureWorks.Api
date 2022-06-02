using System.Collections.Generic;

namespace AdventureWorks.Api.Models
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
