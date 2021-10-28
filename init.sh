dotnet tool install -g thinkinghome.migrator.cli
dotnet tool install -g Microsoft.Web.LibraryManager.Cli 


cd src/Migrations
dotnet clean Migrations.csproj
dotnet restore Migrations.csproj
dotnet build Migrations.csproj
migrate-database sqlite "Data Source=../mydb.db" "bin/Debug/net5.0/Migrations.dll"


cd ../
ls
dotnet test Tariff.sln -v n