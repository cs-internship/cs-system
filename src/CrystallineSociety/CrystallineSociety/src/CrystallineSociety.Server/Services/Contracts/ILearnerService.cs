namespace CrystallineSociety.Server.Services.Contracts
{
    public interface ILearnerService
    {
        Task<LearnerDto> GetLearnerByIdAsync(Guid id);
        Task<LearnerDto> GetLearnerByUsernameAsync(string username);
        IQueryable<Shared.Dtos.LearnerDto> GetLearners();
    }
}
