using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Find_My_Movie;

namespace classes {
    class crew {

        private int id;
        private string creditId;
        private string name;
        private string image;

        public int Id {
            get {
                return id;
            }

            set {
                id = value;
            }
        }

        public string CreditId {
            get {
                return creditId;
            }

            set {
                creditId = value;
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

        public string Image {
            get {
                return image;
            }

            set {
                image = value;
            }
        }
    }

}
