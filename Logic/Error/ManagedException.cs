using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Error
{

    public enum ManagedExceptionType
    { 
        Convertion,
        File,
        NotValidInfo
    }

    public class ManagedException:Exception
    {
        public string Description { get; set; }
        public ManagedExceptionType Type { get; set; }

        public ManagedException():base()
        {
            this.Description = this.Message;
        }

        public ManagedException(ManagedExceptionType type,  string message):base(message)
        {
            this.Type = type;
            this.Description = message;
        }

    }
}
