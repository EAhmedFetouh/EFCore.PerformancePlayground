using Database;
using Dtos;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Methods;

public class ProductService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public ProductService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task SaveStepByStepAsync(List<ProductInputDto> products)
    {
        await using var db = _contextFactory.CreateDbContext();
        foreach (var product in products)
        {
            var entity = new Product { Name = product.Name };
            db.Products.Add(entity);
            await db.SaveChangesAsync();

            var details = new ProductDetail { ProductId = entity.Id, Description = product.Description };
            db.ProductDetails.Add(details);

            var category = new ProductCategory { ProductId = entity.Id, Category = product.Category };
            db.ProductCategories.Add(category);

            var inventory = new ProductInventory { ProductId = entity.Id, Stock = product.Stock };
            db.ProductInventories.Add(inventory);

            var log = new ProductLog { ProductId = entity.Id, Action = "Created", Timestamp = DateTime.UtcNow };
            db.ProductLogs.Add(log);

            await db.SaveChangesAsync();
        }
    }

    public async Task SaveWithTransactionAsync(List<ProductInputDto> products)
    {
        await using var db = _contextFactory.CreateDbContext();
        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            foreach (var product in products)
            {
                var entity = new Product { Name = product.Name };
                db.Products.Add(entity);
                await db.SaveChangesAsync();

                db.ProductDetails.Add(new ProductDetail { ProductId = entity.Id, Description = product.Description });
                db.ProductCategories.Add(new ProductCategory { ProductId = entity.Id, Category = product.Category });
                db.ProductInventories.Add(new ProductInventory { ProductId = entity.Id, Stock = product.Stock });
                db.ProductLogs.Add(new ProductLog { ProductId = entity.Id, Action = "Created", Timestamp = DateTime.UtcNow });
            }

            await db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    //public async Task SaveParallelDbContextsAsync(List<ProductInputDto> products)
    //{
    //    var tasks = products.Select(async product =>
    //    {
    //        using var db = new AppDbContext();
    //        var entity = new Product { Name = product.Name };
    //        db.Products.Add(entity);
    //        await db.SaveChangesAsync();

    //        db.ProductDetails.Add(new ProductDetail { ProductId = entity.Id, Description = product.Description });
    //        db.ProductCategories.Add(new ProductCategory { ProductId = entity.Id, Category = product.Category });
    //        db.ProductInventories.Add(new ProductInventory { ProductId = entity.Id, Stock = product.Stock });
    //        db.ProductLogs.Add(new ProductLog { ProductId = entity.Id, Action = "Created", Timestamp = DateTime.UtcNow });

    //        await db.SaveChangesAsync();
    //    });

    //    await Task.WhenAll(tasks);
    //}


    public async Task SaveParallelSafeAsync(List<ProductInputDto> products)
    {
        var tasks = products.Select(async product =>
        {
            await using var db = _contextFactory.CreateDbContext();

            var entity = new Product { Name = product.Name };
            db.Products.Add(entity);
            await db.SaveChangesAsync();

            db.ProductDetails.Add(new ProductDetail { ProductId = entity.Id, Description = product.Description });
            db.ProductCategories.Add(new ProductCategory { ProductId = entity.Id, Category = product.Category });
            db.ProductInventories.Add(new ProductInventory { ProductId = entity.Id, Stock = product.Stock });
            db.ProductLogs.Add(new ProductLog { ProductId = entity.Id, Action = "Created", Timestamp = DateTime.UtcNow });

            await db.SaveChangesAsync();
        });

        await Task.WhenAll(tasks);
    }


  public async Task SaveWithTooManyIfs(List<ProductInputDto> products)
    {
        await using var db = _contextFactory.CreateDbContext();
        foreach (var p in products)
        {
            if (p != null)
            {
                if (!string.IsNullOrWhiteSpace(p.Name))
                {
                    if (p.Name.StartsWith("Product"))
                    {
                        if (p.Stock >= 0)
                        {
                            var product = new Product { Name = p.Name };
                            db.Products.Add(product);
                            await db.SaveChangesAsync();

                            if (!string.IsNullOrEmpty(p.Description))
                            {
                                db.ProductDetails.Add(new ProductDetail { ProductId = product.Id, Description = p.Description });
                            }

                            if (!string.IsNullOrEmpty(p.Category))
                            {
                                db.ProductCategories.Add(new ProductCategory { ProductId = product.Id, Category = p.Category });
                            }

                            db.ProductInventories.Add(new ProductInventory { ProductId = product.Id, Stock = p.Stock });
                            db.ProductLogs.Add(new ProductLog { ProductId = product.Id, Action = "Created", Timestamp = DateTime.UtcNow });

                            await db.SaveChangesAsync();
                        }
                    }
                }
            }
        }
    }

    public async Task SaveWithHeavyForeach(List<ProductInputDto> products)
    {
        await using var db = _contextFactory.CreateDbContext();
        foreach (var p in products)
        {
            var product = new Product { Name = p.Name };
            db.Products.Add(product);
            await db.SaveChangesAsync();

            foreach (var c in new[] { p.Category })
            {
                db.ProductCategories.Add(new ProductCategory { ProductId = product.Id, Category = c });
            }

            foreach (var d in new[] { p.Description })
            {
                db.ProductDetails.Add(new ProductDetail { ProductId = product.Id, Description = d });
            }

            foreach (var s in new[] { p.Stock })
            {
                db.ProductInventories.Add(new ProductInventory { ProductId = product.Id, Stock = s });
            }

            foreach (var l in new[] { "Created" })
            {
                db.ProductLogs.Add(new ProductLog { ProductId = product.Id, Action = l, Timestamp = DateTime.UtcNow });
            }

            await db.SaveChangesAsync();
        }
    }

    public async Task SaveWithoutAsync(List<ProductInputDto> products)
    {
        await using var db = _contextFactory.CreateDbContext();
        foreach (var p in products)
        {
            var product = new Product { Name = p.Name };
            db.Products.Add(product);
            db.SaveChanges();

            db.ProductDetails.Add(new ProductDetail { ProductId = product.Id, Description = p.Description });
            db.ProductCategories.Add(new ProductCategory { ProductId = product.Id, Category = p.Category });
            db.ProductInventories.Add(new ProductInventory { ProductId = product.Id, Stock = p.Stock });
            db.ProductLogs.Add(new ProductLog { ProductId = product.Id, Action = "Created", Timestamp = DateTime.UtcNow });

            db.SaveChanges();
        }
    }

}