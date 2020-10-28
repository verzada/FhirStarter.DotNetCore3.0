# Getting Started with STU3

The very basic way of doing it:

Copy the project "FhirStarter.STU3.Twisted.DotNetCore3" from \src\STU3 into your solution folder. Rename the project and folder (if you'd like) to something appropiate.

You are good to go.

The next level:

Adjusting Starting.cs file to accommodate possible connection strings and such.

## Create a IFhirService with a constructor

Use the IFhirService interface to create a new FHIR service for a FHIR resource.

Take a look at the provided PatientService example for more details.

## Logging

It possible to change from Log4Net to your preferred logging tool. FhirStarter still uses the Microsoft Logging library though, but since the logging is defined in Program.cs it is possible to adjust the logging setting.

# About AppSettings

[About AppSettings](../Shared/AboutAppSettings.md)
