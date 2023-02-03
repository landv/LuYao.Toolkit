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
    [BindIndex("IU_ViewModelState_Type", true, "Type")]
    [BindTable("ViewModelState", Description = "", ConnName = "LuYao.Toolkit", DbType = DatabaseType.None)]
    public partial class ViewModelState
    {
        #region 属性
        private Int32 _Id;
        /// <summary>自增主键</summary>
        [DisplayName("自增主键")]
        [Description("自增主键")]
        [DataObjectField(true, true, false, 0)]
        [BindColumn("Id", "自增主键", "")]
        public Int32 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

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

        private String _Type;
        /// <summary>类型</summary>
        [DisplayName("类型")]
        [Description("类型")]
        [DataObjectField(false, false, true, 512)]
        [BindColumn("Type", "类型", "")]
        public String Type { get => _Type; set { if (OnPropertyChanging("Type", value)) { _Type = value; OnPropertyChanged("Type"); } } }

        private String _Value;
        /// <summary>值</summary>
        [DisplayName("值")]
        [Description("值")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn("Value", "值", "")]
        public String Value { get => _Value; set { if (OnPropertyChanging("Value", value)) { _Value = value; OnPropertyChanged("Value"); } } }
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
                    case "Type": return _Type;
                    case "Value": return _Value;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "Id": _Id = value.ToInt(); break;
                    case "CreatedAt": _CreatedAt = value.ToDateTime(); break;
                    case "UpdatedAt": _UpdatedAt = value.ToDateTime(); break;
                    case "Type": _Type = Convert.ToString(value); break;
                    case "Value": _Value = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得ViewModelState字段信息的快捷方式</summary>
        public partial class _
        {
            /// <summary>自增主键</summary>
            public static readonly Field Id = FindByName("Id");

            /// <summary>创建时间</summary>
            public static readonly Field CreatedAt = FindByName("CreatedAt");

            /// <summary>更新时间</summary>
            public static readonly Field UpdatedAt = FindByName("UpdatedAt");

            /// <summary>类型</summary>
            public static readonly Field Type = FindByName("Type");

            /// <summary>值</summary>
            public static readonly Field Value = FindByName("Value");

            static Field FindByName(String name) => Meta.Table.FindByName(name);
        }

        /// <summary>取得ViewModelState字段名称的快捷方式</summary>
        public partial class __
        {
            /// <summary>自增主键</summary>
            public const String Id = "Id";

            /// <summary>创建时间</summary>
            public const String CreatedAt = "CreatedAt";

            /// <summary>更新时间</summary>
            public const String UpdatedAt = "UpdatedAt";

            /// <summary>类型</summary>
            public const String Type = "Type";

            /// <summary>值</summary>
            public const String Value = "Value";
        }
        #endregion
    }
}