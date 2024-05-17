using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Powerups
{
    [TestFixture]
    public class PowerupTest
    {
        [Test]
        public void SpeedPowerup_IncreasesSpeed()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();

            playerController.speed = 5f;

            float speed1 = playerController.speed;

            SpeedBonus powerup = ScriptableObject.CreateInstance<SpeedBonus>();

            powerup.plusSpeed = 3f;

            powerup.Apply(playerObject);

            float speed2 = playerController.speed;

            Assert.IsTrue(speed1 < speed2);//megvaltozik a sebesseg


        }

        [Test]
        public void SlowPowerup_DescreasesSpeed()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();

            playerController.speed = 5f;

            float speed1 = playerController.speed;

            SlowPowerUp powerup = ScriptableObject.CreateInstance<SlowPowerUp>();

            powerup.minusSpeed = 3f;
            powerup.speedBase = speed1;

            powerup.Apply(playerObject);
            float speed2 = playerController.speed;
            Assert.IsTrue(speed1 > speed2);//csokken

        }

        [Test]
        public void SlowPowerup_CanBeRemoved()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();

            playerController.speed = 5f;

            float speed1 = playerController.speed;

            SlowPowerUp powerup = ScriptableObject.CreateInstance<SlowPowerUp>();

            powerup.minusSpeed = 3f;
            powerup.speedBase = speed1;

            powerup.Apply(playerObject);
            powerup.Remove(playerObject);

            float speed3 = playerController.speed;
            Assert.IsTrue(speed1 == speed3);//alap lesz


        }

        [Test]
        public void PlusBomb_IncreasesAmount()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            BombController bombController = playerObject.AddComponent<BombController>();

            bombController.bombAmount = 1;
            bombController.bombsRemaining = 1;
            int amount = bombController.bombAmount;
            int remaining = bombController.bombsRemaining;



            PlusBomb powerup = ScriptableObject.CreateInstance<PlusBomb>();

            powerup.Apply(playerObject);

            Assert.IsTrue(amount < bombController.bombAmount && remaining < bombController.bombsRemaining);//nottek


        }

        [Test]
        public void ObstacleController_Works()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            BombController bombController = playerObject.AddComponent<BombController>();
            ObstacleController obstacleController = playerObject.AddComponent<ObstacleController>();
            obstacleController.obstacleAmount = 1;

            ObstaclePowerUp powerup = ScriptableObject.CreateInstance<ObstaclePowerUp>();
            powerup.plusObstacles = 999;

            powerup.Apply(playerObject);

            Assert.IsTrue(obstacleController.obstacleAmount == 1000);//nottek
        }

        [Test]
        public void NoBomb_Applies()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            BombController bombController = playerObject.AddComponent<BombController>();
            

            NoBomb powerup = ScriptableObject.CreateInstance<NoBomb>();

            powerup.Apply(playerObject);

            Assert.IsTrue(bombController.noBomb);
        }
        [Test]
        public void NoBomb_Removes()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            BombController bombController = playerObject.AddComponent<BombController>();


            NoBomb powerup = ScriptableObject.CreateInstance<NoBomb>();

            powerup.Apply(playerObject);
            powerup.Remove(playerObject);

            Assert.IsTrue(!bombController.noBomb);
        }

        [Test]
        public void Invincibility_Applies()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            BombController bombController = playerObject.AddComponent<BombController>();


            Invincibility powerup = ScriptableObject.CreateInstance<Invincibility>();
            

            powerup.applytest(playerObject);

            Assert.IsTrue(playerController.invincibility);
        }
        [Test]
        public void Invincibility_Removes()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            BombController bombController = playerObject.AddComponent<BombController>();


            Invincibility powerup = ScriptableObject.CreateInstance<Invincibility>();

            powerup.applytest(playerObject);
            powerup.removetest(playerObject);

            Assert.IsTrue(!playerController.invincibility);
        }

        [Test]
        public void InstantBomb_Applies()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            BombController bombController = playerObject.AddComponent<BombController>();


            InstantBomb powerup = ScriptableObject.CreateInstance<InstantBomb>();


            powerup.Apply(playerObject);

            Assert.IsTrue(bombController.instantBombDrop);
        }
        [Test]
        public void InstantBomb_Removes()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            BombController bombController = playerObject.AddComponent<BombController>();


            InstantBomb powerup = ScriptableObject.CreateInstance<InstantBomb>();

            powerup.Apply(playerObject);
            powerup.Remove(playerObject);

            Assert.IsTrue(!bombController.instantBombDrop);
        }

        [Test]
        public void Detonator_Applies()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            BombController bombController = playerObject.AddComponent<BombController>();


            Detonator powerup = ScriptableObject.CreateInstance<Detonator>();


            powerup.Apply(playerObject);

            Assert.IsTrue(bombController.detonator);
        }

        [Test]
        public void BombRadiusPlus_Applies()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            BombController bombController = playerObject.AddComponent<BombController>();
            bombController.explosionRadius = 1;
            int rad = bombController.explosionRadius;

            BombRadiusPlus powerup = ScriptableObject.CreateInstance<BombRadiusPlus>();


            powerup.Apply(playerObject);

            Assert.IsTrue(rad < bombController.explosionRadius);
        }
        [Test]
        public void BombRadiusPlus_NotApply_WhenMinus()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            BombController bombController = playerObject.AddComponent<BombController>();
            bombController.explosionRadius = 1;
            int rad = bombController.explosionRadius;

            BombRadiusPlus powerup = ScriptableObject.CreateInstance<BombRadiusPlus>();
            BombRadiusMinus powerdown = ScriptableObject.CreateInstance<BombRadiusMinus>();


            powerdown.Apply(playerObject);
            powerup.Apply(playerObject);

            Assert.IsTrue(rad == bombController.explosionRadius && bombController.isReducedRadius);
        }

        [Test]
        public void BombRadiusMinus_Applies()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            BombController bombController = playerObject.AddComponent<BombController>();


            BombRadiusMinus powerup = ScriptableObject.CreateInstance<BombRadiusMinus>();


            powerup.Apply(playerObject);

            Assert.IsTrue(bombController.isReducedRadius && bombController.explosionRadius == 1);
        }
        [Test]
        public void BombRadiusMinus_Removes()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerController playerController = playerObject.AddComponent<PlayerController>();
            BombController bombController = playerObject.AddComponent<BombController>();
            bombController.explosionRadius = 5;

            BombRadiusMinus powerup = ScriptableObject.CreateInstance<BombRadiusMinus>();
            powerup.explosionRadius = bombController.explosionRadius;


            powerup.Apply(playerObject);
            Assert.IsTrue(bombController.isReducedRadius && bombController.explosionRadius == 1);
            powerup.Remove(playerObject);
            Assert.IsTrue(!bombController.isReducedRadius && bombController.explosionRadius == 5);
        }
    }
}

