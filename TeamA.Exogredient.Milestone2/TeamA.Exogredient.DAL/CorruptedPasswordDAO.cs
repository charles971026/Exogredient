﻿using System.Threading.Tasks;
using System.Collections.Generic;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.CRUD;
using TeamA.Exogredient.AppConstants;

namespace TeamA.Exogredient.DAL
{
    public class CorruptedPasswordDAO : IMasterNOSQLDAOReadOnly
    {
        public async Task<List<string>> ReadAsync()
        {
            using (Session session = MySQLX.GetSession(Constants.NOSQLConnection))
            {

                Schema schema = session.GetSchema(Constants.CorruptedPassSchemaName);

                var collection = schema.GetCollection(Constants.CorruptedPassCollectionName);

                DocResult result = await collection.Find().ExecuteAsync().ConfigureAwait(false);

                List<string> resultList = new List<string>();

                while (result.Next())
                {
                    string temp = (string)result.Current[Constants.CorruptedPassPasswordField];

                    resultList.Add(temp);
                }

                return resultList;
            }
        }
    }
}