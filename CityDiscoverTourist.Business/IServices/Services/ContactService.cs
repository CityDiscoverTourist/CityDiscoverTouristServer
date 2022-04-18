using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Task = CityDiscoverTourist.Data.Models.Task;

namespace CityDiscoverTourist.Business.IServices.Services;

public class ContactService: IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;

    public ContactService(IContactRepository experienceRepository, IMapper mapper)
    {
        _contactRepository = experienceRepository;
        _mapper = mapper;
    }

    public async Task<Contact> Get(int id)
    {
        var entity = await _contactRepository.Get(id);

        //var shaped = _dataShaper.ShapeData(entity, fields);

        return _mapper.Map<Contact>(entity);
    }

    public async Task<Contact> CreateAsync(Contact request)
    {
        var entity = _mapper.Map<Contact>(request);
        entity = await _contactRepository.Add(entity);
        return _mapper.Map<Contact>(entity);
    }

    public async Task<Contact> UpdateAsync(Contact request)
    {
        var entity = _mapper.Map<Contact>(request);
        entity = await _contactRepository.Update(entity);
        return _mapper.Map<Contact>(entity);
    }

    public async Task<Contact> DeleteAsync(int id)
    {
        var entity = await _contactRepository.Delete(id);
        return _mapper.Map<Contact>(entity);
    }
}