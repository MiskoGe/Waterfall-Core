﻿using Cosmos.HAL.BlockDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waterfall.System.WaterfallVFS
{
    public class WPartition : BlockDevice
    {
        /// <summary>
        /// Hosting device.
        /// </summary>
        public readonly BlockDevice Host;
        /// <summary>
        /// Starting sector.
        /// </summary>
        public readonly ulong StartingSector;
        public static List<WPartition> Partitions = new();
        public override BlockDeviceType Type => Host.Type;
        /// <summary>
        /// Create new instance of the <see cref="Partition"/> class.
        /// </summary>
        /// <param name="aHost">A hosting device.</param>
        /// <param name="aStartingSector">A starting sector.</param>
        /// <param name="aSectorCount">A sector count.</param>
        public WPartition(BlockDevice aHost, ulong StartingSector, ulong SectorCount)
        {
            Host = aHost;
            this.StartingSector = StartingSector;
            mBlockCount = SectorCount;
            mBlockSize = Host.BlockSize;
        }

        /// <summary>
        /// Read block from partition.
        /// </summary>
        /// <param name="aBlockNo">A block to read from.</param>
        /// <param name="aBlockCount">A number of blocks in the partition.</param>
        /// <param name="aData">A data that been read.</param>
        /// <exception cref="OverflowException">Thrown when data lenght is greater then Int32.MaxValue.</exception>
        /// <exception cref="Exception">Thrown when data size invalid.</exception>
        public override void ReadBlock(ulong aBlockNo, ulong aBlockCount, ref byte[] aData)
        {
            CheckDataSize(aData, aBlockCount);
            Host.ReadBlock(StartingSector + aBlockNo, aBlockCount, ref aData);
        }

        /// <summary>
        /// Write block to partition.
        /// </summary>
        /// <param name="aBlockNo">A block number to write to.</param>
        /// <param name="aBlockCount">A number of blocks in the partition.</param>
        /// <param name="aData">A data to write.</param>
        /// <exception cref="OverflowException">Thrown when data lenght is greater then Int32.MaxValue.</exception>
        /// <exception cref="Exception">Thrown when data size invalid.</exception>
        public override void WriteBlock(ulong aBlockNo, ulong aBlockCount, ref byte[] aData)
        {
            CheckDataSize(aData, aBlockCount);
            Host.WriteBlock(StartingSector + aBlockNo, aBlockCount, ref aData);
        }

        /// <summary>
        /// To string.
        /// </summary>
        /// <returns>string value.</returns>
        public override string ToString()
        {
            return "Partition";
        }
    }
}
