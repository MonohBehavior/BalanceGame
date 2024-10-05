using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace BalanceGame
{
    public class BalanceGameManager : MonoBehaviour
    {
        [SerializeField] private DropItemShopSO dropItemShopSO;

        [SerializeField] private StageUIViewer stageUI;
        [SerializeField] private LayerMask layers;
        [SerializeField] private Transform stageObject;
        [SerializeField] private Rigidbody stageRigidbody;
        [SerializeField] private ConstantForce force;

        [SerializeField] private GameObject debuggingCube;
        [SerializeField] private GameObject droppingPipe;
        [SerializeField] private TextAsset jsonData;

        private List<DropItemObject> itemsList = new();

        private Coroutine itemFallingCo;

        private Dictionary<int, Stack<GameObject>> itemsPool = new();

        private void Start()
        {
            GameInfo.TotalStageData = JsonConvert.DeserializeObject<TotalStageData>(jsonData.text);
            UpdateStageInfo();

            itemFallingCo = StartCoroutine(RunPipe());

            GameEvents.GameOverEvent += StopTheStage;
            GameEvents.StageClearEvent += OnStageClear;
            GameEvents.RestartEvent += OnRestart;
        }

        private void UpdateStageInfo()
        {
            var plateInfo = GameInfo.TotalStageData.levelDatas[GameInfo.CurrentStage].PlateInfo;
            stageObject.localScale = new Vector3(plateInfo.Size, 1, plateInfo.Size);
            stageRigidbody.mass = plateInfo.Mass;
            force.force = new Vector3(0, plateInfo.ConstantForce, 0);
        }

        public void OnPressingRestartButton()
        {
            GameEvents.RestartEvent?.Invoke();
        }

        private void OnStageClear()
        {
            StopTheStage();
            GameInfo.CurrentStage++;
            UpdateStageInfo();
        }

        private void OnRestart()
        {
            stageRigidbody.velocity = Vector3.zero;
            stageRigidbody.angularVelocity = Vector3.zero;
            stageObject.rotation = Quaternion.identity;

            Time.timeScale = 1;

            droppingPipe.transform.position = new Vector3(0f, 1.6f, 0f);

            foreach (var item in itemsList)
            {
                var index = item.Index;
                item.DropItemGameObject.SetActive(false);

                Stack<GameObject> pool;
                bool hasPool = itemsPool.ContainsKey(index);

                pool = hasPool ? itemsPool[index] : new Stack<GameObject>();
                pool.Push(item.DropItemGameObject);
                itemsPool[index] = pool;
            }

            itemsList.Clear();

            itemFallingCo = StartCoroutine(RunPipe());
            StartCoroutine(ReactivateConstantForce());
        }

        // TODO: FUTURE MONOH WILL FIND BETTER SOLUTION I TRUST YOU
        private IEnumerator ReactivateConstantForce()
        {
            yield return new WaitForSeconds(1f);

            force.enabled = true;
        }

        private void StopTheStage()
        {
            Time.timeScale = 0;

            force.enabled = false;

            if (itemFallingCo != null)
            {
                StopCoroutine(itemFallingCo);
                itemFallingCo = null;
            }
        }

        private IEnumerator RunPipe()
        {
            int length;
            SpawnGameObjects();

            var speed = GameInfo.CurrentStageData.FallingSpeed;
            GameInfo.FellItemFromPipeCount = 0;
            stageUI.ChangeMode(GameInfo.CurrentStageData.StageType);

            while (true)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100, layers))
                {
                    debuggingCube.transform.position = hit.point;
                    var pipePoint = new Vector3(hit.point.x, 1.6f, hit.point.z);
                    droppingPipe.transform.position = pipePoint;
                }

                GameInfo.TimePassed += Time.deltaTime;
                length = itemsList.Count;

                for (int i = 0; i < length; i++)
                {
                    if (GameInfo.FellItemFromPipeCount > i)
                    {
                        continue;
                    }
                    var fallingDist = Time.deltaTime * speed;
                    itemsList[i].DropItemGameObject.transform.localPosition -= new Vector3(0, fallingDist, 0);

                    if (itemsList[i].DropItemGameObject.transform.localPosition.y <= -4.8f)
                    {
                        itemsList[i].DropItemGameObject.transform.SetParent(null);
                        // TODO: don't get it here
                        var rigidBody = itemsList[i].DropItemGameObject.GetComponent<Rigidbody>();
                        rigidBody.velocity = Vector3.zero;
                        rigidBody.useGravity = true;
                        rigidBody.isKinematic = false;
                        GameInfo.FellItemFromPipeCount++;
                    }
                }

                var stageType = GameInfo.CurrentStageData.StageType;

                var tempFailed = 0;
                var waitSecond = 1f;

                bool levelClear = false;
                switch (stageType)
                {
                    case StageClearType.Survive:
                        levelClear = IsSurviveStageCleared(GameInfo.TimePassed);
                        stageUI.UpdateLeftTime(GameInfo.TimePassed / (GameInfo.CurrentStageData.SurviveAimSeconds + waitSecond));
                        break;
                    case StageClearType.Drop:
                        levelClear = IsAllItemsDropped(GameInfo.FellItemFromPipeCount);
                        var leftNum = (GameInfo.CurrentStageData.DropItemList.Count - GameInfo.FellItemFromPipeCount);
                        stageUI.UpdateLeftItem(leftNum);
                        break;
                    case StageClearType.FailLessThan:
                        levelClear = IsFailLessThanCleared(GameInfo.FellItemFromPipeCount, tempFailed);
                        var leftFailNum = GameInfo.CurrentStageData.FailLessThen - GameInfo.FailedItemCount;
                        stageUI.UpdateFailLessThen(leftFailNum);
                        break;
                    default:
                        break;
                }

                if (levelClear)
                {
                    yield return new WaitForSeconds(waitSecond);

                    GameEvents.StageEndEvent?.Invoke();
                    GameEvents.StageClearEvent?.Invoke();
                }

                yield return null;
            }
        }

        private bool IsSurviveStageCleared(float timePassed)
        {
            return GameInfo.CurrentStageData.SurviveAimSeconds <= timePassed;
        }

        private bool IsAllItemsDropped(int count)
        {
            return GameInfo.CurrentStageData.DropItemList.Count <= count;
        }

        private bool IsFailLessThanCleared(int count, int tempFailed)
        {
            return GameInfo.CurrentStageData.DropItemList.Count <= count &&
                GameInfo.CurrentStageData.FailLessThen >= tempFailed;
        }

        private void SpawnGameObjects()
        {
            var itemsDataList = GameInfo.CurrentStageData.DropItemList;
            var length = itemsDataList.Count;
            var startingPos = -4;

            for (int i = 0; i < length; i++)
            {
                var index = itemsDataList[i].GameObjectType;

                var gameObject = !itemsPool.ContainsKey(index) || itemsPool[index].Count == 0 ?
                    Instantiate(dropItemShopSO.Items[index]) : itemsPool[index].Pop();
                gameObject.transform.SetParent(droppingPipe.transform);
                gameObject.SetActive(true);
                gameObject.transform.localPosition = new Vector3(0, startingPos + i, 0);
                gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                gameObject.transform.localScale = new Vector3(2f, 1.5f, 2f);
                var rigidBody = gameObject.GetComponent<Rigidbody>();
                rigidBody.isKinematic = true;
                rigidBody.useGravity = false;
                itemsList.Add(new DropItemObject(index, gameObject));
            }
        }
    }
}