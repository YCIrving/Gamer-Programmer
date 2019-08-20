using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;

public class XInquireLockByUserNameWindow : EditorWindow
{

    public static string unityUserName="";
 

    public static void init()
    {
        unityUserName = "";
    }
    public static void showWindow()
    {      
        // xSetUserNameWindow.CreateInstance<xSetUserNameWindow>().showModal();
        XInquireLockByUserNameWindow.CreateInstance<XInquireLockByUserNameWindow>().Show();
        
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
                
        GUI.Label(new Rect(30, 10, Screen.width, 20), "Please enter the user name: ");
        GUI.Label(new Rect(30, 40, 100, 20), "User Name");
        unityUserName = GUI.TextField(new Rect(100, 40, 200, 20), unityUserName, 20);
        
        if (GUI.Button(new Rect(100, 80, 200, 20), "OK"))
        {
            
            if (string.IsNullOrEmpty(unityUserName))
            {
                EditorUtility.DisplayDialog("User Name cannot be empty", "", "OK");
            }
            else
            {
                XLock.InquireLockByUserName(unityUserName);
                this.Close();
            }
        }
    }
}