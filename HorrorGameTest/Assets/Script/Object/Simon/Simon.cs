using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SimonGame
{
    public class Simon : InteractiveObj
    {
        public bool canPlayTheSimon = false; // Can we press the buttons

        [SerializeField] private List<numberOfIterationToDo> numberOfIterationToDo = new List<numberOfIterationToDo>(); // Interation Of The Simon
        private int numberOfIterationToDoIndex;
        
        [SerializeField] private List<SimonState> simomStates = new List<SimonState>();// action of the Simon
        private List<SimonState> pressedButtonsStates = new List<SimonState>(); // pressed action by player
        private int numberOfState; // numbers of actions needed until verification
        private int pressedNumbers; // numbers of pressed actions by the player

        public Action OnPlaySimon;
        
        public static Simon instance;
        private void Awake()
        {
            instance = this;
        }

        public override void Interact(PlayerController player)
        {
            BatterieChecker(player);
        }

        public void ChangeButtonsCollider()
        {
            OnPlaySimon?.Invoke();
        }

        private void BatterieChecker(PlayerController player)
        {
            switch (player.numberOfBatterie)
            {
                case <1:
                    Debug.Log("Il vous faut au moins une batterie en main !");
                    break;
            
                case >=1:
                    Debug.Log("Bien bien");
                    PlaySimon();
                    break;
            }
            player.RemoveBatterie(player.numberOfBatterie);
        } 
        
        
        private void PlaySimon()
        {
            canPlayTheSimon = true;
            if (!numberOfIterationToDo[numberOfIterationToDoIndex].hasAlreadyBeenAdded)
            {
                for (int i = 0; i < numberOfIterationToDo[numberOfIterationToDoIndex].number; i++)
                {
                    SimonState state = (SimonState)Random.Range(0, Enum.GetValues(typeof(SimonState)).Length);
                    simomStates.Add(state);
                    numberOfState++;
                }
                numberOfIterationToDo[numberOfIterationToDoIndex].hasAlreadyBeenAdded = true;
            }
            ChangeButtonsCollider();
        }

        public void PressButton(SimonState pressedState)
        {
            pressedButtonsStates.Add(pressedState);
            Verification();
        }

        private void Verification()
        {
            if (pressedButtonsStates[pressedNumbers] == simomStates[pressedNumbers])
            {
                pressedNumbers++;
                if (pressedNumbers == numberOfState)
                {
                    GameWon();
                }
            }
            else
            {
                GameLost();
            }
        }

        private void GameWon()
        {
            ClearPlayerData();
            numberOfIterationToDoIndex++;
            Debug.Log("Gagner !");
        }

        private void GameLost()
        {
            Debug.Log("Tu as perdu");
            ClearPlayerData();
        }

        private void ClearPlayerData()
        {
            pressedNumbers = 0;
            pressedButtonsStates.Clear();
            canPlayTheSimon = false;
            ChangeButtonsCollider();
        }
    }

    [Serializable]
    public enum SimonState
    {
        Red,
        Blue,
        Yellow,
        Green
    }

    [Serializable]
    public class numberOfIterationToDo
    {
        public int number;
        public bool hasAlreadyBeenAdded;
    }
}