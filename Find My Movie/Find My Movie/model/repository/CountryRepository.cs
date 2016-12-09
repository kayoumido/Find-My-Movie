using Find_My_Movie.model.dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.repository {
    class CountryRepository : ICountryRepository {
        public List<fmmCountry> GetCountries(string sort) {
            throw new NotImplementedException();
        }

        public fmmCountry GetCountry(int id) {
            throw new NotImplementedException();
        }

        public bool Insert(fmmCountry country) {
            throw new NotImplementedException();
        }

        public bool Delete(int id) {
            throw new NotImplementedException();
        }
    }
}
