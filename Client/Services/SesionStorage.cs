using Blazored.LocalStorage;

namespace EnergyDistribution.Client.Services
{
    public static class SesionStorage
    {

        public static async Task GuardarStorage<T>(this ILocalStorageService localStorageService, string key, T item) where T : class
        {
            await localStorageService.SetItemAsStringAsync(key, Crypto.EncriptarObjeto(item, "QAMQ.zd55g5D{Q%>"));
        }

        public static async Task<T?> ObtenerStorage<T>(this ILocalStorageService localStorageService, string key) where T : class
        {
            var itemJson = await localStorageService.GetItemAsStringAsync(key);

            if (itemJson != null)
            {
                try
                {
                    var item = Crypto.DesencriptarObjeto<T>(itemJson, "QAMQ.zd55g5D{Q%>");
                    return item;
                }
                catch
                {

                    return null;
                }

            }
            return null;

        }
    }
}
