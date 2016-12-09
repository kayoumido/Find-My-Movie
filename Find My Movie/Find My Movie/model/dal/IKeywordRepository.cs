using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface IKeywordRepository {
        List<Keyword> GetKeywords(string sort);

        Keyword GetKeyword(int id);

        bool Insert(Keyword keyword);

        bool Delete(int id);
    }
}
