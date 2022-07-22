using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
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
    private readonly IBlobService _blobService;

    public CustomerService(UserManager<ApplicationUser> userManager, IMapper mapper, IBlobService blobService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _blobService = blobService;
    }

    public PageList<CustomerResponseModel> GetAll(CustomerParams @params)
    {
        var customers = _userManager.Users!.AsQueryable();

        Search(ref customers, @params);

        var mappedData = _mapper.Map<IEnumerable<CustomerResponseModel>>(customers);
        return PageList<CustomerResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<CustomerResponseModel> Get(string id)
    {
        var entity = await _userManager.FindByIdAsync(id);
        CheckDataNotNull("Customer", entity);
        return _mapper.Map<CustomerResponseModel>(entity);
    }

    public async Task<CustomerResponseModel> UpdateUser(string id, bool isLock)
    {
        var entity = await _userManager.FindByIdAsync(id);
        CheckDataNotNull("Customer", entity);

        entity.LockoutEnabled = isLock;
        await _userManager.SetLockoutEnabledAsync(entity, isLock);

        return _mapper.Map<CustomerResponseModel>(entity);
    }

    public async Task<CustomerResponseModel> UpdateAsync(CustomerRequestModel request)
    {
        var mappedData = _mapper.Map<ApplicationUser>(request);

        var user = await _userManager.FindByIdAsync(mappedData.Id);

        user.ImagePath = await _blobService.UploadAvatarImgPathAsync(request.Image, user.Id, "customer");
        user.Address = mappedData.Address;
        user.Gender = mappedData.Gender;
        user.PhoneNumber = mappedData.PhoneNumber;
        user.SecurityStamp = mappedData.SecurityStamp;

        var entity = await _userManager.UpdateAsync(user);
        return _mapper.Map<CustomerResponseModel>(user);
    }

    private static void Search(ref IQueryable<ApplicationUser> customers, CustomerParams param)
    {
        if (!customers.Any()) return;

        if (param.Email != null) customers = customers.Where(x => x.Email.Contains(param.Email));

        if (param.IsLock != null) customers = customers.Where(x => x.LockoutEnabled == param.IsLock);
    }
}