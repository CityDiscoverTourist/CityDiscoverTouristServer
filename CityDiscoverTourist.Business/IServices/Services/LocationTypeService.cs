using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace CityDiscoverTourist.Business.IServices.Services;

public class LocationTypeService : BaseService, ILocationTypeService
{
    private readonly ILocationTypeRepository _locationTypeRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<LocationType> _sortHelper;

    public LocationTypeService(ILocationTypeRepository locationTypeRepository, IMapper mapper,
        ISortHelper<LocationType> sortHelper)
    {
        _locationTypeRepository = locationTypeRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<LocationTypeResponseModel> GetAll(LocationTypeParams @params, Language language)
    {
        var listAll = _locationTypeRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);

        var mappedData = _mapper.Map<IEnumerable<LocationTypeResponseModel>>(sortedQuests);

        var locationTypeResponseModels = mappedData as LocationTypeResponseModel[] ?? mappedData.ToArray();
        foreach (var item in locationTypeResponseModels)
        {
            item.Name = ConvertLanguage(language, item.Name!);
        }

        return PageList<LocationTypeResponseModel>.ToPageList(locationTypeResponseModels, @params.PageNumber, @params.PageSize);
    }

    public async Task<LocationTypeResponseModel> Get(int id, Language language)
    {
        var entity = await _locationTypeRepository.Get(id);
        CheckDataNotNull("LocationType", entity);

        entity.Name = ConvertLanguage(language, entity.Name!);

        return _mapper.Map<LocationTypeResponseModel>(entity);
    }

    public async Task<LocationTypeResponseModel> Get(int id)
    {
        var entity = await _locationTypeRepository.Get(id);
        CheckDataNotNull("LocationType", entity);

        var objTitle = JObject.Parse(entity.Name!);
        var title = (string) objTitle["vi"]! + " | " + (string) objTitle["en"]!;

        entity.Name = title;

        return _mapper.Map<LocationTypeResponseModel>(entity);
    }

    public async Task<LocationTypeResponseModel> CreateAsync(LocationTypeRequestModel request)
    {
        request.Validate();
        var requestName = GetVietNameseName(request.Name!);

        var existValue = _locationTypeRepository.GetAll();
        foreach (var exist in existValue)
        {
            if (Trim(ConvertLanguage(Language.vi, exist.Name)) == Trim(requestName))
            {
                throw new AppException("Location type name is exist");
            }
        }

        var entity = _mapper.Map<LocationType>(request);

        entity.Name = JsonHelper.JsonFormat(request.Name);

        entity = await _locationTypeRepository.Add(entity);

        return _mapper.Map<LocationTypeResponseModel>(entity);
    }

    public async Task<LocationTypeResponseModel> UpdateAsync(LocationTypeRequestModel request)
    {
        var entity = _mapper.Map<LocationType>(request);

        entity.Name = JsonHelper.JsonFormat(request.Name);

        entity = await _locationTypeRepository.Update(entity);
        return _mapper.Map<LocationTypeResponseModel>(entity);
    }

    public async Task<LocationTypeResponseModel> DeleteAsync(int id)
    {
        var locationType = _locationTypeRepository.GetByCondition(x => x.Id == id).Include(data => data.Locations)
            .ToList().FirstOrDefault();
        if (locationType != null && locationType.Locations!.Count == 0)
        {
            locationType.Status = CommonStatus.Inactive.ToString();
            await _locationTypeRepository.UpdateFields(locationType, r => r.Status!);
        }

        return _mapper.Map<LocationTypeResponseModel>(locationType);
    }

    public async Task<LocationTypeResponseModel> DisableAsync(int id)
    {
        var entity = _locationTypeRepository.GetByCondition(x => x.Id == id)
            .Include(data => data.Locations).ToList().FirstOrDefault();

        if(entity == null || entity.Locations!.Count != 0) return _mapper.Map<LocationTypeResponseModel>(entity);

        entity.Status = CommonStatus.Inactive.ToString();
        await _locationTypeRepository.UpdateFields(entity, r => r.Status!);

        return _mapper.Map<LocationTypeResponseModel>(entity);
    }

    public async Task<LocationTypeResponseModel> EnableAsync(int id)
    {
        var entity = await _locationTypeRepository.Get(id);
        entity.Status = CommonStatus.Active.ToString();
        await _locationTypeRepository.UpdateFields(entity, r => r.Status!);
        return _mapper.Map<LocationTypeResponseModel>(entity);
    }

    public Task<bool> CheckExisted(string name)
    {
        var existValue = _locationTypeRepository.GetAll();
        foreach (var exist in existValue)
        {
            if (Trim(ConvertLanguage(Language.vi, exist.Name)) == Trim(name))
            {
                return Task.FromResult((true));
            }
        }
        return Task.FromResult(false);
    }

    private static void Search(ref IQueryable<LocationType> entities, LocationTypeParams param)
    {
        if (!entities.Any()) return;

        if (param.Name != null)
            entities = entities.Where(r => r.Name!.Contains(param.Name.Trim()));
        if (param.Status != null) entities = entities.Where(x => x.Status == param.Status);
    }
}