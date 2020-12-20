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
    ///  �ں���ҵ��ʵ��������õ��ĸ���ʵ��Repository�ִ������ӿڶ�������ԣ�ʹ����òִ��������ҵ�����
    /// </summary>
    public class TutorClassTypeRepository : BaseRepository<TutorClassType, Int32>, ITutorClassTypeRepository
    {
        /// <summary>
   ///     �ִ�����ʵ�֡�����Ŀ�����Ϣ
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