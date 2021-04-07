using System.Collections.Generic;

namespace CustomerApi.ViewModel
{
    public class CustomerViewModel
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public List<AddressViewModel> Addresses { get; set; }
    }
}
