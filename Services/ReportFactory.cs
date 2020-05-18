using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tema2.Services
{
    public class ReportFactory
    {
        public enum ReportTypes { Json, CSV};

        public static Report Create(ReportTypes reportTypes)
        {
            Report report = null;

            if (reportTypes.Equals(ReportTypes.CSV))
            {
                report = new ReportCSV();
                return report;
            }
            else if (reportTypes.Equals(ReportTypes.Json))
            {
                report = new ReportJSON();
                return report;
            }
            return report;
        }
    }
}
