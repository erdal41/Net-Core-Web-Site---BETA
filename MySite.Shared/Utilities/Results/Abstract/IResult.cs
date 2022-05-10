using MySite.Shared.Entities.Concrete;
using MySite.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;

namespace MySite.Shared.Utilities.Results.Abstract
{
    public interface IResult
    {
        public ResultStatus ResultStatus { get; }
        public string Message { get; }
        public Exception Exception { get; }
        public IEnumerable<ValidationError> ValidationErrors { get; set; }
    }
}