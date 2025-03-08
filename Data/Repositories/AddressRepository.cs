using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class AddressRepository(DataContext context) : BaseRepository<AddressEntity>(context), IAddressRepository
    {
        // Fake method
        public void FakeMethod()
        {
            // Do nothing
        }
    }
}
