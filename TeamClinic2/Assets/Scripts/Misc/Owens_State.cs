using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//we do not need MonoBehaviour, we are not going to be using this as a component
//going to be using NEW keyword which you cannot use with MonoBehaviour
public class Owens_State
{
    //delegate is a method pointer
    //
    //public System.Action ActiveAction;  //add system to your namespace so you don't need this every action
    public Action ActiveAction;
    public Action OnEnterAction; //what happens when we enter the state
    public Action OnExitAction; //what happens when we leave the state


    //let's create a constructor
    //can type ctor for shortcut with autocomplete
    //state is going to be methods on our objects
    //while "in" a state a method will be executed every frame
    public Owens_State(Action active, Action onEnter, Action onExit)
    {
        /*
        ActiveAction = Shoot; 
        ActiveAction.Invoke(); //this invokes any method tied to active action //Shoot(); 
        //we don't know what these all can be, this can be on an enemy/player/ect
        //they all could have a different set of states
        //we want this to be generic
        */

        ActiveAction = active;
        OnEnterAction = onEnter;
        OnExitAction = onExit;


    }

    //all of these execute the things our state machine can do
    //while something is active this runs
    public void Execute()
    {
        if (ActiveAction != null) //lets make sure there is an action we can take
        {
            ActiveAction.Invoke(); //call our action everyframe
        }
    }

    //whenever a state is added we'll call the OnEnter
    public void OnEnter()
    {
        if (OnEnterAction != null)
        {
            OnEnterAction.Invoke();
        }
    }

    //whenver a state is removed we'll call the OnExit
    public void OnExit()
    {
        if (OnExitAction != null)
        {
            OnExitAction.Invoke();
        }
    }

    void Shoot()
    {

    }


}
