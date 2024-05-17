using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.Monsters
{


    public class DefaultMonsterTest
    {

        [Test]
        public void RandomDirection_ReturnsValidDirection()
        {
            // Arrange
            GameObject gameObject = new GameObject("Monster");
            DefaultMonsterScript monsterScript = gameObject.AddComponent<DefaultMonsterScript>();

            // Add a Collider2D component to the mock game object
            Collider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.offset = Vector2.zero; // Set the offset to zero to avoid errors


            // Act
            Vector2 randomDirection = monsterScript.RandomDirection();

            // Assert
            Assert.Contains(randomDirection, new Vector2[] { Vector2.up, Vector2.down, Vector2.right, Vector2.left });
        }

        [Test]
        public void RandomMovement_ChangesDirection()
        {
            // Arrange
            GameObject gameObject = new GameObject("Monster");
            DefaultMonsterScript monsterScript = gameObject.AddComponent<DefaultMonsterScript>();

            // Add a Collider2D component to the mock game object
            Collider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.offset = Vector2.zero; // Set the offset to zero to avoid errors

            // Set initial direction
            monsterScript.SetDirection(Vector2.up);

            // Store the initial direction
            Vector2 initialDirection = monsterScript.direction;


            // Act
            while(monsterScript.direction == initialDirection)
            {
                monsterScript.RandomMovement();
            }

            // Assert
            Assert.AreNotEqual(initialDirection, monsterScript.direction);
        }

        [Test]
        public void Monster_Dies_When_Colliding_With_Explosion()
        {
            // Arrange
            GameObject monsterObject = new GameObject();
            DefaultMonsterScript monsterScript = monsterObject.AddComponent<DefaultMonsterScript>();
            monsterScript.SetSpriteRenderer(monsterObject.AddComponent<SpriteRenderer>());
            monsterScript.speed = 4f;

            GameObject explosionObject = new GameObject();
            explosionObject.tag = "Explosion";
            CircleCollider2D explosionCollider = explosionObject.AddComponent<CircleCollider2D>();
            explosionCollider.isTrigger = true;


            // Act
            monsterScript.OnTriggerEnter2DTEST(explosionCollider);

            // Assert
            Assert.AreEqual(99f, monsterScript.speed); // Check if the monster game object has been destroyed
        }

    }
}