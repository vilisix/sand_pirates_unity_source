    using System;
    using System.Xml;
    using Random = UnityEngine.Random;

    public class NameFactory
    {
        private static string[] names = new string[50]
        {
            "Dick",
            "Mario",
            "Goopy",
            "Renato",
            "Eli",
            "Rickey",
            "Jefferey",
            "Bret",
            "Derek",
            "Wesley",
            "Chuck",
            "Daren",
            "Jay",
            "Floren",
            "Billy",
            "Oliver",
            "Miguel",
            "Freeman",
            "Kip",
            "Anibal",
            "Pasqe",
            "Mau",
            "Keneth",
            "Manual",
            "Dalton",
            "Carson",
            "Steve",
            "Jason",
            "Jamaal",
            "Bryce",
            "Dario",
            "Lewis",
            "Wilfred",
            "Graham",
            "Clark",
            "Reuben",
            "Les",
            "Enoch",
            "Rubin",
            "Mitch",
            "Raymon",
            "Cletus",
            "Arturo",
            "Russel",
            "Steven",
            "Al",
            "Willy",
            "Devon",
            "Normand",
            "Franny"
        };

        public static string GetRandomName()
        {
            return names[Random.Range(0, 49)];
        }
        
    }
