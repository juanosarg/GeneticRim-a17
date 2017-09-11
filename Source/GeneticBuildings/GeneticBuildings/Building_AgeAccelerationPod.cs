
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using RimWorld;
using Verse.Sound;
using UnityEngine;


namespace GeneticBuildings
{
    public class Building_AgeAccelerationPod : Building_Casket
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

        public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            if (base.TryAcceptThing(thing, allowSpecialEffects))
            {
                if (allowSpecialEffects)
                {
                    SoundDef.Named("CryptosleepCasketAccept").PlayOneShot(new TargetInfo(base.Position, base.Map, false));
                }
                return true;
            }
            return false;
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
        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
        {
            foreach (FloatMenuOption o in base.GetFloatMenuOptions(myPawn))
            {
                yield return o;
            }
            if (this.innerContainer.Count == 0)
            {
                if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, TraverseMode.ByPawn))
                {
                    FloatMenuOption failer = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
                    yield return failer;
                }
                else
                {
                    JobDef jobDef = JobDefOf.EnterCryptosleepCasket;
                    string jobStr = "EnterCryptosleepCasket".Translate();
                    Action jobAction = delegate
                    {
                        Job job = new Job(jobDef, this);
                       myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    };
                    yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(jobStr, jobAction, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, this, "ReservedBy");
                }
            }
        }

        [DebuggerHidden]
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }
            if (base.Faction == Faction.OfPlayer && this.innerContainer.Count > 0 && this.def.building.isPlayerEjectable)
            {
                Command_Action eject = new Command_Action();
                eject.action = new Action(this.EjectContents);
                eject.defaultLabel = "CommandPodEject".Translate();
                eject.defaultDesc = "CommandPodEjectDesc".Translate();
                if (this.innerContainer.Count == 0)
                {
                    eject.Disable("CommandPodEjectFailEmpty".Translate());
                }
                eject.hotKey = KeyBindingDefOf.Misc1;
                eject.icon = ContentFinder<Texture2D>.Get("UI/Commands/PodEject", true);
                yield return eject;
            }
        }



        [DebuggerHidden]
        private IEnumerable<Building_AgeAccelerationPod> UnopenedCasketsInGroup()
        {
            yield return this;
            if (this.groupID != -1)
            {
                foreach (Thing t in base.Map.listerThings.ThingsOfDef(ThingDef.Named("AncientCryptosleepCasket")))
                {
                    Building_AgeAccelerationPod casket = t as Building_AgeAccelerationPod;
                    if (casket.groupID == this.groupID && !casket.contentsKnown)
                    {
                        yield return casket;
                    }
                }
            }
        }


        public static Building_AgeAccelerationPod FindAgeAccelerationPodFor(Pawn p, Pawn traveler, bool ignoreOtherReservations = false)
        {
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where typeof(Building_AgeAccelerationPod).IsAssignableFrom(def.thingClass)
                                               select def;
            foreach (ThingDef current in enumerable)
            {
                Predicate<Thing> validator = delegate (Thing x)
                {
                    bool arg_2F_0;
                    if (!((Building_AgeAccelerationPod)x).HasAnyContents)
                    {
                        bool ignoreOtherReservations2 = ignoreOtherReservations;
                        arg_2F_0 = traveler.CanReserve(x, 1, -1, null, ignoreOtherReservations2);
                    }
                    else
                    {
                        arg_2F_0 = false;
                    }
                    return arg_2F_0;
                };
                Building_AgeAccelerationPod building_AgeAccelerationPod = (Building_AgeAccelerationPod)GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(current), PathEndMode.InteractionCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
                if (building_AgeAccelerationPod != null)
                {
                    return building_AgeAccelerationPod;
                }
            }
            return null;
        }



    }
}



