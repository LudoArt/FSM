using UnityEngine;
using BaseFSM;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Miner miner = new Miner(0, "徐子杨");
        while(!EntityManager.GetInstance())
        {

        }
        EntityManager.GetInstance().RegisterEntity(miner);
    }
}
