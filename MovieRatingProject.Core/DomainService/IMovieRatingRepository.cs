using MovieRatingProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRatingProject.Core.DomainService
{
   public interface IMovieRatingRepository
    {
        IList<MovieRating> GetAllMovieRatings();



    }
}
