using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSParser.Repository
{
    public class SourceRepository : ISourceRepository
    {
        private readonly RSScontext _context;
        public SourceRepository(RSScontext context) => _context = context;

        public async Task<IEnumerable<RssSource>> AddRangeAsync(IEnumerable<RssSource> sources)
        {
            _context.RssSources.AddRange(sources);

            await _context.SaveChangesAsync();

            return sources;
        }

        public async Task<IEnumerable<RssSource>> GetAllAsync()
        {
            return await Task.Run(() => _context.RssSources.ToList()).ConfigureAwait(false);
        }

        public async Task<RssSource> GetById(int id)
        {
            return await _context.RssSources.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
