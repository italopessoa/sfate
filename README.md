# Support Wheel of Fate - API

A REST API that receives a request and should select two engineers at random to both complete a half day of support each. The API works by itself, you can do all the operations and test the results on your browser.

[![Build status](https://ci.appveyor.com/api/projects/status/517lk04q410q7kyf?svg=true)](https://ci.appveyor.com/project/italopessoa/support-wheel-of-fate)
# [Check it out ](http://swfbau.azurewebsites.net/swagger)

## Controllers
### ShiftController 
- Create and list engineer turns

### AuthController
- Authenticate users using [JWT](https://jwt.io), even though there is no solid user management implementation, token based authentication works just fine, and will make easier to integrate with other clients.

### ValuesController
- Used to list some application settings

## Settings
```json
    "Jwt": {
        "Issuer": "http://localhost:5000",
        "Audience": "http://localhost:5000",
        "Key": "veryVerySecretKey",
        "LifeTimeInMinutes":"30"
    },
    "App":{
        "MAX_SHIFT_SUM_HOURS_DURATION": "8",
        "WEEK_SCAN_PERIOD" : "1",
        "SHIFT_DURATION" : "4"
    }
```
- MAX_SHIFT_SUM_HOURS_DURATION: 
    - maximum amount of shifts (in hours) a engineer can do on in a period of N weeks
- WEEK_SCAN_PERIOD
    - set how many weeks to take in count to filter engineers
    - e.g 1: the current week and the previous one, therefore 2 weeks
 - SHIFT_DURATION
    - how long (in hours) lasts a shift
    
`To avoid creating a new table with only two values that will barely change, I decided to use environment variables to store them.`

## Continuous integrations & Continuous Delivery
- AppVeyor
- Windows Azure App Service

```yaml
version: 1.0.{build}
branches:
  only:
  - master
image: Visual Studio 2017
configuration: Release
platform: Any CPU
dotnet_csproj:
  patch: true
  file: '**\*.csproj'
  version: '{version}'
  package_version: '{version}'
  assembly_version: '{version}'
  file_version: '{version}'
  informational_version: '{version}'
build_script:
- cmd: >-
    dotnet build BAU.Api

    dotnet publish -c Release BAU.Api
test_script:
- cmd: dotnet test .\BAU.Test\
artifacts:
- path: '\BAU.Api\bin\Any CPU\Release\netcoreapp2.0\publish'
  name: WebApi
  type: WebDeployPackage
deploy:
- provider: Environment
  name: BAU-production
```
## Architecture

Using a simple Controller-> Service -> Repository implementation the code is organized in folders just for simplicity and avoid configuration errors on the on the beginning of developmento. 
- Repositories are in the folder DAL (Data Access Layer)
- Services are in the folder Service
- Since almost everything is covered by tests, in the future, those parts should be put in separated `Class Libraries`
- create  Startup class diferent files of to hold different settings

## Packages
- Microsoft.EntityFrameworkCore
- Swashbuckle.AspNetCore
- AutoMapper

## Running
Set `SqlServer` connections string value or use Entity Framework in Memory (Startup.cs).

Execute

```cmd
dotnet restore
cd BAU.Api
dotnet run
```
When access the swagger interface use the following credentials to authenticate:
```json
{
  "username": "support",
  "password": "support"
}
```
## DRAFTS
```sql
--engenheiros sem turno e sem turno consecutivo
DECLARE @TODAY DATE = CONVERT(DATE, GETDATE())
DECLARE @TOMORROW DATE = CONVERT(DATE, DATEADD(DAY,1,GETDATE()))
DECLARE @YESTERDAY DATE = CONVERT(DATE, DATEADD(DAY,-1,GETDATE()))
DECLARE @MAX_SHIFT_HOURS_ALLOWED INT = 8
SELECT eg.id,EG.name
FROM 
	ENGINEER EG 
	LEFT JOIN ENGINEERSHIFT EGS 
		ON EG.ID = EGS.ENGINEERID
WHERE 
	(EGS.SHIFTDATE IS NOT NULL AND EGS.SHIFTDATE BETWEEN DATEADD(DAY, -13, @TODAY) AND @TODAY)
	OR (
		EGS.SHIFTDATE IS NULL --AINDA NAO POSSUEM TURNO
		OR (
			EGS.SHIFTDATE <> @YESTERDAY 
			AND EGS.SHIFTDATE <> @TOMORROW
			AND EGS.SHIFTDATE <> @TODAY
			) --NAO POSSUI TURNO ONTEM, HOJE OU AMANHA
		)
GROUP BY 
	eg.id,EG.name
HAVING 
	SUM(EGS.DURATION) < @MAX_SHIFT_HOURS_ALLOWED OR SUM(EGS.DURATION) IS NULL
``` 
