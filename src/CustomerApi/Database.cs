using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;

namespace CustomerApi
{
    public static class Database
    {
        public static void StartupDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            using (var connection = new SqlConnection(configuration.GetConnectionString("LocalDB")))
            {
                var databaseName = configuration["DatabaseName"];

                string sqlCreateDatabase = @$"
                    IF EXISTS 
                        (
                            SELECT name FROM master.dbo.sysdatabases 
                        WHERE name = N'{databaseName}'
                        )
                    BEGIN
                        SELECT 'Database Name already Exist' AS Message
                    END
                    ELSE
                    BEGIN
                        CREATE DATABASE [{databaseName}]
                        SELECT 'New Database is Created'
                    END
                ";

                connection.Execute(sqlCreateDatabase);

                var sqlCreateTables = $@"
                IF EXISTS (
	                SELECT TABLE_NAME FROM [{databaseName}].INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = N'customers'
                )
                BEGIN
	                SELECT 'Table Name already Exist' AS Message
                END
                ELSE
                BEGIN
	                SELECT 'New Database is Created'
	                CREATE TABLE [{databaseName}]..[Customers](
		                Id int IDENTITY(1,1) PRIMARY KEY,
		                Name varchar(50) not null,
		                EmailAddress varchar(50) not null
	                )
                END


                IF EXISTS (
	                SELECT TABLE_NAME FROM [{databaseName}].INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = N'Addresses'
                )
                BEGIN
	                SELECT 'Table Name already Exist' AS Message
                END
                ELSE
                BEGIN
	                SELECT 'New Database is Created'
	                CREATE TABLE [{databaseName}]..[Addresses](
		                Id int IDENTITY(1,1) PRIMARY KEY,
		                Street varchar(50) not null,
		                Number int,
		                Complement varchar(50),
		                City varchar(50) not null,
		                CustomerId int not null,
		                FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
	                )
                END
                ";

                connection.Execute(sqlCreateTables);
            }
        }
    }
}
