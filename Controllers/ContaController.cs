using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutenticacaoEFCookie.Dados;
using AutenticacaoEFCookie.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutenticacaoEFCookie.Controllers
{
    public class ContaController : Controller //pq fazemos isso mesmo? Tem a ver com o using Microsoft.AspNetCore.Mvc;
    {
        readonly AutenticacaoContext _contexto;

        public ContaController(AutenticacaoContext context)
        {
            _contexto = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario usuario)
        { //
            try
            {
                Usuario user = _contexto.Usuarios.Include("UsuarioPermissoes")
                .Include("UsuariosPermissoes.Permissao")
                .FirstOrDefault(c => c.Email == usuario.Email && c.Senha == usuario.Senha);
                //se o email for igual ao email que ele passou, E se a senha tb for

                if (user != null)
                {
                    //quer dizer que encontrou alguém
                    var claims = new List<Claim>();
                    //o que é claim? Vou armazenar tudo dentro da claim - claim do usuario. Vai ficar armazenado no CurrentUser
                    //qdo fizer login, preenche as infos na claim. Qdo for navegar em outra página, pode pegar isso da claim do usuario, não precisa ficar perguntando de novo.
                    //Edilson: qdo vc entra numa página, reivindica a autenticação do usuário, o login.
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
                    claims.Add(new Claim(ClaimTypes.Name, user.Nome));
                    claims.Add(new Claim(ClaimTypes.Sid, user.IdUsuario.ToString())); //to passando o Id do usuario, o S-Id para a claim.

                    foreach (var item in user.UsuarioPermissoes)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item.Permissao.Nome));
                    }

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Financeiro");
                }

                return View(usuario);
            }
            catch (System.Exception)
            {
                return null;
            }
        }
        public IActionResult Sair()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}
