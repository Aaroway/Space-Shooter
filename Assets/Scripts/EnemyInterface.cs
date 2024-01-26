using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBehavior
{
    void ExecuteBehavior()
}
public class DefaultEnemyBehavior : IEnemyBehavior
{
    public void ExecuteBehavior()
    {
        // Implement default enemy behavior
    }
}

public class FiringEnemyBehavior : IEnemyBehavior
{
    public void ExecuteBehavior()
    {
        // Implement firing enemy behavior
    }
}
public class ZigZagEnemyBehavior : IEnemyBehavior
{
    public void ExecuteBehavior()
    {

    }
}
public class LeftToRightEnemyBehavior : IEnemyBehavior
{
    public void ExecuteBehavior()
    {

    }
}
public class RightToLeftEnemyBehavior : IEnemyBehavior
{
    public void ExecuteBehavior()
    {

    }
}
public class ShieldedBehavior : IEnemyBehavior
{
    public void ExecuteBehavior()
    {

    }
}
public class SmartEnemyBehavior : IEnemyBehavior
{
    public void ExecuteBehavior()
    {

    }
}


