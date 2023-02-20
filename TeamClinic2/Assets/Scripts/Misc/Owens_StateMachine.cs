using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//handles which state is active and when to transition to/from states
//this will be a component so lets leave MonoBehaviour

//this example is a stack finite state machine
//creates a stack of states so if you remember a state, it knows what to fall back to
public class Owens_StateMachine : MonoBehaviour
{
    public Stack<Owens_State> States { get; set; } //auto implements the get/sets properties

    private void Awake()
    {
        States = new Stack<Owens_State>();
    }

    private void Update()
    {
        if (GetCurrentState() != null)
        {
            GetCurrentState().ActiveAction.Invoke();  //if we have a state let's invoke it every frame
        }
    }

    //push is going to take a state and put it on the top of the stack
    public void PushState(Action active, Action onEnter, Action onExit)
    {
        //whenever we move a state to the top, the previous state gets pushed down
        if (GetCurrentState() != null) //if there is a state
        {
            GetCurrentState().OnExit();
        }

        Owens_State state = new Owens_State(active, onEnter, onExit);
        States.Push(state);

        GetCurrentState().OnEnter(); //set the state to its enter action

    }

    //pop removes a state from the top of the stack
    public void PopState()
    {
        if (GetCurrentState() != null)
        {
            GetCurrentState().OnExit();
            GetCurrentState().ActiveAction = null; //saftey
            States.Pop();
            GetCurrentState().OnEnter();
        }
    }

    //get the current state
    private Owens_State GetCurrentState()
    {
        return States.Count > 0 ? States.Peek() : null;
        //? are teranary operators they allow us to define in-line if/else statement
        //peek checks the first item
    }
}