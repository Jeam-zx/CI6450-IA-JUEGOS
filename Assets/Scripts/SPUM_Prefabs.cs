using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

/// <summary>
/// Enum representing the different states a player can be in.
/// </summary>
public enum PlayerState
{
    IDLE,
    MOVE,
    ATTACK,
    DAMAGED,
    DEBUFF,
    DEATH,
    OTHER,
}

/// <summary>
/// Class that manages the SPUM prefabs, including animations and movement.
/// </summary>
public class SPUM_Prefabs : MonoBehaviour
{
    public float _version;
    public bool EditChk;
    public string _code;
    public Animator _anim;
    private AnimatorOverrideController OverrideController;

    public string UnitType;
    public List<SpumPackage> spumPackages = new List<SpumPackage>();
    public List<PreviewMatchingElement> ImageElement = new();
    public List<SPUM_AnimationData> SpumAnimationData = new();
    public Dictionary<string, List<AnimationClip>> StateAnimationPairs = new();
    public List<AnimationClip> IDLE_List = new();
    public List<AnimationClip> MOVE_List = new();
    public List<AnimationClip> ATTACK_List = new();
    public List<AnimationClip> DAMAGED_List = new();
    public List<AnimationClip> DEBUFF_List = new();
    public List<AnimationClip> DEATH_List = new();
    public List<AnimationClip> OTHER_List = new();

    public float moveSpeed = 5f;
    private Kinematic kinematic;

    public GameObject directionIndicatorPrefab; // Prefab for the direction indicator
    private GameObject directionIndicatorInstance;
    public float indicatorDistance = 1.0f; // Distance of the indicator from the character

    /// <summary>
    /// Initializes the SPUM prefabs and sets up the direction indicator.
    /// </summary>
    void Start()
    {
        OverrideControllerInit();

        kinematic = GetComponent<Kinematic>();

        // Instantiate the direction indicator
        if (directionIndicatorPrefab != null)
        {
            directionIndicatorInstance = Instantiate(directionIndicatorPrefab, transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Initializes the animator override controller and sets up animation clips.
    /// </summary>
    public void OverrideControllerInit()
    {
        Animator animator = _anim;
        OverrideController = new AnimatorOverrideController();
        OverrideController.runtimeAnimatorController = animator.runtimeAnimatorController;

        // Get all animation clips
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            // Override with cloned clips
            OverrideController[clip.name] = clip;
        }

        animator.runtimeAnimatorController = OverrideController;
        foreach (PlayerState state in Enum.GetValues(typeof(PlayerState)))
        {
            var stateText = state.ToString();
            StateAnimationPairs[stateText] = new List<AnimationClip>();
            switch (stateText)
            {
                case "IDLE":
                    StateAnimationPairs[stateText] = IDLE_List;
                    break;
                case "MOVE":
                    StateAnimationPairs[stateText] = MOVE_List;
                    break;
                case "ATTACK":
                    StateAnimationPairs[stateText] = ATTACK_List;
                    break;
                case "DAMAGED":
                    StateAnimationPairs[stateText] = DAMAGED_List;
                    break;
                case "DEBUFF":
                    StateAnimationPairs[stateText] = DEBUFF_List;
                    break;
                case "DEATH":
                    StateAnimationPairs[stateText] = DEATH_List;
                    break;
                case "OTHER":
                    StateAnimationPairs[stateText] = OTHER_List;
                    break;
            }
        }
    }

    /// <summary>
    /// Checks if all animation lists have items.
    /// </summary>
    /// <returns>True if all lists have items, otherwise false.</returns>
    public bool AllListsHaveItemsExist()
    {
        List<List<AnimationClip>> allLists = new List<List<AnimationClip>>()
        {
            IDLE_List, MOVE_List, ATTACK_List, DAMAGED_List, DEBUFF_List, DEATH_List, OTHER_List
        };

        return allLists.All(list => list.Count > 0);
    }

    /// <summary>
    /// Populates the animation lists based on the SPUM packages.
    /// </summary>
    [ContextMenu("PopulateAnimationLists")]
    public void PopulateAnimationLists()
    {
        IDLE_List = new();
        MOVE_List = new();
        ATTACK_List = new();
        DAMAGED_List = new();
        DEBUFF_List = new();
        DEATH_List = new();
        OTHER_List = new();

        var groupedClips = spumPackages
            .SelectMany(package => package.SpumAnimationData)
            .Where(spumClip => spumClip.HasData &&
                               spumClip.UnitType.Equals(UnitType) &&
                               spumClip.index > -1)
            .GroupBy(spumClip => spumClip.StateType)
            .ToDictionary(
                group => group.Key,
                group => group.OrderBy(clip => clip.index).ToList()
            );

        foreach (var kvp in groupedClips)
        {
            var stateType = kvp.Key;
            var orderedClips = kvp.Value;
            switch (stateType)
            {
                case "IDLE":
                    IDLE_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
                case "MOVE":
                    MOVE_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
                case "ATTACK":
                    ATTACK_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
                case "DAMAGED":
                    DAMAGED_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
                case "DEBUFF":
                    DEBUFF_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
                case "DEATH":
                    DEATH_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
                case "OTHER":
                    OTHER_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    break;
            }
        }
    }

    /// <summary>
    /// Plays the specified animation for the given player state.
    /// </summary>
    /// <param name="PlayState">The player state for which to play the animation.</param>
    /// <param name="index">The index of the animation in the list.</param>
    public void PlayAnimation(PlayerState PlayState, int index)
    {
        Animator animator = _anim;
        var animations = StateAnimationPairs[PlayState.ToString()];
        OverrideController[PlayState.ToString()] = animations[index];
        var StateStr = PlayState.ToString();

        bool isMove = StateStr.Contains("MOVE");
        bool isDebuff = StateStr.Contains("DEBUFF");
        bool isDeath = StateStr.Contains("DEATH");
        animator.SetBool("1_Move", isMove);
        animator.SetBool("5_Debuff", isDebuff);
        animator.SetBool("isDeath", isDeath);
        if (!isMove && !isDebuff)
        {
            AnimatorControllerParameter[] parameters = animator.parameters;
            foreach (AnimatorControllerParameter parameter in parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                {
                    bool isTrigger = parameter.name.ToUpper().Contains(StateStr.ToUpper());
                    if (isTrigger)
                    {
                        animator.SetTrigger(parameter.name);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Loads an animation clip from the "Animations" folder.
    /// </summary>
    /// <param name="clipPath">The path to the animation clip.</param>
    /// <returns>The loaded animation clip.</returns>
    AnimationClip LoadAnimationClip(string clipPath)
    {
        AnimationClip clip = Resources.Load<AnimationClip>(clipPath.Replace(".anim", ""));

        if (clip == null)
        {
            Debug.LogWarning($"Failed to load animation clip '{clipPath}'.");
        }

        return clip;
    }
}