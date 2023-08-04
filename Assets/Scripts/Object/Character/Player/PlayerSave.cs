using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            SaveData();
        else if (Input.GetKeyDown(KeyCode.Keypad2))
            LoadData();
    }

    private void SaveData()
    {
        TestData testData = new()
        {
            posX = this.transform.position.x,
            posY = this.transform.position.y,
            posZ = this.transform.position.z
        };
        Debug.Log("存储当前位置：" + new Vector3(testData.posX, testData.posY, testData.posZ));

        testData.Container.Add("1", 1);
        print("存储键值" + testData.Container["1"]);

        GameManager.Instance.SaveLoadManager.SaveData(testData, "TestData");
    }

    private void LoadData()
    {
        TestData testData = GameManager.Instance.SaveLoadManager.LoadData<TestData>("TestData");
        Debug.Log("加载当前位置：" + new Vector3(testData.posX, testData.posY, testData.posZ));
        print("读取键值" + testData.Container["1"]);
    }
}
