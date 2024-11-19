using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    private Table table;

    [Header("Materials")]
    public Material MBonus;
    public Material MCombat;
    public Material MEntrance;
    public Material MExit;
    public Material MParkour;
    public Material MShop;
    public Material MMoldOrb;
    public Material MWaveSurvival;

    void Start() {
        table = new();
        Generate();
    }

    public void Generate() {
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }

        table.Generate();
        DrawRoomsDebug();
    }

    public void DrawRoomsDebug() {
        foreach (Row row in table.table) {
            GameObject room = Instantiate(roomPrefab, RowPosition(row), Quaternion.identity);
            room.transform.SetParent(transform, true);
            switch (GlobalReference.GetReference<GameManagerReference>().Get(row.id).roomType) {
                case RoomType.BONUS:
                    room.GetComponent<MeshRenderer>().material = MBonus;
                    break;
                case RoomType.COMBAT:
                    room.GetComponent<MeshRenderer>().material = MCombat;
                    break;
                case RoomType.ENTRANCE:
                    room.GetComponent<MeshRenderer>().material = MEntrance;
                    break;
                case RoomType.EXIT:
                    room.GetComponent<MeshRenderer>().material = MExit;
                    break;
                case RoomType.PARKOUR:
                    room.GetComponent<MeshRenderer>().material = MParkour;
                    break;
                case RoomType.SHOP:
                    room.GetComponent<MeshRenderer>().material = MShop;
                    break;
                case RoomType.MOLDORB:
                    room.GetComponent<MeshRenderer>().material = MMoldOrb;
                    break;
                case RoomType.WAVESURVIVAL:
                    room.GetComponent<MeshRenderer>().material = MWaveSurvival;
                    break;
                default:
                    break;
            }
            room.name = table.RowReference(row);
            int i = 0;
            foreach (int branch in row.branches) {
                // Debug.DrawLine(RowPosition(row), RowPosition(table.GetRow(branch)), Color.black, 100, true);
                room.GetComponent<LineRenderer>().positionCount = i + 2;
                room.GetComponent<LineRenderer>().SetPosition(i, RowPosition(row));
                room.GetComponent<LineRenderer>().SetPosition(i+1, RowPosition(table.GetRow(branch)));
                i+=2;
            }
        }
    }

    public void ChangeSeed(string value) {
        if (value == "") value = "-1";
        table.seed = int.Parse(value);
    }
    public void ChangeMaxObjectPerDepth(string value) {
        if (value == "") value = "0";
        table.maxObjectPerDepth = int.Parse(value);
    }
    public void ChangeMaxBranchCount(string value) {
        if (value == "") value = "0";
        table.maxBranchCount = int.Parse(value);
    }
    public void ChangeTargetDepth(string value) {
        if (value == "") value = "0";
        table.targetDepth = int.Parse(value);
    }


    public Vector3 RowPosition(Row row) => new(2 * row.depth, 1, -2 * table.GetIndexInDepth(row));
}