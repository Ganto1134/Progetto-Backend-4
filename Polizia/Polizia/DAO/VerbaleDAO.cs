using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Polizia.Models;

namespace Polizia.DAO
{
    public class VerbaleDAO
    {
        private readonly string _connectionString;

        public VerbaleDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AppConn");
        }

        public List<Verbale> ReadAll()
        {
            List<Verbale> verbali = new List<Verbale>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM VERBALI";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            verbali.Add(new Verbale
                            {
                                IdVerbale = (int)reader["IdVerbale"],
                                IdAnagrafica = (int)reader["IdAnagrafica"],
                                IdViolazione = (int)reader["IdViolazione"],
                                DataViolazione = (DateTime)reader["DataViolazione"],
                                IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                                Nominativo_Agente = reader["Nominativo_Agente"].ToString(),
                                DataTrascrizioneVerbale = (DateTime)reader["DataTrascrizioneVerbale"],
                                Importo = (decimal)reader["Importo"],
                                DecurtamentoPunti = reader["DecurtamentoPunti"] as int?
                            });
                        }
                    }
                }
            }
            return verbali;
        }

        // Metodi per i report
        public List<VerbaleReport> GetVerbaliPerTrasgressore()
        {
            List<VerbaleReport> report = new List<VerbaleReport>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT a.Cognome, a.Nome, COUNT(v.IdVerbale) AS TotaleVerbali
                    FROM VERBALI v
                    JOIN ANAGRAFICHE a ON v.IdAnagrafica = a.IdAnagrafica
                    GROUP BY a.Cognome, a.Nome
                ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            report.Add(new VerbaleReport
                            {
                                Cognome = reader["Cognome"].ToString(),
                                Nome = reader["Nome"].ToString(),
                                TotaleVerbali = (int)reader["TotaleVerbali"]
                            });
                        }
                    }
                }
            }
            return report;
        }

        public List<VerbaleReport> GetPuntiDecurtatiPerTrasgressore()
        {
            List<VerbaleReport> report = new List<VerbaleReport>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT a.Cognome, a.Nome, SUM(v.DecurtamentoPunti) AS TotalePunti
                    FROM VERBALI v
                    JOIN ANAGRAFICHE a ON v.IdAnagrafica = a.IdAnagrafica
                    WHERE v.DecurtamentoPunti IS NOT NULL
                    GROUP BY a.Cognome, a.Nome
                ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            report.Add(new VerbaleReport
                            {
                                Cognome = reader["Cognome"].ToString(),
                                Nome = reader["Nome"].ToString(),
                                TotalePunti = (int)reader["TotalePunti"]
                            });
                        }
                    }
                }
            }
            return report;
        }

        public List<Verbale> GetViolazioniConPiuDiDieciPunti()
        {
            List<Verbale> violazioni = new List<Verbale>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT v.Importo, a.Cognome, a.Nome, v.DataViolazione, v.DecurtamentoPunti
                    FROM VERBALI v
                    JOIN ANAGRAFICHE a ON v.IdAnagrafica = a.IdAnagrafica
                    WHERE v.DecurtamentoPunti > 10
                ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            violazioni.Add(new Verbale
                            {
                                Importo = (decimal)reader["Importo"],
                                DataViolazione = (DateTime)reader["DataViolazione"],
                                DecurtamentoPunti = (int)reader["DecurtamentoPunti"],
                                IdAnagrafica = 0, // not needed here
                                IdViolazione = 0, // not needed here
                                IndirizzoViolazione = null, // not needed here
                                Nominativo_Agente = null, // not needed here
                                DataTrascrizioneVerbale = DateTime.MinValue // not needed here
                            });
                        }
                    }
                }
            }
            return violazioni;
        }

        public List<Verbale> GetViolazioniConImportoMaggioreDiQuattrocento()
        {
            List<Verbale> violazioni = new List<Verbale>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT v.Importo, a.Cognome, a.Nome, v.DataViolazione, v.DecurtamentoPunti
                    FROM VERBALI v
                    JOIN ANAGRAFICHE a ON v.IdAnagrafica = a.IdAnagrafica
                    WHERE v.Importo > 400
                ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            violazioni.Add(new Verbale
                            {
                                Importo = (decimal)reader["Importo"],
                                DataViolazione = (DateTime)reader["DataViolazione"],
                                DecurtamentoPunti = reader["DecurtamentoPunti"] as int?,
                                IdAnagrafica = 0, // not needed here
                                IdViolazione = 0, // not needed here
                                IndirizzoViolazione = null, // not needed here
                                Nominativo_Agente = null, // not needed here
                                DataTrascrizioneVerbale = DateTime.MinValue // not needed here
                            });
                        }
                    }
                }
            }
            return violazioni;
        }

        public void Create(Verbale verbale)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO VERBALI (IdAnagrafica, IdViolazione, DataViolazione, IndirizzoViolazione, Nominativo_Agente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti) VALUES (@IdAnagrafica, @IdViolazione, @DataViolazione, @IndirizzoViolazione, @Nominativo_Agente, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAnagrafica", verbale.IdAnagrafica);
                    cmd.Parameters.AddWithValue("@IdViolazione", verbale.IdViolazione);
                    cmd.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                    cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nominativo_Agente", verbale.Nominativo_Agente ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale);
                    cmd.Parameters.AddWithValue("@Importo", verbale.Importo);
                    cmd.Parameters.AddWithValue("@DecurtamentoPunti", verbale.DecurtamentoPunti ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Verbale Read(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM VERBALI WHERE IdVerbale = @IdVerbale";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdVerbale", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Verbale
                            {
                                IdVerbale = (int)reader["IdVerbale"],
                                IdAnagrafica = (int)reader["IdAnagrafica"],
                                IdViolazione = (int)reader["IdViolazione"],
                                DataViolazione = (DateTime)reader["DataViolazione"],
                                IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                                Nominativo_Agente = reader["Nominativo_Agente"].ToString(),
                                DataTrascrizioneVerbale = (DateTime)reader["DataTrascrizioneVerbale"],
                                Importo = (decimal)reader["Importo"],
                                DecurtamentoPunti = reader["DecurtamentoPunti"] as int?
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

        public void Update(Verbale verbale)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE VERBALI SET IdAnagrafica = @IdAnagrafica, IdViolazione = @IdViolazione, DataViolazione = @DataViolazione, IndirizzoViolazione = @IndirizzoViolazione, Nominativo_Agente = @Nominativo_Agente, DataTrascrizioneVerbale = @DataTrascrizioneVerbale, Importo = @Importo, DecurtamentoPunti = @DecurtamentoPunti WHERE IdVerbale = @IdVerbale";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAnagrafica", verbale.IdAnagrafica);
                    cmd.Parameters.AddWithValue("@IdViolazione", verbale.IdViolazione);
                    cmd.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                    cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nominativo_Agente", verbale.Nominativo_Agente ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale);
                    cmd.Parameters.AddWithValue("@Importo", verbale.Importo);
                    cmd.Parameters.AddWithValue("@DecurtamentoPunti", verbale.DecurtamentoPunti ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdVerbale", verbale.IdVerbale);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM VERBALI WHERE IdVerbale = @IdVerbale";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdVerbale", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

