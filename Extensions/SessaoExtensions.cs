namespace LojaOnline.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public static class SessaoExtensions   
{
    public static void SetObjetoComoJson(this ISession session, string chave, object valor)
    {
        session.SetString(chave, JsonConvert.SerializeObject(valor));
    }

    public static T? GetObjetoComoJson<T>(this ISession session, string chave)
    {
        var valor = session.GetString(chave);
        return valor == null ? default : JsonConvert.DeserializeObject<T>(valor);
    }
}

