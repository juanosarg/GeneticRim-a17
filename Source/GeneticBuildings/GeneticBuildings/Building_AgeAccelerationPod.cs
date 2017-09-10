﻿
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using RimWorld;

namespace GeneticBuildings
{
    public class Building_AgeAccelerationPod : Building_CryptosleepCasket
    {
        public int groupID = -1;

        public int TickCounter = 0;
        public int OneHour = 7500;

        public override void Tick()
        {
            
                foreach (Thing current in ((IEnumerable<Thing>)this.innerContainer))
                {
                    Pawn pawn = current as Pawn;
                    if (pawn != null)
                    {
                        TickCounter++;
                        if (TickCounter >= OneHour)
                        {
                            pawn.ageTracker.DebugMake1YearOlder();
                            TickCounter = 0;
                            this.EjectContents();

                    }
                }
                }
                base.Tick();
                this.innerContainer.ThingOwnerTick(true);
                
            
            
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
        }

        public override void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            base.PreApplyDamage(dinfo, out absorbed);
            if (absorbed)
            {
                return;
            }
            if (!this.contentsKnown && this.innerContainer.Count > 0 && dinfo.Def.harmsHealth && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
            {
                bool flag = false;
                foreach (Thing current in ((IEnumerable<Thing>)this.innerContainer))
                {
                    Pawn pawn = current as Pawn;
                    if (pawn != null)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    this.EjectContents();
                }
            }
            absorbed = false;
        }

        public override void EjectContents()
        {
            List<Thing> list = new List<Thing>();
            if (!this.contentsKnown)
            {
                list.AddRange(this.innerContainer);
                list.AddRange(this.UnopenedCasketsInGroup().SelectMany((Building_AgeAccelerationPod c) => c.innerContainer));
            }
            bool contentsKnown = this.contentsKnown;
            base.EjectContents();
            if (!contentsKnown)
            {
                ThingDef filthSlime = ThingDefOf.FilthSlime;
                FilthMaker.MakeFilth(base.Position, base.Map, filthSlime, Rand.Range(8, 12));
                this.SetFaction(null, null);
                foreach (Building_AgeAccelerationPod current in this.UnopenedCasketsInGroup())
                {
                    current.EjectContents();
                }
                List<Pawn> source = (from t in list
                                     where t is Pawn
                                     select t).Cast<Pawn>().ToList<Pawn>();
                IEnumerable<Pawn> enumerable = from p in source
                                               where p.RaceProps.Humanlike && p.GetLord() == null && p.Faction.def == FactionDefOf.SpacerHostile
                                               select p;
                if (enumerable.Any<Pawn>())
                {
                    Faction faction = Find.FactionManager.FirstFactionOfDef(FactionDefOf.SpacerHostile);
                    LordMaker.MakeNewLord(faction, new LordJob_AssaultColony(faction, false, false, false, false, false), base.Map, enumerable);
                }
            }
        }

        public override string GetInspectString()
        {
            string text = "";
            string str;
            if (!this.contentsKnown)
            {
                str = "UnknownLower".Translate();
            }
            else
            {
                str = this.innerContainer.ContentsString;
            }
            if (!text.NullOrEmpty())
            {
                text += "\n";
            }
            return text + "CasketContains".Translate() + ": " + str + "\n" + "AccAgeProg".Translate() + ": " + ((float)TickCounter / (float)OneHour).ToStringPercent();
        }



        [DebuggerHidden]
        private IEnumerable<Building_AgeAccelerationPod> UnopenedCasketsInGroup()
        {
            yield return this;
            if (this.groupID != -1)
            {
                foreach (Thing t in base.Map.listerThings.ThingsOfDef(ThingDefOf.AncientCryptosleepCasket))
                {
                    Building_AgeAccelerationPod casket = t as Building_AgeAccelerationPod;
                    if (casket.groupID == this.groupID && !casket.contentsKnown)
                    {
                        yield return casket;
                    }
                }
            }
        }
    }
}



