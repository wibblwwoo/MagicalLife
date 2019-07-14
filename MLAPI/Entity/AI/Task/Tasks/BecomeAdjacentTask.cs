﻿using MagicalLifeAPI.DataTypes;
using MagicalLifeAPI.Entity.AI.Task.Qualifications;
using MagicalLifeAPI.GUI;
using MagicalLifeAPI.Pathfinding;
using MagicalLifeAPI.World;
using ProtoBuf;
using System;
using System.Collections.Generic;

namespace MagicalLifeAPI.Entity.AI.Task.Tasks
{
    [ProtoContract]
    public class BecomeAdjacentTask : MagicalTask
    {
        [ProtoMember(1)]
        public Point3D Target { get; private set; }

        public BecomeAdjacentTask(Guid boundID, Point3D target) : base(Dependencies.CreateEmpty(), boundID, new List<Qualification> { new CanMoveQualification() }, PriorityLayers.Default)
        {
            this.Target = target;
        }

        public override void MakePreparations(Living living)
        {
        }

        public override void Reset()
        {
            //We don't need to do anything to reset this.
        }

        public override void Tick(Living l)
        {
            this.CompleteTask();
        }

        public override bool CreateDependencies(Living l)
        {
            List<Point3D> result = WorldUtil.GetNeighboringTiles(this.Target);
            result.RemoveAll(x => !World.Data.World.GetTile(l.DimensionID, x.X, x.Y).IsWalkable);

            ComponentSelectable entitySelected = l.GetExactComponent<ComponentSelectable>();
            Point3D adjacentLocation = PathUtil.GetFirstReachable(result, entitySelected.MapLocation);

            if (adjacentLocation == null)
            {
                return false;
            }
            else
            {
                MoveTask task = new MoveTask(this.BoundID, adjacentLocation);
                this.Dependencies.PreRequisite.Add(task);
                return true;
            }
        }
    }
}