using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using PrintDebug;
public class PrintAndSavaLogInfo
{
    static int m_Line = 0;

    private static PrintAndSavaLogInfo _instance;
    public static PrintAndSavaLogInfo GetSingle()
    {
        if (_instance == null)
        {
            _instance = new PrintAndSavaLogInfo();
            DebugLog.EnableLog = false;//关闭Debug.Log输出。
        }
        return _instance;
    }

    public  void Print_M(string info)//static
    {
        m_Line++;
        string path = Application.dataPath + "/LogFile.txt";
        StreamWriter sw;
        PrintDebug.DebugLog.Log(path);
        //Debug.Log(path);
        if (m_Line == 1)
        {
            sw = new StreamWriter(path, false);
            System.DateTime datetime = System.DateTime.Now;
            string fileTitle = "创建时间：" + string.Format("{0:F}", datetime);
            sw.WriteLine(fileTitle);
        }
        else
        {
            sw = new StreamWriter(path, true);
        }
        string lineInfo = m_Line + "\t" + "时刻 " + Time.time + ": ";
        sw.WriteLine(lineInfo);
        sw.WriteLine(info);
        PrintDebug.DebugLog.Log(info);
        sw.Flush();
        sw.Close();
        AssetDatabase.Refresh();
    }
}
