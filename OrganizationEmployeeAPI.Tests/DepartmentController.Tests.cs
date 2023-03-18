using Microsoft.AspNetCore.Mvc;
using Moq;
using OrganizationEmployeeAPI.Contracts;
using OrganizationEmployeeAPI.Controllers;
using OrganizationEmployeeAPI.Models;

namespace OrganizationEmployeeAPI.Tests
{
    public class DepartmentControllerTest
    {
        private readonly Mock<IDepartment> _departmentRepository;

        public static List<Department> GetDepartments()
        {
            return new List<Department>() {
            new Department(){DepartmentId = 1, DepartmentName = "IT"},
            new Department(){DepartmentId=2, DepartmentName = "Human Resources"},
            new Department(){DepartmentId=3, DepartmentName = "Accounting"},
            new Department(){DepartmentId=4, DepartmentName = "Finance"}
            };
        }
        public DepartmentControllerTest()
        {
            _departmentRepository = new Mock<IDepartment>();
        }

        [Fact]
        public async Task GetDepartmentList_DepartmentList()
        {
            //Arrange
            var departmentList = GetDepartments();

            _departmentRepository.Setup(x => x.GetDepartments())
                .Returns(Task.FromResult((departmentList).AsEnumerable()));

            var departmentController = new DepartmentController(_departmentRepository.Object);

            //act
            var departmentResult = await departmentController.Get();

            //assert
            var objectResult = Assert.IsType<OkObjectResult>(departmentResult);
            Assert.NotNull(departmentResult);
            var model = Assert.IsAssignableFrom<IEnumerable<Department>>(objectResult.Value);
            var modelCount = model.Count();
            Assert.Equal(4, modelCount);
        }

        [Fact]
        public async Task GetDepartmentByID_Department()
        {
            //arrange
            var departmentList = GetDepartments();
            _departmentRepository.Setup(x => x.GetDepartmentByID(2))
                .Returns(Task.FromResult(departmentList[1]));

            var departmentController = new DepartmentController(_departmentRepository.Object);

            //act
            var departmentResult = await departmentController.GetDeptById(2);

            //assert
            Assert.NotNull(departmentResult);
            var objectResult = Assert.IsType<OkObjectResult>(departmentResult);
            var model = Assert.IsAssignableFrom<Department>(objectResult.Value);
            Assert.Equal(departmentList[1].DepartmentId, model.DepartmentId);
        }

        [Fact]
        public async Task AddDepartment_Department()
        {
            Department? dep = null;
            //arrange
            var departmentList = GetDepartments();
            _departmentRepository.Setup(x => x.InsertDepartment(It.IsAny<Department>()))
                .Callback<Department>(x => dep = x);

            var departmentController = new DepartmentController(_departmentRepository.Object);

            //act
            var departmentResult = await departmentController.Post(departmentList[2]);

            //assert
            _departmentRepository.Verify(x => x.InsertDepartment(It.IsAny<Department>()), Times.Once);
            Assert.Equal(dep.DepartmentName, departmentList[2].DepartmentName);
            Assert.Equal(dep.DepartmentId, departmentList[2].DepartmentId);
        }

        [Fact]
        public void DeleteDepartment_Department()
        {
            //arrange
            var departmentList = GetDepartments();

            _departmentRepository.Setup(x => x.DeleteDepartment(2))
                .Returns(true);

            var departmentController = new DepartmentController(_departmentRepository.Object);

            //act
            var departmentResult = departmentController.Delete(2);

            //assert
            Assert.NotNull(departmentResult);
            _departmentRepository.Verify(x => x.DeleteDepartment(2),Times.Once);
        }

        [Fact]
        public async Task UpdateDepartment_Department()
        {
            //arrange
            var departmentList = GetDepartments();
            _departmentRepository.Setup(x => x.GetDepartmentByID(2))
                .Returns(Task.FromResult(departmentList[1]));

            var departmentController = new DepartmentController(_departmentRepository.Object);

            var dep = new Department() { DepartmentId = 2, DepartmentName = "New Test Department" };

            //act
            var departmentResult = await departmentController.GetDeptById(2);
            var objectResult = Assert.IsType<OkObjectResult>(departmentResult);

;           var updatedDep = await departmentController.Put(dep);

            Assert.IsType<OkObjectResult>(updatedDep);
        }
    }

}
