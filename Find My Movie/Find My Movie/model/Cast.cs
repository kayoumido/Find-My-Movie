﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_My_Movie.model {
    public class Cast {
        public int id { get; set; }
        public int castid { get; set; }
        public string creditid { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string character { get; set; }
        public int order { get; set; }
    }
}
