using System.Net;
using UnifieldTech.Models;
using UnifieldTech.Data;
using Microsoft.AspNetCore.Http;
using NuGet.Configuration;

namespace UnifieldTech.ViewModels
{
    public class CpfConfig
    {
        public async Task<HttpResponse> ValidarCPFCliente(string cpf, HttpResponse response, UnifieldTechContext db)
        {
            ValidCPFAttribute a = new ValidCPFAttribute();

            // Validar o CPF antes de adicionar o cliente
            if (!a.IsValid(cpf))
            {
                // CPF inválido
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                //await response.WriteAsJsonAsync(new { ErrorMessage = "CPF inválido" });             
                return response;
            }
            else if (db.Cliente.Any(c => c.CPF == cpf))
            {
                // CPF já cadastrado no banco de dados
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                await response.WriteAsJsonAsync(new { ErrorMessage = "CPF já cadastrado" });
                return response;
            }

            return response;
        }
    }
}

