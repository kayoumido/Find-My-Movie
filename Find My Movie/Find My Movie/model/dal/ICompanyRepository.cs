using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model.dal {
    interface ICompanyRepository {
        List<Company> GetCompanies(string sort);

        Company GetCompany(int id);

        bool Insert(Company company);

        bool Delete(int id);
    }
}
