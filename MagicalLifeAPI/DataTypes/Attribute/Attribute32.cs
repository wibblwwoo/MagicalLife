﻿using MagicalLifeAPI.Entities.Util.Modifier_Remove_Conditions;
using MagicalLifeAPI.Entity.Util;
using ProtoBuf;
using System;
using System.Collections.Generic;

namespace MagicalLifeAPI.Entities.Util
{
    [ProtoContract]
    public class Attribute32
    {
        /// <summary>
        /// The int value is applied to the value of this attribute, while the <see cref="IModifierRemoveCondition"/> is used to determine if the modifier will wear off.
        /// The string value is a display message/reason as to why the modifier was applied.
        /// </summary>
        [ProtoMember(1)]//This doesn't serialize.
        public List<Modifier32> Modifiers { get; private set; } = new List<Modifier32>();

        public Attribute32(Int32 value) : this()
        {
            this.AddModifier(new Modifier32(value, new NeverRemoveCondition(), "Base value"));
        }

        public Attribute32()
        {
        }

        public float GetValue()
        {
            float ret = 0;
            foreach (Modifier32 item in this.Modifiers)
            {
                ret += item.Value;
            }
            return ret;
        }

        /// <summary>
        /// Adds a modifier to the modifiers list.
        /// </summary>
        /// <param name="modifier"></param>
        public void AddModifier(Modifier32 modifier)
        {
            this.Modifiers.Add(modifier);
        }

        public void WearOff()
        {
            lock (this.Modifiers)
            {
                int length = this.Modifiers.Count;
                for (int i = length - 1; i >= 0; i--)
                {
                    if (this.Modifiers[i].RemoveCondition.WearOff())
                    {
                        this.Modifiers.RemoveAt(i);
                    }
                }
            }
        }
    }
}