using EmpTaxCalculation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace EmpTaxCalculation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration DBconn)
        {
            _config = DBconn;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString("SQLcon"));
        }
      
        public IActionResult Index()
        {
            LoadPageData();
            return View();
        }

        private void LoadPageData()
        {
            List<SelectListItem> states = new List<SelectListItem>();

            using (SqlConnection con = GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("sp_GetStates", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    states.Add(new SelectListItem
                    {
                        Value = dr["StateId"].ToString(),
                        Text = dr["StateName"].ToString()
                    });
                }
            }

            ViewBag.States = states;
            ViewBag.Employees = GetEmployees();
        }

        [HttpGet]
        public JsonResult GetCities(int stateId)
        {
            List<SelectListItem> cities = new List<SelectListItem>();
            using (SqlConnection con = GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_GetCitiesByState", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StateId", stateId);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    cities.Add(new SelectListItem
                    {
                        Value = dr["CityId"].ToString(),
                        Text = dr["CityName"].ToString()
                    });
                }
            }
            return Json(cities);
        }

        [HttpPost]
        public IActionResult InsertEmployee(Employee emp)
        {
            if (!ModelState.IsValid)
            {
                LoadPageData();
                return View("Index", emp);
            }
            using (SqlConnection con = GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_InsertEmployee", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpName", emp.EmpName);
                cmd.Parameters.AddWithValue("@StateId", emp.StateId.Value);
                cmd.Parameters.AddWithValue("@CityId", emp.CityId.Value);
                cmd.Parameters.AddWithValue("@Email", emp.Email);
                cmd.Parameters.AddWithValue("@Mobile", emp.Mobile);
                cmd.Parameters.AddWithValue("@AnnualSalary", emp.AnnualSalary.Value);
                cmd.Parameters.AddWithValue("@TaxAmount", emp.TaxAmount);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        private List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection con = GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("sp_GetEmployees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    employees.Add(new Employee
                    {
                        EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                        StateId = Convert.ToInt32(dr["StateId"]),
                        CityId = Convert.ToInt32(dr["CityId"]),
                        StateName = dr["StateName"].ToString(),
                        CityName = dr["CityName"].ToString(),
                        EmpName = dr["EmpName"].ToString(),
                        Email = dr["Email"].ToString(),
                        Mobile = dr["Mobile"].ToString(),
                        AnnualSalary = Convert.ToDecimal(dr["AnnualSalary"]),
                        TaxAmount = Convert.ToDecimal(dr["TaxAmount"])
                    });
                }
            }
            return employees;
        }

        [HttpPost]
        public IActionResult UpdateEmployee(Employee emp)
        {
            if (!ModelState.IsValid)
            {
                LoadPageData();
                return View("Index", emp);
            }
            using (SqlConnection con = GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_UpdateEmployee", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                cmd.Parameters.AddWithValue("@EmpName", emp.EmpName);
                cmd.Parameters.AddWithValue("@StateId", emp.StateId.Value);
                cmd.Parameters.AddWithValue("@CityId", emp.CityId.Value);
                cmd.Parameters.AddWithValue("@Email", emp.Email);
                cmd.Parameters.AddWithValue("@Mobile", emp.Mobile);
                cmd.Parameters.AddWithValue("@AnnualSalary", emp.AnnualSalary.Value);
                cmd.Parameters.AddWithValue("@TaxAmount", emp.TaxAmount);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteEmployee(Employee emp)
        {
            using (SqlConnection con = GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("sp_DeleteEmployee", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
