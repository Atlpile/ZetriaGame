using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

public class TestStructureComponent : BaseComponent
{
    public override IGameStructure GameStructure => TestStructure.Instance;

    private ITestModel _model;
    private ITestSystem _system;
    private ITestUtility _utility;
    private ITestManager _testManager;


    private void Awake()
    {
        _model = GameStructure.GetModel<ITestModel>();
        _utility = GameStructure.GetUtility<ITestUtility>();
        _system = GameStructure.GetSystem<ITestSystem>();

        _testManager = Manager.GetManager<ITestManager>();
    }

    private void Start()
    {
        GameStructure.AddGameEvent<TestEventArgs>(TestEventMethod);

        ModuleTestFunction();
        SendCommandFunction();
        TestManagerFunction();
    }

    private void OnDestroy()
    {
        GameStructure.RemoveGameEvent<TestEventArgs>(TestEventMethod);
    }

    private void ModuleTestFunction()
    {
        _model.ModelTestFunction();
        _utility.UtilityTestFunction();
        _system.SystemTestFunction();
    }

    private void SendCommandFunction()
    {
        _system.SendCommandFunction();
    }

    private void TestManagerFunction()
    {
        _testManager.ManagerTestFunction();
        _testManager.TestCoroutineFunction();
    }

    private void TestEventMethod(TestEventArgs args)
    {
        Debug.Log(this.name + "执行监听的事件");
        Debug.Log(args.name);
        Debug.Log(args.age);
        Debug.Log(args.sex);
        Debug.Log(args.atk);
    }
}



