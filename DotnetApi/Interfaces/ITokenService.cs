using DotnetApi.Models;

namespace DotnetApi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}