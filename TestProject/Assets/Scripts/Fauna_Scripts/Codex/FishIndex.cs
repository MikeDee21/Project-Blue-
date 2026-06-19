using System.Collections.Generic;
using UnityEngine;
public static class FishIndex
{
    public enum FishZone
    {
        Sunlight,
        Twilight,
        Midnight,
        Abyss,
        Hadal,
        Blood,
        Void
    }
    public enum FishType
    {
        // sunlight zone
        ReefShark,
        Barracuda,
        MantaRay,
        Jellyfish,
        Clownfish,
        Rabbitfish,
        Sardine,
        // twilight zone
        VampireSquid,
        PelicanEel,
        Dragonfish,
        Hatchetfish,
        SpiderShark,
        // midnight zone
        Anglerfish,
        GoblinShark,
        GiantSquid,
        Fincrab,
        FrilledShark,
        // abyss zone
        TripodFish,
        SkeletalFish,
        GristyShark,
        MutatedIsopod,
        MonsterEel,
        // hadal zone
        MarianaSnailfish,
        BloodSnapper,
        Nightwing,
        AncientPiranha,
        // blood zone
        BloodBurden,
        BloodTorment,
        // void zone
        GrotesqueKingfish,
        Unknown
    }
    // fish database entry
    public class FishEntry
    {
        public FishType type;
        public FishZone zone;
        public FishEntry(FishType type, FishZone zone)
        {
            this.type = type;
            this.zone = zone;
        }
    }
    // index
    public static readonly List<FishEntry> Fishlist = new List<FishEntry>
    {
        // sunlight
        new FishEntry(FishType.ReefShark, FishZone.Sunlight),
        new FishEntry(FishType.Barracuda, FishZone.Sunlight),
        new FishEntry(FishType.MantaRay, FishZone.Sunlight),
        new FishEntry(FishType.Jellyfish, FishZone.Sunlight),
        new FishEntry(FishType.Clownfish, FishZone.Sunlight),
        new FishEntry(FishType.Rabbitfish, FishZone.Sunlight),
        new FishEntry(FishType.Sardine, FishZone.Sunlight),
        // twilight
        new FishEntry(FishType.VampireSquid, FishZone.Twilight),
        new FishEntry(FishType.PelicanEel, FishZone.Twilight),
        new FishEntry(FishType.Dragonfish, FishZone.Twilight),
        new FishEntry(FishType.Hatchetfish, FishZone.Twilight),
        new FishEntry(FishType.SpiderShark, FishZone.Twilight),
        // midnight
        new FishEntry(FishType.Anglerfish, FishZone.Midnight),
        new FishEntry(FishType.GoblinShark, FishZone.Midnight),
        new FishEntry(FishType.GiantSquid, FishZone.Midnight),
        new FishEntry(FishType.Fincrab, FishZone.Midnight),
        new FishEntry(FishType.FrilledShark, FishZone.Midnight),
        // abyss
        new FishEntry(FishType.TripodFish, FishZone.Abyss),
        new FishEntry(FishType.SkeletalFish, FishZone.Abyss),
        new FishEntry(FishType.GristyShark, FishZone.Abyss),
        new FishEntry(FishType.MutatedIsopod, FishZone.Abyss),
        new FishEntry(FishType.MonsterEel, FishZone.Abyss),
        // hadal
        new FishEntry(FishType.MarianaSnailfish, FishZone.Hadal),
        new FishEntry(FishType.BloodSnapper, FishZone.Hadal),
        new FishEntry(FishType.Nightwing, FishZone.Hadal),
        new FishEntry(FishType.AncientPiranha, FishZone.Hadal),
        // blood
        new FishEntry(FishType.BloodBurden, FishZone.Blood),
        new FishEntry(FishType.BloodTorment, FishZone.Blood),
        // void
        new FishEntry(FishType.GrotesqueKingfish, FishZone.Void)
    };
    // get fish by zone
    public static List<FishType> GetFishByZone(FishZone zone)
    {
        List<FishType> result = new List<FishType>();
        foreach (var fish in Fishlist)
        {
            if (fish.zone == zone)
                result.Add(fish.type);
        }
        return result;
    }
    // get random fish
    public static FishType GetRandomFish(FishZone zone)
    {
        List<FishType> fishInZone = GetFishByZone(zone);
        if (fishInZone.Count == 0)
        {
            Debug.LogWarning("No Fish in zone: " + zone);
            return FishType.Clownfish;
        }
        int randomIndex = UnityEngine.Random.Range(0, fishInZone.Count);
        return fishInZone[randomIndex];
    }
}
