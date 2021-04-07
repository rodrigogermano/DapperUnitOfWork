using CustomerApi.Interfaces;
using CustomerApi.Model;
using Dapper;
using System;
using System.Data;

namespace CustomerApi.Infra.Repositories
{
    internal class AddressRepository : RepositoryBase, IAddressRepository
    {

        public AddressRepository(IDbTransaction transaction)
           : base(transaction)
        {
        }

        public void Add(Address entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            entity.Id = Connection.ExecuteScalar<int>(
                "insert into CustomerDB..addresses (Street, Number, Complement, City, CustomerID) values (@Street, @Number, @Complement, @City, @CustomerID); SELECT SCOPE_IDENTITY()",
                param: new { Street = entity.Street , Number = entity.Number, Complement = entity.Complement, City = entity.City, CustomerID = entity.CustomerId},
                transaction: Transaction
            );
        }

    }
}
