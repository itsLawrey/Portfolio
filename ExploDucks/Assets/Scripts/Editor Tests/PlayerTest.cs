using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Player
{
    [TestFixture]
    public class PlayerTest
    {
        [Test]
        public void PlayerDiesWhenCollidingWithExplosion()
        {
            // Arrange
            GameObject playerObject = new GameObject();
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            playerController.spriteRenderer = playerObject.AddComponent<SpriteRenderer>();
            playerController.speed = 4f;
            playerController.invincibility = false;

            GameObject explosionObject = new GameObject();
            explosionObject.tag = "Explosion";
            CircleCollider2D explosionCollider = explosionObject.AddComponent<CircleCollider2D>();
            explosionCollider.isTrigger = true;

            // Act
            playerController.OnTriggerEnter2D(explosionCollider);

            // Assert
            Assert.IsTrue(playerController.IsDead());
        }

        [Test]
        public void PlayerDoesNotDieWhenInvincibleAndCollidingWithExplosion()
        {
            // Arrange
            GameObject playerObject = new GameObject();
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            playerController.spriteRenderer = playerObject.AddComponent<SpriteRenderer>();
            playerController.speed = 4f;
            playerController.invincibility = true;

            GameObject explosionObject = new GameObject();
            explosionObject.tag = "Explosion";
            CircleCollider2D explosionCollider = explosionObject.AddComponent<CircleCollider2D>();
            explosionCollider.isTrigger = true;

            // Act
            playerController.OnTriggerEnter2D(explosionCollider);

            // Assert
            Assert.IsFalse(playerController.IsDead());
        }


        //[Test]
        //public void PlayerRevivesAfterDeath()
        //{
        //    // Arrange
        //    GameObject playerObject = new GameObject("Player");
        //    PlayerController playerController = playerObject.AddComponent<PlayerController>();
        //    playerObject.AddComponent<Rigidbody2D>(); // Add Rigidbody2D component

        //    // Act
        //    playerController.Die();
        //    playerController.Revive();

        //    // Assert
        //    Assert.IsFalse(playerController.IsDead(), "Player should be alive after being revived.");
        //}
    }
}
