using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class ContactService: IContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository experienceRepository)
    {
        _contactRepository = experienceRepository;
    }

    public IQueryable<Contact> GetContactAsync()
    {
        var contact = _contactRepository.GetAll();
        return contact;
    }

    public async Task<Contact> Get(int id)
    {
        var entity = await _contactRepository.Get(id);

        return entity;
    }

    public async Task<Contact> CreateAsync(Contact request)
    {
        var entity = await _contactRepository.Add(request);
        return entity;
    }

    public async Task<Contact> UpdateAsync(Contact request)
    {
        var entity = await _contactRepository.Update(request);
        return entity;
    }

    public async Task<Contact> DeleteAsync(int id)
    {
        var entity = await _contactRepository.Delete(id);
        return entity;
    }
}