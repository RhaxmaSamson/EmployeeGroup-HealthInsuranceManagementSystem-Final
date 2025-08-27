using EmployeeHealthInsurance.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeHealthInsurance.Services
{
    public interface IOrganizationService
    {
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync();
        Task<Organization> GetOrganizationByIdAsync(int id);
        Task<Organization?> UpdateOrganizationAsync(Organization organization);
        Task DeleteOrganizationAsync(int id); // Added for deleting an organization
    }
}
