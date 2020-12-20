using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Z.EntityFramework.Plus;
using Zxw.Framework.NetCore.EfDbContext;
using Zxw.Framework.NetCore.IoC;

namespace Zxw.Framework.NetCore.UnitOfWork
{
    /// <summary>
    ///  单元操作实现基类( 在单元操作的实现基类中，定义一个只读的DbContext抽象属性，实际的DbContext上下文需要在实现类中进行重写赋值。)
    /// </summary>
    public class EfUnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        /// <summary>
        /// 获取 当前使用的数据访问上下文对象
         /// </summary>
        private DefaultDbContext _context;
        public EfUnitOfWork(DefaultDbContext context)
        {
            _context = context;
        }
            /// <summary>
        ///   通过  依赖注入Autofac IOC 容器  获取或设置 默认的项目数据访问上下文对象
        /// </summary>
        public IRepo GetRepository<IRepo>()
        {
            return AutofacContainer.Resolve<IRepo>(new TypedParameter(typeof(DefaultDbContext), _context));
        }
         /// <summary>
         ///  提交当前单元操作的结果
         /// </summary>
         /// <returns></returns>
        public int Commit()
        {
            return _context.SaveChanges(true);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
