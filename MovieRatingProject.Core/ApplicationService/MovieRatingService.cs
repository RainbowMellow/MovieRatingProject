using MovieRatingProject.Core.DomainService;
using MovieRatingProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieRatingProject.Core.ApplicationService
{
    public class MovieRatingService
    {
        public IMovieRatingRepository RatingRepo;

        public MovieRatingService(IMovieRatingRepository repo)
        {
            RatingRepo = repo;
        }

        public int NumberOfMoviesWithGrade(int grade)
        {
            throw new NotImplementedException();
        }

        public int GetNumberOfReviewsFromReviewer(int reviewer)
        {
            int count = 0;
            foreach (MovieRating m in RatingRepo.GetAllMovieRatings())
            {
                if (m.Reviewer == reviewer)
                {
                    count++;
                }
            }
            return count;
        }

        public object GetMoviesWithHighestNumberOfTopRates()
        {

            var movie5 = RatingRepo.GetAllMovieRatings()
                .Where(r => r.Grade == 5)
                .GroupBy(r => r.Movie)
                .Select(group => new {
                    Movie = group.Key,
                    MovieGrade5 = group.Count()
                });

            int max5 = movie5.Max(grp => grp.MovieGrade5);

         return  movie5
                .Where(grp => grp.MovieGrade5 == max5)
                .Select(grp => grp.Movie)
                .ToList();

           
        }

        public double GetAverageRateFromReviewer(int reviewer)
        {
            
            var AvgReview = RatingRepo.GetAllMovieRatings()
                .Where(r => r.Reviewer == reviewer);
            if (AvgReview.Any() == false)
            {
                throw new Exception("stop stop!!!"); 
            
            }

               var test = AvgReview.Select(r => r.Grade)
                .Average();

            return test;
        }

        public int GetNumberOfRatesByReviewer(int reviewer, int rate)
        {
            var reviews = RatingRepo.GetAllMovieRatings()
                .Where(r => r.Reviewer == reviewer)
                .Where(r => r.Grade == rate);

            if(reviews.Any() == false)
            {
                throw new ArgumentException("No reviews from the reviewer with this rate.");
            }

            return reviews.Count();
        }

        public int GetNumberOfReviews(int movie)
        {
            var reviews = RatingRepo.GetAllMovieRatings()
                .Where(r => r.Movie == movie);

            if(reviews.Any() == false)
            {
                throw new ArgumentException("No reviews for this movie.");
            }

            return reviews.Count();
        }

        public double GetAverageRateOfMovie(int movie)
        {
            var AvgReview = RatingRepo.GetAllMovieRatings()
                .Where(r => r.Movie == movie);

            if (AvgReview.Any() == false)
            {
                throw new ArgumentException("No reviews for this movie.");

            }

            var result = AvgReview.Select(r => r.Grade)
             .Average();

            return result;
        }

        public int GetNumberOfRates(int movie, int rate)
        {
            var reviews = RatingRepo.GetAllMovieRatings()
                 .Where(r => r.Movie == movie && r.Grade == rate);

            if(reviews.Any() == false)
            {

                throw new ArgumentException("No reviews for this movie.");
            }

            return reviews.Count();

        }

        public List<int> GetMostProductiveReviewers()
        {
            var reviewes = RatingRepo.GetAllMovieRatings()
                  .GroupBy(r => r.Reviewer)
                  .Select(group => new {
                      Reviewer = group.Key,
                      Movie = group.Count()
                  });

            int maxReview = reviewes.Max(grp => grp.Movie);

            return reviewes
                   .Where(grp => grp.Movie == maxReview)
                   .Select(grp => grp.Reviewer)
                   .ToList();

        }

        public List<int> GetTopMoviesByReviewer(int reviewer)
        {
            var movies = RatingRepo.GetAllMovieRatings()
                  .Where(r => r.Reviewer == reviewer)
                  .OrderByDescending(r => r.Grade)
                  .ThenByDescending(r => r.Date);

            if(movies.Any() == false)
            {
                throw new ArgumentException("This reviewer has not made any reviews yet.");
            }

            return movies
                   .Select(r => r.Movie)
                   .ToList();
        }

        public List<int> GetReviewersByMovie(int movie)
        {
            var movies = RatingRepo.GetAllMovieRatings()
                  .Where(r => r.Movie == movie)
                  .OrderByDescending(r => r.Grade)
                  .ThenByDescending(r => r.Date);

            if (movies.Any() == false)
            {
                throw new ArgumentException("No reviews for this movie.");
            }

            return movies
                   .Select(r => r.Reviewer)
                   .ToList();
        }

        public object GetTopRatedMovies(int amount)
        {
            return RatingRepo.GetAllMovieRatings()
              .GroupBy(r => r.Movie)
              .Select(grp => new
              {
                  Movie = grp.Key,
                  GradeAvg = grp.Average(x => x.Grade)
              })
              .OrderByDescending(grp => grp.GradeAvg)
              .Select(grp => grp.Movie)
              .Take(amount)
              .ToList();
        }
    }
}
