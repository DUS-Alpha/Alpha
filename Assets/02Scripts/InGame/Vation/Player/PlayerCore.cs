using alpha;
using System;
using UnityEngine;

namespace alpha
{
    // 프로젝트의 중심 규칙과 공통 인프라 (모듈 집합)
    // 특정 책임 범위를 대표하는 "객체 그래프의 루트"
    public class PlayerCore
    {
        public LocomotionRules LocomotionRule;
        public CombatRules CombatRule;
    }
}