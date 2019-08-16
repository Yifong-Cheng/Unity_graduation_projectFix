using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGameSystem
{

    protected MaoGame m_MaoGame = null;

    public IGameSystem (MaoGame maoGame)
    {
        m_MaoGame = maoGame;
    }

    public virtual void Initialize() { }
    public virtual void Release() { }
    public virtual void Update() { }
}
