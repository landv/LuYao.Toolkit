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
    /// <summary>端口转发规则</summary>
    [Serializable]
    [DataObject]
    [Description("端口转发规则")]
    [BindTable("PortProxyRule", Description = "端口转发规则", ConnName = "LuYao.Toolkit", DbType = DatabaseType.None)]
    public partial class PortProxyRule
    {
        #region 属性
        private Int32 _Id;
        /// <summary>主键</summary>
        [DisplayName("主键")]
        [Description("主键")]
        [DataObjectField(true, true, false, 0)]
        [BindColumn("Id", "主键", "")]
        public Int32 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

        private String _Type;
        /// <summary></summary>
        [DisplayName("Type")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("Type", "", "")]
        public String Type { get => _Type; set { if (OnPropertyChanging("Type", value)) { _Type = value; OnPropertyChanged("Type"); } } }

        private String _GroupName;
        /// <summary></summary>
        [DisplayName("GroupName")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn("GroupName", "", "")]
        public String GroupName { get => _GroupName; set { if (OnPropertyChanging("GroupName", value)) { _GroupName = value; OnPropertyChanged("GroupName"); } } }

        private String _ListenOn;
        /// <summary></summary>
        [DisplayName("ListenOn")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("ListenOn", "", "")]
        public String ListenOn { get => _ListenOn; set { if (OnPropertyChanging("ListenOn", value)) { _ListenOn = value; OnPropertyChanged("ListenOn"); } } }

        private String _ListenPort;
        /// <summary></summary>
        [DisplayName("ListenPort")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("ListenPort", "", "")]
        public String ListenPort { get => _ListenPort; set { if (OnPropertyChanging("ListenPort", value)) { _ListenPort = value; OnPropertyChanged("ListenPort"); } } }

        private String _ConnectTo;
        /// <summary></summary>
        [DisplayName("ConnectTo")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn("ConnectTo", "", "")]
        public String ConnectTo { get => _ConnectTo; set { if (OnPropertyChanging("ConnectTo", value)) { _ConnectTo = value; OnPropertyChanged("ConnectTo"); } } }

        private String _ConnectPort;
        /// <summary></summary>
        [DisplayName("ConnectPort")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("ConnectPort", "", "")]
        public String ConnectPort { get => _ConnectPort; set { if (OnPropertyChanging("ConnectPort", value)) { _ConnectPort = value; OnPropertyChanged("ConnectPort"); } } }

        private String _Comment;
        /// <summary></summary>
        [DisplayName("Comment")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn("Comment", "", "")]
        public String Comment { get => _Comment; set { if (OnPropertyChanging("Comment", value)) { _Comment = value; OnPropertyChanged("Comment"); } } }
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
                    case "Type": return _Type;
                    case "GroupName": return _GroupName;
                    case "ListenOn": return _ListenOn;
                    case "ListenPort": return _ListenPort;
                    case "ConnectTo": return _ConnectTo;
                    case "ConnectPort": return _ConnectPort;
                    case "Comment": return _Comment;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "Id": _Id = value.ToInt(); break;
                    case "Type": _Type = Convert.ToString(value); break;
                    case "GroupName": _GroupName = Convert.ToString(value); break;
                    case "ListenOn": _ListenOn = Convert.ToString(value); break;
                    case "ListenPort": _ListenPort = Convert.ToString(value); break;
                    case "ConnectTo": _ConnectTo = Convert.ToString(value); break;
                    case "ConnectPort": _ConnectPort = Convert.ToString(value); break;
                    case "Comment": _Comment = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得端口转发规则字段信息的快捷方式</summary>
        public partial class _
        {
            /// <summary>主键</summary>
            public static readonly Field Id = FindByName("Id");

            /// <summary></summary>
            public static readonly Field Type = FindByName("Type");

            /// <summary></summary>
            public static readonly Field GroupName = FindByName("GroupName");

            /// <summary></summary>
            public static readonly Field ListenOn = FindByName("ListenOn");

            /// <summary></summary>
            public static readonly Field ListenPort = FindByName("ListenPort");

            /// <summary></summary>
            public static readonly Field ConnectTo = FindByName("ConnectTo");

            /// <summary></summary>
            public static readonly Field ConnectPort = FindByName("ConnectPort");

            /// <summary></summary>
            public static readonly Field Comment = FindByName("Comment");

            static Field FindByName(String name) => Meta.Table.FindByName(name);
        }

        /// <summary>取得端口转发规则字段名称的快捷方式</summary>
        public partial class __
        {
            /// <summary>主键</summary>
            public const String Id = "Id";

            /// <summary></summary>
            public const String Type = "Type";

            /// <summary></summary>
            public const String GroupName = "GroupName";

            /// <summary></summary>
            public const String ListenOn = "ListenOn";

            /// <summary></summary>
            public const String ListenPort = "ListenPort";

            /// <summary></summary>
            public const String ConnectTo = "ConnectTo";

            /// <summary></summary>
            public const String ConnectPort = "ConnectPort";

            /// <summary></summary>
            public const String Comment = "Comment";
        }
        #endregion
    }
}