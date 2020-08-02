using Microsoft.Extensions.Configuration;
using Shop.Models;

namespace Shop.Services
{
    public interface ITokenGenerator
    {
        public string GenerateToken(User user, IConfiguration config);
    }
}
