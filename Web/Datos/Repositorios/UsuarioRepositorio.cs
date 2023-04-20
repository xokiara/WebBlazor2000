﻿using Dapper;
using Datos.Interfaces;
using Modelos;
using MySql.Data.MySqlClient;

namespace Datos.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {

        //Declarar variable de cadena de conexión
        private string CadenaConexion;

        //Declarar constructor
        public UsuarioRepositorio(string _cadenaConexion)
        {
            CadenaConexion = _cadenaConexion;
        }

        //Declarar método de conexion de MYSQL
        private MySqlConnection Conexion()
        {
            return new MySqlConnection(CadenaConexion);
        }

        public async Task<bool> ActualizarAsync(Usuario usuario)
        {
            bool resultado = false;
            try
            {
                using MySqlConnection _conexion = Conexion();
                //Abrir conexion
                await _conexion.OpenAsync();
                string sql = @"UPDATE usuario SET Nombre = @Nombre, Contrasena = @Contrasena, Correo = @Correo, Rol = @Rol, Foto = @Foto, EstaActivo = @EstaActivo
                                WHERE CodigoUsuario = @CodigoUsuario;";
                resultado = Convert.ToBoolean(await _conexion.ExecuteAsync(sql, usuario));
            }
            catch (Exception)
            {
            }
            return resultado;
        }

        public async Task<bool> EliminarAsync(string codigo)
        {
            bool resultado = false;
            try
            {
                using MySqlConnection _conexion = Conexion();
                //Abrir conexion
                await _conexion.OpenAsync();
                string sql = "DELETE FROM usuario WHERE CodigoUsuario = @CodigoUsuario;";
                resultado = Convert.ToBoolean(await _conexion.ExecuteAsync(sql, new { codigo }));
            }
            catch (Exception)
            {
            }
            return resultado;
        }

        //Método devolver la lista
        public async Task<IEnumerable<Usuario>> GetListaAsync()
        {
            IEnumerable<Usuario> lista = new List<Usuario>();
            try
            {
                using MySqlConnection _conexion = Conexion();
                //Abrir conexion
                await _conexion.OpenAsync();
                string sql = "SELECT * FROM usuario;";
                lista = await _conexion.QueryAsync<Usuario>(sql);
            }
            catch (Exception)
            {
            }
            return lista;
        }

        //Método devolver usuario por el código
        public async Task<Usuario> GetPorCodigoAsync(string codigo)
        {
            Usuario user = new Usuario();
            try
            {
                using MySqlConnection _conexion = Conexion();
                //Abrir conexion
                await _conexion.OpenAsync();
                string sql = "SELECT * FROM usuario WHERE CodigoUsuario = @CodigoUsuario;";
                user = await _conexion.QueryFirstAsync<Usuario>(sql, new { codigo });
            }
            catch (Exception)
            {
            }
            return user;
        }

        //Método nuevo usuario
        public async Task<bool> NuevoAsync(Usuario usuario)
        {
            bool resultado = false;
            try
            {
                using MySqlConnection _conexion = Conexion();
                //Abrir conexion
                await _conexion.OpenAsync();
                string sql = @"INSERT INTO usuario (CodigoUsuario,Nombre,Contrasena,Correo,Rol,Foto,FechaCreacion,EstaActivo)
                                VALUES (@CodigoUsuario,@Nombre,@Contrasena,@Correo,@Rol,@Foto,@FechaCreacion,@EstaActivo);";
                resultado = Convert.ToBoolean(await _conexion.ExecuteAsync(sql, usuario));
            }
            catch (Exception)
            {
            }
            return resultado;
        }
    }
}
