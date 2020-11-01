
namespace FSMMSG
{
    // 使用模板来达到复用的目的
    public abstract class State<entity_type>
    {
        /// <summary>
        /// 当状态进入的时候执行
        /// </summary>
        /// <param name="miner"></param>
        public abstract void Enter(entity_type entity);
        /// <summary>
        /// 每次更新步骤的时候执行
        /// </summary>
        /// <param name="miner"></param>
        public abstract void Execute(entity_type entity);
        /// <summary>
        /// 当状态退出的时候执行
        /// </summary>
        /// <param name="miner"></param>
        public abstract void Exit(entity_type entity);
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public abstract bool OnMessage(entity_type entity, Telegram msg);
    }
}