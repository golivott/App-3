/**
 * In the depths of this code's abyss,
 * Where logic falters, and clarity, amiss.
 * Brave soul who ventures, heed this plea,
 * A labyrinth of chaos, as you can see.
 * 
 * Loops tangled, like vines in a thicket,
 * Variables lost, their names a cryptic ticket.
 * Nesting so deep, like a maze untold,
 * In this realm of confusion, one's patience may fold.
 * 
 * Functions intertwined, like a dance of confusion,
 * A symphony of chaos, a programmer's illusion.
 * Comments sparse, like stars in the night,
 * Navigate with care, through this digital plight.
 * 
 * Gaze upon this code, a beast of despair,
 * Complexity reigning, a tangled affair.
 * To the next brave coder, a warning to share,
 * May your sanity persist, as you venture in there.
 */

using System;
using UnityEngine;

public enum CardType
{
    DungeonRage,
    Sneak,
    Stability,
    GoldHunter,
    GemSeeker,
    Evasion,
    TreadLightly,
    GemFocus,
    LootScoot,
    SecondWind,
    GuardianAngel,
    BoundingStrides,
    RecklessCharge,
    Sprint,
    NimbleLooting,
    SmashGrab,
    Quickstep,
    SuitUp,
    AdrenalineRush,
    DungeonRepairs,
    EerieSilence,
    Swagger,
    SneakStep,
    SpeedRunner,
    EyesOnThePrize,
    Haste,
    GemRain,
    SilentRunner,
    FuzzyBunnySlippers,
    DeepDiver,
    Brilliance,
}

public enum CardRarity
{
    Bad,
    Normal,
    Good,
}

[CreateAssetMenu(menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public int maxInDeck;
    public float gemCost;
    public float goldCost;
    public CardRarity cardRarity;
    public CardType cardType;

    public void Play()
    {
        switch (cardType)
        {
            case CardType.DungeonRage: DungeonRage(); break;
            case CardType.Sneak: Sneak(); break;
            case CardType.Stability: Stability(); break;
            case CardType.GoldHunter: GoldHunter(); break;
            case CardType.GemSeeker: GemSeeker(); break;
            case CardType.Evasion: Evasion(); break;
            case CardType.TreadLightly: TreadLightly(); break;
            case CardType.GemFocus: GemFocus(); break;
            case CardType.LootScoot: LootScoot(); break;
            case CardType.SecondWind: SecondWind(); break;
            case CardType.GuardianAngel: GuardianAngel(); break;
            case CardType.BoundingStrides: BoundingStrides(); break;
            case CardType.RecklessCharge: RecklessCharge(); break;
            case CardType.Sprint: Sprint(); break;
            case CardType.NimbleLooting: NimbleLooting(); break;
            case CardType.SmashGrab: SmashGrab(); break;
            case CardType.Quickstep: Quickstep(); break;
            case CardType.SuitUp: SuitUp(); break;
            case CardType.AdrenalineRush: AdrenalineRush(); break;
            case CardType.DungeonRepairs: DungeonRepairs(); break;
            case CardType.EerieSilence: EerieSilence(); break;
            case CardType.Swagger: Swagger(); break;
            case CardType.SneakStep: SneakStep(); break;
            case CardType.SpeedRunner: SpeedRunner(); break;
            case CardType.EyesOnThePrize: EyesOnThePrize(); break;
            case CardType.Haste: Haste(); break;
            case CardType.GemRain: GemRain(); break;
            case CardType.SilentRunner: SilentRunner(); break;
            case CardType.FuzzyBunnySlippers: FuzzyBunnySlippers(); break;
            case CardType.DeepDiver: DeepDiver(); break;
            case CardType.Brilliance: Brilliance(); break;
        }
    }

    private void DungeonRage()
    {
        DungeonManager.Instance.AddAnger(2);
    }
    private void Sneak()
    {
        DungeonManager.Instance.AddAngerBlock(2);
        if (DungeonManager.Instance.sneakStepGems > 0)
        {
            DungeonManager.Instance.AddGems(DungeonManager.Instance.sneakStepGems);
        }
    }

    private void Stability()
    {
        DungeonManager.Instance.AddHazardBlock(2);
    }

    private void GoldHunter()
    {
        DungeonManager.Instance.AddGold(4);
    }

    private void GemSeeker()
    {
        DungeonManager.Instance.AddGems(2);
    }

    private void Evasion()
    {
        DungeonManager.Instance.AddAngerBlock(4);
    }

    private void TreadLightly()
    {
        DungeonManager.Instance.AddHazardBlock(4);
    }

    private void GemFocus()
    {
        DungeonManager.Instance.AddGems(4);
    }

    private void LootScoot()
    {
        DungeonManager.Instance.AddGold(7);
        DungeonManager.Instance.LootScootBuff(); // ms buff for 15 sec
    }

    private void SecondWind()
    {
        DungeonManager.Instance.SecondWindBuff();
        // health regen buff for 15 sec
        // ms buff for 15 sec
    }

    private void GuardianAngel()
    {
        DungeonManager.Instance.AddAnger(1);
        DungeonManager.Instance.GuardianAngelBuff();
        // Disable guardians for 15 sec
    }

    private void BoundingStrides()
    {
        DungeonManager.Instance.AddHazardBlock(2);
        // jump buff for 2 min
        DungeonManager.Instance.BoundingStridesBuff();
    }

    private void RecklessCharge()
    {
        for (int i = 0; i < 2; i++)
        {
            DungeonManager.Instance.TriggerHazards();
        }
        DungeonManager.Instance.RecklessChargeBuff();
        // 10 sec to trigger a noise event
        // if it triggers spawn 8 gems in the dungeon
    }

    private void Sprint()
    {
        DungeonManager.Instance.SprintBuff();
        // buff ms for 60 sec
    }

    private void NimbleLooting()
    {
        DungeonManager.Instance.AddAngerBlock(1);
        DungeonManager.Instance.AddGold(2);
        DungeonManager.Instance.NimbleLootingBuff();
        // for each anger blocked spawn 2 gold
    }

    private void SmashGrab()
    {
        DungeonManager.Instance.AddGold(13);
        DungeonManager.Instance.AddAnger(2);
    }

    private void Quickstep()
    {
        DungeonManager.Instance.AddAngerBlock(2);
        DungeonManager.Instance.QuickStepBuff();
        // buff ms for 15 sec
        DeckManager.Instance.DrawCard();
    }

    private void SuitUp()
    {
        Player.Instance.damageMultiplier = 0.7f;
        DungeonManager.Instance.doubleAngerChance = 0.25f;
    }

    private void AdrenalineRush()
    {
        DungeonManager.Instance.TriggerHazards();
        DungeonManager.Instance.AddGold(DungeonManager.Instance.currAnger);
    }

    private void EerieSilence()
    {
        DungeonManager.Instance.AddAngerBlock(8);
        DungeonManager.Instance.AddHazardBlock(2);
        DeckManager.Instance.BurnCard();
    }
    
    private void DungeonRepairs()
    {
        DungeonManager.Instance.AddAngerBlock(7);
        DungeonManager.Instance.AddAnger(1);
    }

    private void Swagger()
    {
        DungeonManager.Instance.AddGold(10);
        DungeonManager.Instance.AddGems(10);
        DeckManager.Instance.AddDungeonRageCard(2);
        DeckManager.Instance.ShuffleDeck();
    }

    private void SneakStep()
    {
        // Any future sneak cards played will spawn 2 gems.
        // This effect may stack up to 6 gems.
        DungeonManager.Instance.SneakStepBuff();
    }

    private void SpeedRunner()
    {
        // Spawns 8 gems at the entrance to level 3 in the dungeon,
        // these gems will disappear after 5 min.
        DungeonManager.Instance.SpeedRunnerBuff();
    }

    private void EyesOnThePrize()
    {
        DungeonManager.Instance.AddAnger(2);
        for (int i = 0; i < 3; i++)
        {
            DungeonManager.Instance.TriggerHazards();
        }

        // an extra card will be available in the gem shop
        DungeonManager.Instance.eyesOnThePrizeBuff += DungeonManager.Instance.eyesOnThePrizeBuff < 3 ? 1 : 0;
    }

    private void Haste()
    {
        DeckManager.Instance.ChangeInterval(DungeonManager.Instance.cardDrawInterval * 0.9f);
    }

    private void GemRain()
    {
        for (int i = 0; i < 3; i++)
        {
            DungeonManager.Instance.TriggerHazards();
        }
        DungeonManager.Instance.GemRainBuff();
        // for the next 3 card draws gem spawns are doubled
    }

    private void SilentRunner()
    {
        DungeonManager.Instance.SilentRunnerBuff();
        // Every 15 seconds player move speed is buffed, 
        // 50% chance to block 1 anger
    }

    private void FuzzyBunnySlippers()
    {
        DungeonManager.Instance.FuzzyBunnySlippersBuff();
        // Every 6 minutes block 4 anger but player move speed cannot 
        // be buffed if they’ve collected the artifact
    }

    private void DeepDiver()
    {
        DungeonManager.Instance.DeepDiverBuff();
        // Spawns 6 gems at the entrance to each level in the dungeon
    }

    private void Brilliance()
    {
        DeckManager.Instance.DrawCard();
        DeckManager.Instance.DrawCard();
    }
}
