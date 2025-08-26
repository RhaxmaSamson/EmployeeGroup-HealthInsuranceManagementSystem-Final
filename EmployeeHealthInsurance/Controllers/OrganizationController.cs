using EmployeeHealthInsurance.Models;
using EmployeeHealthInsurance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize(Roles = "Admin,HRManager")]
public class OrganizationController : Controller
{
    private readonly IOrganizationService _organizationService;

    public OrganizationController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    // GET: Organization/Index
    public async Task<IActionResult> Index()
    {
        var organizations = await _organizationService.GetAllOrganizationsAsync();
        return View(organizations);
    }


    // GET: Organization/Delete/5
    [Authorize(Roles = "Admin")] // Only Admin can delete organizations
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var organization = await _organizationService.GetOrganizationByIdAsync(id.Value);
        if (organization == null)
        {
            return NotFound();
        }

        return View(organization);
    }

    // POST: Organization/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _organizationService.DeleteOrganizationAsync(id);
        TempData["SuccessMessage"] = "Organization deleted successfully!";
        return RedirectToAction(nameof(Index));
    }
}
