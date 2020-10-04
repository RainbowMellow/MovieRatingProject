using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using Moq;
using MovieRatingProject.Core.Entities;
using MovieRatingProject.Core.DomainService;
using MovieRatingProject.Core.ApplicationService;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Globalization;

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


        #region 1. On input N, what are the number of reviews from reviewer N? 

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
        #endregion

        #region 2. On input N, what is the average rate that reviewer N had given?

        [Theory]
        //[InlineData(1, 0.0)]
        [InlineData(2, 3.0)]
        [InlineData(3, 5)]//må man ikke lave udregninger her i eller hvad??

        public void GetAverageRateFromReviewer(int reviewer, double expected)
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
            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);

        }

        [Fact]
        public void GetAverageRateFromReviewerExpectingArugumentNullException()
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
        #endregion

        #region 3. On input N and R, how many times has reviewer N given rate R?

        [Theory]
        [InlineData(2, 3, 1)]
        [InlineData(3, 5, 3)]
        public void GetNumberOfRatesByReviewer(int reviewer, int rate, int expected)
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

            int result = mrs.GetNumberOfRatesByReviewer(reviewer, rate);

            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }

        [Fact]
        public void GetNumberOfRatesByReviewerExpectingArgumentException()
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

            var ex = Assert.Throws<ArgumentException>(() => { int result = mrs.GetNumberOfRatesByReviewer(1, 3); });

            Assert.Equal("No reviews from the reviewer with this rate.", ex.Message);
        }

        #endregion

        #region 4. On input N, how many have reviewed movie N?
        [Theory]
        [InlineData(2, 1)]
        [InlineData(3, 3)]
        public void GetNumberOfReviews(int movie, int expected)
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 2, 3, DateTime.Now),
                new MovieRating(1, 3, 1, DateTime.Now),
                new MovieRating(2, 3, 3, DateTime.Now),
                new MovieRating(3, 3, 5, DateTime.Now)
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);

            int result = mrs.GetNumberOfReviews(movie);

            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }

        [Fact]
        public void GetNumberOfReviewsExpectingArgumentException()
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 2, 3, DateTime.Now),
                new MovieRating(1, 3, 1, DateTime.Now),
                new MovieRating(2, 3, 3, DateTime.Now),
                new MovieRating(3, 3, 5, DateTime.Now)
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);

            var ex = Assert.Throws<ArgumentException>(() => { int result = mrs.GetNumberOfReviews(1); });

            Assert.Equal("No reviews for this movie.", ex.Message);
        }

        #endregion

        #region 5. On input N, what is the average rate the movie N had received?
        [Theory]
        [InlineData(2, 3)]
        [InlineData(3, 5)]
        public void GetAverageRateOfMovie(int movie, double expected)
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 2, 3, DateTime.Now),
                new MovieRating(1, 3, 5, DateTime.Now),
                new MovieRating(2, 3, 5, DateTime.Now),
                new MovieRating(3, 3, 5, DateTime.Now)
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);

            double result = mrs.GetAverageRateOfMovie(movie);

            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }

        [Fact]
        public void GetAverageRateOfMovieExpectingArgumentException()
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 2, 3, DateTime.Now),
                new MovieRating(1, 3, 5, DateTime.Now),
                new MovieRating(2, 3, 5, DateTime.Now),
                new MovieRating(3, 3, 5, DateTime.Now)
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);

            var ex = Assert.Throws<ArgumentException>(() => { double result = mrs.GetAverageRateOfMovie(1); });

            Assert.Equal("No reviews for this movie.", ex.Message);
        }

        #endregion

        #region 6. On input N and R, how many times had movie N received rate R?

        [Theory]
        [InlineData(2, 3, 1)]
        [InlineData(3, 5, 3)]
        public void GetNumberOfRates(int movie, int rate, int expetedresult)
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 2, 3, DateTime.Now),
                new MovieRating(1, 3, 5, DateTime.Now),
                new MovieRating(2, 3, 5, DateTime.Now),
                new MovieRating(3, 3, 5, DateTime.Now)
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);
            int result = mrs.GetNumberOfRates(movie, rate);

            Assert.Equal(expetedresult, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);

        }

        [Fact]
        public void GetNumberOfRatesArgumentException()
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 2, 3, DateTime.Now),
                new MovieRating(1, 3, 5, DateTime.Now),
                new MovieRating(2, 3, 5, DateTime.Now),
                new MovieRating(3, 3, 5, DateTime.Now)
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);

            var ex = Assert.Throws<ArgumentException>(() => { int result = mrs.GetNumberOfRates(1, 5); });

            Assert.Equal("No reviews for this movie.", ex.Message);
        }

        #endregion

        #region 7. What is the id(s) of the movie(s) with the highest number of top rates (5)? 
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
        #endregion

        #region 8. What reviewer(s) had done most reviews?

        [Fact]
        public void GetMostProductiveReviewers()
        {
            ratings = new List<MovieRating>()
            {
               new MovieRating(2, 1, 5, DateTime.Now),
               new MovieRating(2, 2, 5, DateTime.Now),
               new MovieRating(2, 3, 4, DateTime.Now),
               new MovieRating(2, 4, 5, DateTime.Now),

               new MovieRating(3, 2, 5, DateTime.Now),
               new MovieRating(3, 3, 5, DateTime.Now),

               new MovieRating(4, 3, 5, DateTime.Now),
               new MovieRating(4, 4, 5, DateTime.Now),
               new MovieRating(4, 5, 5, DateTime.Now),
               new MovieRating(4, 6, 5, DateTime.Now),
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);

            List<int> expected = new List<int>() { 2, 4 };


            var result = mrs.GetMostProductiveReviewers();

            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }

        #endregion

        #region 9. On input N, what is top N of movies? The score of a movie is its average rate.
<<<<<<< Updated upstream
        //public void GetTopRatedMovies(int amount)
        //{


        //}

        #endregion

        #region 10. On input N, what are the movies that reviewer N has reviewed? 
        //The list should be sorted decreasing by rate first, and date secondly.
        [Theory]
        [InlineData(2, new int[] { 4, 2, 1, 3 })]
        [InlineData(3, new int[] { 3, 2 })]
        [InlineData(4, new int[] { 4, 3, 1, 2 })]

        public void GetTopMoviesByReviewer(int reviewer, int[] expected)
        {
            ratings = new List<MovieRating>()
            {
               new MovieRating(2, 1, 5, DateTime.Now),
               new MovieRating(2, 2, 5, DateTime.Now),
               new MovieRating(2, 3, 4, DateTime.Now),
               new MovieRating(2, 4, 5, DateTime.Now),

               new MovieRating(3, 2, 1, DateTime.Now),
               new MovieRating(3, 3, 5, DateTime.Now),

               new MovieRating(4, 4, 5, DateTime.Parse("31/01/1999", new CultureInfo("da-DK"))),
               new MovieRating(4, 3, 5, DateTime.Parse("31/01/1998", new CultureInfo("da-DK"))),
               new MovieRating(4, 2, 4, DateTime.Parse("31/01/1997", new CultureInfo("da-DK"))),
               new MovieRating(4, 1, 5, DateTime.Parse("31/01/1996", new CultureInfo("da-DK"))),
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);

            var result = mrs.GetTopMoviesByReviewer(reviewer).ToArray();

            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }

        [Fact]
        public void GetTopMoviesByReviewerExpectingArgumentException()
        {
            ratings = new List<MovieRating>()
            {
               new MovieRating(2, 1, 5, DateTime.Now),
               new MovieRating(2, 2, 5, DateTime.Now),
               new MovieRating(2, 3, 4, DateTime.Now),
               new MovieRating(2, 4, 5, DateTime.Now),

               new MovieRating(3, 2, 1, DateTime.Now),
               new MovieRating(3, 3, 5, DateTime.Now),

               new MovieRating(4, 4, 5, DateTime.Parse("31/01/1999", new CultureInfo("da-DK"))),
               new MovieRating(4, 3, 5, DateTime.Parse("31/01/1998", new CultureInfo("da-DK"))),
               new MovieRating(4, 2, 4, DateTime.Parse("31/01/1997", new CultureInfo("da-DK"))),
               new MovieRating(4, 1, 5, DateTime.Parse("31/01/1996", new CultureInfo("da-DK"))),
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);

            var ex = Assert.Throws<ArgumentException>(() => { List<int> result = mrs.GetTopMoviesByReviewer(1); });

            Assert.Equal("This reviewer has not made any reviews yet.", ex.Message);
=======
        [Theory]
        [InlineData(1, new int[] { 4 })]
        [InlineData(2, new int[] { 4, 1 })]
        [InlineData(4, new int[] { 4, 1, 2, 3 })]
        [InlineData(10, new int[] { 4, 1, 2, 3 })]
        public void GetTopRatedMovies(int n, int[] expected)
        {
            ratings = new List<MovieRating>()
            {
                new MovieRating(1, 2, 3, DateTime.Now),     // movie 1 avg = 4                                                            
                new MovieRating(1, 3, 2, DateTime.Now),     // movie 2 avg = 3
                new MovieRating(2, 1, 4, DateTime.Now),     // movie 3 avg = 2.5
                new MovieRating(2, 3, 3, DateTime.Now),     // movie 4 avg = 4.5
                new MovieRating(2, 4, 4, DateTime.Now),
                new MovieRating(3, 4, 5, DateTime.Now)
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);

            var result = mrs.GetTopRatedMovies(n);

            Assert.Equal(new List<int>(expected), result);
>>>>>>> Stashed changes
        }

        #endregion

        #region 11. On input N, who are the reviewers that have reviewed movie N? 
        //The list should be sorted decreasing by rate first, and date secondly.
        [Theory]
        [InlineData(2, new int[] { 2, 4, 3})]
        [InlineData(1, new int[] { 2, 4 })]
        [InlineData(4, new int[] { 4 })]
        public void GetReviewersByMovie(int movie, int[] expected)
        {
            ratings = new List<MovieRating>()
            {
               new MovieRating(2, 1, 5, DateTime.Now),
               new MovieRating(2, 2, 5, DateTime.Now),
               new MovieRating(2, 3, 4, DateTime.Now),

               new MovieRating(3, 2, 1, DateTime.Now),
               new MovieRating(3, 3, 5, DateTime.Now),

               new MovieRating(4, 4, 5, DateTime.Parse("31/01/1999", new CultureInfo("da-DK"))),
               new MovieRating(4, 3, 5, DateTime.Parse("31/01/1998", new CultureInfo("da-DK"))),
               new MovieRating(4, 2, 4, DateTime.Parse("31/01/1997", new CultureInfo("da-DK"))),
               new MovieRating(4, 1, 5, DateTime.Parse("31/01/1996", new CultureInfo("da-DK"))),
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);

            var result = mrs.GetReviewersByMovie(movie).ToArray();

            Assert.Equal(expected, result);
            repoMock.Verify(repo => repo.GetAllMovieRatings(), Times.Once);
        }

        [Fact]
        public void GetReviewersByMovieExpectingArgumentException()
        {
            ratings = new List<MovieRating>()
            {
               new MovieRating(2, 4, 5, DateTime.Now),
               new MovieRating(2, 2, 5, DateTime.Now),
               new MovieRating(2, 3, 4, DateTime.Now),

               new MovieRating(3, 2, 1, DateTime.Now),
               new MovieRating(3, 3, 5, DateTime.Now),

               new MovieRating(4, 4, 5, DateTime.Parse("31/01/1999", new CultureInfo("da-DK"))),
               new MovieRating(4, 3, 5, DateTime.Parse("31/01/1998", new CultureInfo("da-DK"))),
               new MovieRating(4, 2, 4, DateTime.Parse("31/01/1997", new CultureInfo("da-DK"))),
               new MovieRating(4, 5, 5, DateTime.Parse("31/01/1996", new CultureInfo("da-DK"))),
            };

            MovieRatingService mrs = new MovieRatingService(repoMock.Object);

            var ex = Assert.Throws<ArgumentException>(() => { List<int> result = mrs.GetReviewersByMovie(1); });

            Assert.Equal("No reviews for this movie.", ex.Message);
        }
        #endregion


    }
}
