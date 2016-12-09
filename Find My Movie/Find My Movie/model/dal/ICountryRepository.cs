using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface ICountryRepository {
        List<Country> GetCountries(string sort);

        Country GetCountry(int id);

        bool Insert(Country country);

        bool Delete(int id);
    }
}
