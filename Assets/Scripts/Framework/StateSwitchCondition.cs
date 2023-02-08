using System;

namespace UFramework
{
    /// <summary>
    /// 状态转移条件
    /// </summary>
    class StateSwitchCondition
    {
        /// <summary>
        /// 条件
        /// </summary>
        public readonly Func<bool> predicate;
        /// <summary>
        /// 源状态名称
        /// </summary>
        public readonly string sourceStateName;
        /// <summary>
        /// 目标状态名称
        /// </summary>
        public readonly string targetStateName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="predicate">切换条件</param>
        /// <param name="sourceStateName">源状态名称</param>
        /// <param name="targetStateName">目标状态名称</param>
        public StateSwitchCondition(Func<bool> predicate, string sourceStateName, string targetStateName)
        {
            this.predicate = predicate;
            this.sourceStateName = sourceStateName;
            this.targetStateName = targetStateName;
        }

    }
}