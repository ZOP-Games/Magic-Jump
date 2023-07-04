using System.Collections;
using System.IO;
using GameExtensions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class SaveLoadTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void SaveLoadTestsSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator SaveLoadTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        var path = Application.persistentDataPath + "/Saves/Savegame1.mjsd";
        var gObj = new GameObject();
        gObj.AddComponent<BoxCollider>();
        gObj.AddComponent<Rigidbody>();
        gObj.AddComponent<PlayerInput>();
        gObj.AddComponent<Animator>();
        gObj.AddComponent<Player>();
        Player.PlayerReady += SaveManager.SaveAll;
        yield return new WaitForSeconds(1.5f);
        var cond = new FileInfo(path).Exists;
        Assert.That(cond, Is.True);
        var stream = new FileStream(path, FileMode.Open);
        Assert.That(stream.Length, Is.GreaterThan(0));
    }
}