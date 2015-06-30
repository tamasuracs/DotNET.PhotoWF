using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoWF.Common
{
    /// <summary>
    /// Class defining STRING constants
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Best folder name
        /// </summary>
        public static string BEST = "best";

        /// <summary>
        /// Exif TAG signature 
        /// </summary>
        public static string PROGRAM_TAG_SIGNATURE = "PROCESSED_BY_TAG_POPULATOR_APP";

        public static string PATH_DELIMITER = "/";

        public static string[] DELIMITERS = new string[]{
                                                            PATH_DELIMITER,
                                                            "_",
                                                            "/",
                                                            ".",
                                                            "+",
                                                            "?",
                                                            "%"
                                                        };

        /// <summary>
        /// Pattern for the date prefixed "main" folders
        /// </summary>
        public static string MAIN_FOLDER_PATTERN = string.Format(@"(\d\d\d\d[-_]\d\d[-_\d\d]*[^\s]*[^\{0}]*)", PATH_DELIMITER);

        /// <summary>
        /// Pattern for the best folder 
        /// </summary>
        public static string BEST_FOLDER_PATTERN = string.Format(@"([^\{0}]*{1}[^\{0}]*)", PATH_DELIMITER, BEST);

        /// <summary>
        /// Patterns of the file path that should be processed
        /// [pattern to be processed][pattern of the new value]
        /// </summary>
        public static string[][] PATH_PATTERNS_TO_ELIMINATE = new string[][]
                                                                { 
                                                                    //Probably a YYYYMMDD string
                                                                    new string[]{@"(\d\d\d\d)(\d\d)(\d\d)" , "$1-$2-$3"},

                                                                    // Space inclusion for e.g.: "9photo" --> "9 photo"
                                                                    new string[]{@"([\d]+)([a-zA-Z]+)" , "$1 $2"},

                                                                    // Space inclusion for e.g.: "photo9" --> "photo 9"
                                                                    new string[]{@"([a-zA-Z]+)([\d]+)" , "$1 $2"}
                                                                };

        /// <summary>
        /// Normalization dictionary - used when all the EXIF keys are processed
        /// [pattern to be processed][pattern of the new value]
        /// </summary>
        public static string[][] NORMALIZATION_DICT = new string[][]
        {
            //Frequently used names
            new string[]{"Zo[eé][a-z,]*" , "Zoé"},
            new string[]{"Zozi" , "Zoé"},
            new string[]{"[AÁ]gi[a-z]*" , "Ági"},
            new string[]{"Otti[a-z]+" , "Otti"},
            new string[]{"Kati[a-z]+" , "Kati"},
            new string[]{"Any[aá][a-z]*" , "Anya"},
            new string[]{"Alex[a-z]+" , "Alex"},
            new string[]{"Betty[a-z]+" , "Betty"},
            new string[]{"Dan[a-z]*" , "Dani"},
            new string[]{"Krisz[a-z]*" , "Krisz"},
            new string[]{"Peti[a-z]*" , "Peti"},
            new string[]{"Robi[a-z]*" , "Robi"},

            //Frequently used locations
            new string[]{"Frankfurt[a-z]+" , "Frankfurt"},
            new string[]{"Ff[a-z]*" , "Frankfurt"},
            new string[]{"Höchst[a-z]+" , "Höchst"},
            new string[]{"Keszthely[a-z]+" , "Keszthely"},
            new string[]{"Nürnberg[a-z]+" , "Nürnberg"},
            new string[]{"Budapest[a-z]+" , "Budapest"},
            new string[]{"^Pest$" , "Budapest"},

            //Others
            new string[]{"Csal[aá]d[a-z]*" , "Család"},
            new string[]{"Karácsony[a-z]+" , "Karácsony"},
            new string[]{"Túrázás" , "Túra"},
            new string[]{"^Esk[uü]v[őo][a-z]*" , "Esküvő"},
            new string[]{"^Im$" , "Ironman"},

            //Camera generated  substrings
            new string[]{"Dsc" , null},
            new string[]{"Jpg" , null},
            new string[]{"Img" , null},
            new string[]{@"Image[\-_]*" , null},
            new string[]{@"^P\d\d\d\d+" , null},

            //"And"
            new string[]{@"^Az$" , null},
            new string[]{@"^Es$" , null},
            new string[]{@"^És$" , null},

            // To, In english words
            new string[]{@"^In$" , null},
            new string[]{@"^To$" , null},

            //Min 3 digit 
            new string[]{@"^\d\d\d+$" , null},

            //Special characters at the beginning of a word
            new string[]{@"^[-]?\d*$" , null}
        };
    }
}
