﻿using System.Collections;
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
    public int _gold, gold = 0;
    public GameObject upgradePanel;
    private Building GO;


    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];

        mainCamera = Camera.main;
        InvokeRepeating("AddGoldNoCoroutine", 0, 1);
        //StartCoroutine(AddGold(gold));

    }




    public void StartPlacingBuilding(Building buildingPrefab)
    {
        _gold = buildingPrefab.GetComponent<Building>().bildingGold;
        if (resourcePanel.GetComponent<Resource>().allGold >= buildingPrefab.GetComponent<Building>().buildingCost)
        {
            if (flyingBuilding != null)
            {
               Destroy(flyingBuilding.gameObject);
               resourcePanel.GetComponent<Resource>().allGold += buildingPrefab.GetComponent<Building>().buildingCost;

            }

            flyingBuilding = Instantiate(buildingPrefab);
            resourcePanel.GetComponent<Resource>().allGold -= buildingPrefab.GetComponent<Building>().buildingCost;
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


                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBuilding(x, y);
                }
            }
        }
        //else
        //{
        //    var groundPlane = new Plane(Vector3.up, Vector3.zero);
        //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        //    if (groundPlane.Raycast(ray, out float position))
        //    {
        //        Vector3 worldPosition = ray.GetPoint(position);

        //        int x = Mathf.RoundToInt(worldPosition.x);
        //        int y = Mathf.RoundToInt(worldPosition.z);


        //        if (IsPlaceTaken(x, y))
        //        {

        //            if (Input.GetMouseButtonDown(2))
        //            {
        //                OnDestroy(x, y);
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    var groundPlane = new Plane(Vector3.up, Vector3.zero);
        //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        //    if (groundPlane.Raycast(ray, out float position))
        //    {
        //        Vector3 worldPosition = ray.GetPoint(position);

        //        int x = Mathf.RoundToInt(worldPosition.x);
        //        int y = Mathf.RoundToInt(worldPosition.z);

        //       if (Input.GetMouseButtonDown(0))
        //        {
        //            upgradePanel.SetActive(true);
        //        }
        //    }
        //}
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
            for (int x = 0; x < flyingBuilding.Size.x; x++)
            {
                for (int y = 0; y < flyingBuilding.Size.y; y++)
                {
                    grid[placeX + x, placeY + y] = flyingBuilding;
                    gold += _gold;
                    _gold = 0;

                }
            }

            flyingBuilding.SetNormal();
            flyingBuilding = null;
        }
        //public IEnumerator AddGold(int gold)
        //{
        //    while (true)
        //    {
        //        yield return new WaitForSeconds(1);
        //        resourcePanel.GetComponent<Resource>().allGold += gold;
        //    }

        //}
        public void AddGoldNoCoroutine()
        {
            resourcePanel.GetComponent<Resource>().allGold += gold;
        }

    //private void OnDestroy(int placeX, int placeY)
    //{
    //    for (int x = 0; x < GridSize.x; x++)
    //    {
    //        for (int y = 0; y < GridSize.y; y++)
    //        {
    //            grid[placeX + x, placeY + y] = GO;
    //            Destroy(GO);
    //            gold -= _gold;
    //            _gold = 0;

    //        }
    //    }
    //} 
}
