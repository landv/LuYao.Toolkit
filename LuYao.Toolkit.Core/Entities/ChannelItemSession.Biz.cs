using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using NewLife;
using NewLife.Data;
using NewLife.Log;
using NewLife.Model;
using NewLife.Reflection;
using NewLife.Threading;
using NewLife.Web;
using XCode;
using XCode.Cache;
using XCode.Configuration;
using XCode.DataAccessLayer;
using XCode.Membership;
using XCode.Shards;

namespace LuYao.Toolkit.Entities
{
    public partial class ChannelItemSession : Entity<ChannelItemSession>
    {
        #region 对象操作
        static ChannelItemSession()
        {
            // 累加字段，生成 Update xx Set Count=Count+1234 Where xxx
            //var df = Meta.Factory.AdditionalFields;
            //df.Add(nameof(IsFavorite));

            // 过滤器 UserModule、TimeModule、IPModule
        }

        /// <summary>验证并修补数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew">是否插入</param>
        public override void Valid(Boolean isNew)
        {
            // 如果没有脏数据，则不需要进行任何处理
            if (!HasDirty) return;

            // 建议先调用基类方法，基类方法会做一些统一处理
            base.Valid(isNew);

            // 在新插入数据或者修改了指定字段时进行修正
        }

        ///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //protected override void InitData()
        //{
        //    // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        //    if (Meta.Session.Count > 0) return;

        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化ChannelItemSession[ChannelItemSession]数据……");

        //    var entity = new ChannelItemSession();
        //    entity.CreatedAt = DateTime.Now;
        //    entity.LastClick = DateTime.Now;
        //    entity.IsFavorite = 0;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化ChannelItemSession[ChannelItemSession]数据！");
        //}

        ///// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
        ///// <returns></returns>
        //public override Int32 Insert()
        //{
        //    return base.Insert();
        //}

        ///// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        ///// <returns></returns>
        //protected override Int32 OnDelete()
        //{
        //    return base.OnDelete();
        //}
        #endregion

        #region 扩展属性
        #endregion

        #region 扩展查询
        /// <summary>根据主键查找</summary>
        /// <param name="id">主键</param>
        /// <returns>实体对象</returns>
        public static ChannelItemSession FindById(Guid id)
        {

            // 实体缓存
            if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);

            // 单对象缓存
            return Meta.SingleCache[id];

            //return Find(_.Id == id);
        }

        /// <summary>根据收藏查找</summary>
        /// <param name="isFavorite">收藏</param>
        /// <returns>实体列表</returns>
        public static IList<ChannelItemSession> FindAllByIsFavorite(Int32 isFavorite)
        {
            // 实体缓存
            if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.IsFavorite == isFavorite);

            return FindAll(_.IsFavorite == isFavorite);
        }
        #endregion

        #region 高级查询
        /// <summary>高级查询</summary>
        /// <param name="lastClick">最后点击</param>
        /// <param name="isFavorite">收藏</param>
        /// <param name="start">创建时间开始</param>
        /// <param name="end">创建时间结束</param>
        /// <param name="key">关键字</param>
        /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
        /// <returns>实体列表</returns>
        public static IList<ChannelItemSession> Search(DateTime lastClick, Int32 isFavorite, DateTime start, DateTime end, String key, PageParameter page)
        {
            var exp = new WhereExpression();

            if (isFavorite >= 0) exp &= _.IsFavorite == isFavorite;
            exp &= _.CreatedAt.Between(start, end);

            return FindAll(exp, page);
        }

        // Select Count(Id) as Id,Category From ChannelItemSession Where LastClick>'2020-01-24 00:00:00' Group By Category Order By Id Desc limit 20
        //static readonly FieldCache<ChannelItemSession> _CategoryCache = new FieldCache<ChannelItemSession>(nameof(Category))
        //{
        //Where = _.LastClick > DateTime.Today.AddDays(-30) & Expression.Empty
        //};

        ///// <summary>获取类别列表，字段缓存10分钟，分组统计数据最多的前20种，用于魔方前台下拉选择</summary>
        ///// <returns></returns>
        //public static IDictionary<String, String> GetCategoryList() => _CategoryCache.FindAllName();
        public static IList<ChannelItemSession> FindAll(int limit)
        {
            var order = _.IsFavorite.Asc() & _.LastClick.Desc();
            return FindAll(null, order, null, 0, limit);
        }

        public static bool HasSessin() => Meta.Count > 0;
        
        #endregion

        #region 业务操作
        #endregion
    }
}