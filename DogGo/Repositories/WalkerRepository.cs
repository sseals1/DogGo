using DogGo.Models;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;

namespace DogGo.Repositories
{
    public class WalkerRepository : IWalkerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkerRepository(IConfiguration config)
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


        public List<Walker> GetWalkersInNeighborhood(int neighborhoodId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT Id, [Name], ImageUrl, NeighborhoodId
                FROM Walker
                WHERE NeighborhoodId = @neighborhoodId
            ";

                    cmd.Parameters.AddWithValue("@neighborhoodId", neighborhoodId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        List<Walker> walkers = new List<Walker>();
                        while (reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                            };

                            walkers.Add(walker);
                        }

                        return walkers;
                    }
                }
            }
        }
        public List<Walker> GetAllWalkers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT w.[Name], w.Id, w.ImageUrl, w.NeighborhoodId, n.Name AS Neighborhood  FROM Walker w 
                       JOIN Neighborhood n ON w.NeighborhoodId = n.Id
                    ";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walker> walkers = new List<Walker>();
                        while (reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Neighborhood = new Neighborhood()
                                {

                                    Name = reader.GetString(reader.GetOrdinal("Neighborhood"))
                                }
                            };

                            walkers.Add(walker);
                        }

                        return walkers;
                    }
                }
            }
        }

        public Walker GetWalkerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT w.Id, w.Date, w.Duration, o.Name, d.ImageUrl, o.NeighborhoodId, n.Name AS NeighborhoodName FROM Walks w
                       JOIN Dog d ON d.Id = w.DogId
                       JOIN Owner o ON o.Id = d.OwnerId
                       JOIN Neighborhood n ON n.Id = o.NeighborhoodId
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Walker walker = new Walker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),

                                NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),

                                Neighborhood = new Neighborhood()
                                {
                                    Name = reader.GetString(reader.GetOrdinal("NeighborhoodName"))
                                }
                            };

                            if (reader.IsDBNull(reader.GetOrdinal("ImageUrl")) == false)
                            {
                                walker.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                            }

                            walker.Walks = new List<Walks>();
                            Walks walk = new Walks()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                Dog = new Dog()
                                {
                                    OwnerId = reader.GetInt32(reader.GetOrdinal("Id")),
                                }
                            };
                            return walker;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}