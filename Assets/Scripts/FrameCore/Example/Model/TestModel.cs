using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public interface ITestModel : IModel
    {
        int IntValue { get; set; }
        float FloatValue { get; set; }
        bool BoolValue { get; set; }
        string StringValue { get; set; }
        RoleInfo RoleInfoValue { get; set; }
        List<RoleInfo> RoleInfoList { get; set; }
        Dictionary<int, RoleInfo> RoleInfoDic { get; set; }

        void ModelTestFunction();
    }

    public class RoleInfo
    {
        public int hp;
        public int mp;
        public int atk;
        public int def;
    }

    public class TestModel : BaseModel, ITestModel
    {
        public int IntValue { get; set; }
        public float FloatValue { get; set; }
        public bool BoolValue { get; set; }
        public string StringValue { get; set; }
        public RoleInfo RoleInfoValue { get; set; }
        public List<RoleInfo> RoleInfoList { get; set; }
        public Dictionary<int, RoleInfo> RoleInfoDic { get; set; }

        protected override void OnInit()
        {
            IntValue = 1;
            FloatValue = 1.5f;
            BoolValue = true;
            StringValue = "测试内容";

            RoleInfoValue = new RoleInfo()
            {
                hp = 5,
                mp = 10,
                atk = 15,
                def = 20,
            };

            Debug.Log("Model初始化成功");
        }

        public void ModelTestFunction() => Debug.Log("Model中的测试方法");

    }
}


