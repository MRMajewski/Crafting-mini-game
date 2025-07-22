using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IInteractable
    {
        void Interact();

        void StartAnimationInLoop();
        Vector3 GetApproachPosition(); 
    }

