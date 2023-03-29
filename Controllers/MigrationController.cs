using Microsoft.AspNetCore.Mvc;
using Redgate.Text_Migrations_v2.Controllers.Bindings;
using Redgate.Text_Migrations_v2.Db;

namespace Redgate.Text_Migrations_v2.Controllers;

[Route("api/v1/migration"), Route("api/v1/migrations")]
public class MigrationController : Controller
{

    [HttpPost, Route("generate")]
    public ActionResult GenerateMigrationScript([FromBody] MigrationBuilderBinding binding)
    {
        var builder = new MigrationBuilder(binding.From, binding.To);
        var text = builder.Build();
        text.Wait();
        
        var result = text.Result;
        
        var startPos = -1;
        var endPos = -1;
        for (int i = 0; i <= result.Length; i++)
        {
            if (i + 6 <= result.Length && result.Substring(i, 6) == "```sql")
            {
                startPos = i + 6;
            } 
            else if (i + 3 <= result.Length && result.Substring(i, 3) == "```")
            {
                if (startPos > -1)
                {
                    endPos = i;
                }
                else
                {
                    startPos = i + 3;
                }
            }
        }

        return Json(result.Substring(startPos, endPos - startPos).Trim());
    }
    
}