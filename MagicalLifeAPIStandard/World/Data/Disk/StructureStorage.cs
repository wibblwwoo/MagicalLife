﻿using MagicalLifeAPI.Networking.Serialization;
using MagicalLifeAPI.World.Base;
using MagicalLifeAPI.World.Data.Disk.DataStorage;
using System;
using System.IO;

namespace MagicalLifeAPI.World.Data.Disk
{
    /// <summary>
    /// Used to save and read structures from the disk.
    /// </summary>
    public class StructureStorage
    {
        public StructureStorage(string saveName)
        {
        }

        /// <summary>
        /// Saves a structure to disk.
        /// </summary>
        /// <param name="structure">The structure to save.</param>
        /// <param name="dimensionID">The ID of the dimension the structure belongs to.</param>
        internal void SaveStructure(Structure structure, Guid dimensionID, AbstractWorldSink sink)
        {
            bool dimensionExists = WorldStorage.DimensionPaths.TryGetValue(dimensionID, out string path);

            if (!dimensionExists)
            {
                throw new DirectoryNotFoundException("Dimension save folder does not exist!");
            }

            sink.Receive(structure, path + Path.DirectorySeparatorChar + structure.StructureID + ".struct", dimensionID);
        }

        /// <summary>
        /// Loads a structure from disk.
        /// </summary>
        /// <param name="dimensionID">The ID of the dimension that the structure belongs to.</param>
        /// <returns></returns>
        internal Structure LoadStructure(Guid structureID, Guid dimensionID)
        {
            bool dimensionExists = WorldStorage.DimensionPaths.TryGetValue(dimensionID, out string path);

            if (!dimensionExists)
            {
                throw new DirectoryNotFoundException("Dimension save folder does not exist!");
            }

            using (StreamReader sr = new StreamReader(path + Path.DirectorySeparatorChar + structureID + ".struct"))
            {
                return (Structure)ProtoUtil.TypeModel.DeserializeWithLengthPrefix(sr.BaseStream, null, typeof(Structure), ProtoBuf.PrefixStyle.Base128, 0);
            }
        }
    }
}
