using api2.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api2.Models.IRepository
{
    public interface IUserRepository
    {
        User Authenticate(UserSignInDto model);
    }
}
