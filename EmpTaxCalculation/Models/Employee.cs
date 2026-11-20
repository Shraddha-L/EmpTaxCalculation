using System.ComponentModel.DataAnnotations;


namespace EmpTaxCalculation.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "EmpName is required.")]
        public string EmpName { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        public int? StateId { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public int? CityId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage ="Enter valid email id")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile is required.")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage ="Enter valid mobile no")]
        public string Mobile { get; set; } = string.Empty;

        [Required(ErrorMessage = "AnnualSalary is required.")]
        [Range(1, 999999999)]
        public decimal? AnnualSalary { get; set; }

       
        public decimal TaxAmount { get; set; }

        public string StateName { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
    }
}
