using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersData _usersData;

    public UsersController(IUsersData usersData)
    {
        _usersData = usersData;
    }

    // api/Users/me
    [HttpGet("me")]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IResult> GetOwn()
    {
        Guid id = Guid.Parse(HttpContext.User.FindFirst("Id")!.Value);
        var res = await _usersData.Get(id);
        return Results.Ok(res);
    }

// FINISH IT, update user with validation
    // api/Users/me
    [HttpPut("me")]
    [Authorize(Roles = "Admin,Customer")]
    [ValidateModel]
    public async Task<IResult> UpdadeOwn(User user)
    {
        Guid id = Guid.Parse(HttpContext.User.FindFirst("Id")!.Value);
        var res = await _usersData.Get(id);
        return Results.Ok(res);
    }

    // api/Users/{id}
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> Get(Guid id)
    {
        var res = await _usersData.Get(id);
        return Results.Ok(res);
    }

    // api/Users
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> Save([FromBody] User user)
    {
        await _usersData.Create(user);
        return Results.Ok();
    }

    // api/Users/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> Update(Guid id, [FromBody] User user)
    {
        user.Id = id;

        await _usersData.Update(user);
        return Results.Ok();
    }

    // api/Users/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> Delete(Guid id)
    {
        await _usersData.Delete(id);
        return Results.Ok();
    }
}