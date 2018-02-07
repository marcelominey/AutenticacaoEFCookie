using AutenticacaoEFCookie.Models;

namespace AutenticacaoEFCookie.Dados
{
    public class CodeFirstBanco
    {
        public static void Inicializar(AutenticacaoContext contexto){
            contexto.Database.EnsureCreated(); //garante que o banco foi criado

            var usuario = new Usuario(){Nome = "Fernando", Email = "fernando.guerra@corujasdev.com.br", Senha = "123456"};
            contexto.Usuarios.Add(usuario);

            var permissao = new Permissao(){
                Nome = "Financeiro"
            };

            contexto.Permissoes.Add(permissao);

            var usuariopermissao = new UsuarioPermissao(){IdUsuario = usuario.IdUsuario, IdPermissao = permissao.IdPermissao};

            contexto.UsuariosPermissoes.Add(usuariopermissao);
            contexto.SaveChanges();
        }
    }
}