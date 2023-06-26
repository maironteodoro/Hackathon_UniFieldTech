using UnifieldTech.Data;
using System.Net;

namespace UnifieldTech.ViewModels
{
    public class Idade
    {
        public async Task<HttpResponse> MaiorIdade(DateTime date, HttpResponse response)
        {
            DateTime dataAtual = DateTime.Now;
            int idade = dataAtual.Year - date.Year;
            if (date > dataAtual.AddYears(-idade))
                idade--;
            if (idade < 18)
            {
                // Retornar uma resposta de erro informando a idade mínima requerida
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                await response.WriteAsJsonAsync($"Você deve ter no mínimo 18 anos para realizar está operação.");
                return response;
            }
            return response;
        }
    }
}
