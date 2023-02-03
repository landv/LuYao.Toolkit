using System;

namespace LuYao.Toolkit.Channels;

public partial class Channel
{
    public static CrossBorderChannel CrossBorder { get; } = new CrossBorderChannel();
    public class CrossBorderChannel : Channel
    {
        public FunctionItem MercadoToWorldFirst { get; }

        public CrossBorderChannel() : base(nameof(CrossBorder), "跨境电商", Icons.Shopping)
        {
            this.MercadoToWorldFirst = new FunctionItem(this, Guid.Parse("846209761E14421BB59B068225443F31"), nameof(MercadoToWorldFirst))
            {
                Title = "美客多转万里汇",
                Icon = Icons.MicrosoftExcel,
                Description = "将美客多订单报表转为万里汇所需 Excel",
                View = Views.ViewNames.Channels.CrossBorder.MercadoToWorldFirst,
                IsNew = true,
                UseNetwork = false,
                IsDocument = false,
                Multiboxing = true,
                Keywords = new string[] { "MeiKeDuo", "MKD", "Mercado", "WanLiHui", "WLH" },
            };

            this.Items = new[]
            {
                MercadoToWorldFirst,
            };
        }
    }
}
