using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieRatingProject.Core.ApplicationService;
using MovieRatingProject.Core.DomainService;
using MovieRatingProject.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSUnitTestProject
{
    [TestClass]
    public class MovieRatingServicePerformanceTest
    {
        const int TIMEOUT_IN_MILLIS = 4_000;

        const string JSÒN_FILE_NAME = @"C:\Users\Christina\Downloads\ratings.json";

        private static IMovieRatingRepository repository;

        private static int reviewerMostReviews;
        private static int movieMostReviews;

        [ClassInitialize]
        public static void SetupTest(TestContext tc)
        {
            repository = new MovieRatingRepository(JSÒN_FILE_NAME);

            reviewerMostReviews = repository.Ratings
               .GroupBy(r => r.Reviewer)
               .Select(grp => new
               {
                   reviewer = grp.Key,
                   reviews = grp.Count()
               })
               .OrderByDescending(grp => grp.reviews)
               .Select(grp => grp.reviewer)
               .FirstOrDefault();

            movieMostReviews = repository.Ratings
                .GroupBy(r => r.Movie)
                .Select(grp => new
                {
                    movie = grp.Key,
                    reviews = grp.Count()
                })
                .OrderByDescending(grp => grp.reviews)
                .Select(grp => grp.movie)
                .FirstOrDefault();
        }

        [TestMethod]
        [Timeout(TIMEOUT_IN_MILLIS)]
        public void GetNumberOfReviewsFromReviewer()
        {
            MovieRatingService mrs = new MovieRatingService(repository);
            mrs.GetNumberOfReviewsFromReviewer(reviewerMostReviews);
        }

        [TestMethod]
        [Timeout(TIMEOUT_IN_MILLIS)]
        public void GetAverageRateFromReviewer()
        {
            MovieRatingService mrs = new MovieRatingService(repository);

            mrs.GetAverageRateFromReviewer(reviewerMostReviews);
        }

        [TestMethod]
        [Timeout(TIMEOUT_IN_MILLIS)]
        public void GetTopRatedMovies()
        {
            MovieRatingService mrs = new MovieRatingService(repository);

            mrs.GetTopRatedMovies(20);
        }
    }
}
