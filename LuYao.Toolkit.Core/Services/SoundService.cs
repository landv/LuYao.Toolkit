using System.Threading.Tasks;

namespace LuYao.Toolkit.Services
{
    public static class SoundService
    {
        public static async Task Play(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            await ServiceProviderContainer.Provider.PlaySound(url);
        }
    }
}
