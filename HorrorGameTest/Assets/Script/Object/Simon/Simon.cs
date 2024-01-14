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
        
        [SerializeField] internal List<SimonState> simomStates = new List<SimonState>();// actions of the Simon
        internal List<SimonState> pressedButtonsStates = new List<SimonState>(); // pressed actions by player
        private int numberOfState; // numbers of actions needed until verification
        internal int pressedNumbers; // numbers of pressed actions by the player

        internal Button[] AllButtonsAvailable; // All Buttons of the Simon

        public Action OnChangeCollider; // Called when changing collider of the buttons and the simon it self ==> player detection raycast
        public Action OnSetAIHead; // set the AI head in the correct direction 
        public Action OnAITurn; // AI Must plau
        
        public static Simon instance;
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            AllButtonsAvailable = GetComponentsInChildren<Button>();
        }

        public override void Interact(PlayerController player)
        {
            BatterieChecker(player);
        }

        private void ChangeButtonsCollider() // Action to enalble/disable button collider 
        {
            OnChangeCollider?.Invoke();
        }

        private void BatterieChecker(PlayerController player)
        {
            switch (PlayerInteraction.Instance.numberOfBatterie)
            {
                case <1:
                    Debug.Log("Il vous faut au moins une batterie en main !");
                    break;
            
                case >=1:
                    Debug.Log("Bien bien");
                    PlaySimon();
                    break;
            }
            PlayerInteraction.Instance.RemoveBatterie(PlayerInteraction.Instance.numberOfBatterie);
        } 
        
        internal void PlaySimon()
        {
            if (PlayerInteraction.Instance.isLightEquipped) PlayerInteraction.Instance.isLightEquipped = false;

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
            StartCoroutine(CoroutineDebugAllColors(simomStates));
            
            OnSetAIHead?.Invoke();
        }

        private int coroutineDebugColorCount = 0;
        private IEnumerator CoroutineDebugAllColors(List<SimonState> simomStates)
        {
            coroutineDebugColorCount = 0;
            foreach (var state in simomStates)
            {
                foreach (var button in AllButtonsAvailable)
                {
                    if (button.buttonState == state)
                    {
                        button.CallButton();
                        yield return new WaitForSeconds(.75f);
                    }
                }
            }
            canPlayTheSimon = true;
            ChangeButtonsCollider();
            
            yield return new WaitForSeconds(1);
            OnAITurn?.Invoke();
        }

        public void PressButton(SimonState pressedState)
        {
            pressedButtonsStates.Add(pressedState);
            Verification();
        }

        internal void Verification()
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
            OnSetAIHead?.Invoke();
        }

        private void GameLost()
        {
            Debug.Log("Tu as perdu");
            ClearPlayerData();
            OnSetAIHead?.Invoke();
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