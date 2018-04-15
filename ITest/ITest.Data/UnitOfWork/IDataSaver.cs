namespace ITest.Data.UnitOfWork
{
    public interface IDataSaver
    {
        void SaveChanges();

        void SaveChangesAsync();
    }
}
