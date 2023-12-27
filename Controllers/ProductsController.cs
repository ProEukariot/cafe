using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/[Controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductsData _productData;

    public ProductsController(IProductsData productsData)
    {
        _productData = productsData;
    }

    // api/Products?...
    [HttpGet, AllowAnonymous]
    public async Task<IResult> GetAll([FromQuery] ProductParams query, [FromHeader]CancellationToken t)
    {
        var res = await _productData.Get(HttpContext.RequestAborted);

        #region filtering

        res = res.Where(p => p.Price > query.MinPrice)
            .Where(p =>
            {
                if (query.MaxPrice == 0)
                    return true;
                return p.Price < query.MaxPrice;
            })
            .Where(p =>
            {
                if (query.Category == null)
                    return true;
                return p.Category!.Contains(query.Category);
            })
            .Where(p =>
             {
                 if (query.Size == null)
                     return true;
                 return p.Category!.Contains(query.Size);
             }
            );

        #endregion

        return Results.Ok(res);
    }

    // api/Products/{id}
    [HttpGet("{id}"), AllowAnonymous]
    public async Task<IResult> Get(Guid id)
    {
        var res = await _productData.Get(id);
        return Results.Ok(res);
    }

    // api/Products
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> Save([FromBody] Product product)
    {
        product.Id = Guid.NewGuid();

        await _productData.Create(product);
        return Results.Ok();
    }

    // api/Products/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> Update(Guid id, [FromBody] Product product)
    {
        product.Id = id;

        // get from db and update 'not null' fields

        await _productData.Update(product);
        return Results.Ok();
    }

    // api/Products/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> Delete(Guid id)
    {
        await _productData.Delete(id);
        return Results.Ok();
    }

}

public class ProductParams
{
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public string? Category { get; set; }
    public string? Size { get; set; }
}
