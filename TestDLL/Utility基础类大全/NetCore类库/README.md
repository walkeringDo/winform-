基于EF Core的.NetCore快速开发框架（EF-Code First ：Repository，UnitOfWork，DbContext）

UnitOfWork、Repository模式以及DbContext实现类的构造，来给架构案例添加增删改查功能

此项目主要分成两部分：Zxw.Framework.NetCore （核心类库）和 NetCore.WebsiteProject（项目）两部分


Zxw.Framework.NetCore 项目说明：

    Attributes —— 一些缓存（redis）常用的方法属性
    CodeGenerator —— 代码生成器，用于生成Repository和Service层的代码
    CodeTemplate —— Repository和Service层代码模板
    EfDbContext —— EF上下文
    Extensions —— 一些常用的扩展方法
    Filters —— 一些常用的拦截器
    Helpers —— 一些常用的帮助类
    IoC —— IoC容器封装类，Autofac
    Models —— 实体接口类，IBaseModel<TKey>
    Options —— 一些常用的配置类
    Repositories —— Repository层的父类  （包括BaseRepository与IRepository）
	UnitOfWork——（包括EFUnitOfWork与IUnitOfWork）UnitOfWork模式（主要作用是在数据持久化过程中，数据提，确保数据的完整性，对象使用确保同一上下文(EfDbContext)对象。如果有异常，提供回滚）

框架使用

如 NetCore.WebsiteProject所示，按照此项目结构创建好：

    Zxw.Framework.Website —— 网站（前端后台展示）
    Zxw.Framework.Website.Controllers —— 控制器（通过接口 IUnitOfWork(为了减少与数据库通讯的次数) 调用UnitOfWork模式 调用EfDbContext）
    Zxw.Framework.Website.IRepositories —— 仓储接口继承父类接口IRepository
    Zxw.Framework.Website.Models —— 数据库字段实体
    Zxw.Framework.Website.Repositories —— 仓储继承父类BaseRepository（业务实体的仓储实现：实现各个实体的 Repository 仓储操作（可以发现，通用仓储操作在数据组件中封装好后，在实际业务中只需要编写非常少量的代码即可实现各个实体的仓储操作，这就是封装的好处。））
    Zxw.Framework.Website.ViewModels —— ViewModels 自定义字段

关于 Repository层的作用 
 
       在数据库系统中，对于数据层来说，所有的操作归根结底无非“C（增加）、R（读取）、U（修改）、D（删除）”这四种操作。四种操作当中，与与业务相关度最大的是读取操作，根据各种不同的业务需求提交不同的查询，其最终执行应该放到业务层面中去进行，而增加，修改，删除这三种操作较为通用，可以作为通用数据操作封装到Repository中。


