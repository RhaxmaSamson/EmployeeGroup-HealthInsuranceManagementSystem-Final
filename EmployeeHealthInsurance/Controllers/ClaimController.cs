using EmployeeHealthInsurance.DTOs;
using EmployeeHealthInsurance.Models;
using EmployeeHealthInsurance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize]
public class ClaimController : Controller
{
    private readonly IClaimService _claimService;
    private readonly IEnrollmentService _enrollmentService;

    public ClaimController(IClaimService claimService, IEnrollmentService enrollmentService)
    {
        _claimService = claimService;
        _enrollmentService = enrollmentService;
    }

    [Authorize(Roles = "Employee")]
    public IActionResult Submit()
    {
        return View();
    }

    [Authorize(Roles = "Employee")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(ClaimDto claimDto)
    {
        if (ModelState.IsValid)
        {
            var claim = await _claimService.SubmitClaimAsync(claimDto);
            if (claim != null)
            {
                return RedirectToAction("List");
            }
            ModelState.AddModelError("", "Failed to submit claim.");
        }
        return View(claimDto);
    }

    [Authorize(Roles = "Employee,HRManager,Admin")]
    public async Task<IActionResult> List()
    {
        if (User.IsInRole("Employee"))
        {
            var email = User?.Identity?.Name ?? string.Empty;
            var myClaims = await _claimService.ListClaimsForEmployeeAsync(email);
            return View(myClaims);
        }

        var claims = await _claimService.ListAllClaimsAsync();
        return View(claims);
    }

    [Authorize(Roles = "Employee,HRManager,Admin")]
    public async Task<IActionResult> Details(int id)
    {
        var claim = await _claimService.GetClaimDetailsAsync(id);
        if (claim == null)
        {
            return NotFound();
        }

        if (User.IsInRole("Employee"))
        {
            var email = User?.Identity?.Name ?? string.Empty;
            if (!string.Equals(claim.Enrollment?.Employee?.Email, email, System.StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }
        }

        return View(claim);
    }

    [Authorize(Roles = "HRManager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, ClaimStatus status)
    {
        var updated = await _claimService.UpdateClaimStatusAsync(id, status);
        if (!updated)
        {
            return NotFound();
        }
        return RedirectToAction("Details", new { id });
    }
}
