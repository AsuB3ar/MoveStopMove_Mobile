using CustomAttribute;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys
{
    public class Pool : MonoBehaviour
    {
        [SerializeField]
        GAMECONST.GAMEPLAY_MODE type;
        [SerializeField]
        protected GameObject mainPool;
        [SerializeField]
        public bool IsSetParent = false;
        [ConditionalField(nameof(type), false, GAMECONST.GAMEPLAY_MODE.STANDARD_PVP)]
        [SerializeField]
        PhotonPool photon;
        [HideInInspector]
        private GameObject obj;
        private bool network = false;
        Queue<GameObject> objects = new Queue<GameObject>();
        

        Quaternion initQuaternion;
        int numObj = 10;
        List<int> serializeData = new List<int>();
        private void Awake()
        {
            switch (type)
            {
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVP:
                    photon.SetSerializeData(serializeData);
                    break;
            }
        }
        public void Initialize(GameObject obj,Quaternion initQuaternion = default,int numObj = 10, bool network = false)
        {
            this.network = network;
            this.numObj = numObj;
            this.obj = obj;
            this.initQuaternion = initQuaternion;
            AddObject();
            if(network)
                photon.UpdatePhotonData();
        }
        public void AddObject()
        {
            switch (type)
            {
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVE:                    
                    break;
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVP:
                    if (!photon.photonView.IsMine) return; //DEV: This case has to implement
                    break;
            }

            for (int i = 0; i < numObj; i++)
            {
                GameObject obj;
                if (network == false)
                    obj = Instantiate(this.obj, Vector3.zero, this.initQuaternion, mainPool.transform);
                else
                {
                    obj = NetworkManager.Inst.Instantiate(this.obj.name);
                    obj.transform.parent = transform;
                    if (network)
                        serializeData.Add(obj.GetComponent<PhotonView>().ViewID);
                }

                obj.SetActive(false);
                objects.Enqueue(obj);
            }
            if (network)
                photon.UpdatePhotonData();
        }
        public void Push(GameObject obj,bool checkContain = true)
        {
            if (obj == null) return;

            if (checkContain)
            {
                if (objects.Contains(obj))
                    return;
            }            
            objects.Enqueue(obj);
          
            if (IsSetParent)
            {
                obj.transform.SetParent(mainPool.transform);
            }
            obj.SetActive(false);
            obj.transform.position = Vector3.zero;
            if (network)
                serializeData.Add(obj.GetComponent<PhotonView>().ViewID);
        }

        public GameObject Pop()
        {
            if(objects.Count == 0)
            {
                AddObject();
            }

            GameObject returnObj = objects.Dequeue();
            returnObj.SetActive(true);
            if(network)
                serializeData.Remove(returnObj.GetComponent<PhotonView>().ViewID);
            return returnObj;
        }
        public void InitPhotonData()
        {
            for(int i = 0; i < serializeData.Count; i++)
            {
                GameObject gObject = PhotonView.Find(serializeData[i]).gameObject;
                objects.Enqueue(gObject);
                gObject.transform.parent = transform;
            }
        }
    }
}

