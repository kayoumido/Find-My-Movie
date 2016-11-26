using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Find_My_Movie;
using Castle.ActiveRecord;

namespace classes {
    [ActiveRecord("collection")]
    class collection : ActiveRecordBase<collection> {

        [PrimaryKey(PrimaryKeyType.Native, "id")]
        public int id { get; set; }

        [Property("name")]
        public string name { get; set; }

        [Property("poster")]
        public string poster { get; set; }

    }
}
