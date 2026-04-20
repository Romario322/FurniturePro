using AutoMapper;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Clients;
using FurniturePro.Core.Repositories;
using UchetCartridge.Core.Services.Interfaces;

namespace UchetCartridge.Core.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    private readonly IMapper _mapper;

    public ClientService(IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    public async Task<List<ClientDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _clientRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<ClientDTO>>(entities);
        return models;
    }

    public async Task<ClientDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _clientRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<ClientDTO>(entity);
        return model;
    }

    public async Task<List<ClientDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default)
    {
        var entities = await _clientRepository.GetAfterDateAsync(dateTime, ct);

        var models = _mapper.Map<List<ClientDTO>>(entities);
        return models;
    }

    public async Task<int> CreateAsync(CreateClientDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<Client>(modelDto);
        var createdEntity = await _clientRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task UpdateAsync(int id, UpdateClientDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _clientRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _clientRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _clientRepository.DeleteByIdAsync(id, ct);
}