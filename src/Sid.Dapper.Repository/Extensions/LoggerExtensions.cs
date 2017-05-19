using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Sid.Dapper.Repository.Extensions
{
    internal static class LoggerExtensions
    {
        public static void LogSql(this ILogger logger, string sql, object param = null)
        {
            if (logger != null)
            {
                var message = $"Executing sql: {sql}";
                if (param != null)
                {
                    message += $", {JsonConvert.SerializeObject(param)}.";
                }

                logger.LogInformation(message);
            }
        }
    }
}