using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishEntry
{
    public string Name;

    [TextArea(5, 20)]
    public string Description;
    //public Sprite FishImage;
    public bool IsUnlocked;

}
public class FishCodex : MonoBehaviour
{
    public static FishCodex instance;


    [SerializeField] private List<FishEntry> FishEntries;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            UnlockFish(FishIndex.FishType.Anglerfish);

            Debug.Log(
                GetEntry(FishIndex.FishType.Anglerfish)
                .Description
            );
        }
    }

    public FishEntry GetEntry(FishIndex.FishType fish)
    {
        return FishEntries[(int)fish];
    }

    public bool IsUnlocked(FishIndex.FishType fish)
    {
        return FishEntries[(int)fish].IsUnlocked;
    }
    public void UnlockFish(FishIndex.FishType fish)
    {
        int index = (int)fish;

        if (index < 0 || index >= FishEntries.Count)
        {
            Debug.LogError($"Fish entry missing for {fish}");
            return;
        }

        FishEntries[index].IsUnlocked = true;
    }


}

