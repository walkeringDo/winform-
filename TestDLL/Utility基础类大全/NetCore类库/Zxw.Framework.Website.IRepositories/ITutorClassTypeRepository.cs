using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Zxw.Framework.NetCore.Attributes;
using Zxw.Framework.NetCore.Repositories;
using Zxw.Framework.Website.Models;

namespace Zxw.Framework.Website.IRepositories
{
  /// <summary>
   ///     仓储操作接口——课程类别信息
   /// </summary>
    public interface ITutorClassTypeRepository:IRepository<TutorClassType, Int32>
    {
        [MemoryCache]
        IList<TutorClassType> GetByMemoryCached(Expression<Func<TutorClassType, bool>> where = null);

        [RedisCache]
        IList<TutorClassType> GetByRedisCached(Expression<Func<TutorClassType, bool>> where = null);
    }
}