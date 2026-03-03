using Microsoft.EntityFrameworkCore;
using Talaorasan.Server.Data;
using Talaorasan.Server.Entities;
using Talaorasan.Shared.Requests;
using Talaorasan.Shared.Requests.Transaction;
using Talaorasan.Shared.Response;

namespace Talaorasan.Server.Logic
{
    public interface IPersonService
    {
        Task<TransactionResponseDto<Guid>> Manage(PersonDto person, CancellationToken token);
        Task<CollectionResponseDto<PersonDto>> GetAll(CollectionRequest request, CancellationToken token);
    }
    public class PersonService : IPersonService
    {
        private readonly TalaorasanDbContext _db;
        public PersonService(TalaorasanDbContext db)
        {
            _db = db;
        }

        public async Task<TransactionResponseDto<Guid>> Manage(PersonDto person, CancellationToken token)
        {
            try
            {
                var getPerson = _db.Persons.AsNoTracking().FirstOrDefault(p => p.PersonId == person.PersonId) ?? new Person();
                getPerson.FirstName = person.FirstName;
                getPerson.LastName = person.LastName;
                getPerson.MiddleName = person.MiddleName;
                getPerson.ExtensionName = person.ExtensionName;
                getPerson.Prefix = person.Prefix;
                getPerson.Suffix = person.Suffix;

                if (_db.Persons.AsNoTracking().Any(p => p.PersonId == person.PersonId))
                    _db.Persons.Update(getPerson);
                else
                {
                    getPerson.PersonId = Guid.NewGuid();
                    getPerson.CreatedUtc = DateTime.UtcNow;
                    _db.Persons.Add(getPerson);
                }
                var result = await _db.SaveChangesAsync(token);
                return new TransactionResponseDto<Guid>()
                {
                    Success = result > 0,
                    Message = result > 0 ? "Person saved successfully." : "Failed to save person.",
                    Output = getPerson.PersonId
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TransactionResponseDto<Guid>();
            }
        }
        public async Task<CollectionResponseDto<PersonDto>> GetAll(CollectionRequest request, CancellationToken token)
        {
            try
            {
                var skip = request.Skip * request.Take;
                var persons = skip == 0 && request.Take == 0 ?
                              _db.Persons.AsNoTracking() :
                              _db.Persons.Skip(skip).Take(request.Take).AsNoTracking();
                var query = (from person in persons.ToList()
                             select new PersonDto
                             {
                                 PersonId = person.PersonId,
                                 FirstName = person.FirstName,
                                 LastName = person.LastName,
                                 MiddleName = person.MiddleName,
                                 ExtensionName = person.ExtensionName,
                                 Suffix = person.Suffix,
                                 Prefix = person.Prefix,
                             }).ToArray();
                var total = await _db.Persons.CountAsync(token);
                var pages = request.Take == 0 ? 1 : (int)Math.Ceiling((double)total / request.Take);
                return new CollectionResponseDto<PersonDto>(query, pages, total);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new CollectionResponseDto<PersonDto>(Array.Empty<PersonDto>(), 0, 0);
            }
        }


    }
}
