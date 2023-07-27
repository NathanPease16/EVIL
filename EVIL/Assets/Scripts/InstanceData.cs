using System.IO;
using UnityEngine;

public class InstanceData : MonoBehaviour
{
    [Header("Directory")]
    private const string directoryName = "runtime";
    private static string appDirectory = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
    private static string dataDirectory = Path.Combine(appDirectory, directoryName);
    public static string AppDirectory { get { return appDirectory; } }
    public static string DataDirectory { get { return dataDirectory; } }

    public static bool Exists(string name) 
    {
        string path = GetPath(name);

        return File.Exists(path);
    }

    public static void SetInt(string name, int value) 
    {
        string path = GetPath(name);

        Validate(path);

        File.WriteAllText(path, value.ToString());
    }

    public static int GetInt(string name) 
    {
        string path = GetPath(name);
        return int.Parse(File.ReadAllText(path));
    }

    public static void SetLong(string name, long value)
    {
        string path = GetPath(name);

        Validate(path);

        File.WriteAllText(path, value.ToString());
    }

    public static long GetLong(string name) 
    {
        string path = GetPath(name);
        return long.Parse(File.ReadAllText(path));
    }

    public static void SetFloat(string name, float value) 
    {
        string path = GetPath(name);

        Validate(path);

        File.WriteAllText(path, value.ToString());
    }

    public static float GetFloat(string name) 
    {
        string path = GetPath(name);
        return float.Parse(File.ReadAllText(path));
    }

    public static void SetBool(string name, bool value) 
    {
        string path = GetPath(name);

        Validate(path);

        File.WriteAllText(path, value.ToString().ToLower());
    }

    public static bool GetBool(string name) 
    {
        string path = GetPath(name);
        return bool.Parse(File.ReadAllText(path));
    }

    public static void SetString(string name, string value)
    {
        string path = GetPath(name);

        Validate(path);
        
        File.WriteAllText(path, value);
    }

    public static string GetString(string name)
    {
        string path = GetPath(name);
        return File.ReadAllText(path);
    }

    public static void SetVector3(string name, Vector3 value)
    {
        string path = GetPath(name);

        Validate(path);
        
        File.WriteAllText(path, $"{value.x},{value.y},{value.z}");
    }

    public static Vector3 GetVector3(string name)
    {
        string path = GetPath(name);
        string[] coords = File.ReadAllText(path).Split(',');

        float x = float.Parse(coords[0]);
        float y = float.Parse(coords[1]);
        float z = float.Parse(coords[2]);

        return new Vector3(x, y, z);
    }

    public static void SetQuaternion(string name, Quaternion value)
    {
        string path = GetPath(name);

        Validate(path);
        
        File.WriteAllText(path, $"{value.x},{value.y},{value.z},{value.z}");
    }

    public static Quaternion GetQuaternion(string name)
    {
        string path = GetPath(name);
        string[] coords = File.ReadAllText(path).Split(',');

        float x = float.Parse(coords[0]);
        float y = float.Parse(coords[1]);
        float z = float.Parse(coords[2]);
        float w = float.Parse(coords[3]);

        return new Quaternion(x, y, z, w);
    }

    private static void Validate(string path)
    {
        if (!Directory.Exists(dataDirectory))
            Directory.CreateDirectory(dataDirectory);
        if (!File.Exists(path))
            using (File.Create(path)) {;}
    }

    private static string GetPath(string name) 
    {
        return Path.Combine(dataDirectory, name + ".txt");
    }
}
