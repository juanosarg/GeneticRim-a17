using System.Collections.Generic;
using Verse;
using Verse.AI;


namespace GeneticBuildings
{
    internal class Recipe_CarryToAgePod : RecipeWorker
    {
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients)
        {
            
            if (Building_AgeAccelerationPod.FindAgeAccelerationPodFor(pawn, billDoer, true) != null)
            {
                //Messages.Message("Found valid one", MessageSound.Standard);
                    JobDef jDef = LocalJobDefOf.CarryToAgeAccelerationPod;
               
                    Building_AgeAccelerationPod building_AgeAccelerationPod = Building_AgeAccelerationPod.FindAgeAccelerationPodFor(pawn, billDoer, false);
                    if (building_AgeAccelerationPod == null)
                    {
                        building_AgeAccelerationPod = Building_AgeAccelerationPod.FindAgeAccelerationPodFor(pawn, billDoer, true);
                    }
                //Log.Message(billDoer.ToString());
               // Log.Message(pawn.ToString());
                Job job = new Job(jDef, pawn, building_AgeAccelerationPod);
                    job.count = 1;
                    billDoer.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                

                //Messages.Message("Ordering the job", MessageSound.Standard);

            }
            else {
               

                Messages.Message("CannotCarryToAccelerationPod".Translate() + ": " + "NoAccelerationPod".Translate(), pawn, MessageSound.RejectInput);
            }
        }
       
			
    }
}
