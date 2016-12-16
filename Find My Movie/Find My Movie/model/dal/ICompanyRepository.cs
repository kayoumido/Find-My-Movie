using System.Collections.Generic;

namespace Find_My_Movie.model.dal {
    interface ICompanyRepository {
        List<fmmCompany> GetCompanies();

        fmmCompany GetCompany(int id);

        bool Insert(fmmCompany company, int movieID);

        bool Delete(int id);
    }
}
