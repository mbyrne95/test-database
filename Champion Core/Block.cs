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

        }
    }
}