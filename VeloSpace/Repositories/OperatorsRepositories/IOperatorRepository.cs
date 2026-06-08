using VeloSpace.Model.OperatorShi;

namespace VeloSpace.Repositories.OperatorsRepositories;

public interface IOperatorRepository
{
    Task<IEnumerable<Operator>> GetAllAsync();

    Task<Operator> GetByIdAsync(long id);

    Task AddAsync(Operator @operator);

    Task UpdateAsync(Operator @operator);

    Task DeleteAsync(long id);

    Task<(IEnumerable<Operator> Items, int TotalItems)> SearchAsync(
        string? name,
        int? cpf,
        long? operatorStatusId,
        long? launchProviderId,
        int page,
        int pageSize,
        string sortBy,
        string sortDir
    );
}