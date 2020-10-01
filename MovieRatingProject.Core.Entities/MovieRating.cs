using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRatingProject.Core.Entities
{
    public class MovieRating
    {
        public int Reviewer { get; private set; }
        public int Movie { get; private set; }
        public int Grade { get; private set; }
        public DateTime Date { get; private set; }

        public MovieRating(int r, int m, int g, DateTime d)
        {
            Reviewer = r;
            Movie = m;
            Grade = g;
            Date = d;
        }
    }
}
