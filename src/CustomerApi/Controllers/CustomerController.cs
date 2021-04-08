using CustomerApi.Infra.uow;
using CustomerApi.Model;
using CustomerApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IConfiguration _configuration;

        public CustomerController(
            ILogger<CustomerController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerViewModel customer)
        {
            try
            {
                using (var uow = new UnitOfWork(_configuration.GetConnectionString("LocalDB")))
                {
                    List<string> erros = new List<string>();

                    //TODO: I'II use AutoMapper resolve this;
                    var _customer = new Customer()
                    {
                        Name = customer.Name,
                        EmailAddress = customer.EmailAddress
                    };

                    uow.CustomerRepository.Add(_customer);

                    //TODO: I'II use AutoMapper resolve this;
                    customer.Addresses.ForEach(address =>
                    {

                        if (string.IsNullOrEmpty(address.Street))
                            erros.Add("the street field is required.");
                        else
                            _customer.Addresses.Add(
                                new Address()
                                {
                                    Street = address.Street,
                                    City = address.City,
                                    Complement = address.Complement,
                                    Number = address.Number,
                                    CustomerId = _customer.Id
                                });

                    });

                    _customer.Addresses.ForEach(address => uow.AddressRepository.Add(address));

                    if (erros.Count > 0)
                        uow.Rollback();
                    else
                        uow.Commit();
                }

                return Ok(customer);
            }
            catch (Exception error)
            {
                _logger.LogError(error.GetBaseException().Message, error);
                return BadRequest(error);
            }
        }
    }
}
