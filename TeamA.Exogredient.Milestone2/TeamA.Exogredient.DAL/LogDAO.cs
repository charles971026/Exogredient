using System;
using System.Threading.Tasks;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.CRUD;
using TeamA.Exogredient.AppConstants;
using TeamA.Exogredient.DataHelpers;
using System.Collections.Generic;


namespace TeamA.Exogredient.DAL
{
    /// <summary>
    /// DAO for accessing the logs in the data store.
    /// </summary>
    public class LogDAO : IMasterNOSQLDAO<string>
    {
        private string NOSQLConnection;
        public LogDAO(string connection)
        {
            NOSQLConnection = connection;
        }

        /// <summary>
        /// Asynchronously inserts the <paramref name="record"/>'s data into the data store defined
        /// by the <paramref name="groupName"/>.
        /// </summary>
        /// <param name="record">The data to insert into the data store (INOSQLRecord)</param>
        /// <param name="groupName">The name of the group to create (string)</param>
        /// <returns>Task(bool) whether the function executed without exception</returns>
        public async Task<bool> CreateAsync(INOSQLRecord record, string groupName)
        {
            // Try converting the INOSQLRecord to a LogRecord, if it failed throw an argument exception.
            try
            {
                LogRecord temp = (LogRecord)record;
            }
            catch
            {
                throw new ArgumentException(Constants.LogCreateInvalidArgument);
            }

            LogRecord logRecord = (LogRecord)record;

            // Get the session inside a using statement to properly dispose/close.
            using (Session session = MySQLX.GetSession(NOSQLConnection))
            {
                // Create the schema if it doesn't exist.
                Schema schema;
                try
                {
                    schema = session.CreateSchema(Constants.LogsSchemaName);
                }
                catch
                {
                    schema = session.GetSchema(Constants.LogsSchemaName);
                }

                // Create the collection if it doesn't exist, then store the result in a variable.
                var collection = schema.CreateCollection(Constants.LogsCollectionPrefix + groupName, ReuseExisting: true);

                // Create json string to insert into the data store.
                string document = $@"{{""{Constants.LogsTimestampField}"": ""{logRecord.Timestamp}"", " +
                                  $@"""{Constants.LogsOperationField}"": ""{logRecord.Operation}"", " +
                                  $@"""{Constants.LogsIdentifierField}"": ""{logRecord.Identifier}"", " +
                                  $@"""{Constants.LogsIPAddressField}"": ""{logRecord.IPAddress}"", " +
                                  $@"""{Constants.LogsErrorTypeField}"": ""{logRecord.ErrorType}""}}";

                // Asynchronously add the document to the data store.
                await collection.Add(document).ExecuteAsync().ConfigureAwait(false);

                return true;
            }
        }

        /// <summary>
        /// Asynchronously deletes the record defined by the <paramref name="uniqueId"/> from
        /// the data store further defined by the <paramref name="groupName"/>.
        /// </summary>
        /// <param name="uniqueId">The id of the record in the data store (string)</param>
        /// <param name="groupName">The name of the group the record is stored in (string)</param>
        /// <returns>Task (bool) whether the function executed without exception</returns>
        public async Task<bool> DeleteAsync(string uniqueId, string groupName)
        {
            // Get the session inside a using statement to properly dispose/close.
            using (Session session = MySQLX.GetSession(NOSQLConnection))
            {
                // Get the schema and collection.
                Schema schema = session.GetSchema(Constants.LogsSchemaName);
                var collection = schema.GetCollection(Constants.LogsCollectionPrefix + groupName);

                // Asynchronously execute a find on the id field where the value equals the uniqueId.
                DocResult result = await collection.Find($"{Constants.LogsIdField} = :id").Bind("id", uniqueId).ExecuteAsync().ConfigureAwait(false);

                string resultstring = "";

                while (result.Next())
                {
                    resultstring = (string)result.Current[Constants.LogsIdField];
                }

                // If the uniqueId passed to the function was not found in the data store, throw an argument exception.
                if (resultstring.Equals(""))
                {
                    throw new ArgumentException(Constants.LogDeleteDNE);
                }

                // Otherwise asynchronously execute a remove on the id field where the value equals the uniqueId.
                await collection.Remove($"{Constants.LogsIdField} = :id").Bind("id", uniqueId).ExecuteAsync().ConfigureAwait(false);

                return true;
            }
        }

        /// <summary>
        /// Find the id of the record that was inserted into the data store.
        /// </summary>
        /// <param name="record">The record to find (INOSQLRecord)</param>
        /// <param name="groupName">The name of the group where the record is located (string)</param>
        /// <returns>Task (string), the id of the record</returns>
        public async Task<string> FindIdFieldAsync(INOSQLRecord record, string groupName)
        {
            // Try converting the INOSQLRecord to a LogRecord, if it failed throw an argument exception.
            try
            {
                LogRecord temp = (LogRecord)record;
            }
            catch
            {
                throw new ArgumentException(Constants.LogFindInvalidArgument);
            }

            LogRecord logRecord = (LogRecord)record;

            // Get the session inside a using statement to properly dispose/close.
            using (Session session = MySQLX.GetSession(NOSQLConnection))
            {
                // Get the schema and collection.
                Schema schema = session.GetSchema(Constants.LogsSchemaName);
                var collection = schema.GetCollection(Constants.LogsCollectionPrefix + groupName);

                // The items of the log record which all need to be found.
                var documentParams = new DbDoc(new { timestamp = logRecord.Timestamp, operation = logRecord.Operation, identifier = logRecord.Identifier, ip = logRecord.IPAddress });

                // Asynchronously execute a find on the fields that correspond to the parameter values above.
                DocResult result = await collection.Find($"{Constants.LogsTimestampField} = :timestamp && {Constants.LogsOperationField} = :operation && {Constants.LogsIdentifierField} = :identifier && {Constants.LogsIPAddressField} = :ip").Bind(documentParams).ExecuteAsync().ConfigureAwait(false);

                // Prepare string to be returned
                string resultstring = "";

                while (result.Next())
                {
                    resultstring = (string)result.Current[Constants.LogsIdField];
                }

                if (resultstring.Equals(""))
                {
                    throw new ArgumentException(Constants.LogFindDNE);
                }

                return resultstring;
            }
        }

        /// <summary>
        /// An async method to read all the logs in a specified month.
        /// </summary>
        /// <param name="year">Year needed to get specific month.</param>
        /// <param name="month">Month needed to get specific logs.</param>
        /// <param name="amountOfDays">Amount of days needed for organization.</param>
        /// <returns>A list of list of the LogResult ofbject.</returns>
        public async Task<List<List<LogResult>>> ReadSpecificMonthAsync(string year, string month, int amountOfDays)
        {

            List<List<LogResult>> logsForMonth = new List<List<LogResult>>();

            // Get the session inside a using statement to properly dispose/close.
            using (Session session = MySQLX.GetSession(NOSQLConnection))
            {
                // Get log schema.
                Schema schema = session.GetSchema(Constants.LogsSchemaName);

                // Use the amountOfDays variable to loop to get dates.
                for (int date = 1; date < amountOfDays + 1; date ++)
                {
                    // Make group name based on year and month.
                    string groupName = year + month;
                    string day = "";

                    if (date < 10)
                    {
                        // If the date is less than 10, a "0" has to be added for formatting purposes.
                        day += "0";
                    }
                    // Convert date into a string for use.
                    day += Convert.ToString(date);

                    groupName += day;

                    // Get logs for a specific day.
                    var collection = schema.GetCollection(Constants.LogsCollectionPrefix + groupName);

                    // Need to check if collection exists beforehand.
                    List<LogResult> logsForDay = new List<LogResult>();
                    if (collection.ExistsInDatabase()) {
                        DocResult result = await collection.Find().ExecuteAsync().ConfigureAwait(false);
                        while (result.Next())
                        {
                            LogResult logObject = new LogResult((string)result.Current[Constants.LogsTimestampField], (string)result.Current[Constants.LogsOperationField],
                                (string)result.Current[Constants.LogsIdentifierField], (string)result.Current[Constants.LogsIPAddressField], (string)result.Current[Constants.LogsErrorTypeField]);
                            logsForDay.Add(logObject);
                        }
                    }
                    logsForMonth.Add(logsForDay);
                }            
            }
            return logsForMonth;        
        }
    
    }
}
