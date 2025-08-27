using EmployeeHealthInsurance.Data;
using EmployeeHealthInsurance.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeHealthInsurance.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ApplicationDbContext _context;

        public OrganizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
        {
            return await _context.Organizations.ToListAsync();
        }

        public async Task<Organization> GetOrganizationByIdAsync(int id)
        {
            return await _context.Organizations.FindAsync(id);
        }

        public async Task<Organization?> UpdateOrganizationAsync(Organization organization)
        {
            var existing = await _context.Organizations.FindAsync(organization.OrganizationId);
            if (existing == null) return null;

            existing.OrganizationName = organization.OrganizationName;
            existing.ContactPerson = organization.ContactPerson;
            existing.ContactEmail = organization.ContactEmail;
            existing.Address = organization.Address;
            existing.ContactPhone = organization.ContactPhone;

            _context.Organizations.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

        // Implementation for deleting an organization
        public async Task DeleteOrganizationAsync(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
                await _context.SaveChangesAsync();
            }
        }
    }
}
