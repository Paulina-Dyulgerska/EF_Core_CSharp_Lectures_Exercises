using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DTOs.User
{
   public class UserWithSoldProductsDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<UserSoldProductDTO> SoldProducts { get; set; }
    }
}
