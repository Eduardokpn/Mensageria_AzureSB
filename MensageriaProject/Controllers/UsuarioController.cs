using Azure.Messaging.ServiceBus;
using MensageriaProject.Interfaces;
using MensageriaProject.Models;
using MensageriaProject.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MensageriaProject.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _IUsuarioRepository;
        private readonly ServiceBusService _serviceBusService;
        public readonly IEmailService _emailService;


        public UsuarioController(IUsuarioRepository usuarioRepository, ServiceBusService serviceBusService, IEmailService emailService)
        {
            _IUsuarioRepository = usuarioRepository;
            _serviceBusService = serviceBusService;
            _emailService = emailService;
        }
        [HttpPost]
        public async Task<IActionResult> EnviarEmails()
        {
            var mensagens = await _serviceBusService.ReceberMensagensAsync();

            foreach (var mensagem in mensagens)
            {
                var email = mensagem; // Ajuste conforme o formato da mensagem
                await _emailService.SendEmailResendAsync(email);
            }

            return StatusCode(200, "Emails enviados com sucesso!");
        }
        // GET: Exibe o formulário de cadastro
        public IActionResult Cadastrar()
        {
            return View(new Usuario());
        }

        // POST: Salva os dados do usuário
        [HttpPost]
        public async Task<IActionResult> Cadastrar(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            try
            {
                _IUsuarioRepository.Add(usuario);
                var message = JsonConvert.SerializeObject(usuario.Email);
                await _serviceBusService.EnviarMensagemAsync(message);
                TempData["Sucesso"] = "Usuário cadastrado com sucesso!";
                await _serviceBusService.ReceberMensagensAsync();
                TempData["Verifique"] = "Verifique seu email";

                return RedirectToAction("Cadastrar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao cadastrar usuário: {ex.Message}");
                return View(usuario);
            }
        }
    }
}
