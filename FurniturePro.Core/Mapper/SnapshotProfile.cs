using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Models.DTO.Snapshots;

namespace FurniturePro.Core.Mapper;

public class SnapshotProfile : Profile
{
    public SnapshotProfile()
    {
        CreateMap<Snapshot, SnapshotDTO>();
        CreateMap<CreateSnapshotDTO, Snapshot>();
        CreateMap<UpdateSnapshotDTO, Snapshot>();
    }
}
