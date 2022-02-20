echo "Run script to delete unwanted files"
cd /d "%~dp0"
del Source\IntelligenceReporting.WebApi\bin\Release\net6.0\publish\appsettings.Development.json
del Source\IntelligenceReporting.WebApi\bin\Release\net6.0\publish\appsettings.LocalProd.json
del Source\IntelligenceReporting.WebApi\bin\Release\net6.0\publish\appsettings.LocalStaging.json
del Source\IntelligenceReporting.WebApi\bin\Release\net6.0\publish\appsettings.AwsStaging.json
del Source\IntelligenceReporting.WebApp\bin\Release\net6.0\publish\wwwroot\*.br
del Source\IntelligenceReporting.WebApp\bin\Release\net6.0\publish\wwwroot\*.gz
del Source\IntelligenceReporting.WebApp\bin\Release\net6.0\publish\wwwroot\appsettings.Development.json
del Source\IntelligenceReporting.WebApp\bin\Release\net6.0\publish\wwwroot\*.br
del Source\IntelligenceReporting.WebApp\bin\Release\net6.0\publish\wwwroot\_framework\*.gz
del Source\IntelligenceReporting.WebApp\bin\Release\net6.0\publish\wwwroot\_framework\*.br
echo "Files removed"