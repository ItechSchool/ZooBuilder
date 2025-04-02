using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestTest : MonoBehaviour
{
    [Test]
    public void SimplePasses()
    {
        // Use the Assert class to test conditions
        Assert.AreEqual(1, 1);
    }

    [UnityTest]
    public IEnumerator MyTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
        Assert.AreEqual(1, 1);
    }
}
