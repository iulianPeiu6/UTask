using Microsoft.EntityFrameworkCore;
using UTask.DataAccess.Context;

namespace UTask.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UTaskContext dbContext;

        public UnitOfWork(UTaskContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Commit()
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
    }
}
