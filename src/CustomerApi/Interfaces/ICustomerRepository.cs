using CustomerApi.Model;
using System.Collections.Generic;

namespace CustomerApi.Interfaces
{
    public interface ICustomerRepository
    {
        void Add(Customer entity);
        //IEnumerable<Customer> All();
        //void Delete(int id);
        //void Delete(Customer entity);
        //Customer Find(int id);
        //Customer FindByName(string name);
        //void Update(Customer entity);
    }
}
