using MovieRatingProject.Core.DomainService;
using MovieRatingProject.Core.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MovieRatingProject.Infrastructure.Data
{
    public class MovieRatingRepository : IMovieRatingRepository
    {
        public MovieRating[] Ratings { get; private set; }
        
        
        public IList<MovieRating> GetAllMovieRatings()
        {
            return Ratings;
        }

        public MovieRatingRepository(string JsonFileName)
        {
            //Stopwatch sw = Stopwatch.StartNew();
            Ratings = ReadAllRatings(JsonFileName);
            //sw.Stop();
            //Console.WriteLine("Time = {0:f4} seconds", sw.ElapsedMilliseconds / 1000d);
        }

        private MovieRating[] ReadAllRatings(string jsonFileName)
        {
            var ratingsList = new List<MovieRating>();

            using (StreamReader streamReader = new StreamReader(jsonFileName))
            using (JsonTextReader reader = new JsonTextReader(streamReader))
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        MovieRating mr = ReadOneMovieRating(reader);
                        ratingsList.Add(mr);
                    }
                }
                return ratingsList.ToArray();
            }
        }

        private static MovieRating ReadOneMovieRating(JsonTextReader reader)
        {
            reader.Read();
            int reviewer = (int)reader.ReadAsInt32();

            reader.Read();
            int movie = (int)reader.ReadAsInt32();

            reader.Read();
            int grade = (int)reader.ReadAsInt32();

            reader.Read();
            DateTime date = (DateTime)reader.ReadAsDateTime();

            return new MovieRating(reviewer, movie, grade, date);
        }
    }
}

