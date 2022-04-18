using AutoMapper;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CustomerAnswerService: ICustomerAnswerService
{
    private readonly ICustomerAnswerRepository _answerRepository;
    private readonly IMapper _mapper;

    public CustomerAnswerService(ICustomerAnswerRepository answerRepository, Mapper mapper)
    {
        _answerRepository = answerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerAnswer> Get(int id)
    {
        var entity = await _answerRepository.Get(id);

        //var shaped = _dataShaper.ShapeData(entity, fields);

        return _mapper.Map<CustomerAnswer>(entity);
    }

    public async Task<CustomerAnswer> CreateAsync(CustomerAnswer request)
    {
        var entity = _mapper.Map<CustomerAnswer>(request);
        entity = await _answerRepository.Add(entity);
        return _mapper.Map<CustomerAnswer>(entity);
    }

    public async Task<CustomerAnswer> UpdateAsync(CustomerAnswer request)
    {
        var entity = _mapper.Map<CustomerAnswer>(request);
        entity = await _answerRepository.Update(entity);
        return _mapper.Map<CustomerAnswer>(entity);
    }

    public async Task<CustomerAnswer> DeleteAsync(int id)
    {
        var entity = await _answerRepository.Delete(id);
        return _mapper.Map<CustomerAnswer>(entity);
    }
}