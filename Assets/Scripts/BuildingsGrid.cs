using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);

    private Building[,] grid;
    private Building flyingBuilding;
    private Camera mainCamera;
    [SerializeField]
    private GameObject resourcePanel;
    public int _gold, addgold = 0;
    public GameObject upgradePanel;
    public int go = 0;
    private Building up;

    private Resource res;

    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];
        mainCamera = Camera.main;
        StartCoroutine(AddGold());
        Building.IsAction += OnBuildingSelected;
        res = resourcePanel.GetComponent<Resource>();

    }

    private void OnBuildingSelected(Building buildingAvtive)
    {
       
            upgradePanel.SetActive(true);
            up = buildingAvtive;
        
    }




    public void StartPlacingBuilding(Building buildingPrefab)
    {
        _gold = buildingPrefab.bildingGold;
        if (res.allGold >= buildingPrefab.buildingCost)
        {
            if (flyingBuilding != null)
            {
                Destroy(flyingBuilding.gameObject);
            }

            flyingBuilding = Instantiate(buildingPrefab);

        }
        else
        {
            return;
        }
    }



    private void Update()
    {
        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool available = true;

                if (x < 0 || x > GridSize.x - flyingBuilding.Size.x) available = false;
                if (y < 0 || y > GridSize.y - flyingBuilding.Size.y) available = false;

                if (available && IsPlaceTaken(x, y)) available = false;


                flyingBuilding.transform.position = new Vector3(x, 0, y);
                flyingBuilding.SetTransparent(available);


                if (available && Input.GetMouseButtonDown(1))
                {
                    PlaceFlyingBuilding(x, y);
                }
            }
        }

    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null) return true;
            }
        }

        return false;
    }

    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        res.allGold -= flyingBuilding.buildingCost;
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = flyingBuilding;

                addgold += _gold;
                _gold = 0;

            }
        }

        flyingBuilding.SetNormal();
        flyingBuilding = null;
    }
    private IEnumerator AddGold()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(1f);
            res.allGold += addgold;
            Debug.Log(res.allGold);
        }

    }

    public void ClosePanel() 
    {
        upgradePanel.SetActive(false);
    }

    public void SellBuild() 
    {
        addgold -= up.bildingGold;
        res.allGold += up.buildingCost / 2;
        Destroy(up.gameObject);
        upgradePanel.SetActive(false);

    }

    public void UpgradeBuild() 
    {
        addgold -= up.bildingGold;
        StartPlacingBuilding(up.upgrade);
        Destroy(up.gameObject);
        upgradePanel.SetActive(false);
    }


    private void OnDestroy()
    {
        Building.IsAction -= OnBuildingSelected;
    }
}

