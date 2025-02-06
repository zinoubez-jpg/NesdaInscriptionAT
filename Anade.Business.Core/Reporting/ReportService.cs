using AspNetCore.Reporting;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Anade.Business.Core
{
    public class ReportService : IReportService
    {

        public byte[] GenerateReportAsync(string reportName, string reportType, Dictionary<string, object> ListNavPropWithProp, Dictionary<string, string> parameters)
        {

            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("Anade.Business.Core.dll", string.Empty);

            string rdlcFilePath = string.Format("{0}Reports\\{1}.rdlc", fileDirPath, reportName);
            // string headerPath = string.Format("{0}Reports\\SubReport\\Header.rdlc", fileDirPath);
            // string footerPath = string.Format("{0}Reports\\SubReport\\Footer.rdlc", fileDirPath);
            // string visasPath = string.Format("{0}Reports\\SubReport\\Visas.rdlc", fileDirPath);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);
            //LocalReport headerReport = new LocalReport(headerPath);
            //LocalReport footerReport = new LocalReport(footerPath);
            //LocalReport visasReport = new LocalReport(visasPath);
            foreach (KeyValuePair<string, object> entry in ListNavPropWithProp)
            {

                report.AddDataSource(entry.Key, entry.Value);
            }


            var mainReport = report.Execute(GetRenderType(reportType), 1, parameters);


            return mainReport.MainStream;

        }

        public byte[] GenerateReportWithParamsOnlyAsync(string reportName, string reportType, Dictionary<string, string> parameters)
        {

            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("Anade.Business.Core.dll", string.Empty);

            string rdlcFilePath = string.Format("{0}Reports\\{1}.rdlc", fileDirPath, reportName);
            string headerPath = string.Format("{0}Reports\\Header.rdlc", fileDirPath);
            string footerPath = string.Format("{0}Reports\\Footer.rdlc", fileDirPath);
            string visasPath = string.Format("{0}Reports\\Visas.rdlc", fileDirPath);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport report = new LocalReport(rdlcFilePath);
            LocalReport headerReport = new LocalReport(headerPath);
            LocalReport footerReport = new LocalReport(footerPath);
            LocalReport visasReport = new LocalReport(visasPath);


            var mainReport = report.Execute(GetRenderType(reportType), 1, parameters);


            return mainReport.MainStream;
        }

        public RenderType GetRenderType(string reportType)
        {
            var renderType = RenderType.Pdf;
            switch (reportType.ToLower())
            {
                default:
                case "pdf":
                    renderType = RenderType.Pdf;
                    break;
                case "word":
                    renderType = RenderType.Word;
                    break;
                case "excel":
                    renderType = RenderType.Excel;
                    break;
            }

            return renderType;
        }



    }


}
