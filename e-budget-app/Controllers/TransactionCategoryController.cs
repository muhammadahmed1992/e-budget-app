using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using e_budget_app.Entities;
using e_budget_app.Utilities.Dtos;

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
                                    Id = (int)sqlDataReader["CategoryId"],
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
        public IActionResult CreateUser([FromBody] TransactionCategoryDto transactionCategoryDto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        string query = "Insert into TransactionCategory(Name) Values(@Name); " +
                            "SELECT CAST(scope_identity() AS int)";
                        using SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@Name", transactionCategoryDto.Name);

                        connection.Open();

                        Int32? categoryId = (Int32)cmd.ExecuteScalar();

                        if (categoryId != null)
                        {
                            return Ok($"Category Created with ID: {categoryId}");
                        }
                        else
                        {
                            return BadRequest("Category Creation Failed");
                        }
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

        [HttpPut("{id}")]
        public IActionResult UpdateTransactionCategoryAtId([FromRoute] int id, [FromBody] TransactionCategoryDto transactionCategoryDto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        string query = "Update TransactionCategory " +
                            "Set Name = @Name" +
                            "Where CategoryId = @Id";
                        using SqlCommand cmd = new SqlCommand(query, connection);
                        connection.Open();

                        cmd.Parameters.AddWithValue("@Name", transactionCategoryDto.Name);

                        connection.Open();

                        Int32? categoryId = (Int32)cmd.ExecuteScalar();

                        if (categoryId != null)
                        {
                            return Ok($"Category Updated at ID: {categoryId}");
                        }
                        else
                        {
                            return BadRequest("Category Updation Failed");
                        }

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
    }
}
