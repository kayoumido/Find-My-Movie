using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Find_My_Movie;

namespace classes {
    class movie {

        private int id;
        private string imdbId;
        private string title;
        private string ogTitle;
        private string altTitle;
        private int adult;
        private double budget;
        private string homepage;
        private double runTime;
        private string tagLine;
        private double voteAverage;
        private string ogLanguage;
        private string overview;
        private double popularity;
        private string poster;
        private string releaseDate;
        private int fkCollection;

        public int Id {
            get {
                return id;
            }

            set {
                id = value;
            }
        }

        public string ImdbId {
            get {
                return imdbId;
            }

            set {
                imdbId = value;
            }
        }

        public string Title {
            get {
                return title;
            }

            set {
                title = value;
            }
        }

        public string OgTitle {
            get {
                return ogTitle;
            }

            set {
                ogTitle = value;
            }
        }

        public string AltTitle {
            get {
                return altTitle;
            }

            set {
                altTitle = value;
            }
        }

        public int Adult {
            get {
                return adult;
            }

            set {
                adult = value;
            }
        }

        public double Budget {
            get {
                return budget;
            }

            set {
                budget = value;
            }
        }

        public string Homepage {
            get {
                return homepage;
            }

            set {
                homepage = value;
            }
        }

        public double RunTime {
            get {
                return runTime;
            }

            set {
                runTime = value;
            }
        }

        public string TagLine {
            get {
                return tagLine;
            }

            set {
                tagLine = value;
            }
        }

        public double VoteAverage {
            get {
                return voteAverage;
            }

            set {
                voteAverage = value;
            }
        }

        public string OgLanguage {
            get {
                return ogLanguage;
            }

            set {
                ogLanguage = value;
            }
        }

        public string Overview {
            get {
                return overview;
            }

            set {
                overview = value;
            }
        }

        public double Popularity {
            get {
                return popularity;
            }

            set {
                popularity = value;
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

        public string ReleaseDate {
            get {
                return releaseDate;
            }

            set {
                releaseDate = value;
            }
        }

        public int FkCollection {
            get {
                return fkCollection;
            }

            set {
                fkCollection = value;
            }
        }
    }

}
