using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace ForceTemples
{
    class TempleTools
    {
        public static void TryGainFTempleThought(Pawn pawn)
        {
            Room room = pawn.GetRoom(RegionType.Set_Passable);
            if (room != null)
            {
                int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
                if (ThoughtDefOf.AteInImpressiveDiningRoom.stages[scoreStageIndex] != null)
                {
                    pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(XDefOf.JoyActivityInImpressiveFTemple, scoreStageIndex), null);
                }
            }
        }

    }
}
