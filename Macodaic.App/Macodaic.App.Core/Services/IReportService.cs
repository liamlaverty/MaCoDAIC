using Macodaic.App.Core.Models;

namespace Macodaic.App.Core.Services
{
    public interface IReportService
    {
        List<IReportableEntity> ReportableEntities { get; }

        void RegisterReportableEntity(IReportableEntity reportableEntity);
        void ReportAllEntities();
    }
}
