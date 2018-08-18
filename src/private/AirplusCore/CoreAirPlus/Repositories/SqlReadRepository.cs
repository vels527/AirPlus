using CoreAirPlus.Services;

namespace CoreAirPlus.Repositories
{
    public class SqlReadRepository
    {
        private IDbReadService _db;
        public SqlReadRepository(IDbReadService db)
        {
            _db = db;
        }

    }
}