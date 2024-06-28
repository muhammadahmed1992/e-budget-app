using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using e_budget_app.Entities;
using e_budget_app.Utilities.Dtos;

namespace e_budget_app.Controllers
{
    [ApiController]
    [Route("api/transactiontype")]
    public class TransactionTypeController : ControllerBase
    {
        private readonly string? _connectionString;

        public TransactionTypeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult GetAllTransactionTypes()
        {
            List<TransactionType> transactionTypes = new List<TransactionType>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using SqlCommand cmd = new SqlCommand("SELECT * FROM TransactionType", connection);
                    connection.Open();
                    using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            TransactionType transactionType = new TransactionType
                            {
                                Id = (int)sqlDataReader["TransactionTypeID"],
                                Name = sqlDataReader["Name"].ToString()
                            };
                            transactionTypes.Add(transactionType);
                        }
                    }
                }
                return Ok(transactionTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateTransactionType([FromBody] TransactionTypeDto transactionTypeDto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO TransactionType(Name) VALUES(@Name); SELECT CAST(scope_IDentity() AS int)";
                    using SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", transactionTypeDto.Name);

                    connection.Open();

                    int? typeID = (int?)cmd.ExecuteScalar();

                    if (typeID != null)
                    {
                        return Ok($"Type Created with ID: {typeID}");
                    }
                    else
                    {
                        return BadRequest("Type Creation Failed");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{ID}")]
        public IActionResult UpdateTransactionTypeAtId([FromRoute] int id, [FromBody] TransactionTypeDto transactionTypeDto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE TransactionType SET Name = @Name WHERE TransactionTypeID = @Id";
                    using SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", transactionTypeDto.Name);
                    cmd.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return Ok(new { status = "200", message = $"Type Updated at {id}" });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{ID}")]
        public IActionResult GetTransactionTypeAtID([FromRoute] int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM TransactionType WHERE TransactionTypeID = @Id";
                    using SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    using SqlDataReader sqlDataReader = cmd.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        TransactionType transactionType = new TransactionType
                        {
                            Id = (int)sqlDataReader["TransactionTypeID"],
                            Name = sqlDataReader["Name"].ToString()
                        };
                        return Ok(transactionType);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{ID}")]
        public IActionResult DeleteTransactionType([FromRoute] int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "DELETE FROM TransactionType WHERE TransactionTypeID = @Id";
                    using SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return Ok(new { status = "200", message = $"Type Deleted at {id}" });
                    }
                    else
                    {
                        return NotFound();
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
