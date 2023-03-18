using OrganizationEmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;
using OrganizationEmployeeAPI.Contracts;

namespace OrganizationEmployeeAPI.Repository
{
    public class DepartmentRepository : IDepartment
    {

        private readonly APIDbContext _appDBContext;

        public DepartmentRepository(APIDbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }        

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await _appDBContext.Departments.ToListAsync();
        }

        public async Task<Department> GetDepartmentByID(int ID)
        {
            return await _appDBContext.Departments.FindAsync(ID);
        }

        public async Task<Department> InsertDepartment(Department objDepartment)
        {
            _appDBContext.Departments.Add(objDepartment);
            await _appDBContext.SaveChangesAsync();
            return objDepartment;

        }

        public async Task<Department> UpdateDepartment(Department objDepartment)
        {
            _appDBContext.Entry(objDepartment).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();
            return objDepartment;

        }

        public bool DeleteDepartment(int ID)
        {
            var department = _appDBContext.Departments.Find(ID);
            bool result;
            if (department != null)
            {
                _appDBContext.Entry(department).State = EntityState.Deleted;
                _appDBContext.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }       
    }
}