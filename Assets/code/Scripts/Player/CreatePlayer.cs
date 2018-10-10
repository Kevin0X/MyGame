using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayer : MonoBehaviour {

    private IProduct HeroFactory;
    private IProduct ZombieFactory;

    private GameObject Hero;
    private List<GameObject> Zombies = new List<GameObject>();

    private Vector3 heroPos = new Vector3(-16, -11, 30);
    private Vector3 zombiePos = new Vector3(-4, -8, -16);

    private void Start()
    {
        HeroFactory = Factory.Create(1);
        ZombieFactory = Factory.Create(2);

        CreateHero();
        StartCoroutine(CreateZombie());
    }

    //创建英雄
    private void CreateHero()
    {
        Hero = HeroFactory.InsObj(transform);
        Hero.transform.position = heroPos;
    }

    //创建僵尸
    private IEnumerator CreateZombie()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            GameObject go = ZombieFactory.InsObj(transform);
            go.transform.position = zombiePos;
            Zombies.Add(go);
        }
    }

    private void OnDestroy()
    {
        Destroy(Hero);
        foreach(GameObject go in Zombies)
        {
            Destroy(go);
        }
    }
}
