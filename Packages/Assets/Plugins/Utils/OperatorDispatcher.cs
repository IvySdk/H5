using System;
using System.Collections.Concurrent;

namespace Ivy.Utils
{
    public class OperatorDispatcher : MonoSingleton<OperatorDispatcher>
    {
        private readonly ConcurrentQueue<Action> SaveQueue = new ConcurrentQueue<Action>();
        // 每帧 保存操作 数量上限
        private const int MAX_INVOKE_COUNT = 50;
        // 每帧 保存操作 时间上限
        private const int MAX_INVOKE_TICK = 25;
        
        void Update()
        {
            int ti = Environment.TickCount;
            // 限制每帧保存操作数量
            for (int i = 0; i < MAX_INVOKE_COUNT; i++)
            {
                // 限制每帧保存操作时长
                if (Environment.TickCount - ti > MAX_INVOKE_TICK)
                    return;

                if (!SaveQueue.TryDequeue(out var savePacket)) continue;
                try {
                    savePacket?.Invoke();
                }catch (Exception)
                { }
            }
        }

        public void AddActionToQueue(Action act)
        {
            SaveQueue.Enqueue(act);
        }

        public bool IsAllSaved()
        {
            return SaveQueue.Count == 0;
        }

        public override void Init()
        {
        }
    }
}

