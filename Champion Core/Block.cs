using System;
using System.Diagnostics;
using System.IO;

namespace ChampionCore
{
    public class Block : IBlock
    {
        readonly byte[] firstSector;
        readonly long?[] cachedHeaderValue = new long?[5];
        readonly Stream stream;
        readonly BlockStorage storage;
        readonly uint id;

        bool isFirstSectorDirty = false;
        bool isDisposed = false;

        public event EventHandler Disposed;

        public uint Id { get { return id; } }

        public Block (BlockStorage storage, uint id, byte[] firstSector, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("null stream");
            } 
            if (firstSector == null)
            {
                throw new ArgumentNullException("null first sector");
            }
            if (firstSector.Length != storage.DiskSectorSize)
            {
                throw new ArgumentNullException("first sector length must be" + storage.DiskSectorSize);
            }
            this.storage = storage;
            this.id = id;
            this.stream = stream;
            this.firstSector = firstSector;
        }

        public long GetHeader (int field)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("block");
            }
            if (field < 0)
            {
                throw new IndexOutOfRangeException();
            }
            if (field >= (storage.BlockHeaderSize/8))
            {
                throw new ArgumentException("invalid field " + field);
            }

            if (field < cachedHeaderValue.Length)
            {
                if (cachedHeaderValue[field] == null)
                {
                    cachedHeaderValue[field] = BufferHelper.ReadBufferInt64(firstSector, field * 8);
                }
                return (long)cachedHeaderValue[field];
            }
            else 
            {
                return BufferHelper.ReadBufferInt64(firstSector, field * 8);
            }
        }

        public void SetHeader (int field, long value)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("Block");
            }
            if (field < 0) 
            {
                throw new IndexOutOfRangeException();
            }

            //update cache if field is cached
            if(field < cachedHeaderValue.Length)
            {
                cachedHeaderValue[field] = value;
            }

            //write in cached buffer
            BufferHelper.WriteBuffer((long)value, firstSector, field * 8);
            isFirstSectorDirty = true;
                 
        }

        public void Read(byte[] dest, int destOffset, int  srcOffset, int count)
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException("block");
            }

            //validate argument
            if (false == ((count >= 0) && ((count + srcOffset) <= storage.BlockContentSize)))
            {
                throw new ArgumentOutOfRangeException("requested count is outside of src bounds");
            }

            if (false == ((count + destOffset) <= dest.Length))
            {
                throw new ArgumentOutOfRangeException("requested count is outside of dest bounds");
            }

            var dataCopied = 0;
            var copyFromFirstSector = (storage.BlockHeaderSize + srcOffset) < storage.DiskSectorSize;

            if (copyFromFirstSector)
            {
                var toBeCopied = Math.Min(storage.DiskSectorSize - storage.BlockHeaderSize - srcOffset, count);

                Buffer.BlockCopy(src: firstSector, srcOffset: storage.BlockHeaderSize + srcOffset, dst: dest, dstOffset: destOffset, count: toBeCopied);

                dataCopied += toBeCopied;
            }

            if (dataCopied < count)
            {
                if (copyFromFirstSector)
                {
                    stream.Position = (Id * storage.BlockSize) + storage.DiskSectorSize;
                } 
                else
                {
                    stream.Position = (Id * storage.BlockSize) + storage.BlockHeaderSize + srcOffset;
                }
            }

            while (dataCopied < count)
            {
                var bytesToRead = Math.Min(storage.DiskSectorSize, count - dataCopied);
                var thisRead = stream.Read(dest, destOffset + dataCopied, bytesToRead);
                if (thisRead == 0)
                {
                    throw new EndOfStreamException();
                }
                dataCopied += thisRead;
            }
        }

        public void Write(byte[] src, int srcOffset, int dstOffset, int count)
        {

        }
    }
}