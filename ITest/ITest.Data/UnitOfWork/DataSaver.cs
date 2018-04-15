namespace ITest.Data.UnitOfWork
{
    public class DataSaver : IDataSaver
    {
        private readonly ITestDbContext context;

        public DataSaver(ITestDbContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public async void SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
