using System.IdentityModel.Tokens.Jwt;

namespace CommVill.DAL.Interface
{
    public interface IAuthRepository
    {
        Task<JwtSecurityToken> GenerateJWTToken(string email);
    }
}
