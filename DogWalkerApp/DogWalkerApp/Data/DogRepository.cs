using DogWalkerApp.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogWalkerApp.Data
{
   public class DogRepository
    {
        //The Connection to the SQL Server
        public SqlConnection Connection
        {
            get
            {
                string _connectionString = "Data Source=localhost\\SQLEXPRESS; Initial Catalog=DogWalkerApp; Integrated Security=True";
                return new SqlConnection(_connectionString);
            }
        }

        //First Method
        public List<Dog> GetAllDogs()
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
                        Select d.Id, d.Name, d.OwnerId, d.Breed, d.Notes, o.Name
                        FROM Dog d
                        Left Join Owner o
                        ON d.OwnerId = o.Id";
                    //Execute reader actually runs the sql command
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> allDogs = new List<Dog>();

                    while(reader.Read())
                    {
                        //get ordinal returns us what position the Id column is in 
                        int idColumn = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumn);

                        int nameColumn = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumn);

                        int ownerIdColumn = reader.GetOrdinal("OwnerId");
                        int ownerIdValue = reader.GetInt32(ownerIdColumn);

                        int breedIdColumn = reader.GetOrdinal("Breed");
                        string breedValue = reader.GetString(breedIdColumn);

                        int notesIdColumn = reader.GetOrdinal("Notes");
                        string notesValue = reader.GetString(notesIdColumn);

                        int ownerNameColumn = reader.GetOrdinal("Name");
                        string ownerNameValue = reader.GetString(ownerNameColumn);

                        var dog = new Dog()
                        {
                            Id = idValue,
                            Name = nameValue,
                            OwnerId = ownerIdValue,
                            Breed = breedValue,
                            Notes = notesValue,
                            Owner = new Owner()
                            {
                                Id = ownerIdValue,
                                Name = ownerNameValue

                            }

                        };

                        allDogs.Add(dog);
                    }

                    reader.Close();

                    return allDogs;
                }
            }
        }

        //Next Method

    }
}
