using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface ICountryRepository {
        List<fmmCountry> GetCountries(string sort);

        fmmCountry GetCountry(int id);

        bool Insert(fmmCountry country);

        bool Delete(int id);
    }
}
