using System;

namespace CustomerApi.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository CustomerRepository { get; }
        IAddressRepository AddressRepository { get; }
        void Commit();
        void Rollback();
    }
}
