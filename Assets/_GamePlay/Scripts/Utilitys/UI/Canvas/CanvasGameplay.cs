using MoveStopMove.Core;
using MoveStopMove.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilitys.Input;
using TMPro;

public class CanvasGameplay : UICanvas
{
    public class PlayerProfile
    {
        protected int id;
        protected string name;
        public int score = 0;
        public int Id => id;
        public string Name => name;
        public PlayerProfile(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
    private readonly Vector3 TARGET_INDICATOR_UP = Vector3.up * 1.5f;
    private const float SENSITIVITY = 0.1f;
    float minX = UITargetIndicator.WIDTH / 2;
    float maxX = Screen.width - UITargetIndicator.WIDTH / 2;

    float minY = UITargetIndicator.HEIGHT / 2;
    float maxY = Screen.height - UITargetIndicator.HEIGHT / 2;

    public JoyStick joyStick;
    [SerializeField]
    Transform canvasIndicatorTF;
    [SerializeField]
    TMP_Text remainingPlayersNum;
    [SerializeField]
    GameObject playerRanks;
    [SerializeField]
    List<UITargetIndicator> targetIndicators;
    [SerializeField]
    List<TMP_Text> playerNames;
    List<PlayerProfile> playerScores = new List<PlayerProfile>();
    Queue<UITargetIndicator> notUseIndicator = new Queue<UITargetIndicator>();
    Dictionary<BaseCharacter, UITargetIndicator> indicators = new Dictionary<BaseCharacter, UITargetIndicator>();
    List<BaseCharacter> characters = new List<BaseCharacter>();

    private void Awake()
    {
        for (int i = 0; i < targetIndicators.Count; i++)
        {
            notUseIndicator.Enqueue(targetIndicators[i]);
        }
        NetworkManager.Inst._OnLeftRoom += OnLeftRoom;
        NetworkManager.Inst._OnPlayerAddPoint += UpdatePlayerScores;
    }

    public void ChangeMode(GAMECONST.GAMEPLAY_MODE mode)
    {
        switch (mode)
        {
            case GAMECONST.GAMEPLAY_MODE.STANDARD_PVE:
                if(NetworkManager.Inst.IsMasterClient)
                    NetworkManager.Inst._OnPlayerStatusRoomChange -= OnPlayerEnterRoom;
                playerRanks.SetActive(false);
                break;
            case GAMECONST.GAMEPLAY_MODE.STANDARD_PVP:
                if (NetworkManager.Inst.IsMasterClient)
                {
                    NetworkManager.Inst._OnPlayerStatusRoomChange += OnPlayerEnterRoom;
                    playerScores.Add(new PlayerProfile(NetworkManager.Inst.LocalPlayer.ActorNumber, NetworkManager.Inst.LocalPlayer.NickName));
                }                              
                UIRankUpdate();
                playerRanks.SetActive(true);
                break;
        }
    }
    public void FixedUpdate()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i] == null) continue;
            indicators[characters[i]].SetLevel(characters[i].Level);
            Vector3 pos = GameplayManager.Inst.PlayerCamera.WorldToScreenPoint(characters[i].transform.position + TARGET_INDICATOR_UP * characters[i].Size);

            //NOTE: Because Clippane camera do not stick exactly to camera(0.01 forward)
            //=> Indicator error when character bettween the clippane camera plane and the actual camera plane
            //The situation happens when distance between character and actual camera plane are < distance between character and clippane camera plane
            if (pos.z < 0)
            {
                pos *= -1;
            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            pos.z = 0;
            indicators[characters[i]].transform.position = pos;
            indicators[characters[i]].SetDirection();

        }
    }
    public void SetRemainingPlayerNumber(int num)
    {
        remainingPlayersNum.text = num.ToString();
    }
    public void SubscribeTarget(BaseCharacter character, bool isPlayer = false)
    {
        if (character == null && indicators.ContainsKey(character))
            return;

        UITargetIndicator indicatorScript = notUseIndicator.Dequeue();
        indicatorScript.gameObject.SetActive(true);
        //indicatorScript.SetColor(new UnityEngine.Color(1f, 107f/255, 107f/255, 1f));
        indicatorScript.SetColor(GameplayManager.Inst.GetColor(character.Color));
        if (isPlayer)
        {
            indicatorScript.SetActiveDirection(false);
        }

        indicators.Add(character, indicatorScript);
        characters.Add(character);
    }
    public void UnsubcribeTarget(BaseCharacter character)
    {
        if (!indicators.ContainsKey(character)) return; //DEV: Need to optimize

        indicators[character].gameObject.SetActive(false);
        notUseIndicator.Enqueue(indicators[character]);
        indicators.Remove(character);
        characters.Remove(character);
    }
    public void SettingButton()
    {
        UIManager.Inst.OpenUI(UIID.UICSetting);
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);
    }
    public override void Open()
    {
        base.Open();
        GameplayManager.Inst.SetCameraPosition(CameraPosition.Gameplay);
    }
    public override void Close()
    {
        base.Close();
    }
    public void ClearTargets()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            UnsubcribeTarget(characters[i]);
            i--;
        }
    }
    public void AddLocalPlayerScore(int score)
    {
        playerScores.Find(p => p.Id == NetworkManager.Inst.LocalPlayer.ActorNumber).score += score;
        UIRankUpdate();
        object[] data = new object[playerScores.Count * 3];
        for(int i = 0; i < playerScores.Count; i++)
        {
            data[i * 3] = playerScores[i].Id;
            data[i * 3 + 1] = playerScores[i].score;
            data[i * 3 + 2] = playerScores[i].Name;
        }
        NetworkManager.Inst.RaiseEvent(NetworkManager.EVENT.PLAYER_ADD_POINT, data);
    }
    protected void OnPlayerEnterRoom(Photon.Realtime.Player player, bool value)
    {
        if (value)
        {
            playerScores.Add(new PlayerProfile(player.ActorNumber, player.NickName));
        }
        else
        {
            playerScores.Remove(playerScores.Find(p => p.Id == player.ActorNumber));
        }
        AddLocalPlayerScore(0);
    }
    List<PlayerProfile> oldPlayerScores = new List<PlayerProfile>();
    protected void UpdatePlayerScores(int[] id, int[] value, string[] name)
    {
        oldPlayerScores.Clear();
        oldPlayerScores.AddRange(playerScores);
        for(int i = 0; i < id.Length; i++)
        {
            PlayerProfile profile = playerScores.Find(p => p.Id == id[i]);
            if (profile != null)
            {
                profile.score = value[i];
                oldPlayerScores.Remove(profile);
            }
            else
            {
                profile = new PlayerProfile(id[i], name[i]);
                profile.score = value[i];
                playerScores.Add(profile);
            }           
        }

        for(int i = 0; i < oldPlayerScores.Count; i++)
        {
            playerScores.Remove(oldPlayerScores[i]);
        }
        UIRankUpdate();
    }
    protected void UIRankUpdate()
    {
        Debug.Log("=====Player Ranks: " + playerScores.Count);
        for (int i = 0; i < playerNames.Count; i++)
        {
            if(i < playerScores.Count)
            {
                playerNames[i].gameObject.SetActive(true);
                playerNames[i].text = $"{playerScores[i].Name}: {playerScores[i].score}";
            }
            else
            {
                playerNames[i].gameObject.SetActive(false);
            }
        }
    }
    protected void OnLeftRoom()
    {
        playerScores.Clear();
    }

    private void OnDestroy()
    {
        NetworkManager.Inst._OnLeftRoom -= OnLeftRoom;
        NetworkManager.Inst._OnPlayerAddPoint -= UpdatePlayerScores;
    }
}
