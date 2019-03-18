using System.Collections.Generic;
using System.Threading.Tasks;

namespace RSSParser.Repository
{
    public interface ISourceRepository
    {
        Task<IEnumerable<RssSource>> GetAllAsync();

        Task<IEnumerable<RssSource>> AddRangeAsync(IEnumerable<RssSource> sources);

        Task<RssSource> GetById(int id);
    }
}
