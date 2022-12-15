using Microsoft.AspNetCore.Mvc;
using SiteCopa2022.EventHubs;

namespace SiteCopa2022.Controllers;

public class VotacaoController : Controller
{
    private readonly ILogger<VotacaoController> _logger;
    private readonly VotacaoProducer _producer;

    public VotacaoController(ILogger<VotacaoController> logger,
        VotacaoProducer producer)
    {
        _logger = logger;
        _producer = producer;
    }

    public async Task<IActionResult> VotoBrasil()
    {
        return await ProcessarVoto("Brasil");
    }

    public async Task<IActionResult> VotoArgentina()
    {
        return await ProcessarVoto("Argentina");
    }

    public async Task<IActionResult> VotoFranca()
    {
        return await ProcessarVoto("Franca");
    }

    public async Task<IActionResult> VotoOutro()
    {
        return await ProcessarVoto("Outro");
    }

    private async Task<IActionResult> ProcessarVoto(string pais)
    {
        _logger.LogInformation($"Processando voto para o interesse: {pais}");
        await _producer.Send(pais);
        _logger.LogInformation($"Informações sobre o voto '{pais}' enviadas para o Azure Event Hubs!");

        TempData["Voto"] = pais;
        return RedirectToAction("Index", "Home", new { voto = pais });
    }
}