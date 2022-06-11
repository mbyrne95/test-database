using System;

namespace ChampionCore
{
    public interface IIndex<K, V>
    {
        //create new entry at index mapping key K to value V
        void Insert(K key, V value);

        //find entry by key
        Tuple<K, V> Get(K key);

        //find all entries that contain key larger than/equal to specified key
        IEnumerable<Tuple<K, V>> LargerThanOrEqualTo(K key);

        //find all entries larger than key
        IEnumerable<Tuple<K, V>> LargerThan(K key);
        
        //find all entries that contain key less than/equal to specified key
        IEnumerable<Tuple<K, V>> LessThanOrEqualTo(K key);

        //find all entries less than key
        IEnumerable<Tuple<K, V>> LessThan(K key);

        //delete entry from this index
        bool Delete(K key, V value, IComparer<V> valueComparer = null);

        //delete all entries of given key
        bool Delete(K key);
    }

}