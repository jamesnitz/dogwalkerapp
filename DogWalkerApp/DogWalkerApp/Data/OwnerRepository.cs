using DogWalkerApp.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogWalkerApp.Data
{
    public class OwnerRepository
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

        //First Method
        public List<Owner> getAllOwners()
        {
            //Opens the connection
            using (SqlConnection conn = Connection)
            {
                //opens the gates for the connection
                conn.Open();

                //Here's the sql command 
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        Select o.Id, o.Name, o.Address, o.NeighborhoodId, n.Name NeighborhoodName
                        FROM Owner o
                        Left Join Neighborhood n
                        ON o.NeighborhoodId = n.Id";
                    //Execute reader actually runs the sql command
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Owner> allOwners = new List<Owner>();

                    while (reader.Read())
                    {
                        //get ordinal returns us what position the Id column is in 
                        int idColumn = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumn);

                        int nameColumn = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumn);

                        int addressColumn = reader.GetOrdinal("Address");
                        string addressValue = reader.GetString(addressColumn);

                        int neighborIdColumn = reader.GetOrdinal("NeighborhoodId");
                        int neighborhoodIdValue = reader.GetInt32(neighborIdColumn);

                        int neighborhoodNameColumn = reader.GetOrdinal("NeighborhoodName");
                        string neighborhoodNameValue = reader.GetString(neighborhoodNameColumn);

                        var owner = new Owner()
                        {
                            Id = idValue,
                            Name = nameValue,
                            Address = addressValue,
                            NeighborhoodId = neighborhoodIdValue,
                            Neighborhood = new Neighborhood()
                            {
                                Id = neighborhoodIdValue,
                                Name = neighborhoodNameValue

                            }

                        };

                        allOwners.Add(owner);
                    }

                    reader.Close();

                    return allOwners;
                }
            }
        }

        //Next Method
        public Owner addOwner(Owner ownertoAdd)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Owner (Name, Address, NeighborhoodId, Phone)
                        OUTPUT INSERTED.Id
                        VALUES (@Name, @Address, @NeighborhoodId, @Phone)";

                    cmd.Parameters.Add(new SqlParameter("@Name", ownertoAdd.Name));
                    cmd.Parameters.Add(new SqlParameter("@Address", ownertoAdd.Address));
                    cmd.Parameters.Add(new SqlParameter("@NeighborhoodId", ownertoAdd.NeighborhoodId));
                    cmd.Parameters.Add(new SqlParameter("@Phone", ownertoAdd.Name));

                    int id = (int)cmd.ExecuteScalar();

                    ownertoAdd.Id = id;

                    return ownertoAdd;
                }
            }

        }
    }
}
