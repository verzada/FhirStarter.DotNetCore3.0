# How AppSettings works

The FhirStarterSettings needs to be configured according to the version of FHIR that is being used.

Note the difference between STU3 and R4

## STU3
``` json
"FhirStarterSettings": {
    "FhirServiceAssemblies": [ "FhirStarter.STU3.Twisted.DotNetCore3" ],
    "FhirDetonatorAssembly": "FhirStarter.STU3.Detonator.DotNetCore3",
    "FhirInstigatorAssembly": "FhirStarter.STU3.Instigator.DotNetCore3",
    "MockupEnabled": "false",
    "EnableValidation": "true",
    "LogRequestWhenError": "false",
    "FhirPublisher": "ACME",
    "FhirDescription": "ACME delivers the best services"
}
```

## R4

``` json
"FhirStarterSettings": {
    "FhirServiceAssemblies": [ "FhirStarter.R4.Twisted.DotNetCore3" ],
    "FhirDetonatorAssembly": "FhirStarter.R4.Detonator.DotNetCore3",
    "FhirInstigatorAssembly": "FhirStarter.R4.Instigator.DotNetCore3",
    "MockupEnabled": "false",
    "EnableValidation": "true",
    "LogRequestWhenError": "false",
    "FhirPublisher": "ACME",
    "FhirDescription": "ACME delivers the best services"
}
```

Except for the assemblies, the difference is not that huge.
The reason why the assemblies needs to be configured, is due to the nature of loading assemblies in .Net. If the assemblies are not specified, all of the assemblies in the application folder will be loaded into memory. We only need three types of assemblies:

* The assembly/assemblies which contains the FHIR Service(s)
* The Detonator assembly
* The Instigator assembly

The FhirService assembly is usually the only one that needs adjustment.

## Other Settings

### MockupEnabled
Is a value that can be used to enable services that has been created with the IFhirServiceMockup interface. Or other types of mockup directly in your IFhirService.

### EnableValidation
Enables validation of the Fhir Resource

### LogRequestWhenError
..

### FhirPublisher
Output for metadata

### FhirDescription 
Output for metadata