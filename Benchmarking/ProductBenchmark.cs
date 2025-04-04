using BenchmarkDotNet.Attributes;
using Methods;
using Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;
using Database;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Benchmarking;

[MemoryDiagnoser]
public class ProductBenchmark
{
    private readonly ProductService _service;
    private List<ProductInputDto> _data;

    public ProductBenchmark()
    {
        var factory = new PooledDbContextFactory<AppDbContext>(
      new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=performance;Trusted_Connection=True;Encrypt=False")
          .Options
  );

        _service = new ProductService(factory);
        _data = ProductDataGenerator.Generate(100);
    }

    [Benchmark]
    public async Task SaveStepByStep() => await _service.SaveStepByStepAsync(_data);

    [Benchmark]
    public async Task SaveWithTransaction() => await _service.SaveWithTransactionAsync(_data);

    [Benchmark]
    public async Task SaveParallelDbContexts() => await _service.SaveParallelSafeAsync(_data);

    [Benchmark]
    public async Task SaveWithTooManyIfs() => await _service.SaveWithTooManyIfs(_data);

    [Benchmark]
    public async Task SaveWithHeavyForeach() => await _service.SaveWithHeavyForeach(_data);

    [Benchmark]
    public Task SaveWithoutAsync() => _service.SaveWithoutAsync(_data);

}