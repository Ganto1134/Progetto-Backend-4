using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Polizia.Models;

namespace Polizia.DAO
{
    public class AnagraficaDAO
    {
        private readonly string _connectionString;

        public AnagraficaDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AppConn");
        }

        public void Create(Anagrafica anagrafica)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO ANAGRAFICHE (Cognome, Nome, Indirizzo, Città, CAP, Cod_Fisc) VALUES (@Cognome, @Nome, @Indirizzo, @Città, @CAP, @Cod_Fisc)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Cognome", anagrafica.Cognome ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nome", anagrafica.Nome ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Città", anagrafica.Città ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CAP", anagrafica.CAP ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Cod_Fisc", anagrafica.Cod_Fisc ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Anagrafica Read(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM ANAGRAFICHE WHERE IdAnagrafica = @IdAnagrafica";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAnagrafica", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Anagrafica
                            {
                                IdAnagrafica = (int)reader["IdAnagrafica"],
                                Cognome = reader["Cognome"].ToString(),
                                Nome = reader["Nome"].ToString(),
                                Indirizzo = reader["Indirizzo"].ToString(),
                                Città = reader["Città"].ToString(),
                                CAP = reader["CAP"].ToString(),
                                Cod_Fisc = reader["Cod_Fisc"].ToString()
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

        public List<Anagrafica> ReadAll()
        {
            List<Anagrafica> anagrafiche = new List<Anagrafica>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM ANAGRAFICHE";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            anagrafiche.Add(new Anagrafica
                            {
                                IdAnagrafica = (int)reader["IdAnagrafica"],
                                Cognome = reader["Cognome"].ToString(),
                                Nome = reader["Nome"].ToString(),
                                Indirizzo = reader["Indirizzo"].ToString(),
                                Città = reader["Città"].ToString(),
                                CAP = reader["CAP"].ToString(),
                                Cod_Fisc = reader["Cod_Fisc"].ToString()
                            });
                        }
                    }
                }
            }
            return anagrafiche;
        }

        public void Update(Anagrafica anagrafica)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE ANAGRAFICHE SET Cognome = @Cognome, Nome = @Nome, Indirizzo = @Indirizzo, Città = @Città, CAP = @CAP, Cod_Fisc = @Cod_Fisc WHERE IdAnagrafica = @IdAnagrafica";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Cognome", anagrafica.Cognome);
                    cmd.Parameters.AddWithValue("@Nome", anagrafica.Nome);
                    cmd.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo);
                    cmd.Parameters.AddWithValue("@Città", anagrafica.Città);
                    cmd.Parameters.AddWithValue("@CAP", anagrafica.CAP);
                    cmd.Parameters.AddWithValue("@Cod_Fisc", anagrafica.Cod_Fisc);
                    cmd.Parameters.AddWithValue("@IdAnagrafica", anagrafica.IdAnagrafica);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Prima elimina i record correlati nella tabella VERBALI
                string deleteVerbaliQuery = "DELETE FROM VERBALI WHERE IdAnagrafica = @IdAnagrafica";
                using (SqlCommand cmd = new SqlCommand(deleteVerbaliQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAnagrafica", id);
                    cmd.ExecuteNonQuery();
                }

                // Ora elimina il record nella tabella ANAGRAFICHE
                string deleteAnagraficaQuery = "DELETE FROM ANAGRAFICHE WHERE IdAnagrafica = @IdAnagrafica";
                using (SqlCommand cmd = new SqlCommand(deleteAnagraficaQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAnagrafica", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
