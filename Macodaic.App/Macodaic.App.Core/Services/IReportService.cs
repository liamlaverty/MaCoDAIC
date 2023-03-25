using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services
{
    public interface IReportService
    {
        List<IReportableEntity> ReportableEntities { get; }

        void RegisterReportableEntity(IReportableEntity reportableEntity);
        void ReportAllEntities();
    }


    public class ReportService : IReportService
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
