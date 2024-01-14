using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using SimonGame;
using UnityEngine;


namespace AI
{
    public class AI_PlayGame : MonoBehaviour
    {

        public AI_State currentState;
        
        // AI Modules
        private AI_HeadFollowing aiHeadFollowing;
        
        private void Start()
        {
            ChangeState(AI_State.Waiting);
            aiHeadFollowing = GetComponent<AI_HeadFollowing>();
            Simon.instance.OnSetAIHead += PlaySimon;
            Simon.instance.OnAITurn += PressButtons;
        }

        private void Update()
        {
            StateMachine(currentState);
        }

        [ContextMenu("Play Simon")]
        private void PlaySimon()
        {
            //Simon.instance.PlaySimon();
            ChangeState(AI_State.Waiting);
        }
        
        
        
        [ContextMenu("PressButtons")]
        private void PressButtons()
        {
            ChangeState(AI_State.Playing);
            StartCoroutine(WaitBetweenEachPress());
        }

        IEnumerator WaitBetweenEachPress()
        {
            foreach (var state in Simon.instance.simomStates)
            {
                foreach (var button in Simon.instance.AllButtonsAvailable)
                {
                    if (button.buttonState == state)
                    {
                        button.CallButton();
                        Simon.instance.pressedButtonsStates.Add(Simon.instance.simomStates[Simon.instance.pressedNumbers]);
                        Simon.instance.Verification();
                        yield return new WaitForSeconds(.8f);
                    }
                }
            }
        }

        
        private void StateMachine(AI_State state)
        {
            switch (state)
            {
                case AI_State.Waiting:
                    aiHeadFollowing.WhatToLookAt(PlayerController.Instance.transform);
                    break;
                case AI_State.Playing:
                    aiHeadFollowing.WhatToLookAt(Simon.instance.transform);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void ChangeState(AI_State state)
        {
            currentState = state;
        }
    }
}


public enum AI_State
{
    Waiting,
    Playing,
}
