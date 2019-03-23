﻿using ProtoBuf;

namespace MagicalLifeAPI.Components.Resource
{
    /// <summary>
    /// Used to hold a <see cref="AbstractHarvestable"/> component.
    /// </summary>
    public interface IHarvestable
    {
        [ProtoMember(1)]
        AbstractHarvestable HarvestingBehavior { get; set; }
    }
}