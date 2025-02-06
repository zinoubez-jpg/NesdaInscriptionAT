using AspNetCore.Reporting;
using System.Collections.Generic;

namespace Anade.Business.Core
{
    public interface IReportService
    {
        public byte[] GenerateReportAsync(string reportName, string reportType, Dictionary<string, object> ListNavPropWithProp, Dictionary<string, string> parameters);
        public RenderType GetRenderType(string reportType);
        public byte[] GenerateReportWithParamsOnlyAsync(string reportName, string reportType, Dictionary<string, string> parameters);

    }
}
