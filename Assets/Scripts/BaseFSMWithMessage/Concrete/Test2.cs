using UnityEngine;
using FSMMSG;

public class Test2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Miner miner = new Miner(0, "徐子杨");
        MinersWife minersWife = new MinersWife(1, "大傻逼");

        //while(!EntityManager.GetInstance())
        //{
        //    // 延迟到EntityManager初始化为止
        //    // 好像Unity有一个可以指定脚本初始化顺序的地方，忘了在哪了(在Project Setting里）
        //}
        EntityManager.GetInstance().RegisterEntity(miner);
        EntityManager.GetInstance().RegisterEntity(minersWife);
    }
}
