using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class testű
{
    [UnityTest]
    public IEnumerator PlayTest()
    {
        // This simply waits for one frame and then asserts true
        yield return null;
        Assert.IsTrue(true);
    }
}
