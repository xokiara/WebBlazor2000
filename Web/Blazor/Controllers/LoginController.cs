﻿using Datos.Interfaces;
using Datos.Repositorios;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using System.Security.Claims;

namespace Blazor.Controllers
{
    public class LoginController : Controller
    {
        private readonly Config _config;

        //Llamar repositorio de login y de usuario
        private ILoginRepositorio _loginRepositorio;
        private IUsuarioRepositorio _usuariosRepositorio;

        LoginController(Config config)
        {
            _config = config;
            _loginRepositorio = new LoginRepositorio(config.CadenaConexion);
            _usuariosRepositorio = new UsuarioRepositorio(config.CadenaConexion);
        }

        [HttpPost("autenticar/validar")]
        public async Task<IActionResult> Validacion(Login login)
        {
            string rol = string.Empty;
            try
            {
                bool usuarioValido = await _loginRepositorio.ValidarUsuarioAsync(login);

                if (usuarioValido)
                {
                    Usuario user = await _usuariosRepositorio.GetPorCodigoAsync(login.CodigoUsuario);

                    if (user.EstaActivo)
                    {
                        rol = user.Rol;

                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, user.CodigoUsuario),
                            new Claim(ClaimTypes.Role, user.Rol)
                        };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        //Inicio de sesión
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddMinutes(10) });
                    }
                    else
                    {
                        return LocalRedirect("/login/Usuario inactivo");
                    }
                }
                //Datos incorrectos
                else
                {
                    return LocalRedirect("/login/Datos de usuario invalidos");
                }
            }
            catch (Exception)
            {

            }
            return LocalRedirect("/");
        }

        [HttpGet("/salir")]
        public async Task<IActionResult> Cerrar()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/login");
        }
    }
}
