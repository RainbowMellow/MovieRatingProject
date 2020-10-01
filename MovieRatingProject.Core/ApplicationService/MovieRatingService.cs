using MovieRatingProject.Core.DomainService;
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
            throw new NotImplementedException();
        }

        public object GetMoviesWithHighestNumberOfTopRates()
        {
            throw new NotImplementedException();
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
    }
}
