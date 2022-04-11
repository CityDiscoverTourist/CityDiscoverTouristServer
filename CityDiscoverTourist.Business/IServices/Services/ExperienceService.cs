using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Task = CityDiscoverTourist.Data.Models.Task;

namespace CityDiscoverTourist.Business.IServices.Services;

public class ExperienceService: IExperienceService
{
    private readonly IExperienceRepository _experienceRepository;
    private readonly IMapper _mapper;

    public ExperienceService(IExperienceRepository taskRepository, IMapper mapper)
    {
        _experienceRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<ExperienceResponseModel> Get(int id, string? fields)
    {
        var entity = await _experienceRepository.Get(id);

        //var shaped = _dataShaper.ShapeData(entity, fields);

        return _mapper.Map<ExperienceResponseModel>(entity);
    }

    public async Task<ExperienceResponseModel> CreateAsync(ExperienceRequestModel request)
    {
        var entity = _mapper.Map<Experience>(request);
        entity = await _experienceRepository.Add(entity);
        return _mapper.Map<ExperienceResponseModel>(entity);
    }

    public async Task<ExperienceResponseModel> UpdateAsync(ExperienceRequestModel request)
    {
        var entity = _mapper.Map<Experience>(request);
        entity = await _experienceRepository.Update(entity);
        return _mapper.Map<ExperienceResponseModel>(entity);
    }

    public async Task<ExperienceResponseModel> DeleteAsync(int id)
    {
        var entity = await _experienceRepository.Delete(id);
        return _mapper.Map<ExperienceResponseModel>(entity);
    }
}