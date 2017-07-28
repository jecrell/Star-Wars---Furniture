using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using RimWorld;

namespace ForceTemples
{
    class JoyGiver_UseHolocron : JoyGiver_InteractBuilding
    {
        protected override bool CanDoDuringParty
        {
            get
            {
                return true;
            }
        }

        protected override Job TryGivePlayJob(Pawn pawn, Thing t)
        {
            if (!JoyGiver_UseHolocron.ThingHasStandableSpaceOnAllSides(t))
            {
                return null;
            }
            return new Job(this.def.jobDef, t);
        }

        public static bool ThingHasStandableSpaceOnAllSides(Thing t)
        {
            CellRect cellRect = t.OccupiedRect();
            CellRect.CellRectIterator iterator = cellRect.ExpandedBy(1).GetIterator();
            while (!iterator.Done())
            {
                IntVec3 current = iterator.Current;
                if (!cellRect.Contains(current))
                {
                    if (!current.Standable(t.Map))
                    {
                        return false;
                    }
                }
                iterator.MoveNext();
            }
            return true;
        }
    }
}