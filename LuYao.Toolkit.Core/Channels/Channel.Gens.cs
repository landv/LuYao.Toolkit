using System;

namespace LuYao.Toolkit.Channels;

public partial class Channel
{
    public static GensChannel Gens { get; } = new GensChannel();
    public class GensChannel : Channel
    {
        public FunctionItem GenGuid { get; }
        public FunctionItem GenPassword { get; }
        public FunctionItem GenAesKey { get; }
        public FunctionItem GenRsaKey { get; }
        public FunctionItem GenXCodeEntity { get; }
        public FunctionItem GenLinesByRange { get; }

        public GensChannel() : base(nameof(Gens), "数据生成", Icons.Refresh)
        {
            this.GenGuid = new FunctionItem(this, Guid.Parse("E135CA55B356496D910B7A95BBC552D3"), nameof(GenGuid))
            {
                Title = "生成 GUID",
                Icon = Icons.Earth,
                Description = "生成随机 Guid",
                View = Views.ViewNames.Channels.Gens.GenGuid,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "ShengChengGuid", "SCGuid" },
            };

            this.GenPassword = new FunctionItem(this, Guid.Parse("CFD68DBC4D054968A9C859FA358F2152"), nameof(GenPassword))
            {
                Title = "生成密码",
                Icon = Icons.FormTextboxPassword,
                Description = "生成随机密码",
                View = Views.ViewNames.Channels.Gens.GenPassword,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "ShengChengMiMa", "SCMM" },
            };

            this.GenAesKey = new FunctionItem(this, Guid.Parse("23A02AD776D74772B88919707B4E00F3"), nameof(GenAesKey))
            {
                Title = "生成 AES 密钥",
                Icon = Icons.Key,
                Description = "生成随机 AES 密钥",
                View = Views.ViewNames.Channels.Gens.GenAesKey,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "ShengChengAesMiYao", "SCAESMY" },
            };

            this.GenRsaKey = new FunctionItem(this, Guid.Parse("A5DB45C57100451F8847AA79C9EDEC9F"), nameof(GenRsaKey))
            {
                Title = "生成 RSA 密钥",
                Icon = Icons.KeyPlus,
                Description = "生成随机 RSA 密钥",
                View = Views.ViewNames.Channels.Gens.GenRsaKey,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "ShengChengRsaMiYao", "SCRSAMY" },
            };

            this.GenXCodeEntity = new FunctionItem(this, Guid.Parse("629FE6AC348840459F46FB59E195FA47"), nameof(GenXCodeEntity))
            {
                Title = "生成 XCode 实体",
                Icon = Icons.Clover,
                Description = "通过 XML 文件生成 XCode 实体",
                View = Views.ViewNames.Channels.Gens.GenXCodeEntity,
                IsNew = false,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "ShengChengXCodeShiTi", "SCXCodeST" },
            };

            this.GenLinesByRange = new FunctionItem(this, Guid.Parse("E8972DB2AECD4C88B2EB3038AF7CF835"), nameof(GenLinesByRange))
            {
                Title = "模板批量生成",
                Icon = Icons.FormatLineStyle,
                Description = "通过一个数字范围和字符串模板生成一批字符串，常用于分表系统的迁移语句编写。",
                View = Views.ViewNames.Channels.Gens.GenLinesByRange,
                IsNew = true,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "MuBanPiLiangShengCheng", "MBPLSC" },
            };

            this.Items = new[]
            {
                GenGuid,
                GenPassword,
                GenAesKey,
                GenRsaKey,
                GenXCodeEntity,
                GenLinesByRange,
            };
        }
    }
}
