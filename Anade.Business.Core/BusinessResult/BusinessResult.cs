using System.Collections.Generic;
using System.Resources;

namespace Anade.Business.Core
{
    /// <summary>
    /// This class represents the result of a busines operation execution.
    /// The Succeeded property value indicate the succes or not of the busines operation.
    /// The Messages property contains the list of messages returned in case of success, warning or error.
    /// </summary>
    public class BusinessResult
    {
        //public BusinessResult(bool succeeded=true)
        public BusinessResult()
        {
            Messages = new HashSet<MessageResult>();

        }
        public BusinessResult(bool succeeded)
        {
            Messages = new HashSet<MessageResult>();
            Succeeded = succeeded;

        }
        public ICollection<MessageResult> Messages { get; }
        public bool Succeeded { get; protected set; }
        public static BusinessResult Success
        {
            get
            {
                ResourceManager resourceManager = new ResourceManager("Anade.Business.Core.Resources.BusinessMessage", typeof(BusinessResult).Assembly);

                var result = new BusinessResult(true);
                result.Messages.Add(new MessageResult { Message = resourceManager.GetString("OperationSuccess"), MessageType = MessageType.Success });
                return result;
            }
        }
        public static BusinessResult Failure
        {
            get
            {
                ResourceManager resourceManager = new ResourceManager("Anade.Business.Core.Resources.BusinessMessage", typeof(BusinessResult).Assembly);

                var result = new BusinessResult(false);
                result.Messages.Add(new MessageResult { Message = resourceManager.GetString("OperationFailed"), MessageType = MessageType.Warning });
                return result;
            }
        }
        public static BusinessResult CheckBusinessResults(params BusinessResult[] businessResults)
        {
            foreach (var businessResult in businessResults)
            {
                if (!businessResult.Succeeded)
                    return businessResult;
            }
            return Success;
        }


    }
}
