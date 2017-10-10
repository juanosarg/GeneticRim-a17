
using Verse;
using System;




namespace NewHatcher
{
    class CompRecombinator : ThingComp
    {

        private Random rand = new Random();


        public CompProperties_Recombinator Props
        {
            get
            {
                return (CompProperties_Recombinator)this.props;
            }
        }



        public override void CompTick()
        {

            // Log.Warning(this.parent.ParentHolder.ToString());
            if (!(this.parent.ParentHolder is Pawn_CarryTracker))
            {
                this.Hatch();

            }

        }


        public void Hatch()
        {

            int randomNumber = rand.Next(1, 11);
            //giLog.Warning(randomNumber.ToString());


            if (randomNumber == 10)
            {
                if (ResearchProjectDef.Named("AdvancedGeneticEngineering").IsFinished) {
                    GenSpawn.Spawn(ThingDef.Named("ThrumboGenetic"), this.parent.Position, this.parent.Map);
                }
                else
                {
                    RecombinateAgain();
                }                
            }
            else if(randomNumber == 9)
            {
                if (ResearchProjectDef.Named("HumanoidGeneticEngineering").IsFinished)
                {
                    GenSpawn.Spawn(ThingDef.Named("HumanoidGenetic"), this.parent.Position, this.parent.Map);
                }
                else
                {
                    RecombinateAgain();
                }
            }
            else if(randomNumber == 8)
            {
                if (ResearchProjectDef.Named("ReptilianGenome").IsFinished)
                {
                    GenSpawn.Spawn(ThingDef.Named("ReptilianGenetic"), this.parent.Position, this.parent.Map);
                }
                else
                {
                    RecombinateAgain();
                }
            }
            else if(randomNumber == 7)
            {
                if (ResearchProjectDef.Named("InsectoidGenome").IsFinished)
                {
                    GenSpawn.Spawn(ThingDef.Named("InsectoidGenetic"), this.parent.Position, this.parent.Map);
                }
                else
                {
                    RecombinateAgain();
                }
            }
            else if(randomNumber==6)
            {
                GenSpawn.Spawn(ThingDef.Named("BearGenetic"), this.parent.Position, this.parent.Map);
            }
            else if(randomNumber == 5)
            {
                GenSpawn.Spawn(ThingDef.Named("ChickenGenetic"), this.parent.Position, this.parent.Map);
            }
            else if (randomNumber == 4)
            {
                GenSpawn.Spawn(ThingDef.Named("BoomalopeGenetic"), this.parent.Position, this.parent.Map);
            }
            else if (randomNumber == 3)
            {
                GenSpawn.Spawn(ThingDef.Named("MuffaloGenetic"), this.parent.Position, this.parent.Map);
            }
            else if (randomNumber == 2)
            {
                GenSpawn.Spawn(ThingDef.Named("WolfGenetic"), this.parent.Position, this.parent.Map);
            }
            else 
            {
                GenSpawn.Spawn(ThingDef.Named("GR_RuinedGenetic"), this.parent.Position, this.parent.Map);
            }

            this.parent.Destroy(DestroyMode.Vanish);
        }

        private void RecombinateAgain()
        {
           int randomNumber = rand.Next(1,7);

           if (randomNumber == 6)
            {
                GenSpawn.Spawn(ThingDef.Named("BearGenetic"), this.parent.Position, this.parent.Map);
            }
            else if (randomNumber == 5)
            {
                GenSpawn.Spawn(ThingDef.Named("ChickenGenetic"), this.parent.Position, this.parent.Map);
            }
            else if (randomNumber == 4)
            {
                GenSpawn.Spawn(ThingDef.Named("BoomalopeGenetic"), this.parent.Position, this.parent.Map);
            }
            else if (randomNumber == 3)
            {
                GenSpawn.Spawn(ThingDef.Named("MuffaloGenetic"), this.parent.Position, this.parent.Map);
            }
            else if (randomNumber == 2)
            {
                GenSpawn.Spawn(ThingDef.Named("WolfGenetic"), this.parent.Position, this.parent.Map);
            }
            else
            {
                GenSpawn.Spawn(ThingDef.Named("GR_RuinedGenetic"), this.parent.Position, this.parent.Map);
            }

        }



    }
}
