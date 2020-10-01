using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using Moq;
using MovieRatingProject.Core.Entities;
using MovieRatingProject.Core.DomainService;
using MovieRatingProject.Core.ApplicationService;

namespace XUnitMovieRatingTest
{
    public class MovieRatingServiceTest
    {
        private List<MovieRating> ratings = null;
        private Mock<IMovieRatingRepository> repoMock;

        public MovieRatingServiceTest()
        {
            repoMock = new Mock<IMovieRatingRepository>();
            repoMock.Setup(repo => repo.GetAllMovieRatings()).Returns(() => ratings);
        }



        //  1. On input N, what are the number of reviews from reviewer N? 

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        public void GetNumberOfReviewsFromReviewer(int reviewer, int expected)
        {
            // arrange
            ratings = new List<MovieRating>()
            {
                new MovieRating(2, 1, 3, DateTime.Now),
                new MovieRating(3, 1, 4, DateTime.Now),
                new MovieRating(3, 2, 3, DateTime.Now),
                new MovieRating(4, 1, 4, DateTime.Now)
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);

            // act

            int result = mrs.GetNumberOfReviewsFromReviewer(reviewer);

            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }

        //2. On input N, what is the average rate that reviewer N had given?

        [Theory]
        //[InlineData(1, 0.0)]
        [InlineData(2, 3.0)]
        [InlineData(3, 5)]//må man ikke lave udregninger her i eller hvad??
        
        public void GetAverageRateFromReviewer(int reviewer, double expeted)
        {
            // arrange
            ratings = new List<MovieRating>()
            {
                new MovieRating(2, 1, 3, DateTime.Now),
                new MovieRating(3, 1, 5, DateTime.Now),
                new MovieRating(3, 2, 5, DateTime.Now),
                new MovieRating(3, 3, 5, DateTime.Now)
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);
            //act
            double result = mrs.GetAverageRateFromReviewer(reviewer);

            //assert
            Assert.Equal( expeted, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);

        }
        [Fact]
        public void GetAverageRateFromReviewerExpetcingArugumentNullException()
        {
            // arrange
            ratings = new List<MovieRating>()
            {
                new MovieRating(2, 1, 3, DateTime.Now),
                new MovieRating(3, 1, 5, DateTime.Now),
                new MovieRating(3, 2, 5, DateTime.Now),
                new MovieRating(3, 3, 5, DateTime.Now)
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);
            //act
          

            var ex = Assert.Throws<Exception>(() => { double result = mrs.GetAverageRateFromReviewer(1); });

            Assert.Equal("stop stop!!!", ex.Message);
        }

        //  7. What is the id(s) of the movie(s) with the highest number of top rates (5)? 
        [Fact]
        public void GetMoviesWithHighestNumberOfTopRates()
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 1, 5, DateTime.Now),
                new MovieRating(1, 2, 5, DateTime.Now),

                new MovieRating(2, 1, 4, DateTime.Now),
                new MovieRating(2, 2, 5, DateTime.Now),

                new MovieRating(2, 3, 5, DateTime.Now),
                new MovieRating(3, 3, 5, DateTime.Now),
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);

            List<int> expected = new List<int>() { 2, 3 };

            // act
            var result = mrs.GetMoviesWithHighestNumberOfTopRates();

            // assert
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);

        }
    }
}
