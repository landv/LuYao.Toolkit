using CsvHelper.Configuration;
using System.Text;

namespace LuYao.Toolkit.Rdm;

public class RdpConnectionCsvMap : ClassMap<RdpConnection>
{
    public RdpConnectionCsvMap()
    {
        Map(i => i.Id).Name(RenameSnakeCase(nameof(RdpConnection.Id)));

        Map(i => i.UpdatedAt).Name(RenameSnakeCase(nameof(RdpConnection.UpdatedAt)));

        Map(i => i.Name).Name(RenameSnakeCase(nameof(RdpConnection.Name)));
        Map(i => i.Host).Name(RenameSnakeCase(nameof(RdpConnection.Host)));
        Map(i => i.Port).Name(RenameSnakeCase(nameof(RdpConnection.Port)));
        Map(i => i.Username).Name(RenameSnakeCase(nameof(RdpConnection.Username)));
        Map(i => i.Password).Name(RenameSnakeCase(nameof(RdpConnection.Password)));

        Map(i => i.GroupName).Name(RenameSnakeCase(nameof(RdpConnection.GroupName)));

        Map(i => i.AllowAnimation).Name(RenameSnakeCase(nameof(RdpConnection.AllowAnimation)));
        Map(i => i.AllowContents).Name(RenameSnakeCase(nameof(RdpConnection.AllowContents)));
        Map(i => i.AllowThemes).Name(RenameSnakeCase(nameof(RdpConnection.AllowThemes)));
        Map(i => i.AllowWallpaper).Name(RenameSnakeCase(nameof(RdpConnection.AllowWallpaper)));
        Map(i => i.AudioSetting).Name(RenameSnakeCase(nameof(RdpConnection.AudioSetting)));
        Map(i => i.AuthenticationLevel).Name(RenameSnakeCase(nameof(RdpConnection.AuthenticationLevel)));
        Map(i => i.AutoExpand).Name(RenameSnakeCase(nameof(RdpConnection.AutoExpand)));
        Map(i => i.BitmapCaching).Name(RenameSnakeCase(nameof(RdpConnection.BitmapCaching)));
        Map(i => i.ColorDepth).Name(RenameSnakeCase(nameof(RdpConnection.ColorDepth)));
        Map(i => i.ConnectToConsole).Name(RenameSnakeCase(nameof(RdpConnection.ConnectToConsole)));
        Map(i => i.DesktopSize).Name(RenameSnakeCase(nameof(RdpConnection.DesktopSize)));
        Map(i => i.DisplayHeight).Name(RenameSnakeCase(nameof(RdpConnection.DisplayHeight)));
        Map(i => i.DisplayWidth).Name(RenameSnakeCase(nameof(RdpConnection.DisplayWidth)));
        Map(i => i.Domain).Name(RenameSnakeCase(nameof(RdpConnection.Domain)));
        Map(i => i.EnableCredSspSupport).Name(RenameSnakeCase(nameof(RdpConnection.EnableCredSspSupport)));
        Map(i => i.KeyboardSetting).Name(RenameSnakeCase(nameof(RdpConnection.KeyboardSetting)));
        Map(i => i.RedirectDisks).Name(RenameSnakeCase(nameof(RdpConnection.RedirectDisks)));
        Map(i => i.RedirectPorts).Name(RenameSnakeCase(nameof(RdpConnection.RedirectPorts)));
        Map(i => i.RedirectPrinters).Name(RenameSnakeCase(nameof(RdpConnection.RedirectPrinters)));
        Map(i => i.RedirectSmartCards).Name(RenameSnakeCase(nameof(RdpConnection.RedirectSmartCards)));
        Map(i => i.SmartSizing).Name(RenameSnakeCase(nameof(RdpConnection.SmartSizing)));
        Map(i => i.Remark).Name(RenameSnakeCase(nameof(RdpConnection.Remark)));
    }
    private static string RenameSnakeCase(string name)
    {
        var builder = new StringBuilder();
        var previousUpper = false;

        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c))
            {
                if (i > 0 && !previousUpper)
                {
                    builder.Append("_");
                }
                builder.Append(char.ToLowerInvariant(c));
                previousUpper = true;
            }
            else
            {
                builder.Append(c);
                previousUpper = false;
            }
        }
        return builder.ToString();
    }
}
