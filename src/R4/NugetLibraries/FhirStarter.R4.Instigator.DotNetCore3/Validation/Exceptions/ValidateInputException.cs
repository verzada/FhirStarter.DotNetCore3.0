﻿using System;

namespace FhirStarter.R4.Instigator.DotNetCore3.Validation.Exceptions
{
    public class ValidateInputException : ArgumentException
    {
        public ValidateInputException(string message) : base(message)
        {
        }

        public ValidateInputException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
