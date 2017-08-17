using Verse;

namespace NewHatcher
{
    public class CompProperties_Incubator : CompProperties
    {
        public float hatcherDaystoHatch = 1f;

        public PawnKindDef hatcherPawn;

        public CompProperties_Incubator()
        {
            //Messages.Message("Patataaa", MessageSound.Standard);
            this.compClass = typeof(CompIncubator);
        }
    }
}