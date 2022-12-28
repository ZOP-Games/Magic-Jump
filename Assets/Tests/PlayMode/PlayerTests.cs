using System.Collections;
using System.Linq;
using GameExtensions;
using GameExtensions.UI;
using GameExtensions.UI.Menus;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerTests : InputTestFixture
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    private Gamepad gamepad = InputSystem.AddDevice<Gamepad>();
    private const int HubBuildIndex = 1;
    private static readonly Vector3 StartPos = new Vector3(1, 0, 1);

    [UnityTest]
    public IEnumerator HubLoadTest()
    {
        yield return SceneSetup();
        Assert.That(SceneManager.GetActiveScene().buildIndex is HubBuildIndex);
        Assert.That(Object.FindObjectOfType<Player>() is not null);
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerJumpTest()
    {
        var player = Object.FindObjectOfType<Player>();
        var tf = player.transform.position;
        Assert.That(player is not null);
        Assert.That(player.PInput.currentControlScheme == "Controller");
        Assert.That(tf == StartPos);
        yield return null;
        var y =tf.y;
        Press(gamepad.buttonSouth);
        yield return null;
        Assert.That(tf.y, Is.GreaterThan(y));
    }

    [UnityTest]
    public IEnumerator PlayerAttackTest()
    {
        var player = Object.FindObjectOfType<Player>();
        yield return null;
        var enemy = Object.FindObjectOfType<EnemyBase>();
        Assert.That(enemy is not null);
    }

    private IEnumerator SceneSetup()
    {
        gamepad = InputSystem.AddDevice<Gamepad>();
        var es = new GameObject ();
        es.AddComponent<EventSystem>();
        var mc = new GameObject ();
        mc.AddComponent<MenuController> ();
        GameObject gObj = new ();
        var mainMenu = gObj.AddComponent<MainMenu>();
        for (var i = 0; i < 4; i++)
        {
            var child = new GameObject
            {
                transform =
                {
                    parent = mainMenu.transform
                }
            };
            child.AddComponent<Button>();
        }
        Assert.That(mc.GetComponent<MenuController>() is not null);
        yield return null;
        yield return new WaitWhile(() => SceneManager.GetActiveScene().buildIndex == 0);
    }

}
