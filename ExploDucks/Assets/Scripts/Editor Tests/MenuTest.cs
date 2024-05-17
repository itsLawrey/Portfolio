using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;


namespace Tests.Manager
{
    [TestFixture]
    public class MenuTest
    {

        [SetUp]
        public void Setup()
        {
            // Load the scene that contains the objects needed for the test
            EditorSceneManager.OpenScene("Assets/Scenes/Map 1.unity");
        }

        



        [Test]
        public void AdvanceRound_RoundPlusPlus()
        {
            // Arrange
            GameObject roundManagerObject = new GameObject();
            RoundManager roundManager = roundManagerObject.AddComponent<RoundManager>();


            int firstRound = roundManager.CurrentRound();

            // Act
            roundManager.UpdateRoundCount();

            // Assert
            Assert.AreNotEqual(firstRound, roundManager.CurrentRound());
        }
    }
}

