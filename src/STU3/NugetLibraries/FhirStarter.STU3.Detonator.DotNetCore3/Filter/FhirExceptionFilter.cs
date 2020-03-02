﻿using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FhirStarter.STU3.Detonator.DotNetCore3.Filter
{
    public class FhirExceptionFilter:IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var operationOutCome = new OperationOutcome { Issue = new List<OperationOutcome.IssueComponent>() };
            var issue = new OperationOutcome.IssueComponent
            {
                Severity = OperationOutcome.IssueSeverity.Fatal,
                Code = OperationOutcome.IssueType.NotFound,
                Details = new CodeableConcept("StandardException", context.Exception.GetType().ToString(), context.Exception.Message),
                Diagnostics = context.Exception.StackTrace
            };

            //if (ShowStackTrace)
            //{
            //    issue.Diagnostics = exception.StackTrace;
            //}

            //var responseIssue = CheckForHttpResponseException(exception, ShowStackTrace);
            //if (responseIssue != null)
            //    operationOutCome.Issue.Add(responseIssue);

            operationOutCome.Issue.Add(issue);
            context.Result = new ObjectResult(operationOutCome);
            context.ExceptionHandled = true;
            
        }
    }
}