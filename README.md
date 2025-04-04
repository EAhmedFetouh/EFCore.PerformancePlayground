# EFCore.SavePerformanceComparison

A benchmark project that compares different Entity Framework Core data-saving patterns in terms of performance, memory usage, and scalability.

## 🚀 Purpose
This repository demonstrates how the way you save data in EF Core can significantly impact application performance.

It compares 6 different save strategies:

- `SaveStepByStep`
- `SaveWithTransaction`
- `SaveParallelDbContexts`
- `SaveWithTooManyIfs`
- `SaveWithHeavyForeach`
- `SaveWithoutAsync`

Each method inserts related data across 5 tables:
- Products
- ProductDetails
- ProductCategories
- ProductInventories
- ProductLogs

## 📊 Benchmarking Tools
- **BenchmarkDotNet** — for accurate performance measurements
- **SQL Server LocalDB** — used as the backend database
- **EF Core 8** — ORM used for data access

## 🧪 Scenario
Each method receives a list of `ProductInputDto`, and inserts them along with related child records (Details, Inventory, etc). The goal is to measure the time and memory differences between various approaches:

- Using SaveChanges multiple times vs batching
- Parallel insert using `Task.WhenAll`
- Complex `if` conditions vs clean LINQ logic
- With and without `async/await`

## 📈 Results Summary (sample for 100 records)

| Method                  | Time (ms) | Memory    | Notes                     |
|------------------------|-----------|-----------|---------------------------|
| SaveStepByStep         | 281       | 17.72 MB  | SaveChanges per record   |
| SaveWithTransaction    | 106       | 10.56 MB  | Best balance              |
| SaveParallelDbContexts | **41**    | 5.35 MB   | Fastest, parallel-safe    |
| SaveWithTooManyIfs     | 288       | 17.72 MB  | Overcomplicated ifs       |
| SaveWithHeavyForeach   | 299       | 17.73 MB  | Redundant loops           |
| SaveWithoutAsync       | 219       | 16.99 MB  | Blocking calls            |

## ✅ Recommendations
- Prefer `SaveWithTransaction` for safe, clean performance
- Use `SaveParallelDbContexts` with `IDbContextFactory` for high-throughput scenarios
- Avoid unnecessary `if` nesting and repetitive `foreach` loops

## 📂 Structure
```
├── Entities/
├── Dtos/
├── Methods/
├── Benchmarking/
├── Database/
└── Program.cs
```

## ⚙️ Getting Started
1. Clone the repo
2. Run migrations:
   ```bash
   dotnet ef migrations add Init
   dotnet ef database update
   ```
3. Run the benchmark:
   ```bash
   dotnet run -c Release
   ```

## 🧑‍💻 Author
Ahmed Fetouh

## 📘 License
MIT
