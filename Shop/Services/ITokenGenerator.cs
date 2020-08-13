using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shop.Models;
using System.Threading.Tasks;

namespace Shop.Services
{
    public interface ITokenGenerator
    {
        public Task<string> GenerateTokenAsync(User user, IConfiguration config, UserManager<User> userManager);
    }
}
