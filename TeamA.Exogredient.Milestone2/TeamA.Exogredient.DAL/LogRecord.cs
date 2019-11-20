﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TeamA.Exogredient.DAL
{
    public class LogRecord
    {
        public string Timestamp { get; }

        public string Operation { get; }

        public string Identifier { get; }

        public string IPAddress { get; }

        public string ErrorType { get; }

        public List<string> Fields { get; }

        public LogRecord(string timestamp, string operation,
                         string identifier, string ipAddress, string errorType = "null")
        {
            Fields = new List<string>();

            Timestamp = timestamp;
            Fields.Add(timestamp);

            Operation = operation;
            Fields.Add(operation);

            Identifier = identifier;
            Fields.Add(identifier);

            IPAddress = ipAddress;
            Fields.Add(ipAddress);

            ErrorType = errorType;
            Fields.Add(errorType);
        }
    }
}
