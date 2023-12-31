using Application.DaoInterface;
using Domain.Models;

namespace GrpcClient.Mappers;

public class PetMapper
{
    private readonly IUserDao userService;

    public PetMapper(IUserDao userService)
    {
        this.userService = userService;
    }

    public async Task<Pet> MapToEntity(PetProto proto)
    {
        return new Pet()
        {
            Id = proto.Id,
            Description = proto.Description,
            IsVaccinated = proto.IsVaccinated,
            Weight = proto.Weight,
            PetName = proto.PetName,
            PetType = Enum.Parse<PetType.Type>(proto.PetType),
            PetOwner = (PetOwner) await userService.GetByEmailAsync(proto.OwnerEmail)
        };
    }
    
    public async Task<IEnumerable<Pet>> MapToEntityList(PetsProto proto)
    {
        return await Task.WhenAll(proto.Pets
            .Select(async pet => await MapToEntity(pet)));
    }
    
    public async Task<PetProto> MapToProto(Pet pet)
    {
        return new PetProto()
        {
            Id = pet.Id,
            Description = pet.Description,
            IsVaccinated = pet.IsVaccinated,
            Weight = pet.Weight,
            PetName = pet.PetName,
            PetType = pet.PetType.ToString(),
            OwnerEmail = pet.PetOwner.Email
        };
    }
}