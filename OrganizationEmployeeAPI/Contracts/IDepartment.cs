using OrganizationEmployeeAPI.Models;

namespace OrganizationEmployeeAPI.Contracts
{
    public interface IDepartment
    {
        Task<IEnumerable<Department>> GetDepartments();
        Task<Department> GetDepartmentByID(int ID);
        Task<Department> InsertDepartment(Department objDepartment);
        Task<Department> UpdateDepartment(Department objDepartment);
        bool DeleteDepartment(int ID);
    }
}
