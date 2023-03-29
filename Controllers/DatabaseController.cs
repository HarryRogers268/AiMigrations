using Microsoft.AspNetCore.Mvc;
using Redgate.Text_Migrations_v2.Core;

namespace Redgate.Text_Migrations_v2.Controllers;

[Route("api/v1/database"), Route("api/v1/databases")]
public class DatabaseController : Controller
{
    private readonly IDatabaseRepository _databaseRepository;

    public DatabaseController(IDatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }
    
    [HttpGet, Route("")]
    public ActionResult GetDatabases()
    {
        var databases = _databaseRepository.GetDatabases();
        return Json(databases);
    }
    
}