# IntelligenceReporting

## Dev setup
* Install Visual Studio 2022 https://visualstudio.microsoft.com/vs/
* Install SQL Server > 2019 (Express will do) pref. Developer https://www.microsoft.com/en-us/sql-server/sql-server-downloads
* Install SQL Server Management Studio (to query and manage the DB) https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15

* Open C:\repos\IntelligenceReporting\Source\IntelligenceReporting.sln in Visual Studio
* Right-click on IntelligenceReporting.Database\localhost.publish.xml and publish to create the database
* Right-click the solution and Set Startup Projects: WebApi & WebApp
* Run the solution
* Execute the sync from WebApi's swagger to initialize your database from Vault staging
* Play away

## Sync from Vault production
* Right-click on IntelligenceReporting.Database\Local Prod.publish.xml and publish to create the local prod-sourced database
* Follow the instructions in the WebApi's appsettings.config to set up the connection to Vault's prod replica MySql database
* Work as per the dev setup instructions above.  It takes over an hour to sync the database.  
  If this falls over, just restart it and it will pick up from where it got up to (sale lives are added in order of modify date)
