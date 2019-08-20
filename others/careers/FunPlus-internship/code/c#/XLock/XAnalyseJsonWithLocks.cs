using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class FileLock
{
    public string lockOwner;
    public string lockName;
    public string note;

}
    
public class XAnalyseJsonWithLocks
{
    public string err;
    public FileLock[] result;
    // public FileLock[] data;
}
