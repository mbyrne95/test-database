using System;

namespace ChampionCore
{
    public interface IBlockStorage
    {

        //bytes of data per block
        int BlockContentSize
        {
            get;
        }

        //bytes in header
        int BlockHeaderSize
        {
            get;
        }

        //total block size
        int BlockSize
        {
            get;
        }

        //find block by id
        IBlock Find(uint blockId);

        IBlock CreateNew();

    }

    public interface IBlock : IDisposable
    {
        //id of block
        uint Id
        {
            get;
        }

        //block can contain header metadata. each identified by number and 8 bytes values
        long GetHeader(int field);

        //change value of specified header
        void SetHeader(int field, long value);

        //read content of the block (src) into given buffer (dst)
        void Read(byte[] dst, int dstOffset, int srcOffet, int count);

        //write content of given buffer (src) into this (dst)
        void Write(byte[] src, int srcOffset, int dstOffset, int count);
    }
}