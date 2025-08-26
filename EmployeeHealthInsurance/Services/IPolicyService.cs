using EmployeeHealthInsurance.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeHealthInsurance.Services
{
    public interface IPolicyService
    {

        Task<IEnumerable<Policy>> GetAllPoliciesAsync();
        Task<Policy> GetPolicyByIdAsync(int id);
       
        Task DeletePolicyAsync(int id);


    }
}