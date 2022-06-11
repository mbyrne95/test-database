using System;
using System.Collections.Generic;

namespace ChampionApplication
{
    public interface IChampionDatabase
    {
        void Insert();
        void Update();
        void Delete();
        championModel Find(string nane);

    }
}
