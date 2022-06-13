using AutoMapper;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CustomerService : BaseService, ICustomerService
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerService(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public PageList<CustomerResponseModel> GetAll(CustomerParams @params)
    {
        var customers = _userManager.Users!.AsQueryable();
        var mappedData = _mapper.Map<IEnumerable<CustomerResponseModel>>(customers);
        return PageList<CustomerResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<CustomerResponseModel> Get(string id)
    {
        var entity = await _userManager.FindByIdAsync(id);
        CheckDataNotNull("Customer", entity);
        return _mapper.Map<CustomerResponseModel>(entity);
    }

    public Task<CustomerResponseModel> CreateAsync(ApplicationUser request)
    {
        throw new NotImplementedException();
    }

    public async Task<CustomerResponseModel> UpdateAsync(ApplicationUser request)
    {
        var entity = await _userManager.UpdateAsync(request);
        return _mapper.Map<CustomerResponseModel>(entity);
    }

    public Task<CustomerResponseModel> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}