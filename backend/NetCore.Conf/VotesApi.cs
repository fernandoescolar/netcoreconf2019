
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace NetCore.Conf
{
    public static class VotesApi
    {
        private const string route = "votes";
        private const string ConnectionStringName = "DefaultConnection";
        private const string SelectQuery = @"select QuestionId, Username, Usefull, Crazy from dbo.Votes";
        private const string DeleteQuery = @"truncate table dbo.Votes";
        private const string InsertQuery = @"
update dbo.Votes set Usefull = @Usefull, Crazy = @Crazy where QuestionId = @QuestionId and Username = @Username 
if @@ROWCOUNT = 0
 begin
    insert into dbo.Votes (QuestionId, Username, Usefull, Crazy) VALUES (@QuestionId, @Username, @Usefull, @Crazy)
end
";

        [FunctionName("Vote_Select")]
        public static async Task<IActionResult> SelectAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = route)]HttpRequest req, TraceWriter log, ExecutionContext context)
        {
            log.Info("Vote_Select invoked");
            var successful = true;
            var errorMessage = string.Empty;
            var votes = new List<Vote>() as IEnumerable<Vote>;
            try
            {
                var config = GetConfiguration(log, context);
                var cnnString = config.GetConnectionString(ConnectionStringName);
                using (var connection = new SqlConnection(cnnString))
                {
                    connection.Open();
                    votes = await connection.QueryAsync<Vote>(SelectQuery);
                    log.Info("Votes read from database successfully!");
                }
            }
            catch (Exception ex)
            {
                successful = false;
                errorMessage = ex.Message;
                log.Error("Run: " + errorMessage);
            }

            log.Info("Vote_Select end");
            return successful
                        ? (IActionResult)new OkObjectResult(votes)
                        : new BadRequestObjectResult(errorMessage);
        }

        [FunctionName("Vote_CreateOrUpdate")]
        public static async Task<IActionResult> CreateOrUpdateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = route)]HttpRequest req, TraceWriter log, ExecutionContext context)
        {
            log.Info("Vote_CreateOrUpdate invoked");
            var successful = true;
            var errorMessage = string.Empty;
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var input = JsonConvert.DeserializeObject<Vote>(requestBody);
                input.Validate();

                var config = GetConfiguration(log, context);
                var cnnString = config.GetConnectionString(ConnectionStringName);
                using (var connection = new SqlConnection(cnnString))
                {
                    connection.Open();
                    await connection.ExecuteAsync(InsertQuery, input);
                    log.Info("Vote added to database successfully!");
                }
            }
            catch (Exception ex)
            {
                successful = false;
                errorMessage = ex.Message;
                log.Error("Run: " + errorMessage);
            }

            log.Info("Vote_CreateOrUpdate end");
            return successful
                        ? (IActionResult)new OkResult()
                        : new BadRequestObjectResult(errorMessage);
        }

        [FunctionName("Vote_Delete")]
        public static async Task<IActionResult>DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = route)]HttpRequest req, TraceWriter log, ExecutionContext context)
        {
            log.Info("Vote_Delete invoked");
            var successful = true;
            var errorMessage = string.Empty;
            try
            {
                var config = GetConfiguration(log, context);
                var cnnString = config.GetConnectionString(ConnectionStringName);
                using (var connection = new SqlConnection(cnnString))
                {
                    connection.Open();
                    await connection.ExecuteAsync(DeleteQuery);
                    log.Info("database cleaned successfully!");
                }
            }
            catch (Exception ex)
            {
                successful = false;
                errorMessage = ex.Message;
                log.Error("Run: " + errorMessage);
            }

            log.Info("Vote_Delete end");
            return successful
                        ? (IActionResult)new OkResult()
                        : new BadRequestObjectResult(errorMessage);
        }

        private static IConfigurationRoot GetConfiguration(TraceWriter log, ExecutionContext context)
        {
            log.Info("Reading configuration...");
            try
            {
                return new ConfigurationBuilder()
                            .SetBasePath(context.FunctionAppDirectory)
                            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();
            }
            catch (Exception ex)
            {
                log.Error("Configuration: " + ex.Message);
                throw ex;
            }
        }
    }
}
