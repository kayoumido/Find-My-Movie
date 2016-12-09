using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.repository {
    class GenreRepository : IGenreRepository {
        public List<fmmGenre> GetGenres(string sort) {
            throw new NotImplementedException();
        }

        public fmmGenre GetGenre(int id) {
            throw new NotImplementedException();
        }

        public bool Insert(fmmGenre genre) {
            throw new NotImplementedException();
        }

        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
