# Forum_Platform
Go TO Develop branch
The project is made for Onnorokom Software Limited

Configuration:

You need to create database first and then change appsettings.json file with connection string and smtp configuration.
No need to create table. You have to apply migrations.
dotnet ef database update --project Forum.Web --context ApplicationDbContext
dotnet ef database update --project Forum.Web --context SystemDbContext
Defaul Moderator email: moderator@Forum.com and password: moderator@Forum.com
