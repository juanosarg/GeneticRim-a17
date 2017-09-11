
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace GeneticBuildings
{
    public class JobDriver_CarryToAgeAccelerationPod : JobDriver
    {
        private TargetIndex TakeeInd = TargetIndex.A;

        private TargetIndex DropPodInd = TargetIndex.B;

        protected Pawn Takee
        {
            get
            {
                return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
            }
        }

        protected Building_AgeAccelerationPod DropPod
        {
            get
            {
                return (Building_AgeAccelerationPod)base.CurJob.GetTarget(TargetIndex.B).Thing;
            }
        }

        [DebuggerHidden]
        protected override IEnumerable<Toil> MakeNewToils()
        {
            /* Log.Message(Takee.ToString());
             Log.Message(DropPod.ToString());
              Log.Message(DropPodInd.ToString());
              Log.Message(TakeeInd.ToString());

              // this.FailOnDestroyedOrNull(TargetIndex.A);
              //  this.FailOnDestroyedOrNull(TargetIndex.B);
              //  this.FailOnAggroMentalState(TargetIndex.A);
             // yield return Toils_Reserve.Reserve(this.TakeeInd, 1, -1, null);
              yield return Toils_Reserve.Reserve(this.DropPodInd, 1, -1, null);
             yield return Toils_Haul.StartCarryThing(TakeeInd, false, false);
              //eld return Toils_General.Wait(75500);

              // yield return Toils_Goto.GotoThing(this.DropPodInd, PathEndMode.Touch);



              Toil prepare = Toils_General.Wait(500);
               prepare.FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
               prepare.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
               yield return prepare;
               yield return new Toil
               {
                   initAction = delegate
                   {
                       this.DropPod.TryAcceptThing(this.Takee, true);
                   },
                   defaultCompleteMode = ToilCompleteMode.Instant


               };
              Log.Message("Ending");
              yield break;
              */
            Log.Message(Toils_Reserve.Reserve(this.DropPodInd, 1).ToString());
            yield return Toils_Reserve.Reserve(this.DropPodInd, 1);
            Log.Message(Toils_Goto.GotoThing(this.DropPodInd, PathEndMode.Touch).ToString());
            yield return Toils_Goto.GotoThing(this.DropPodInd, PathEndMode.Touch);


            yield break;
        }
    }
}