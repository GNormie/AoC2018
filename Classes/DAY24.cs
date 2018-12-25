using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY24
    {
        public static bool debugCombat = false;

        public static List<UnitGroup> lstCombatants = new List<UnitGroup>();
        public static Util.PriorityQueue<Tuple<UnitGroup, UnitGroup>> combatQueue = new Util.PriorityQueue<Tuple<UnitGroup, UnitGroup>>();


        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(24));
            ParseInput(linesInput);

            int minimumBoostToWin = 0;
            bool keepBoosting = true;

            List<UnitGroup> copyOfCombatants = lstCombatants.ConvertAll(Unit => new UnitGroup(Unit));

            int staleMateLimit = 15;
            while (keepBoosting)
            {
                int staleMateCount = 0;
                lstCombatants = copyOfCombatants.ConvertAll(Unit => new UnitGroup(Unit));
                lstCombatants.Where(r => r.faction == Faction.InmuneSystem).ForEach(r => r.attackPower = r.attackPower + minimumBoostToWin);

                int lastTurnTotal = lstCombatants.Sum(r => r.unitsAmount);

                bool gameFinished = false;
                while (gameFinished == false)
                {
                    TargetSelectionLogic();
                    CombatLogic();
                    gameFinished = EndCondition();
                    if (lastTurnTotal == lstCombatants.Sum(r => r.unitsAmount))
                    {
                        if (staleMateCount == staleMateLimit)
                            break;
                        else
                            staleMateCount++;
                    }
                    else
                        lastTurnTotal = lstCombatants.Sum(r => r.unitsAmount);
                }

                if (minimumBoostToWin == 0)
                    Console.WriteLine("PART 1: " + lstCombatants.Sum(r => r.unitsAmount));

                if (lstCombatants.All(r => r.faction == Faction.InmuneSystem))
                {
                    Console.WriteLine("DEER SURVIVES, Boost: " + minimumBoostToWin);
                    Console.WriteLine("PART 2: " + lstCombatants.Sum(r => r.unitsAmount));
                    keepBoosting = false;
                    break;
                }
                minimumBoostToWin += 1;
            }
        }

        public static void TargetSelectionLogic()
        {
            var PossibleTargets = lstCombatants.OrderByDescending(r => r.EffectivePower).ThenByDescending(r => r.initiative).ToList();
            List<UnitGroup> takenTargets = new List<UnitGroup>();
            foreach (var Soldier in PossibleTargets)
            {
                var possibleTargets = lstCombatants
                    .Except(takenTargets)
                    .Where(r => r.faction == Soldier.enemyFaction)
                    .Where(r => r.unitsAmount > 0)
                    .Where(r => Soldier.damageCalculation(r) > 0)
                    .OrderByDescending(r => Soldier.damageCalculation(r))
                    .ThenByDescending(r => r.EffectivePower)
                    .ThenByDescending(r => r.initiative);
                if (possibleTargets.Count() > 0)
                {
                    var leTarget = possibleTargets.First();
                    combatQueue.Enqueue(new Tuple<UnitGroup, UnitGroup>(Soldier, leTarget), -1 * (Soldier.initiative));
                    takenTargets.Add(leTarget);
                }
            }
        }

        public static void CombatLogic()
        {
            while (combatQueue.Count > 0)
            {
                var combatTurn = combatQueue.Dequeue();
                combatTurn.Item1.Attack(combatTurn.Item2);
                if (debugCombat)
                    Console.ReadLine();
            }
        }

        public static bool EndCondition()
        {
            if (debugCombat)
                Console.Clear();
            lstCombatants.RemoveAll(r => r.unitsAmount <= 0);
            if (lstCombatants.All(r => r.faction == Faction.Infection) || lstCombatants.All(r => r.faction == Faction.InmuneSystem))
                return true;
            else
                return false;
        }

        public enum Faction
        {
            InmuneSystem = 1,
            Infection = 2
        }

        public class UnitGroup
        {
            public int ID;
            public int unitsAmount;
            public int HPperUnit;
            public int attackPower;
            public Element damageType;
            public int initiative;
            public Faction faction;

            public List<Element> Weaknesses;
            public List<Element> Inmunnity;

            public int EffectivePower { get { return unitsAmount * attackPower; } }

            public Faction enemyFaction
            {
                get
                {
                    if (this.faction == Faction.InmuneSystem)
                        return Faction.Infection;
                    else
                        return Faction.InmuneSystem;
                }
            }


            public UnitGroup(int _ID, int _units, int _HP, List<Element> weakn, List<Element> inmune, int _attackPower, int _initiative, Element _damageType, Faction _faction)
            {
                ID = _ID;
                unitsAmount = _units;
                HPperUnit = _HP;
                Weaknesses = weakn;
                Inmunnity = inmune;
                attackPower = _attackPower;
                initiative = _initiative;
                damageType = _damageType;
                faction = _faction;
            }

            public UnitGroup(UnitGroup U)
            {
                ID = U.ID;
                unitsAmount = U.unitsAmount;
                HPperUnit = U.HPperUnit;
                Weaknesses = U.Weaknesses;
                Inmunnity = U.Inmunnity;
                attackPower = U.attackPower;
                initiative = U.initiative;
                damageType = U.damageType;
                faction = U.faction;
            }

            public int damageCalculation(UnitGroup enemyGroup)
            {
                if (enemyGroup.Inmunnity.Count > 0 && enemyGroup.Inmunnity.Contains(this.damageType))
                    return 0;
                else if (enemyGroup.Weaknesses.Count > 0 && enemyGroup.Weaknesses.Contains(this.damageType))
                    return EffectivePower * 2;
                else
                    return EffectivePower;
            }

            public void Attack(UnitGroup enemyGroup)
            {
                if (unitsAmount <= 0)
                {
                    return;
                }
                if (enemyGroup.unitsAmount <= 0)
                    Debug.Assert(false);
                int damage = damageCalculation(enemyGroup);
                int unitRemoval = damage / enemyGroup.HPperUnit;
                enemyGroup.unitsAmount = enemyGroup.unitsAmount - unitRemoval;
                if (debugCombat)
                    Console.WriteLine(this.faction + " group " + ID + " attacks defending group " + enemyGroup.ID + ", killing " + unitRemoval + " units | Remaining Soldiers: " + enemyGroup.unitsAmount);
            }
        }

        public enum Element
        {
            Slashing,
            Bludgeon,
            Fire,
            Cold,
            Radiation
        }

        public static Element wordToElement(string w)
        {
            if (string.IsNullOrWhiteSpace(w))
            {
                Debug.Assert(false);
                return Element.Fire;
            }
            if (w == "fire")
                return Element.Fire;
            if (w == "radiation")
                return Element.Radiation;
            if (w == "cold")
                return Element.Cold;
            if (w == "slashing")
                return Element.Slashing;
            if (w == "bludgeoning")
                return Element.Bludgeon;

            Debug.Assert(false);
            return Element.Fire;
        }

        public static void ParseInput(string[] linesInput)
        {
            //Next time i should copy paste the input instead of wasting a whole hour
            //coming up with universal parsing for these terrible inputs
            #region hugeParsing
            Faction toMake = Faction.InmuneSystem;
            int unitID = 1;
            foreach (string line in linesInput)
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                if (line[0] == 'I')
                {
                    if (line[1] == 'm')
                        toMake = Faction.InmuneSystem;
                    else if (line[1] == 'n')
                        toMake = Faction.Infection;
                    unitID = 1;
                    continue;
                }

                string[] words = line.Split(' ');
                int numberOfSoldiers = Convert.ToInt32(words[0]); //UNIT NUMBER
                int HPperUnit = Convert.ToInt32(words[4]); //HP
                string WnI = line.Between("(", ")");
                string[] leDamage = line.Between("does ", " damage").Split(' ');
                int damage = Convert.ToInt32(leDamage[0]);
                Element damageType = wordToElement(leDamage[1]);
                List<Element> WEAK = new List<Element>();
                List<Element> INMUNE = new List<Element>();
                int initiative = Convert.ToInt32(line.Between("initiative "));
                if (string.IsNullOrWhiteSpace(WnI) == false)
                {
                    if (WnI.Contains(";"))
                    {
                        string[] weakNinmune = WnI.Replace(',', ' ').Split(';');
                        string[] lleftside = weakNinmune[0].Trim().Split(' ');
                        string[] rightside = weakNinmune[1].Trim().Split(' ');
                        List<string[]> lstParse = new List<string[]>();
                        lstParse.Add(lleftside);
                        lstParse.Add(rightside);
                        foreach (var leftside in lstParse)
                        {
                            foreach (var left in leftside)
                            {
                                if (left != " " && left != "to" && left != "immune" && left != "weak" && left != "")
                                {
                                    if (leftside[0] == "weak")
                                    {
                                        WEAK.Add(wordToElement(left));
                                    }
                                    else if (leftside[0] == "immune")
                                    {
                                        INMUNE.Add(wordToElement(left));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string[] weakNinmune = WnI.Replace(',', ' ').Split(' ');
                        foreach (var left in weakNinmune)
                        {
                            if (left != " " && left != "to" && left != "immune" && left != "weak" && left != "")
                            {
                                if (weakNinmune[0] == "weak")
                                {
                                    WEAK.Add(wordToElement(left));
                                }
                                else if (weakNinmune[0] == "immune")
                                {
                                    INMUNE.Add(wordToElement(left));
                                }
                            }
                        }
                    }
                }
                UnitGroup soldier = new UnitGroup(unitID, numberOfSoldiers, HPperUnit, WEAK, INMUNE, damage, initiative, damageType, toMake);
                lstCombatants.Add(soldier);
                unitID++;
            }
            #endregion
        }
    }
}
