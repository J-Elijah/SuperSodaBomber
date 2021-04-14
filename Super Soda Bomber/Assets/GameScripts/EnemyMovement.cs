﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperSodaBomber.Events;

/*
EnemyMovement
    Uses the Enemy AI and manages the health along with its event.
*/
namespace SuperSodaBomber.Enemies{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private Enemy_ScriptObject scriptObject;   //holds saved data for the enemy
        [SerializeField] private VoidEvent enemyDeathEvent;         //contains the events when the enemy dies
        [SerializeField] private Transform attackSource;         //contains the events when the enemy dies
        private float health;

        private BaseEnemy chosenScript;
        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            health = scriptObject.health;
            chosenScript = gameObject.AddComponent(EnemyProcessor.Fetch(scriptObject, gameObject)) as BaseEnemy;
            chosenScript.Init(scriptObject, attackSource);

            //change sprite and buff health if it's at phase 2
            if (scriptObject.enemyPhase == EnemyPhase.Phase2){
                if (scriptObject.phase2Sprite != null){
                    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = scriptObject.phase2Sprite;
                }
                health *= scriptObject.healthMultiplier;
            }
        }

        void FixedUpdate()
        {
            chosenScript.InvokeState();
        }

        public void Damage(float hp){
            health -= hp;

            if (health <=0){
                Die();
            }
        }

        //when the enemy rans out of health
        void Die(){
            enemyDeathEvent?.Raise();
            Destroy(gameObject);
        }
    }
}