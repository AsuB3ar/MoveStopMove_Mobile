using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class GAMECONST
{

    public enum PHYSIC_OTYPE
    {
        Character = 0,
        Sensor = 1,
        Model = 2
    }

    public enum GAMEPLAY_MODE
    {
        STANDARD_PVP = 0,
        STANDARD_PVE = 1,
    }

    public enum NETWORK_OBJECT_TYPE
    {
        NONE = -1,
        MANAGER = 0,
        RESOURCE = 1,
        GAMEPLAY = 2,
    }

    #region GAME_PLAY
    public static readonly string ANIM_IS_IDLE = "IsIdle";
    public static readonly string ANIM_IS_DEAD = "IsDead";
    public static readonly string ANIM_IS_ATTACK = "IsAttack";
    public static readonly string ANIM_IS_WIN = "IsWin";
    public static readonly string ANIM_IS_DANCE = "IsDance";
    public static readonly string ANIM_IS_ULTI = "IsUlti";

    public static readonly int ANIM_IS_ATTACK_FRAMES = 33;
    public static readonly int ANIM_IS_SPECIAL_ATTACK_FRAMES = 47;

    public const float ANIM_IS_ATTACK_TIME = 1.03f;
    public const float ANIM_IS_DEAD_TIME = 2.06f;
    public const float INIT_CHARACTER_HEIGHT = 0.63f;
    #endregion

    #region SCENE_NAME
    public static readonly string INIT_SCENE = "InitScene";
    public static readonly string LOAD_START_SCENE = "LoadStartScene";
    public static readonly string STANDARD_PVE_SCENE = "PveStandardScene";
    public static readonly string STANDARD_PVP_SCENE = "PvpStandardScene";
    public static readonly string INIT_PVP_RESOUCRCES_SCENE = "InitPvpResourcesScene";
    public static readonly string PVP_LOBBY_SCENE = "PvpLobbyScene";
    #endregion
}
