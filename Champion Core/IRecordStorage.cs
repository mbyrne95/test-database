using System;

namespace ChampionCore
{
    public interface IRecordStorage
    {
        //effectively update a record
        void Update(uint recordId, byte[] data);

        //grab record data
        byte[] Find(uint recordId);

        //creates new empty record
        uint Create();

        //create with given data, return id
        uint Create(byte[] data);

        //create with other shit
        uint Create(Func<uint, byte[]> dataGenerator);

        //this deletes a record by its id
        void Delete(uint recordId);
    }
}