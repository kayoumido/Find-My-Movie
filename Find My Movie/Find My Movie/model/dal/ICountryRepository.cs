using System.Collections.Generic;

namespace Find_My_Movie.model.dal {
    interface ICountryRepository {
        List<fmmCountry> GetCountries();

        fmmCountry GetCountry(int id);

        bool Insert(fmmCountry country, int movieID);

        bool Delete(int id);
    }
}
