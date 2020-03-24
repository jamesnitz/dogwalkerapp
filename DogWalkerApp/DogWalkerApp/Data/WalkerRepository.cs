using DogWalkerApp.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogWalkerApp.Data
{
   public class WalkerRepository
    {
        //The Connection to the SQL Server
        public SqlConnection Connection
        {
            get
            {
                string _connectionString = "Data Source=localhost\\SQLEXPRESS; Initial Catalog=DogWalking; Integrated Security=True";
                return new SqlConnection(_connectionString);
            }
        }

        public List<Walker> getAllWalkers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        Select w.Id, w.Name, w.NeighborhoodId, n.Name
                        From Walker w
                        Left Join Neighborhood n
                        ON w.NeighborhoodId = n.Id";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walker> allWalkers = new List<Walker>();

                    while (reader.Read())
                    {
                        int idColumn = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumn);

                        int walkerNameColumn = reader.GetOrdinal("Name");
                        string walkerNameValue = reader.GetString(walkerNameColumn);

                        int neighborhoodIdColumn = reader.GetOrdinal("NeighborhoodId");
                        int neighborhoodIdValue = reader.GetInt32(neighborhoodIdColumn);

                        int neighborhoodNameColumn = reader.GetOrdinal("Name");
                        string neighborhoodNameValue = reader.GetString(neighborhoodNameColumn);

                        var walker = new Walker()
                        {
                            Id = idValue,
                            Name = walkerNameValue,
                            NeighborhoodId = neighborhoodIdValue,
                            Neighborhood = new Neighborhood()
                            {
                                Id = neighborhoodIdValue,
                                Name = neighborhoodNameValue

                            }
                        };

                        allWalkers.Add(walker);
                    }

                    reader.Close();

                    return allWalkers;
                }
            }

        }


    }
}
