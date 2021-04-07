using CustomerApi.Interfaces;
using CustomerApi.Model;
using Dapper;
using System;
using System.Data;

namespace CustomerApi.Infra.Repositories
{
    internal class CustomerRepository : RepositoryBase, ICustomerRepository
    {

        public CustomerRepository(IDbTransaction transaction)
           : base(transaction)
        {
        }

        public void Add(Customer entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            entity.Id = Connection.ExecuteScalar<int>(
                "insert into CustomerDB..Customers (name, emailaddress) values (@Name, @EmailAddress); SELECT SCOPE_IDENTITY()",
                param: new { Name = entity.Name, EmailAddress = entity.EmailAddress },
                transaction: Transaction
            );
        }

    }
}
