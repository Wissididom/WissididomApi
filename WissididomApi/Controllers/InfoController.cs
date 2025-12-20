using Microsoft.AspNetCore.Mvc;
using WissididomApi.Logic;

namespace WissididomApi.Controllers;

[Route("api/[controller]")]
public class InfoController(IVersionsInfo versionsInfo) : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return versionsInfo.GetAssemblyVersions();
    }
}
