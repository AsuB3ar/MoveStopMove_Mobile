using MoveStopMove.Core;
using MoveStopMove.Core.Data;
using MoveStopMove.Manager;
using CustomAttribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System.Threading.Tasks;

[DefaultExecutionOrder(-100)]
public class SceneInitialize : MonoBehaviour
{   
    public enum SCENE_TYPE
    {
        INIT = 0,
        LOAD_START = 1,
        STANDARD_PVE = 2,
        STANDARD_PVP = 3,
        INIT_PVP_RESOURCES = 4,
    }
    [SerializeField]
    SCENE_TYPE type;

    [ConditionalField(nameof(type), false, SCENE_TYPE.INIT)]
    [SerializeField]
    GameData gameData;
    [ConditionalField(nameof(type), false, SCENE_TYPE.STANDARD_PVE, SCENE_TYPE.STANDARD_PVP)]
    [SerializeField]
    Camera playerCamera;
    [ConditionalField(nameof(type), false, SCENE_TYPE.STANDARD_PVE)]
    [SerializeField]
    BaseCharacter playerScript;
    [ConditionalField(nameof(type), false, SCENE_TYPE.STANDARD_PVE, SCENE_TYPE.STANDARD_PVP)]
    [SerializeField]
    GameObject targetIndicator;
    [ConditionalField(nameof(type), false, SCENE_TYPE.STANDARD_PVE, SCENE_TYPE.STANDARD_PVP)]
    [SerializeField]
    CameraMove cameraMove;
    [ConditionalField(nameof(type), false, SCENE_TYPE.STANDARD_PVE, SCENE_TYPE.STANDARD_PVP)]
    [SerializeField]
    GameObject playerPvp;

    private void Awake()
    {
        switch (type)
        {
            case SCENE_TYPE.INIT:
                SceneManager.Inst.LoadScene(GAMECONST.LOAD_START_SCENE);
                SceneManager.Inst._OnSceneLoaded += (name) => //DEV: Need to optimize, may be error here
                {
                    if (string.Compare(name, GAMECONST.LOAD_START_SCENE) == 0)
                    {
                        SceneManager.Inst.LoadScene(GAMECONST.STANDARD_PVE_SCENE);
                    }
                };
                break;
            case SCENE_TYPE.LOAD_START:
                break;
            case SCENE_TYPE.STANDARD_PVE:
                GameplayManager.Inst.GameMode = GAMECONST.GAMEPLAY_MODE.STANDARD_PVE;
                GameplayManager.Inst.PlayerScript = playerScript;
                GameplayManager.Inst.PlayerCamera = playerCamera;
                GameplayManager.Inst.TargetIndicator = targetIndicator;
                GameplayManager.Inst.CameraMove = cameraMove;
                gameData.OnInitData();
                UIManager.Inst.GetUI<CanvasGameplay>(UIID.UICGamePlay).ChangeMode(GAMECONST.GAMEPLAY_MODE.STANDARD_PVE);
                break;
            case SCENE_TYPE.INIT_PVP_RESOURCES:
                WaitingInitResource();          
                break;
            case SCENE_TYPE.STANDARD_PVP:                              
                GameplayManager.Inst.PlayerCamera = playerCamera;
                GameplayManager.Inst.TargetIndicator = targetIndicator;
                GameplayManager.Inst.CameraMove = cameraMove;

                GameObject character = NetworkManager.Inst.Instantiate(playerPvp.name);
                //character.transform.parent = Level;
                BaseCharacter characterScript = Cache.GetBaseCharacter(character);
                GameplayManager.Inst.PlayerScript = characterScript;
                GameplayManager.Inst.SetCameraFollow(GameplayManager.Inst.Player.transform);
                GameplayManager.Inst.PlayerScript.OnInit();
                //GameplayManager.Inst.Player.transform.parent = gameObject.transform;

                IReadOnlyList<PhotonPropertyGameObject> datas = PhotonPropertyGameObject.AllObjects;
                for (int i = 0; i < datas.Count; i++)
                {
                    datas[i].OnCompleteInit();
                }
                Debug.Log("DATA COUNT: " + datas.Count);
                UIManager.Inst.GetUI<CanvasGameplay>(UIID.UICGamePlay).ChangeMode(GAMECONST.GAMEPLAY_MODE.STANDARD_PVP);

                break;
        }
    }
    private void Start()
    {
        switch (type)
        {
            case SCENE_TYPE.STANDARD_PVE:
                UIManager.Inst.OpenUI(UIID.UICMainMenu);
                break;
            case SCENE_TYPE.STANDARD_PVP:              
                GameManager.Inst.StopGame();
                LevelManager.Inst.OpenLevel(GameManager.Inst.GameData.CurrentRegion);
                UIManager.Inst.OpenUI(UIID.UICGamePlay);
                SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);
                GameManager.Inst.StartGame();
                break;
        }
    }
    async void WaitingInitResource()
    {     
        var IsyncStates = FindObjectsOfType<MonoBehaviourPun>().OfType<ISyncState>();
        List<ISyncState> syncStates = new List<ISyncState>(IsyncStates);
        bool isCompleteInit;
        do
        {
            isCompleteInit = true;
            for(int i = 0; i < syncStates.Count; i++)
            {
                switch (syncStates[i].State)
                {
                    case ISyncState.STATE.ON_INIT:
                        isCompleteInit = false;
                        break;
                    case ISyncState.STATE.READY:
                        syncStates.RemoveAt(i);
                        i--;
                        break;
                }
            }
            await Task.Delay(500);
        } while (!isCompleteInit);
        Debug.Log("INIT RESOURCE: COMPLETE");
        SceneManager.Inst.LoadPhotonScene(GAMECONST.STANDARD_PVP_SCENE);
    }
}
