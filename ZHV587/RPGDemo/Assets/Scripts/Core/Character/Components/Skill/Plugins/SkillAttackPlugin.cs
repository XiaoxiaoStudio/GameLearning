using System.Collections.Generic;
using System.Security;
using Air2000;
using UnityEngine;

public class SkillAttackPlugin : SkillPlugin
{
    #region hit
    public List<HitInfo> m_HitInfos = new List<HitInfo>();
    public class HitInfo
    {
        public Player Player;
        public int CurrentHitCount;
        public float LastHitTime = -1f;
        public void Update(float interval, int totalHitCount, Player attacker)
        {
            if (Player == null || attacker == null) return;
            if (CurrentHitCount > totalHitCount)
            {
                return;
            }
            if (LastHitTime == -1f)
            {
                //first hit
                LastHitTime = Time.realtimeSinceStartup;
                CurrentHitCount++;
                attacker.Attack(Player);
            }
            else
            {
                float deltaHitTime = Time.realtimeSinceStartup - LastHitTime;
                if (deltaHitTime >= interval)
                {
                    LastHitTime = Time.realtimeSinceStartup;
                    CurrentHitCount++;
                    attacker.Attack(Player);
                }
            }
        }
    }
    public HitInfo TryGetHitInfo(Player player)
    {
        if (m_HitInfos == null || m_HitInfos.Count == 0) return null;
        for (int i = 0; i < m_HitInfos.Count; i++)
        {
            HitInfo info = m_HitInfos[i];
            if (info == null) continue;
            if (info.Player == player) return info;
        }
        return null;
    }
    public void AddHitInfo(Player player)
    {
        if (m_HitInfos == null) m_HitInfos = new List<HitInfo>();
        HitInfo info = new HitInfo() { Player = player };
    }
    #endregion

    public float Radius;
    public float Angles;
    public float HitInterval;
    public int HitCount; // Single character total avaliable hit count.

    public override bool Begin()
    {
        if (base.Begin() == false)
        {
            return false;
        }
        if (m_HitInfos == null) m_HitInfos = new List<HitInfo>();
        m_HitInfos.Clear();
        return true;
    }
    public override void Update()
    {
        base.Update();
        List<Player> tempEnemys = null;
        if (BattleScene.Instace != null)
        {
            tempEnemys = BattleScene.Instace.GetAllEnemy(Player);
        }
        if (tempEnemys != null && tempEnemys.Count > 0)
        {
            for (int i = 0; i < tempEnemys.Count; i++)
            {
                Player tempEnemy = tempEnemys[i];
                if (tempEnemy == null || tempEnemy.Character == null)
                {
                    continue;
                }
                float dis = Character.DistanceToCharacter(tempEnemy.Character);
                if (dis <= Radius)
                {
                    if (Angles != 0 && Angles != 360)
                    {
                        Vector3 dir = tempEnemy.Character.WorldPosition - Character.WorldPosition;
                        dir.Normalize();
                        float dot = Vector3.Dot(Character.transform.forward, dir);//get dot value
                        float cosValue = Mathf.Cos((Mathf.PI / 180) * (Angles / 2));
                        if (dot < cosValue)
                        {
                            return;
                        }
                    }
                    HitInfo info = TryGetHitInfo(tempEnemy);
                    if (info == null)
                    {
                        info = new HitInfo();
                        info.Player = tempEnemy;
                        if (m_HitInfos == null) m_HitInfos = new List<HitInfo>();
                        m_HitInfos.Add(info);
                    }
                    info.Update(HitInterval, HitCount, Player);
                }
            }
        }
    }
    public override void End()
    {
        base.End();
    }
    public override void ParseXML(SecurityElement element, Skill skill)
    {
        base.ParseXML(element, skill);

        float.TryParse(element.Attribute("Radius"), out Radius);
        float.TryParse(element.Attribute("Angles"), out Angles);
        float.TryParse(element.Attribute("HitInterval"), out HitInterval);
        int.TryParse(element.Attribute("HitCount"), out HitCount);
    }
}
