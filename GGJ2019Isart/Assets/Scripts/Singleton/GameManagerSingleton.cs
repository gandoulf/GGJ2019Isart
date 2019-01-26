using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSingleton : Singleton<GameManagerSingleton>
{
    // (Optional) Prevent non-singleton constructor use.
    protected GameManagerSingleton() { }

    // Then add whatever code to the class you need as you normally would.
    public APathNode[] pathNode;
}