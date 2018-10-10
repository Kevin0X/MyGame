using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProduct
{
    GameObject InsObj(Transform parent);
}

public class HeroFactory : MonoBehaviour, IProduct
{
    public virtual GameObject InsObj(Transform parent)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Char/Hero1"));
        return go;
    }
}
public class ZombieFactory : MonoBehaviour, IProduct
{
    public virtual GameObject InsObj(Transform parent)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Char/Zombie"));
        return go;
    }
}

public static class Factory
{
    public static IProduct Create(int id)
    {
        switch (id)
        {
            case 1:
                return new HeroFactory();
            case 2:
                return new ZombieFactory();
            default:
                return null;
        }
    }
}
