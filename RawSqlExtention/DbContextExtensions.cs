using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Ghak.ExecuteRawSql
{
    public static class DbContextExtensions
    {
        public static async Task<List<T>> ExecuteRawQueryAsync<T>(this DbContext context,  string query, params object[] parameters)
        {
            await using var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            if (parameters.Any())
            {
                command.Parameters.AddRange(parameters);
            }

            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }

            await using var dataReader = await command.ExecuteReaderAsync();
            var resultLis = ReadData<T>(dataReader);
            if (command.Connection.State == ConnectionState.Open)
            {
                command.Connection.Close();
            }

            return resultLis;
        }

        public static List<T> ExecuteRawQuery<T>(this DbContext context, string query, params object[] parameters)
        {
            using var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            if (parameters.Any())
            {
                command.Parameters.AddRange(parameters);
            }

            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }

            using var dataReader = command.ExecuteReader();
            var resultLis = ReadData<T>(dataReader);

            if (command.Connection.State == ConnectionState.Open)
            {
                command.Connection.Close();
            }

            return resultLis;
        }

        private static List<T> ReadData<T>(DbDataReader reader)
        {
            var list = new List<object>();

            foreach (var item in reader)
            {
                IDictionary<string, object> expando = new ExpandoObject();

                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(item))
                {
                    var obj = propertyDescriptor.GetValue(item);
                    expando.Add(propertyDescriptor.Name, obj);
                }

                list.Add(new Dictionary<string, object>(expando));
            }

            var serialize = JsonConvert.SerializeObject(list);
            return JsonConvert.DeserializeObject<List<T>>(serialize);
        }

    }
}
