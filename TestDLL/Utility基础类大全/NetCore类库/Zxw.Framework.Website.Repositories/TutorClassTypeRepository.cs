using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Zxw.Framework.NetCore.Attributes;
using Zxw.Framework.NetCore.EfDbContext;
using Zxw.Framework.NetCore.Repositories;
using Zxw.Framework.Website.IRepositories;
using Zxw.Framework.Website.Models;

namespace Zxw.Framework.Website.Repositories
{
    /// <summary>
    ///  在核心业务实现中添加用到的各个实体Repository仓储操作接口对象的属性，使其调用仓储操作完成业务操作
    /// </summary>
    public class TutorClassTypeRepository : BaseRepository<TutorClassType, Int32>, ITutorClassTypeRepository
    {
        /// <summary>
   ///     仓储操作实现——科目类别信息
     /// </summary>
        public TutorClassTypeRepository(DefaultDbContext dbContext) : base(dbContext)
        {
        }

        public IList<TutorClassType> GetByMemoryCached(Expression<Func<TutorClassType, bool>> @where = null)
        {
            return Get(where).ToList();
        }

        public IList<TutorClassType> GetByRedisCached(Expression<Func<TutorClassType, bool>> @where = null)
        {
            return Get(where).ToList();
        }
    }
}