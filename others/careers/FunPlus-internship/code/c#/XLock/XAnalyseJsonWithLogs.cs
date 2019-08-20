using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class LockLog
{
    public string level;
    public long ts;
    public string caller;
    public string msg;

}
    
public class XAnalyseJsonWithLogs
{
    public string err;
    public LockLog[] result;    
    // public LockLog[] data;
}
