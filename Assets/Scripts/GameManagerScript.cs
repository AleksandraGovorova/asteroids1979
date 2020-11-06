using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //текущее количество астероидов в сцене
    public int numberOfAsteroids;
    public int levelNumber = 1;
    public GameObject asteroid;
    public GameObject alien;

    public void UpdateNumberOfAsteroids(int change)
    {
        //изменение количества астероидов в сцене
        numberOfAsteroids += change;
        //Проверка, остались ли астероиды
        if (numberOfAsteroids <= 0)
        {
            //Новый уровень
            Invoke("StartNewLevel", 3f);
        }
    }

    void StartNewLevel()
    {
        levelNumber++;
        //создание астеродов на уровне 
        for (int i = 0; i < levelNumber*2; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-15f, 15f), 11.5f);
            Instantiate(asteroid, spawnPosition, Quaternion.identity);
            numberOfAsteroids++;
        }
    }
}
