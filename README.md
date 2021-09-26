A .NET Core web app - project.

Used working environment and tools: 
DOT.NET Core 2.0,
Entity Frameowork Core (EF Core),
Visual Studio 2017/2019,
Microsoft SQL Server Management Studio 18

Set up: 
The app connects to Microsoft SQL Server (.\express) by default. To change it, change the connection string file.
To establish connections to your database go to Visual Studio > Package Manager Console and write the command update-database (make sure that the target project is JobSearching).  
When starting the app, make sure that the target project is JobSearching.

Idea of the app:
Users register to the app. The first registered is the admin. The admin creates other user accounts and registers games. All users can sign up for a game, starting with defualt of 100 points. For test purposes, tournaments are hosted where the current user gets random scores for all games they have signed up for.
