
using BenchmarkDotNet.Running;
using Benchmarking;

//using (var scope = new Database.AppDbContext())
//{
//    scope.Database.EnsureCreated();
//}
BenchmarkRunner.Run<ProductBenchmark>();
