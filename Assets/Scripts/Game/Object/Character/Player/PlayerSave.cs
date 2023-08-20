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
        GameManager.Instance.SaveLoadManager.UpdateData<TestData>("TestData", data =>
        {
            data.posX = this.transform.position.x;
            data.posY = this.transform.position.y;
            data.posZ = this.transform.position.z;
            Debug.Log("存储当前位置：" + new Vector3(data.posX, data.posY, data.posZ));

            data.Container.Add("1", 1);
            print("存储键值" + data.Container["1"]);
        });
    }
    private void LoadData()
    {
        TestData testData = GameManager.Instance.SaveLoadManager.LoadData<TestData>("TestData");
        Debug.Log("加载当前位置：" + new Vector3(testData.posX, testData.posY, testData.posZ));
        print("读取键值" + testData.Container["1"]);
    }
}
