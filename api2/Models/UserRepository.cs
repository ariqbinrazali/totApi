using api2.Data;
using api2.Models.Dtos;
using api2.Models.IRepository;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace api2.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _db;
        public UserRepository(ApplicationDBContext db)
        {
            this._db = db;
        }
           
            public User Authenticate(UserSignInDto model)
            {
                var user = _db.Users.SingleOrDefault(x => x.Username == model.Username);
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

                if (isValidPassword)
                {
                    //generate Token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var secretBytes = Encoding.UTF8.GetBytes(Utilities.SecretKey);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim(ClaimTypes.Name, user.Username)
                        }),
                        Expires = DateTime.Now.AddMinutes(30),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretBytes), SecurityAlgorithms.HmacSha256)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    user.Token = tokenHandler.WriteToken(token);

                    return user;
                }

                return null;
            }
        }
    }

