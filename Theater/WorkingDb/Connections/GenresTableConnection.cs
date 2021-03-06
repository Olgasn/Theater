﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Theater.Models.Plays;
using Theater.WorkingDb.Interfaces;

namespace Theater.WorkingDb.Connections
{
    public class GenresTableConnection:IGenreDao
    {
        public static string ConnectionStr = ConfigurationManager.ConnectionStrings["TheaterDb"].ConnectionString;

        private const string readGenres = @"SELECT *
                                           FROM genres";

        private string searchGenreById = @"SELECT *
                                         FROM genres
                                         WHERE Id=@Id";


        private static GenresTableConnection instance;
        public static GenresTableConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GenresTableConnection();
                }
                return instance;
            }
        }

        private GenresTableConnection()
        {

        }
        public List<Genre> GetAllGenres()
        {
            List<Genre> genres = new List<Genre>();
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand commandGenres = new SqlCommand(readGenres, connection);
                using (SqlDataReader readerPlays = commandGenres.ExecuteReader())
                {
                    while (readerPlays.Read())
                    {
                        genres.Add(new Genre(Convert.ToInt32(readerPlays.GetValue(0)),
                                           Convert.ToString(readerPlays.GetValue(1))
                                       ));
                    }
                }
            }
            return genres;
        }

        public Genre GetGenreById(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(searchGenreById, connection);

                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Genre(id,
                                         Convert.ToString(reader.GetValue(1)));
                    }
                }
            }
            return null;
        }
    }
}