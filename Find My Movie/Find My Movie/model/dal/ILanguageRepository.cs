using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface ILanguageRepository {
        List<fmmLanguage> GetLanguages(string sort);

        fmmLanguage GetLanguage(int id);

        bool Insert(fmmLanguage language, int movieID);

        bool Delete(int id);
    }
}
