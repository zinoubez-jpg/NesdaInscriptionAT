using System;
using System.Collections.Generic;
using System.Text;

namespace Anade.Business.Core
{
    /// <summary>
    /// This enumration represents diffrent type of messages that can be added after business operation executed.
    /// </summary>
    public enum MessageType
    {
        Success = 1,
        Warning = 2,
        Error = 3
    }
}
