using UnityEngine;
using UnityEngine.SceneManagement;

static public class GameManager
{
    public const float maxLife = 100;
    static private float life = 100;

    public enum STATE
    {
        Running,
        Paused,
        Over,
    };

    static public float Life
    {
        get { return life; }
        set
        {
            life = value;
            //if (life<=0)
            //{
            //    life = 100;
            //    Restart();
            //}
        }
    }


    static public void Restart()
    {
        Life = 0;
        SceneManager.LoadScene(0);
    }

}
