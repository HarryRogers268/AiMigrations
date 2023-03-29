using Microsoft.AspNetCore.Mvc;
using Redgate.Text_Migrations_v2.Controllers.Bindings;
using Redgate.Text_Migrations_v2.Core;
using Redgate.Text_Migrations_v2.Db;

namespace Redgate.Text_Migrations_v2.Controllers;

[Route("api/v1/improvement"), Route("api/v1/improvements")]
public class ImprovementController : Controller
{

    [HttpPost, Route("")]
    public ActionResult GetImprovements([FromBody] ImprovementBinding binding)
    {
        var builder = new ImprovementBuilder(binding.To);
        var text = builder.Build();
        text.Wait();
        
        var result = text.Result;
        var improvementString = new List<string>();
        var improvements = new List<Improvement>();
        
        do
        {
            //magic?
            improvementString.Add(
                result.Substring(
                    result.IndexOf(
                        "Improvement " + (improvementString.Count + 1) + ":", StringComparison.Ordinal) + 
                            ("Improvement " + (improvementString.Count + 1) + ":").Length,
                        (result.IndexOf("Improvement " + (improvementString.Count + 2) + ":", StringComparison.Ordinal) >= 0 ? 
                            result.IndexOf("Improvement " + (improvementString.Count + 2) + ":", StringComparison.Ordinal) : result.Length - 1) -
                    (result.IndexOf("Improvement " + (improvementString.Count + 1) + ":", StringComparison.Ordinal) + 
                     ("Improvement " + (improvementString.Count + 1) + ":").Length)));
        } while (result.IndexOf("Improvement " + (improvementString.Count + 1) + ":", StringComparison.Ordinal) >= 0);

        foreach (var improvement in improvementString)
        {
            var name = improvement.Substring(
                improvement.IndexOf("- name:", StringComparison.Ordinal) + "- name:".Length, 
                improvement.IndexOf("- description:", StringComparison.Ordinal) - improvement.IndexOf("- name:", StringComparison.Ordinal) - "- name:".Length);
            var description = improvement.Substring(
                improvement.IndexOf("- description:", StringComparison.Ordinal) + "- description:".Length, 
                improvement.IndexOf("- sql:", StringComparison.Ordinal) - improvement.IndexOf("- description:", StringComparison.Ordinal) - "- description:".Length);
            var sql = improvement.Substring(
                improvement.IndexOf("- sql:", StringComparison.Ordinal) + "- sql:".Length, 
                improvement.Length - 1 - improvement.IndexOf("- sql:", StringComparison.Ordinal) - "- sql:".Length);
            
            var startPos = -1;
            var endPos = -1;
            for (int i = 0; i <= sql.Length; i++)
            {
                if (i + 6 <= sql.Length && sql.Substring(i, 6) == "```sql")
                {
                    startPos = i + 6;
                } 
                else if (i + 3 <= sql.Length && sql.Substring(i, 3) == "```")
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

            sql = sql.Substring(startPos, endPos - startPos);

            var tempImprovement = new Improvement();
            tempImprovement.Name = name.Trim();
            tempImprovement.Overview = description.Trim();
            tempImprovement.Sql = sql.Trim();
            
            improvements.Add(tempImprovement);
            
        }
        
        return Json(improvements);
    }

    [HttpPost, Route("apply")]
    public ActionResult ApplyImprovement([FromBody] ApplyImprovementBinding binding)
    {
        var sql = binding.Sql;

        var connection = new DbConnection(DbConnection.LocalHost(binding.DbName));
        
        DbConnector.ExecuteSqlNoReturn(connection, sql);
        
        return Ok();
    }

}