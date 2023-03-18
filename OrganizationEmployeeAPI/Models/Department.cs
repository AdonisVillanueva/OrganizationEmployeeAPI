using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganizationEmployeeAPI.Models
{
    [Table("Department")]

    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty(PropertyName = "DepartmentId")]
        public int DepartmentId { get; set; }

        [JsonProperty(PropertyName = "DepartmentName")]
        public string DepartmentName { get; set; }
    }
}
