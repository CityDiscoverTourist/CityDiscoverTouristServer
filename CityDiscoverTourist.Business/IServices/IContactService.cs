using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface IContactService
{
    public IQueryable<Contact> GetContactAsync();
    public Task<Contact> Get(int id);
    public Task<Contact> CreateAsync(Contact request);
    public Task<Contact> UpdateAsync(Contact request);
    public Task<Contact> DeleteAsync(int id);
}