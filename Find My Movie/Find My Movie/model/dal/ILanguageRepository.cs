using System.Collections.Generic;

namespace Find_My_Movie.model.dal {
    interface ILanguageRepository {
        List<fmmLanguage> GetLanguages();

        fmmLanguage GetLanguage(int id);

        bool Insert(fmmLanguage language, int movieID);

        bool Delete(int id);
    }
}
