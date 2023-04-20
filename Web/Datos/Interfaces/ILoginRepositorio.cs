using Modelos;

namespace Datos.Interfaces
{
    public interface ILoginRepositorio
    {
        //Declarar métodos asincronas
        Task<bool> ValidarUsuarioAsync(Login login);
    }
}
