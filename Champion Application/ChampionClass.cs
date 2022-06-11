using System;

namespace ChampionApplication
{
    //database stores champions - define champion class
    public class championModel
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string[] Abilities { get; set; }
        public int[] StartingStats { get; set; }
        public override string ToString()
        {
            return string.Format("[Champion: Name={0}, Tagline={1}, Abilities={3}, StartingStats={4}]", Name, Tag, Abilities, StartingStats);
        }

    }
}