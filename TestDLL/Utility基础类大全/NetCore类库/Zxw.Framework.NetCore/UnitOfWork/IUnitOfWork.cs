using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Zxw.Framework.NetCore.UnitOfWork
{   
    /// <summary>
   /// 业务单元操作接口
   /// </summary>
    public interface IUnitOfWork:IDisposable
    {
         IRepo GetRepository<IRepo>();
        int Commit();
        Task<int> CommitAsync();
    }
}
