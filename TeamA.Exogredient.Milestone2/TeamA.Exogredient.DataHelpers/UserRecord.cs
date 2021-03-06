using System;
using System.Collections.Generic;
using TeamA.Exogredient.AppConstants;

namespace TeamA.Exogredient.DataHelpers
{
    /// <summary>
    /// This object represents a record that is meant to be stored in the User table.
    /// </summary>
    public class UserRecord : ISQLRecord, IMaskableRecord
    {
        private readonly IDictionary<string, object> _data = new Dictionary<string, object>();
        private bool _masked = false;

        /// <summary>
        /// Constructs a UserRecord, the username is the minimum field required as it serves
        /// as identification.
        /// </summary>
        /// <param name="username">The username of the user to be stored in the table (string)</param>
        /// <param name="name">The name of the user to be stored in the table,
        /// if left default it will not be changed during an update (string)</param>
        /// <param name="email">The email address of the user to be stored in the table,
        /// if left default it will not be changed during an update (string)</param>
        /// <param name="phoneNumber">The phone number of the user to be stored in the table,
        /// if left default it will not be changed during an update (string)</param>
        /// <param name="password">The hashed password of the user to be stored in the table,
        /// if left default it will not be changed during an update (string)</param>
        /// <param name="disabled">The disabled status of the user to be stored in the table,
        /// if left default it will not be changed during an update (int)</param>
        /// <param name="userType">The user type of the user to be stored in the table,
        /// if left default it will not be changed during an update (string)</param>
        /// <param name="salt">The salt used in hashing the password to be stored in the table,
        /// if left default it will not be changed during an update (string)</param>
        /// <param name="tempTimestamp">The timestamp the temporary user was created if not NOVALUE to be stored in the table,
        /// if left default it will not be changed during an update (long)</param>
        /// <param name="emailCode">The email code of the user if not NOVALUE to be stored in the table,
        /// if left default it will not be changed during an update (string)</param>
        /// <param name="emailCodeTimestamp">The timestamp the email code was sent if not NOVALUE to be stored in the table,
        /// if left default it will not be changed during an update (long)</param>
        /// <param name="loginFailures">The amount of login failures currently for the user to be stored in the table,
        /// if left default it will not be changed during an update (int)</param>
        /// <param name="lastLoginFailTimestamp">The timestamp of the user's last login failure if not NOVALUE to be stored in the table,
        /// if left default it will not be changed during an update (long)</param>
        /// <param name="emailCodeFailures">The amount of email code failures currently for the user if not NOVALUE to be stored in the table,
        /// if left default it will not be changed during an update (int)</param>
        /// <param name="phoneCodeFailures">The amount of phone code failures currently for the user if not NOVALUE to be stored in the table,
        /// if left default it will not be changed during an update (string)</param>
        public UserRecord(string username, string name = null, string email = null,
                          string phoneNumber = null, string password = null, int disabled = -1, string userType = null, string salt = null,
                          long tempTimestamp = -1, string emailCode = null, long emailCodeTimestamp = -1, int loginFailures = -1,
                          long lastLoginFailTimestamp = -1, int emailCodeFailures = -1, int phoneCodeFailures = -1)
        {
            _data.Add(Constants.UserDAOusernameColumn, username);
            _data.Add(Constants.UserDAOnameColumn, name);
            _data.Add(Constants.UserDAOemailColumn, email);
            _data.Add(Constants.UserDAOphoneNumberColumn, phoneNumber);
            _data.Add(Constants.UserDAOpasswordColumn, password);
            _data.Add(Constants.UserDAOdisabledColumn, disabled);
            _data.Add(Constants.UserDAOuserTypeColumn, userType);
            _data.Add(Constants.UserDAOsaltColumn, salt);
            _data.Add(Constants.UserDAOtempTimestampColumn, tempTimestamp);
            _data.Add(Constants.UserDAOemailCodeColumn, emailCode);
            _data.Add(Constants.UserDAOemailCodeTimestampColumn, emailCodeTimestamp);
            _data.Add(Constants.UserDAOloginFailuresColumn, loginFailures);
            _data.Add(Constants.UserDAOlastLoginFailTimestampColumn, lastLoginFailTimestamp);
            _data.Add(Constants.UserDAOemailCodeFailuresColumn, emailCodeFailures);
            _data.Add(Constants.UserDAOphoneCodeFailuresColumn, phoneCodeFailures);
        }

        /// <summary>
        /// Gets the internal data of this object.
        /// </summary>
        /// <returns>IDictionary of (string, object)</returns>
        public IDictionary<string, object> GetData()
        {
            return _data;
        }

        /// <summary>
        /// Sets the status of this object to "Masked".
        /// </summary>
        public void SetToMasked()
        {
            _masked = true;
        }

        /// <summary>
        /// Evaluates whether or not the object is masked.
        /// </summary>
        /// <returns>(bool) whether the object is masked.</returns>
        public bool IsMasked()
        {
            return _masked;
        }

        /// <summary>
        /// Gets the types of the parameters to this object's constructor.
        /// </summary>
        /// <returns>(Type[]) the array of types of the constructor's parameters</returns>
        public Type[] GetParameterTypes()
        {
            Type[] result = new Type[15]
            {
                typeof(string), typeof(string), typeof(string),
                typeof(string), typeof(string), typeof(int),
                typeof(string), typeof(string), typeof(long),
                typeof(string), typeof(long), typeof(int),
                typeof(long), typeof(int), typeof(int)
            };

            return result;
        }

        /// <summary>
        /// Gets the masking information of this object (a list of tuples: objects and whether they are/should be masked).
        /// </summary>
        /// <returns>(List<Tuple<object, bool>>) the masking information of this object.</object></returns>
        public List<Tuple<object, bool>> GetMaskInformation()
        {
            List<Tuple<object, bool>> result = new List<Tuple<object, bool>>
            {
                new Tuple<object, bool>(_data[Constants.UserDAOusernameColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOusernameColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOnameColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOnameColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOemailColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOemailColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOphoneNumberColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOphoneNumberColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOpasswordColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOpasswordColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOdisabledColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOdisabledColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOuserTypeColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOuserTypeColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOsaltColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOsaltColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOtempTimestampColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOtempTimestampColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOemailCodeColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOemailCodeColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOemailCodeTimestampColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOemailCodeTimestampColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOloginFailuresColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOloginFailuresColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOlastLoginFailTimestampColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOlastLoginFailTimestampColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOemailCodeFailuresColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOemailCodeFailuresColumn]),
                new Tuple<object, bool>(_data[Constants.UserDAOphoneCodeFailuresColumn], Constants.UserDAOIsColumnMasked[Constants.UserDAOphoneCodeFailuresColumn])
            };

            return result;
        }
    }
}
