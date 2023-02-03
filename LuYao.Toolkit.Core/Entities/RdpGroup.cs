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
    [BindTable("RdpGroup", Description = "", ConnName = "LuYao.Toolkit", DbType = DatabaseType.None)]
    public partial class RdpGroup
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

        private DateTime _UpdatedAt;
        /// <summary>更新时间</summary>
        [DisplayName("更新时间")]
        [Description("更新时间")]
        [DataObjectField(false, false, true, 0)]
        [BindColumn("UpdatedAt", "更新时间", "")]
        public DateTime UpdatedAt { get => _UpdatedAt; set { if (OnPropertyChanging("UpdatedAt", value)) { _UpdatedAt = value; OnPropertyChanged("UpdatedAt"); } } }

        private String _Name;
        /// <summary>名称</summary>
        [DisplayName("名称")]
        [Description("名称")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("Name", "名称", "", Master = true)]
        public String Name { get => _Name; set { if (OnPropertyChanging("Name", value)) { _Name = value; OnPropertyChanged("Name"); } } }

        private Int32 _Rank;
        /// <summary>排序</summary>
        [DisplayName("排序")]
        [Description("排序")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Rank", "排序", "")]
        public Int32 Rank { get => _Rank; set { if (OnPropertyChanging("Rank", value)) { _Rank = value; OnPropertyChanged("Rank"); } } }
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
                    case "UpdatedAt": return _UpdatedAt;
                    case "Name": return _Name;
                    case "Rank": return _Rank;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "Id": _Id = (Guid)value; break;
                    case "CreatedAt": _CreatedAt = value.ToDateTime(); break;
                    case "UpdatedAt": _UpdatedAt = value.ToDateTime(); break;
                    case "Name": _Name = Convert.ToString(value); break;
                    case "Rank": _Rank = value.ToInt(); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得RdpGroup字段信息的快捷方式</summary>
        public partial class _
        {
            /// <summary>主键</summary>
            public static readonly Field Id = FindByName("Id");

            /// <summary>创建时间</summary>
            public static readonly Field CreatedAt = FindByName("CreatedAt");

            /// <summary>更新时间</summary>
            public static readonly Field UpdatedAt = FindByName("UpdatedAt");

            /// <summary>名称</summary>
            public static readonly Field Name = FindByName("Name");

            /// <summary>排序</summary>
            public static readonly Field Rank = FindByName("Rank");

            static Field FindByName(String name) => Meta.Table.FindByName(name);
        }

        /// <summary>取得RdpGroup字段名称的快捷方式</summary>
        public partial class __
        {
            /// <summary>主键</summary>
            public const String Id = "Id";

            /// <summary>创建时间</summary>
            public const String CreatedAt = "CreatedAt";

            /// <summary>更新时间</summary>
            public const String UpdatedAt = "UpdatedAt";

            /// <summary>名称</summary>
            public const String Name = "Name";

            /// <summary>排序</summary>
            public const String Rank = "Rank";
        }
        #endregion
    }
}