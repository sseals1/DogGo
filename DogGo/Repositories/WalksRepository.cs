using DogGo.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DogGo.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalksRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        public List<Walks> GetAllWalks()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT w.Id, w.Date, w.Duration, o.Name FROM Walks w
                       JOIN Dog d ON d.Id = w.DogId
                       JOIN Owner o ON o.Id = d.OwnerId
                    ";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walks> walkers = new List<Walks>();
                        while (reader.Read())
                        {
                            Walks walks = new Walks
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration"))
                            };
                            walkers.Add(walks);
                        }

                        return walkers;
                    }
                }
            }
        }

        
        public List<Walks> GetWalksByWalkerId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT w.Id, w.Date, w.Duration, o.Name FROM Walks w
                       JOIN Dog d ON d.Id = w.DogId
                       JOIN Owner o ON o.Id = d.OwnerId
                    ";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walks> walkers = new List<Walks>();
                        while (reader.Read())
                        {
                            Walks walks = new Walks
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")) / 60,
                                TotalWalkTime = walkers.Sum(n => n.Duration),

                            Owner = new Owner
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                }
                            };

                            walkers.Add(walks);
                        }

                        return walkers;
                    }
                }
            }
        }
    }
}
