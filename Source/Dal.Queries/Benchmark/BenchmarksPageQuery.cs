using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;
using Benchmark = Dal.Entities.Benchmark;

namespace Dal.Queries;

/// <inheritdoc />
public class BenchmarksPageQuery : PageQueryBase<Benchmark>
{
    // private const string Separator = " ";
    // private const byte PageSize = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="BenchmarksPageQuery"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public BenchmarksPageQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    // protected override async Task<QueryResult<List<Benchmark>>> ExecuteCoreAsync(PageModel model)
    // {
    //     var entitiesToFetch = Context.Benchmarks.AsNoTracking();
    //     if (model.SearchPhrase is not null)
    //     {
    //         string[] searchWords = model.SearchPhrase.Split(Separator);
    //         entitiesToFetch = entitiesToFetch.Where(entity => searchWords.All(word => entity.Name.Contains(word)));
    //     }
    //
    //     int entitiesCountToSkip = (model.PageNumber - 1) * PageSize;
    //     var benchmarks = await entitiesToFetch
    //         .OrderBy(entity => entity.Name)
    //         .Skip(entitiesCountToSkip)
    //         .Take(PageSize)
    //         .ToListAsync();
    //
    //     return GetSuccessfulResult(benchmarks);
    // }

    /// <inheritdoc/>
    protected override IQueryable<Benchmark> FilterBySearchWord(IQueryable<Benchmark> source, string word)
    {
        return source.Where(entity => entity.Name.ToLower().Contains(word.ToLower()));
    }

    /// <inheritdoc/>
    protected override IQueryable<Benchmark> DefaultOrderBy(IQueryable<Benchmark> source)
    {
        return source.OrderBy(entity => entity.Name);
    }
}