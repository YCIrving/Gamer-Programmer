using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;

public class XListNLogsWindow : EditorWindow
{

    public static int num=0;
    public static string inputNumStr="";
 

    public static void Init()
    {
        num = 0;
        inputNumStr = "0";
    }
    public static void ShowWindow()
    {      
        // xSetUserNameWindow.CreateInstance<xSetUserNameWindow>().showModal();
        var window = XListNLogsWindow.CreateInstance<XListNLogsWindow>();
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
                
        GUI.Label(new Rect(30, 10, Screen.width, 20), "Please enter the num: ");
        GUI.Label(new Rect(30, 40, 100, 20), "Num:");
        inputNumStr = GUI.TextField(new Rect(100, 40, 200, 20), inputNumStr, 20);
        
        if (GUI.Button(new Rect(100, 80, 200, 20), "OK"))
        {
            
            if (string.IsNullOrEmpty(inputNumStr))
            {
                EditorUtility.DisplayDialog("Num cannot be empty", "", "OK");
            }
            else
            {
                try
                {
                    num = Convert.ToInt32(inputNumStr);
                }
                catch (Exception e)
                {
                    EditorUtility.DisplayDialog("Error when converting input to number!", e.ToString(), "OK");
                    throw e;
                }

                if (num > 0)
                {
                    XLock.ListNLogs(num);
                    this.Close();
                }
                else
                {
                    EditorUtility.DisplayDialog("Num should be greater than 0", "", "OK");
                }

            }
        }
    }
}