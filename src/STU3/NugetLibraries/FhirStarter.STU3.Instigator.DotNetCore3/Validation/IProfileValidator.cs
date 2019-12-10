using Hl7.Fhir.Model;

namespace FhirStarter.STU3.Instigator.DotNetCore3.Validation
{

    public interface IProfileValidator
    {
        OperationOutcome Validate(Resource resource, bool onlyErrors = true, bool threadedValidation = true);
    }
}
