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
    [BindTable("RdpConnection", Description = "", ConnName = "LuYao.Toolkit", DbType = DatabaseType.None)]
    public partial class RdpConnection
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

        private Guid _GroupId;
        /// <summary>分组编号</summary>
        [DisplayName("分组编号")]
        [Description("分组编号")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("GroupId", "分组编号", "")]
        public Guid GroupId { get => _GroupId; set { if (OnPropertyChanging("GroupId", value)) { _GroupId = value; OnPropertyChanged("GroupId"); } } }

        private String _Remark;
        /// <summary>备注</summary>
        [DisplayName("备注")]
        [Description("备注")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn("Remark", "备注", "")]
        public String Remark { get => _Remark; set { if (OnPropertyChanging("Remark", value)) { _Remark = value; OnPropertyChanged("Remark"); } } }

        private String _Host;
        /// <summary></summary>
        [DisplayName("Host")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("Host", "", "")]
        public String Host { get => _Host; set { if (OnPropertyChanging("Host", value)) { _Host = value; OnPropertyChanged("Host"); } } }

        private Int32 _Port;
        /// <summary></summary>
        [DisplayName("Port")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Port", "", "")]
        public Int32 Port { get => _Port; set { if (OnPropertyChanging("Port", value)) { _Port = value; OnPropertyChanged("Port"); } } }

        private String _Domain;
        /// <summary></summary>
        [DisplayName("Domain")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("Domain", "", "")]
        public String Domain { get => _Domain; set { if (OnPropertyChanging("Domain", value)) { _Domain = value; OnPropertyChanged("Domain"); } } }

        private String _Username;
        /// <summary></summary>
        [DisplayName("Username")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("Username", "", "")]
        public String Username { get => _Username; set { if (OnPropertyChanging("Username", value)) { _Username = value; OnPropertyChanged("Username"); } } }

        private String _Password;
        /// <summary></summary>
        [DisplayName("Password")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("Password", "", "")]
        public String Password { get => _Password; set { if (OnPropertyChanging("Password", value)) { _Password = value; OnPropertyChanged("Password"); } } }

        private Boolean _ConnectToConsole;
        /// <summary></summary>
        [DisplayName("ConnectToConsole")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("ConnectToConsole", "", "")]
        public Boolean ConnectToConsole { get => _ConnectToConsole; set { if (OnPropertyChanging("ConnectToConsole", value)) { _ConnectToConsole = value; OnPropertyChanged("ConnectToConsole"); } } }

        private Int32 _DesktopSize;
        /// <summary></summary>
        [DisplayName("DesktopSize")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("DesktopSize", "", "")]
        public Int32 DesktopSize { get => _DesktopSize; set { if (OnPropertyChanging("DesktopSize", value)) { _DesktopSize = value; OnPropertyChanged("DesktopSize"); } } }

        private Int32 _DisplayWidth;
        /// <summary></summary>
        [DisplayName("DisplayWidth")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("DisplayWidth", "", "")]
        public Int32 DisplayWidth { get => _DisplayWidth; set { if (OnPropertyChanging("DisplayWidth", value)) { _DisplayWidth = value; OnPropertyChanged("DisplayWidth"); } } }

        private Int32 _DisplayHeight;
        /// <summary></summary>
        [DisplayName("DisplayHeight")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("DisplayHeight", "", "")]
        public Int32 DisplayHeight { get => _DisplayHeight; set { if (OnPropertyChanging("DisplayHeight", value)) { _DisplayHeight = value; OnPropertyChanged("DisplayHeight"); } } }

        private Boolean _AutoExpand;
        /// <summary></summary>
        [DisplayName("AutoExpand")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("AutoExpand", "", "")]
        public Boolean AutoExpand { get => _AutoExpand; set { if (OnPropertyChanging("AutoExpand", value)) { _AutoExpand = value; OnPropertyChanged("AutoExpand"); } } }

        private Boolean _SmartSizing;
        /// <summary></summary>
        [DisplayName("SmartSizing")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("SmartSizing", "", "")]
        public Boolean SmartSizing { get => _SmartSizing; set { if (OnPropertyChanging("SmartSizing", value)) { _SmartSizing = value; OnPropertyChanged("SmartSizing"); } } }

        private Int32 _ColorDepth;
        /// <summary></summary>
        [DisplayName("ColorDepth")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("ColorDepth", "", "")]
        public Int32 ColorDepth { get => _ColorDepth; set { if (OnPropertyChanging("ColorDepth", value)) { _ColorDepth = value; OnPropertyChanged("ColorDepth"); } } }

        private Int32 _AudioSetting;
        /// <summary></summary>
        [DisplayName("AudioSetting")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("AudioSetting", "", "")]
        public Int32 AudioSetting { get => _AudioSetting; set { if (OnPropertyChanging("AudioSetting", value)) { _AudioSetting = value; OnPropertyChanged("AudioSetting"); } } }

        private Int32 _KeyboardSetting;
        /// <summary></summary>
        [DisplayName("KeyboardSetting")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("KeyboardSetting", "", "")]
        public Int32 KeyboardSetting { get => _KeyboardSetting; set { if (OnPropertyChanging("KeyboardSetting", value)) { _KeyboardSetting = value; OnPropertyChanged("KeyboardSetting"); } } }

        private Boolean _RedirectDisks;
        /// <summary></summary>
        [DisplayName("RedirectDisks")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("RedirectDisks", "", "")]
        public Boolean RedirectDisks { get => _RedirectDisks; set { if (OnPropertyChanging("RedirectDisks", value)) { _RedirectDisks = value; OnPropertyChanged("RedirectDisks"); } } }

        private Boolean _RedirectPorts;
        /// <summary></summary>
        [DisplayName("RedirectPorts")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("RedirectPorts", "", "")]
        public Boolean RedirectPorts { get => _RedirectPorts; set { if (OnPropertyChanging("RedirectPorts", value)) { _RedirectPorts = value; OnPropertyChanged("RedirectPorts"); } } }

        private Boolean _RedirectPrinters;
        /// <summary></summary>
        [DisplayName("RedirectPrinters")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("RedirectPrinters", "", "")]
        public Boolean RedirectPrinters { get => _RedirectPrinters; set { if (OnPropertyChanging("RedirectPrinters", value)) { _RedirectPrinters = value; OnPropertyChanged("RedirectPrinters"); } } }

        private Boolean _RedirectSmartCards;
        /// <summary></summary>
        [DisplayName("RedirectSmartCards")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("RedirectSmartCards", "", "")]
        public Boolean RedirectSmartCards { get => _RedirectSmartCards; set { if (OnPropertyChanging("RedirectSmartCards", value)) { _RedirectSmartCards = value; OnPropertyChanged("RedirectSmartCards"); } } }

        private Boolean _BitmapCaching;
        /// <summary></summary>
        [DisplayName("BitmapCaching")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("BitmapCaching", "", "")]
        public Boolean BitmapCaching { get => _BitmapCaching; set { if (OnPropertyChanging("BitmapCaching", value)) { _BitmapCaching = value; OnPropertyChanged("BitmapCaching"); } } }

        private Boolean _AllowWallpaper;
        /// <summary></summary>
        [DisplayName("AllowWallpaper")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("AllowWallpaper", "", "")]
        public Boolean AllowWallpaper { get => _AllowWallpaper; set { if (OnPropertyChanging("AllowWallpaper", value)) { _AllowWallpaper = value; OnPropertyChanged("AllowWallpaper"); } } }

        private Boolean _AllowThemes;
        /// <summary></summary>
        [DisplayName("AllowThemes")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("AllowThemes", "", "")]
        public Boolean AllowThemes { get => _AllowThemes; set { if (OnPropertyChanging("AllowThemes", value)) { _AllowThemes = value; OnPropertyChanged("AllowThemes"); } } }

        private Boolean _AllowContents;
        /// <summary></summary>
        [DisplayName("AllowContents")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("AllowContents", "", "")]
        public Boolean AllowContents { get => _AllowContents; set { if (OnPropertyChanging("AllowContents", value)) { _AllowContents = value; OnPropertyChanged("AllowContents"); } } }

        private Boolean _AllowAnimation;
        /// <summary></summary>
        [DisplayName("AllowAnimation")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("AllowAnimation", "", "")]
        public Boolean AllowAnimation { get => _AllowAnimation; set { if (OnPropertyChanging("AllowAnimation", value)) { _AllowAnimation = value; OnPropertyChanged("AllowAnimation"); } } }

        private Int32 _AuthenticationLevel;
        /// <summary></summary>
        [DisplayName("AuthenticationLevel")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("AuthenticationLevel", "", "")]
        public Int32 AuthenticationLevel { get => _AuthenticationLevel; set { if (OnPropertyChanging("AuthenticationLevel", value)) { _AuthenticationLevel = value; OnPropertyChanged("AuthenticationLevel"); } } }

        private Boolean _EnableCredSspSupport;
        /// <summary></summary>
        [DisplayName("EnableCredSspSupport")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("EnableCredSspSupport", "", "")]
        public Boolean EnableCredSspSupport { get => _EnableCredSspSupport; set { if (OnPropertyChanging("EnableCredSspSupport", value)) { _EnableCredSspSupport = value; OnPropertyChanged("EnableCredSspSupport"); } } }
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
                    case "GroupId": return _GroupId;
                    case "Remark": return _Remark;
                    case "Host": return _Host;
                    case "Port": return _Port;
                    case "Domain": return _Domain;
                    case "Username": return _Username;
                    case "Password": return _Password;
                    case "ConnectToConsole": return _ConnectToConsole;
                    case "DesktopSize": return _DesktopSize;
                    case "DisplayWidth": return _DisplayWidth;
                    case "DisplayHeight": return _DisplayHeight;
                    case "AutoExpand": return _AutoExpand;
                    case "SmartSizing": return _SmartSizing;
                    case "ColorDepth": return _ColorDepth;
                    case "AudioSetting": return _AudioSetting;
                    case "KeyboardSetting": return _KeyboardSetting;
                    case "RedirectDisks": return _RedirectDisks;
                    case "RedirectPorts": return _RedirectPorts;
                    case "RedirectPrinters": return _RedirectPrinters;
                    case "RedirectSmartCards": return _RedirectSmartCards;
                    case "BitmapCaching": return _BitmapCaching;
                    case "AllowWallpaper": return _AllowWallpaper;
                    case "AllowThemes": return _AllowThemes;
                    case "AllowContents": return _AllowContents;
                    case "AllowAnimation": return _AllowAnimation;
                    case "AuthenticationLevel": return _AuthenticationLevel;
                    case "EnableCredSspSupport": return _EnableCredSspSupport;
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
                    case "GroupId": _GroupId = (Guid)value; break;
                    case "Remark": _Remark = Convert.ToString(value); break;
                    case "Host": _Host = Convert.ToString(value); break;
                    case "Port": _Port = value.ToInt(); break;
                    case "Domain": _Domain = Convert.ToString(value); break;
                    case "Username": _Username = Convert.ToString(value); break;
                    case "Password": _Password = Convert.ToString(value); break;
                    case "ConnectToConsole": _ConnectToConsole = value.ToBoolean(); break;
                    case "DesktopSize": _DesktopSize = value.ToInt(); break;
                    case "DisplayWidth": _DisplayWidth = value.ToInt(); break;
                    case "DisplayHeight": _DisplayHeight = value.ToInt(); break;
                    case "AutoExpand": _AutoExpand = value.ToBoolean(); break;
                    case "SmartSizing": _SmartSizing = value.ToBoolean(); break;
                    case "ColorDepth": _ColorDepth = value.ToInt(); break;
                    case "AudioSetting": _AudioSetting = value.ToInt(); break;
                    case "KeyboardSetting": _KeyboardSetting = value.ToInt(); break;
                    case "RedirectDisks": _RedirectDisks = value.ToBoolean(); break;
                    case "RedirectPorts": _RedirectPorts = value.ToBoolean(); break;
                    case "RedirectPrinters": _RedirectPrinters = value.ToBoolean(); break;
                    case "RedirectSmartCards": _RedirectSmartCards = value.ToBoolean(); break;
                    case "BitmapCaching": _BitmapCaching = value.ToBoolean(); break;
                    case "AllowWallpaper": _AllowWallpaper = value.ToBoolean(); break;
                    case "AllowThemes": _AllowThemes = value.ToBoolean(); break;
                    case "AllowContents": _AllowContents = value.ToBoolean(); break;
                    case "AllowAnimation": _AllowAnimation = value.ToBoolean(); break;
                    case "AuthenticationLevel": _AuthenticationLevel = value.ToInt(); break;
                    case "EnableCredSspSupport": _EnableCredSspSupport = value.ToBoolean(); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得RdpConnection字段信息的快捷方式</summary>
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

            /// <summary>分组编号</summary>
            public static readonly Field GroupId = FindByName("GroupId");

            /// <summary>备注</summary>
            public static readonly Field Remark = FindByName("Remark");

            /// <summary></summary>
            public static readonly Field Host = FindByName("Host");

            /// <summary></summary>
            public static readonly Field Port = FindByName("Port");

            /// <summary></summary>
            public static readonly Field Domain = FindByName("Domain");

            /// <summary></summary>
            public static readonly Field Username = FindByName("Username");

            /// <summary></summary>
            public static readonly Field Password = FindByName("Password");

            /// <summary></summary>
            public static readonly Field ConnectToConsole = FindByName("ConnectToConsole");

            /// <summary></summary>
            public static readonly Field DesktopSize = FindByName("DesktopSize");

            /// <summary></summary>
            public static readonly Field DisplayWidth = FindByName("DisplayWidth");

            /// <summary></summary>
            public static readonly Field DisplayHeight = FindByName("DisplayHeight");

            /// <summary></summary>
            public static readonly Field AutoExpand = FindByName("AutoExpand");

            /// <summary></summary>
            public static readonly Field SmartSizing = FindByName("SmartSizing");

            /// <summary></summary>
            public static readonly Field ColorDepth = FindByName("ColorDepth");

            /// <summary></summary>
            public static readonly Field AudioSetting = FindByName("AudioSetting");

            /// <summary></summary>
            public static readonly Field KeyboardSetting = FindByName("KeyboardSetting");

            /// <summary></summary>
            public static readonly Field RedirectDisks = FindByName("RedirectDisks");

            /// <summary></summary>
            public static readonly Field RedirectPorts = FindByName("RedirectPorts");

            /// <summary></summary>
            public static readonly Field RedirectPrinters = FindByName("RedirectPrinters");

            /// <summary></summary>
            public static readonly Field RedirectSmartCards = FindByName("RedirectSmartCards");

            /// <summary></summary>
            public static readonly Field BitmapCaching = FindByName("BitmapCaching");

            /// <summary></summary>
            public static readonly Field AllowWallpaper = FindByName("AllowWallpaper");

            /// <summary></summary>
            public static readonly Field AllowThemes = FindByName("AllowThemes");

            /// <summary></summary>
            public static readonly Field AllowContents = FindByName("AllowContents");

            /// <summary></summary>
            public static readonly Field AllowAnimation = FindByName("AllowAnimation");

            /// <summary></summary>
            public static readonly Field AuthenticationLevel = FindByName("AuthenticationLevel");

            /// <summary></summary>
            public static readonly Field EnableCredSspSupport = FindByName("EnableCredSspSupport");

            static Field FindByName(String name) => Meta.Table.FindByName(name);
        }

        /// <summary>取得RdpConnection字段名称的快捷方式</summary>
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

            /// <summary>分组编号</summary>
            public const String GroupId = "GroupId";

            /// <summary>备注</summary>
            public const String Remark = "Remark";

            /// <summary></summary>
            public const String Host = "Host";

            /// <summary></summary>
            public const String Port = "Port";

            /// <summary></summary>
            public const String Domain = "Domain";

            /// <summary></summary>
            public const String Username = "Username";

            /// <summary></summary>
            public const String Password = "Password";

            /// <summary></summary>
            public const String ConnectToConsole = "ConnectToConsole";

            /// <summary></summary>
            public const String DesktopSize = "DesktopSize";

            /// <summary></summary>
            public const String DisplayWidth = "DisplayWidth";

            /// <summary></summary>
            public const String DisplayHeight = "DisplayHeight";

            /// <summary></summary>
            public const String AutoExpand = "AutoExpand";

            /// <summary></summary>
            public const String SmartSizing = "SmartSizing";

            /// <summary></summary>
            public const String ColorDepth = "ColorDepth";

            /// <summary></summary>
            public const String AudioSetting = "AudioSetting";

            /// <summary></summary>
            public const String KeyboardSetting = "KeyboardSetting";

            /// <summary></summary>
            public const String RedirectDisks = "RedirectDisks";

            /// <summary></summary>
            public const String RedirectPorts = "RedirectPorts";

            /// <summary></summary>
            public const String RedirectPrinters = "RedirectPrinters";

            /// <summary></summary>
            public const String RedirectSmartCards = "RedirectSmartCards";

            /// <summary></summary>
            public const String BitmapCaching = "BitmapCaching";

            /// <summary></summary>
            public const String AllowWallpaper = "AllowWallpaper";

            /// <summary></summary>
            public const String AllowThemes = "AllowThemes";

            /// <summary></summary>
            public const String AllowContents = "AllowContents";

            /// <summary></summary>
            public const String AllowAnimation = "AllowAnimation";

            /// <summary></summary>
            public const String AuthenticationLevel = "AuthenticationLevel";

            /// <summary></summary>
            public const String EnableCredSspSupport = "EnableCredSspSupport";
        }
        #endregion
    }
}