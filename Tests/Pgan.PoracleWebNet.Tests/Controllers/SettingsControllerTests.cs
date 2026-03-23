using Microsoft.AspNetCore.Mvc;
using Moq;
using Pgan.PoracleWebNet.Api.Controllers;
using Pgan.PoracleWebNet.Core.Abstractions.Services;
using Pgan.PoracleWebNet.Core.Models;

namespace Pgan.PoracleWebNet.Tests.Controllers;

public class SettingsControllerTests : ControllerTestBase
{
    private readonly Mock<ISiteSettingService> _siteService = new();
    private readonly SettingsController _sut;

    public SettingsControllerTests() => this._sut = new SettingsController(this._siteService.Object);

    [Fact]
    public async Task GetAllReturnsOk()
    {
        SetupUser(this._sut);
        this._siteService.Setup(s => s.GetAllAsync()).ReturnsAsync([new() { Key = "k", Value = "v" }]);
        Assert.IsType<OkObjectResult>(await this._sut.GetAll());
    }

    [Fact]
    public async Task UpsertReturnsOkWhenAdmin()
    {
        SetupUser(this._sut, isAdmin: true);
        var request = new SettingsController.SiteSettingRequest { Value = "val", Category = "branding" };
        this._siteService.Setup(s => s.CreateOrUpdateAsync(It.IsAny<SiteSetting>())).ReturnsAsync(new SiteSetting { Key = "key1", Value = "val" });

        var result = await this._sut.Upsert("key1", request);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task UpsertReturnsForbidWhenNotAdmin()
    {
        SetupUser(this._sut, isAdmin: false);
        var result = await this._sut.Upsert("key1", new SettingsController.SiteSettingRequest());
        Assert.IsType<ForbidResult>(result);
    }
}
