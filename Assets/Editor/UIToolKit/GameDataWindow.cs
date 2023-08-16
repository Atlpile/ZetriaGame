using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;
using System.Reflection;

public enum E_DataType
{
    GameData, SettingData
}

public class TestClass
{

}


public class GameDataWindow : EditorWindow
{
    private E_DataType _dataType = E_DataType.SettingData;
    private TextField Text_DataName;
    private Button Button_LoadData;
    private ScrollView Scroll_OldData;
    private ScrollView Scroll_NewData;
    private GroupBox Group_OldData;
    private GroupBox Group_NewData;
    private EnumField Enum_DataType;


    [MenuItem("Window/UI Toolkit/GameDataWindow")]
    public static void ShowExample()
    {
        GameDataWindow wnd = GetWindow<GameDataWindow>();
        wnd.titleContent = new GUIContent("GameDataWindow");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UIToolKit/GameDataWindow.uxml");
        visualTree.CloneTree(root);

        GetEditorComponent(root);
        InitWindow(root);
        AddEditorComponentEvent(root);
    }

    private void GetEditorComponent(VisualElement root)
    {
        Text_DataName = root.Q<TextField>(nameof(Text_DataName));
        Button_LoadData = root.Q<Button>(nameof(Button_LoadData));
        Group_OldData = root.Q<GroupBox>(nameof(Group_OldData));
        Group_NewData = root.Q<GroupBox>(nameof(Group_NewData));
        Scroll_OldData = root.Q<ScrollView>(nameof(Scroll_OldData));
        Scroll_NewData = root.Q<ScrollView>(nameof(Scroll_NewData));
        Enum_DataType = root.Q<EnumField>(nameof(Enum_DataType));
    }

    private void InitWindow(VisualElement root)
    {
        Group_OldData.SetEnabled(false);
        Enum_DataType.Init(_dataType);
    }

    private void AddEditorComponentEvent(VisualElement root)
    {
        Button_LoadData.clicked += () =>
        {
            if (Text_DataName.value == "filler text")
            {
                Debug.LogWarning("未输入名称，请检查是否输入名称");
                return;
            }

            var element_toggle = new Toggle("TestBool");
            var element_intField = new IntegerField("TestInt");
            var element_floatField = new FloatField("TestFloat");
            var element_textField = new TextField("TestString");
            SetElementStyle<Toggle>(element_toggle);
            SetElementStyle<IntegerField>(element_intField);
            SetElementStyle<FloatField>(element_floatField);
            SetElementStyle<TextField>(element_textField);


            string dataString = Enum_DataType.value.ToString();
            //ATTENTION：Editor获取Type的程序集 和 运行时获取Type的程序集不同
            Assembly assembly = Assembly.Load("Assembly-CSharp");
            Type t = assembly.GetType(dataString);
            Debug.Log(t.Name);


            // var obj = t.Assembly.CreateInstance(dataString);
            // Debug.Log(obj.GetType());


            // //加载TextField中的数据
            // SettingData data = JsonDataTool.LoadData<SettingData>(Text_DataName.value);

            // //根据数据创建相应的组件，并同步数据
            // element_toggle.label = nameof(data.isFullScreen);
            // element_toggle.value = data.isFullScreen;


            // Group_NewData.Add(element_toggle);
        };

    }

    private void SetElementStyle<T>(T element) where T : VisualElement
    {
        element.style.alignItems = Align.Center;
        element.style.marginLeft = 3;
        element.style.marginRight = 3;
        element.style.marginBottom = 3;
        element.style.marginTop = 3;
    }


    private List<object> GetListData(Type fieldType)
    {
        FieldInfo[] infos = fieldType.GetFields();
        List<object> dataList = new List<object>();

        for (int i = 0; i < infos.Length; i++)
        {

        }
        return null;
    }

    private object GetValue(Type fieldType)
    {
        if (fieldType == typeof(int))
        {

        }
        return null;
    }

    private void GetValue()
    {
        FieldInfo[] infos = typeof(SettingData).GetFields();
        FieldInfo info;
        for (int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            //通过反射得到数据类型
            if (info.FieldType == typeof(int))
            {
                //得到相应数据
                //在编辑器UI上创建相应控件
            }
            else if (info.FieldType == typeof(float))
            {

            }
            else if (info.FieldType == typeof(string))
            {

            }
            else if (info.FieldType == typeof(bool))
            {

            }
        }
    }


}