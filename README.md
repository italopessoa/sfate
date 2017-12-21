# support-wheel-of-fate
[![Build status](https://ci.appveyor.com/api/projects/status/517lk04q410q7kyf?svg=true)](https://ci.appveyor.com/project/italopessoa/support-wheel-of-fate)
# [Check it out ](http://swfbau.azurewebsites.net/swagger)
## Controllers
### ShiftController 
used to manage engineers turns find engineers save turns

### AuthController
- use JWT to authenticate user, even though there is no user management implementation, token based authentication works just fine, and will make easier integrate with other clients.

### ValuesController

## Settings
```json
    "Shift": {
        "MAX_SHIFT_SUM_HOURS_DURATION": "8",
        "WEEK_SCAN_PERIOD" : "1",
        "SHIFT_DURATION" : "4"
    }
```
- MAX_SHIFT_SUM_HOURS_DURATION: 
    - description
- WEEK_SCAN_PERIOD
    - description
 - SHIFT_DURATION
    - description
### SHIFT_DURATION, MAX_SHIFT_SUM_HOURS_DURATION, WEEK_SCAN_PERIOD
used to configure how much time takes each turn and the maximum amount of time an engineer can have in his N weeks period. To avoid creating a new table with only two values that will barely change, I decided to use environment variables to store them.

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
### CI/CD 

- DAL is just a folder not a class library
- create  Startup class diferent files of to hold different settings




### Architecture

there is only two layer on the project, DAL (Data Access Layer) and Controllers. As the project still in development and does not have many features create more layers like a Service or Facade could lead to needless complexity

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
