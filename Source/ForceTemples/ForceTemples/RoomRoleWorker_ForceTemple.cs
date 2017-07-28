using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace ForceTemples
{
    public class RoomRoleWorker_ForceTemple : RoomRoleWorker
    {
        public override float GetScore(Room room)
        {
            int num = 0;
            List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
            for (int i = 0; i < containedAndAdjacentThings.Count; i++)
            {
                Thing thing = containedAndAdjacentThings[i];
                if (thing is Building_Holocron)
                {
                    num++;
                }
            }
            return 30f * (float)num;
        }
    }
}