using IntelligenceReporting.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntelligenceReporting.WebApi.Controllers;

/// <summary>VaultRE sync API</summary>
[Route("api/[controller]")]
[ApiController]
public class VaultSyncController : ControllerBase
{
    private readonly IVaultSyncService _service;

    /// <summary>Constructor</summary>
    public VaultSyncController(IVaultSyncService service)
    {
        _service = service;
    }

    /// <summary>Synchronize data from Vault</summary>
    /// <returns></returns>
    [HttpPost("[action]")]
    public async Task<IActionResult> Sync()
    {
        var result = await _service.Sync();
        return Ok(result);
    }

}