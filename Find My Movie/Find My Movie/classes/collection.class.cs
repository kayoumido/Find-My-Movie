using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Find_My_Movie;

namespace classes {
    class collection {

        private int id;
        private string name;
        private string poster;

        public collection() {

        }

        public int Id {
            get {
                return id;
            }

            set {
                id = value;
            }
        }

        public string Name {
            get {
                return name;
            }

            set {
                name = value;
            }
        }

        public string Poster {
            get {
                return poster;
            }

            set {
                poster = value;
            }
        }
    }
}
