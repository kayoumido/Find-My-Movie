using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model {
    class fmmMovie {

        public int id { get; set; }
        public string imdbid { get; set; }
        public string title { get; set; }
        public string ogtitle { get; set; }
        public bool adult { get; set; }
        public long budget { get; set; }
        public string homepage { get; set; }
        public int? runtime { get; set; }
        public string tagline { get; set; }
        public double voteaverage { get; set; }
        public string oglanguage { get; set; }
        public string overview { get; set; }
        public double popularity { get; set; }
        public string poster { get; set; }
        public string releasedate { get; set; }
        public int fk_collection { get; set; }


    }

}
