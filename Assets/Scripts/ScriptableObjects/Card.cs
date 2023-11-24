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

using UnityEngine;

public enum CardType
{
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

[CreateAssetMenu(menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public CardType cardType;

    public void Play()
    {
        switch (cardType)
        {
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

    private void Sneak() {}
    private void Stability() {}
    private void GoldHunter() {}
    private void GemSeeker() {}
    private void Evasion() {}
    private void TreadLightly() {}
    private void GemFocus() {}
    private void LootScoot() {}
    private void SecondWind() {}
    private void GuardianAngel() {}
    private void BoundingStrides() {}
    private void RecklessCharge() {}
    private void Sprint() {}
    private void NimbleLooting() {}
    private void SmashGrab() {}
    private void Quickstep() {}
    private void SuitUp() {}
    private void AdrenalineRush() {}
    private void EerieSilence() {}
    private void Swagger() {}
    private void SneakStep() {}
    private void SpeedRunner() {}
    private void EyesOnThePrize() {}
    private void Haste() {}
    private void GemRain() {}
    private void SilentRunner() {}
    private void FuzzyBunnySlippers() {}
    private void DeepDiver() {}
    private void Brilliance() {}

    // Add more methods here for other card actions
}
