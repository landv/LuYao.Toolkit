namespace LuYao.Toolkit.Controls.Utils;

internal static class Boxes
{
    public static readonly object True = true;

    public static readonly object False = false;

    public static object Box(bool value)
    {
        if (!value)
        {
            return False;
        }

        return True;
    }
}