using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace LuYao.Toolkit.Entities
{
    /// <summary></summary>
    [Serializable]
    [DataObject]
    [BindIndex("IX_ChannelItemSession_CreatedAt", false, "CreatedAt")]
    [BindIndex("IX_ChannelItemSession_LastClick", false, "LastClick")]
    [BindIndex("IX_ChannelItemSession_IsFavorite", false, "IsFavorite")]
    [BindTable("ChannelItemSession", Description = "", ConnName = "LuYao.Toolkit", DbType = DatabaseType.None)]
    public partial class ChannelItemSession
    {
        #region 属性
        private Guid _Id;
        /// <summary>主键</summary>
        [DisplayName("主键")]
        [Description("主键")]
        [DataObjectField(true, false, false, 0)]
        [BindColumn("Id", "主键", "")]
        public Guid Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

        private DateTime _CreatedAt;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, true, 0)]
        [BindColumn("CreatedAt", "创建时间", "")]
        public DateTime CreatedAt { get => _CreatedAt; set { if (OnPropertyChanging("CreatedAt", value)) { _CreatedAt = value; OnPropertyChanged("CreatedAt"); } } }

        private DateTime _LastClick;
        /// <summary>最后点击</summary>
        [DisplayName("最后点击")]
        [Description("最后点击")]
        [DataObjectField(false, false, true, 0)]
        [BindColumn("LastClick", "最后点击", "")]
        public DateTime LastClick { get => _LastClick; set { if (OnPropertyChanging("LastClick", value)) { _LastClick = value; OnPropertyChanged("LastClick"); } } }

        private Int32 _IsFavorite;
        /// <summary>收藏</summary>
        [DisplayName("收藏")]
        [Description("收藏")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("IsFavorite", "收藏", "")]
        public Int32 IsFavorite { get => _IsFavorite; set { if (OnPropertyChanging("IsFavorite", value)) { _IsFavorite = value; OnPropertyChanged("IsFavorite"); } } }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public override Object this[String name]
        {
            get
            {
                switch (name)
                {
                    case "Id": return _Id;
                    case "CreatedAt": return _CreatedAt;
                    case "LastClick": return _LastClick;
                    case "IsFavorite": return _IsFavorite;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "Id": _Id = (Guid)value; break;
                    case "CreatedAt": _CreatedAt = value.ToDateTime(); break;
                    case "LastClick": _LastClick = value.ToDateTime(); break;
                    case "IsFavorite": _IsFavorite = value.ToInt(); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得ChannelItemSession字段信息的快捷方式</summary>
        public partial class _
        {
            /// <summary>主键</summary>
            public static readonly Field Id = FindByName("Id");

            /// <summary>创建时间</summary>
            public static readonly Field CreatedAt = FindByName("CreatedAt");

            /// <summary>最后点击</summary>
            public static readonly Field LastClick = FindByName("LastClick");

            /// <summary>收藏</summary>
            public static readonly Field IsFavorite = FindByName("IsFavorite");

            static Field FindByName(String name) => Meta.Table.FindByName(name);
        }

        /// <summary>取得ChannelItemSession字段名称的快捷方式</summary>
        public partial class __
        {
            /// <summary>主键</summary>
            public const String Id = "Id";

            /// <summary>创建时间</summary>
            public const String CreatedAt = "CreatedAt";

            /// <summary>最后点击</summary>
            public const String LastClick = "LastClick";

            /// <summary>收藏</summary>
            public const String IsFavorite = "IsFavorite";
        }
        #endregion
    }
}