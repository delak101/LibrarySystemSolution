namespace LibrarySystemApp.Mappings;

using Mapster;
using DTOs;
using Models;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<User, UserDto>.NewConfig();
        TypeAdapterConfig<RegisterDto, User>.NewConfig()
            .Ignore(dest => dest.PasswordHash); // PasswordHash should be set separately
        TypeAdapterConfig<UpdateUserDto, User>.NewConfig()
            .Ignore(dest => dest.PasswordHash); // PasswordHash should be set separately
    }
}
