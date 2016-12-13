using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface ICompanyRepository {
        List<fmmCompany> GetCompanies(string sort);

        fmmCompany GetCompany(int id);

        bool Insert(fmmCompany company, int movieID);

        bool Delete(int id);
    }
}
