/*
 * Copyright © 2017 Fazli Jan, Oleg Ivanov
 * The project is licensed under the MIT License.
 */

using UnityEngine;

public abstract class State : MonoBehaviour
{
    public virtual void Enter()
    {
        AddListeners();
    }

    public virtual void Exit()
    {
        RemoveListeners();
    }

    public virtual void OnUpdate() { }

    protected virtual void OnDestroy()
    {
        RemoveListeners();
    }

    protected virtual void AddListeners() { }

    protected virtual void RemoveListeners() { }
}