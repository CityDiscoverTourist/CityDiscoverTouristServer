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

public class AreaService : BaseService, IAreaService
{
    private readonly IAreaRepository _areaRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<Area> _sortHelper;

    public AreaService(IAreaRepository areaRepository, IMapper mapper, ISortHelper<Area> sortHelper)
    {
        _areaRepository = areaRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<AreaResponseModel> GetAll(AreaParams @params, Language language)
    {
        var listAll = _areaRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<AreaResponseModel>>(sortedQuests);

        var areaResponseModels = mappedData as AreaResponseModel[] ?? mappedData.ToArray();
        foreach (var item in areaResponseModels)
        {
            item.Name = ConvertLanguage(language, item.Name!);
        }

        return PageList<AreaResponseModel>.ToPageList(areaResponseModels, @params.PageNumber, @params.PageSize);
    }

    public async Task<AreaResponseModel> Get(int id, Language language)
    {
        var entity = await _areaRepository.Get(id);
        CheckDataNotNull("Area", entity);

        entity.Name = ConvertLanguage(language, entity.Name!);

        return _mapper.Map<AreaResponseModel>(entity);
    }

    public async Task<AreaResponseModel> Get(int id)
    {
        var entity = await _areaRepository.Get(id);
        CheckDataNotNull("Area", entity);

        var objTitle = JObject.Parse(entity.Name!);
        var title = (string) objTitle["vi"]! + " | " + (string) objTitle["en"]!;

        entity.Name = title;

        return _mapper.Map<AreaResponseModel>(entity);
    }

    public async Task<AreaResponseModel> CreateAsync(AreaRequestModel request)
    {
        var requestName = GetVietNameseName(request.Name!);

        var existValue = _areaRepository.GetAll();
        foreach (var exist in existValue)
        {
            if (Trim(ConvertLanguage(Language.vi, exist.Name)) == Trim(requestName))
            {
                throw new AppException("Area name is exist");
            }
        }

        var entity = _mapper.Map<Area>(request);

        entity.Name = JsonHelper.JsonFormat(request.Name);

        entity = await _areaRepository.Add(entity);
        return _mapper.Map<AreaResponseModel>(entity);
    }

    public async Task<AreaResponseModel> UpdateAsync(AreaRequestModel request)
    {
        var entity = _mapper.Map<Area>(request);

        entity.Name = JsonHelper.JsonFormat(request.Name);


        entity = await _areaRepository.Update(entity);
        return _mapper.Map<AreaResponseModel>(entity);
    }

    public async Task<AreaResponseModel> DeleteAsync(int id)
    {
        var area = _areaRepository.GetByCondition(x => x.Id == id).Include(data => data.Locations).ToList()
            .FirstOrDefault();
        if (area != null && area.Locations!.Count == 0)
        {
            area.Status = CommonStatus.Inactive.ToString();
            await _areaRepository.UpdateFields(area, r => r.Status!);
        }

        return _mapper.Map<AreaResponseModel>(area);
    }

    public async Task<AreaResponseModel> DisableAsync(int id)
    {
        var area = _areaRepository.GetByCondition(x => x.Id == id)
            .Include(data => data.Locations).ToList().FirstOrDefault();
        if (area == null || area.Locations!.Count != 0) return _mapper.Map<AreaResponseModel>(area);

        area.Status = CommonStatus.Inactive.ToString();
        await _areaRepository.UpdateFields(area, r => r.Status!);

        return _mapper.Map<AreaResponseModel>(area);
    }

    public async Task<AreaResponseModel> EnableAsync(int id)
    {
        var area = await _areaRepository.Get(id);
        area.Status = CommonStatus.Active.ToString();
        await _areaRepository.UpdateFields(area, r => r.Status!);
        return _mapper.Map<AreaResponseModel>(area);
    }

    public Task<bool> CheckExisted(string name)
    {
        var existValue = _areaRepository.GetAll();
        foreach (var exist in existValue)
        {
            if (Trim(ConvertLanguage(Language.vi, exist.Name)) == Trim(name))
            {
                return Task.FromResult((true));
            }
        }
        return Task.FromResult(false);
    }

    private static void Search(ref IQueryable<Area> entities, AreaParams param)
    {
        if (!entities.Any()) return;

        if (param.CityId != 0) entities = entities.Where(x => x.CityId == param.CityId);

        if (param.Name != null) entities = entities.Where(x => x.Name!.Contains(param.Name.Trim()));

        if (param.Status != null) entities = entities.Where(x => x.Status == param.Status);
    }
}