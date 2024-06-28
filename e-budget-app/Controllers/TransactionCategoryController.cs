using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using e_budget_app.Entities;

namespace e_budget_app.Controllers
{
    [ApiController]
    [Route("api/transactioncategory")]
    public class TransactionCategoryController : ControllerBase
    {
        private readonly string? _connectionString;
        public TransactionCategoryController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult GetAllTransactionCategories()
        {
            List<TransactionCategory> transactionCategories = new List<TransactionCategory>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        using SqlCommand cmd = new SqlCommand("Select * From TransactionCategory", connection);
                        connection.Open();
                        using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                TransactionCategory transactionCategory = new TransactionCategory
                                {
                                    Id = (int)sqlDataReader["Id"],
                                    Name = sqlDataReader["Name"].ToString()
                                };
                                transactionCategories.Add(transactionCategory);
                            }
                        }
                        return Ok(transactionCategories);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateTransactionCategory()
        {

        }
    }
}
