using Macodaic.App.console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macodaic.App.console.Services
{
    internal interface IReportService
    {
        List<IReportableEntity> ReportableEntities { get; }

        void RegisterReportableEntity(IReportableEntity reportableEntity);
        void ReportAllEntities();
    }


    internal class ReportService : IReportService
    {
        public List<IReportableEntity> ReportableEntities { get; private set; }



        public ReportService() 
        {
            ReportableEntities = new List<IReportableEntity>();
        }


        public void RegisterReportableEntity(IReportableEntity reportableEntity)
        {
            ReportableEntities.Add(reportableEntity);
        }

        public void ReportAllEntities()
        {
            for (int i = 0; i < ReportableEntities.Count; i++)
            {
                ReportableEntities[i].Report();
            }
        }
    }
}
