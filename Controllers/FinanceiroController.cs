using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutenticacaoEFCookie.Controllers
{
    [Authorize(Roles="Financeiro")] // só pode entrar nessa página se tiver autenticado.
    public class FinanceiroController : Controller
    {
        public IActionResult Index(){
            return View();
        }
    }
}