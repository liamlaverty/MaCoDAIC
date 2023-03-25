namespace Macodaic.App.Core.Models

{
    public interface IReportableEntity 
    {
        public abstract void Report();
    }

    public interface ITickableEntity
    {
        public abstract void Tick();
    }
}
