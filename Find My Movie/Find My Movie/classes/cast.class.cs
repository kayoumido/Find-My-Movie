using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Find_My_Movie;

namespace classes {
    class cast {

        private int id;
        private int castId;
        private string creditId;
        private string name;
        private string image;
        private string character;
        private int order;

        public int Id {
            get {
                return id;
            }

            set {
                id = value;
            }
        }

        public int CastId {
            get {
                return castId;
            }

            set {
                castId = value;
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

        public string Character {
            get {
                return character;
            }

            set {
                character = value;
            }
        }

        public int Order {
            get {
                return order;
            }

            set {
                order = value;
            }
        }
    }

}
