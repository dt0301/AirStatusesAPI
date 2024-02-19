using AirStatusesDomain;

namespace AirStatusesInfrastructure.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(User user);
    }
}