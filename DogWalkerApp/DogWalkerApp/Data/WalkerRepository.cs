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
                        Select w.Id, w.Name, w.NeighborhoodId, n.Name NeighborhoodName
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

                        int neighborhoodNameColumn = reader.GetOrdinal("NeighborhoodName");
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

        public void DeleteWalker(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Walker WHERE ID = @ID";

                    cmd.Parameters.Add(new SqlParameter("@id", walkerId));

                    cmd.ExecuteNonQuery();
                }
            }

        }

        public Walker addWalker(Walker walkerToAdd)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Walker (Name, NeighborhoodId)
                        OUTPUT INSERTED.Id
                        VALUES (@Name, @NeighborhoodId)";

                    cmd.Parameters.Add(new SqlParameter("@Name", walkerToAdd.Name));
                    cmd.Parameters.Add(new SqlParameter("@NeighborhoodId", walkerToAdd.NeighborhoodId));

                    int id = (int)cmd.ExecuteScalar();

                    walkerToAdd.Id = id;

                    return walkerToAdd;
                }
            }

        }
        public void updateWalker(int walkerId, Walker walker)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Walker
                        SET Name = @Name, NeighborhoodId = @NeighborhoodId
                        Where Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@Name", walker.Name));
                    cmd.Parameters.Add(new SqlParameter("@NeighborhoodId", walker.NeighborhoodId));
                    cmd.Parameters.Add(new SqlParameter("@id", walkerId));
                    // We don't expect anything back from the database(It's not a real query so we say execute non query)
                    cmd.ExecuteNonQuery();
                }
            }
        }



        public List<Walker> getWalkersByNeighborhoodId(int neighborhoodId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        Select w.Id, w.Name, w.NeighborhoodId, n.Name NeighborhoodName
                        From Walker w
                        Left Join Neighborhood n
                        ON w.NeighborhoodId = n.Id
                        Where n.Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", neighborhoodId));

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Walker> foundWalkers = new List<Walker>();
                    //use while if expecting multiple returns. Use if for 1
                    while (reader.Read())
                    {
                        int idColumn = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumn);

                        int walkerNameColumn = reader.GetOrdinal("Name");
                        string walkerNameValue = reader.GetString(walkerNameColumn);

                        int neighborhoodIdColumn = reader.GetOrdinal("NeighborhoodId");
                        int neighborhoodIdValue = reader.GetInt32(neighborhoodIdColumn);

                        int neighborhoodNameColumn = reader.GetOrdinal("NeighborhoodName");
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
                        foundWalkers.Add(walker);

                    };
                        reader.Close();
                        return foundWalkers;
                    
                }


            }


        }
    }
}
