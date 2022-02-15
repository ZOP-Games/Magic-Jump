using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class GitUpdater : ScriptableObject
{
    // Start is called before the first frame update
    
    [MenuItem("Save To Git/Sync with copy folder")]
    public static void CopyToFolder()
    {
        ProcessStartInfo psi = new ProcessStartInfo("robocopy.exe",
            "\"D:\\User\\Documents\\prects\\unity\\Magic Jump\\Assets\" \"D:\\User\\Documents\\prects\\unity\\mj_clone\\Assets\" /E");
        Process.Start(psi).WaitForExit();
        
        ProcessStartInfo psi2 = new ProcessStartInfo("robocopy.exe",
            "\"D:\\User\\Documents\\prects\\unity\\Magic Jump\\Packages\" \"D:\\User\\Documents\\prects\\unity\\mj_clone\\Packages\" /E");
        Process.Start(psi2).WaitForExit();
        ProcessStartInfo psi3 = new ProcessStartInfo("robocopy.exe",
            "\"D:\\User\\Documents\\prects\\unity\\Magic Jump\\ProjectSettings\" \"D:\\User\\Documents\\prects\\unity\\mj_clone\\ProjectSettings\" /E");
        Process.Start(psi3).WaitForExit();
        ProcessStartInfo psi4 = new ProcessStartInfo("robocopy.exe",
            "\"D:\\User\\Documents\\prects\\unity\\Magic Jump\\UserSettings\" \"D:\\User\\Documents\\prects\\unity\\mj_clone\\UserSettings\" /E");
        Process.Start(psi4).WaitForExit();

    }

    [MenuItem("Save To Git/Commit changes")]
    public static void StartCommit()
    {
        CommitWindow commitWindow = CreateInstance<CommitWindow>();
        commitWindow.ShowUtility();
    }

    [MenuItem("Save To Git/Push To Repo")]
    public static void Push()
    {
        ProcessStartInfo git = new ProcessStartInfo("git.exe", "push ")
        {
            WorkingDirectory = "D:\\User\\Documents\\prects\\unity\\mj_clone"
        };
        Process.Start(git).WaitForExit();
    }

    public static void Commit(string desc)
    {
        ProcessStartInfo git = new ProcessStartInfo("git.exe", "commit -a -m \"" + desc+ "\"")
        {
            WorkingDirectory = "D:\\User\\Documents\\prects\\unity\\mj_clone"
        };
        Process.Start(git).WaitForExit();
    }




}
