using System;
using System.Collections.Generic;
using System.Text;

namespace Anade.Business.Core
{
    public class DataNotUpdatedException:Exception
    {
        public DataNotUpdatedException()
        {

        }

        public DataNotUpdatedException(string message):base(message)
        {

        }

        public DataNotUpdatedException(string message, Exception innerException):base(message,innerException)
        {

        }

    }
}
