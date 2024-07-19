using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Polizia.Models;

namespace Polizia.DAO
{
    public class TipoViolazioneDAO
    {
        private readonly string _connectionString;

        public TipoViolazioneDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AppConn");
        }

        public List<TipoViolazione> ReadAll()
        {
            List<TipoViolazione> violazioni = new List<TipoViolazione>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM TIPI_VIOLAZIONI";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            violazioni.Add(new TipoViolazione
                            {
                                IdViolazione = (int)reader["IdViolazione"],
                                Descrizione = reader["Descrizione"].ToString()
                            });
                        }
                    }
                }
            }
            return violazioni;
        }

        public void Create(TipoViolazione violazione)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO TIPI_VIOLAZIONI (Descrizione) VALUES (@Descrizione)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Descrizione", violazione.Descrizione ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public TipoViolazione Read(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM TIPI_VIOLAZIONI WHERE IdViolazione = @IdViolazione";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdViolazione", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TipoViolazione
                            {
                                IdViolazione = (int)reader["IdViolazione"],
                                Descrizione = reader["Descrizione"].ToString()
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void Update(TipoViolazione violazione)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE TIPI_VIOLAZIONI SET Descrizione = @Descrizione WHERE IdViolazione = @IdViolazione";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Descrizione", violazione.Descrizione ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdViolazione", violazione.IdViolazione);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM TIPI_VIOLAZIONI WHERE IdViolazione = @IdViolazione";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdViolazione", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

