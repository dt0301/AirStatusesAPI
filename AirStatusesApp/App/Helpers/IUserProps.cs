using AirStatusesData.Services.Dto;

namespace AirStatusesApp.App.Helpers
{
    public interface IUserProps
    {
        UserDto GetUserProps();
        Task<UserDto> GetUserPropsAsync();
        bool IsWriter(UserDto userDto);
        bool IsAdmin(UserDto userDto);
    }
}