using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrganizationEmployeeAPI.Contracts;
using OrganizationEmployeeAPI.Controllers;
using OrganizationEmployeeAPI.Models;

namespace OrganizationEmployeeAPI.Tests
{
    public class EmployeeControllerTest
    {
        private readonly Mock<IEmployee> _employeeRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IDepartment _department;

        public static List<Employee> GetEmployees()
        {
            return new List<Employee>() {
            new Employee(){EmployeeId = 1, EmployeeName = "Jane Doe", Department = "IT", DateOfJoining = DateTime.Now, PhotoFileName = "avatar2"},
            new Employee(){EmployeeId=2, EmployeeName = "Human Resources", Department = "HR", DateOfJoining = DateTime.Now, PhotoFileName = "avatar3"},
            new Employee(){EmployeeId=3, EmployeeName = "Accounting", Department = "Finance", DateOfJoining = DateTime.Now, PhotoFileName = "avatar4"},
            new Employee(){EmployeeId=4, EmployeeName = "Finance", Department = "Accounting", DateOfJoining = DateTime.Now, PhotoFileName = "avatar1"},
            };
        }
        public EmployeeControllerTest()
        {
            _employeeRepository = new Mock<IEmployee>();
        }

        [Fact]
        public async Task GetEmployeeList_EmployeeList()
        {
            //Arrange
            var EmployeeList = GetEmployees();

            _employeeRepository.Setup(x => x.GetEmployees())
                .Returns(Task.FromResult((EmployeeList).AsEnumerable()));

            var employeeController = new EmployeeController(_env, _employeeRepository.Object, _department);

            //act
            var employeeResult = await employeeController.Get();

            //assert
            var objectResult = Assert.IsType<OkObjectResult>(employeeResult);
            Assert.NotNull(employeeResult);
            var model = Assert.IsAssignableFrom<IEnumerable<Employee>>(objectResult.Value);
            var modelCount = model.Count();
            Assert.Equal(4, modelCount);
        }

        [Fact]
        public async Task GetEmployeeByID_Employee()
        {
            //arrange
            var EmployeeList = GetEmployees();
            _employeeRepository.Setup(x => x.GetEmployeeByID(2))
                .Returns(Task.FromResult(EmployeeList[1]));

            var employeeController = new EmployeeController(_env, _employeeRepository.Object, _department);

            //act
            var employeeResult = await employeeController.GetEmployeeByID(2);

            //assert
            Assert.NotNull(employeeResult);
            var objectResult = Assert.IsType<OkObjectResult>(employeeResult);
            var model = Assert.IsAssignableFrom<Employee>(objectResult.Value);
            Assert.Equal(EmployeeList[1].EmployeeId, model.EmployeeId);
        }

        [Fact]
        public async Task AddEmployee_Employee()
        {
            Employee? dep = null;
            //arrange
            var employeeList = GetEmployees();
            _employeeRepository.Setup(x => x.InsertEmployee(It.IsAny<Employee>()))
                .Callback<Employee>(x => dep = x);

            var employeeController = new EmployeeController(_env, _employeeRepository.Object, _department);

            //act
            var employeeResult = await employeeController.Post(employeeList[2]);

            //assert
            _employeeRepository.Verify(x => x.InsertEmployee(It.IsAny<Employee>()), Times.Once);
            Assert.Equal(dep.EmployeeName, employeeList[2].EmployeeName);
            Assert.Equal(dep.EmployeeId, employeeList[2].EmployeeId);
        }

        [Fact]
        public void DeleteEmployee_Employee()
        {
            //arrange
            var employeeList = GetEmployees();

            _employeeRepository.Setup(x => x.DeleteEmployee(2))
                .Returns(true);

            var employeeController = new EmployeeController(_env, _employeeRepository.Object, _department);

            //act
            var EmployeeResult = employeeController.Delete(2);

            //assert
            Assert.NotNull(EmployeeResult);
            _employeeRepository.Verify(x => x.DeleteEmployee(2),Times.Once);
        }

        [Fact]
        public async Task UpdateEmployee_Employee()
        {
            //arrange
            var employeeList = GetEmployees();
            _employeeRepository.Setup(x => x.GetEmployeeByID(2))
                .Returns(Task.FromResult(employeeList[1]));

            var employeeController = new EmployeeController(_env, _employeeRepository.Object, _department);

            var dep = new Employee() { EmployeeId = 2, EmployeeName = "New Test Employee" };

            //act
            var employeeResult = await employeeController.Put(employeeList[2]);
            var objectResult = Assert.IsType<OkObjectResult>(employeeResult);

;           var updatedDep = await employeeController.Put(dep);

            Assert.IsType<OkObjectResult>(updatedDep);
        }      
    }

}
