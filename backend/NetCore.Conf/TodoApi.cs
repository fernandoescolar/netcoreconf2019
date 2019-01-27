using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Conf
{
    public class Todo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("n");

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public string Text { get; set; }

        public bool Done { get; set; }
    }

    public static class TodoApi
    {
        [FunctionName("Todo_Get")]
        public static async Task<IActionResult> SelectAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos")]HttpRequest req, ILogger log, ExecutionContext context)
        {
            var cnnString = GetConnectionString(log, context);
            using (var connection = new SqlConnection(cnnString))
            {
                connection.Open();
                var todos = await connection.QueryAsync<Todo>("select Id, Created, Text, Done from dbo.Todos");

                if (todos.Count() == 0) return new EmptyResult();
                return new OkObjectResult(todos);
            }
        }

        [FunctionName("Todo_GetById")]
        public static async Task<IActionResult> SelectByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos/{id}")]HttpRequest req, ILogger log, ExecutionContext context, string id)
        {
            var cnnString = GetConnectionString(log, context);
            using (var connection = new SqlConnection(cnnString))
            {
                connection.Open();
                var todos = await connection.QueryAsync<Todo>("select Id, Created, Text, Done from dbo.Todos where Id = @id", new { id });

                if (todos.Count() == 0) return new NotFoundResult();
                return new OkObjectResult(todos.First());
            }
        }

        [FunctionName("Todo_Create")]
        public static async Task<IActionResult> CreateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todos")]HttpRequest req, ILogger log, ExecutionContext context)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<Todo>(requestBody);

            var cnnString = GetConnectionString(log, context);
            using (var connection = new SqlConnection(cnnString))
            {
                connection.Open();
                await connection.ExecuteAsync("insert into dbo.Todos (Id, Created, Text, Done) values (@Id, @Created, @Text, @Done)", input);

                var location = $"{req.Scheme}://{req.Host}{req.Path}{req.QueryString}/{input.Id}";
                return new CreatedResult(location, input);
            }
        }

        [FunctionName("Todo_Update")]
        public static async Task<IActionResult> UpdateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todos/{id}")]HttpRequest req, ILogger log, ExecutionContext context, string id)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<Todo>(requestBody);

            var cnnString = GetConnectionString(log, context);
            using (var connection = new SqlConnection(cnnString))
            {
                connection.Open();
                await connection.ExecuteAsync("update dbo.Todos set Text = @Text, Done = @Done where Id = @Id", input);

                return new OkObjectResult(input);
            }
        }

        [FunctionName("Todo_Delete")]
        public static async Task<IActionResult> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todos/{id}")]HttpRequest req, ILogger log, ExecutionContext context, string id)
        {
            var cnnString = GetConnectionString(log, context);
            using (var connection = new SqlConnection(cnnString))
            {
                connection.Open();
                await connection.ExecuteAsync("delete from dbo.Todos where Id = @id", new {  id });

                return new OkObjectResult(id);
            }
        }

        private static string GetConnectionString(ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(context.FunctionAppDirectory)
                            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();

            return config.GetConnectionString("DefaultConnection");
        }
    }
}
