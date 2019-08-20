using System.Reflection;
using UnityEditor;
using UnityEngine;

public class XSetUserNameWindow : EditorWindow
{
    public static string unityUserName="";
 
    // [MenuItem("Tools/Test Modal Editor Window")]

    public static void Init()
    {
        unityUserName = "";
    }
    public static void ShowWindow()
    {      
        // xSetUserNameWindow.CreateInstance<xSetUserNameWindow>().showModal();
        unityUserName = PlayerPrefs.GetString("UnityUserName");
        var window = XSetUserNameWindow.CreateInstance<XSetUserNameWindow>();
        window.Show();
        window.Focus();
    }
 
    void OnEnable()
    {
 
    }
 
    void OnDisable()
    {
        //setMainWindowFocus();
    }
 
    void OnGUI()
    {
                
        GUI.Label(new Rect(30, 10, Screen.width, 20), "Please enter your git username: ");
        GUI.Label(new Rect(30, 40, 100, 20), "User Name");
        unityUserName = GUI.TextField(new Rect(100, 40, 200, 20), unityUserName, 20);
        // unityUserName = GUILayout.TextField("User Name", this.unityUserName, 30);
        if (GUI.Button(new Rect(100, 80, 200, 20), "Save"))
        {
            
            if (string.IsNullOrEmpty(unityUserName))
            {
                EditorUtility.DisplayDialog("User Name cannot be empty", "", "OK");
            }
            else
            {
                PlayerPrefs.SetString("UnityUserName", unityUserName);
                PlayerPrefs.Save();                
                EditorUtility.DisplayDialog("User Name Saved", "Set your user name to: " + unityUserName, "OK");
                this.Close();
            }
        }
    }
    void showModal()
    {
        MethodInfo dynShowModal = this.GetType().GetMethod("ShowModal", BindingFlags.NonPublic | BindingFlags.Instance);
        dynShowModal.Invoke(this, new object[] { });
    }
}