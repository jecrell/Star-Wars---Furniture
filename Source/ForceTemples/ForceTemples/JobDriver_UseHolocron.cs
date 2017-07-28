using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Verse;
using Verse.AI;
using Verse.Sound;
using RimWorld;

namespace ForceTemples
{
    class JobDriver_UseHolocron : JobDriver
    {
        private const int ShotDuration = 600;

        [DebuggerHidden]
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
            yield return Toils_Reserve.Reserve(TargetIndex.A, base.CurJob.def.joyMaxParticipants, 0, null);
            Toil chooseCell = new Toil();
            chooseCell.initAction = delegate
            {
                int num = 0;
                while (true)
                {
                    this.CurJob.targetB = this.CurJob.targetA.Thing.RandomAdjacentCell8Way();
                    num++;
                    if (num > 100)
                    {
                        break;
                    }
                    if (this.pawn.CanReserve((IntVec3)this.CurJob.targetB, 1, -1, null, false))
                    {
                        return;
                    }
                }
                Log.Error(this.pawn + " could not find cell adjacent to holocron " + this.TargetThingA);
                this.EndJobWith(JobCondition.Errored);
            };
            yield return chooseCell;
            yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
            yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
            Toil play = new Toil();
            play.initAction = delegate
            {
                this.CurJob.locomotionUrgency = LocomotionUrgency.Walk;
            };
            play.tickAction = delegate
            {
                this.pawn.Drawer.rotator.FaceCell(this.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
                if (this.pawn.jobs.curDriver.ticksLeftThisToil == 300)
                {
                    XDefOf.PJ_HoloSound.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
                }
                if (Find.TickManager.TicksGame > this.startTick + this.CurJob.def.joyDuration)
                {
                    this.EndJobWith(JobCondition.Succeeded);
                    return;
                }
                float statValue = this.TargetThingA.GetStatValue(StatDefOf.EntertainmentStrengthFactor, true);
                float extraJoyGainFactor = statValue;
                JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor);
            };
            play.socialMode = RandomSocialMode.SuperActive;
            play.defaultCompleteMode = ToilCompleteMode.Delay;
            play.defaultDuration = 600;
            play.AddFinishAction(delegate
            {
                TempleTools.TryGainFTempleThought(this.pawn);
            });
            yield return play;
            yield return Toils_Reserve.Release(TargetIndex.B);
            yield return Toils_Jump.Jump(chooseCell);
        }
    }
}
