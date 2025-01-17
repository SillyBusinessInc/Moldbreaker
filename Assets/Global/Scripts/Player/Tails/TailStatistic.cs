using UnityEngine;
[System.Serializable]
public class TailStatistic
{
    public CurrentStatistic slamObjectSize = new(1.0f);
    public CurrentStatistic increaseTailSpeed = new(1.0f);
    public CurrentStatistic comboResetTime = new(2.0f);
    public CurrentStatistic leftTailDamage = new(10.0f);
    public CurrentStatistic rightTailDamage = new(15.0f);
    public CurrentStatistic flipTailDamage = new(20.0f);

    public CurrentStatistic leftTailCooldown = new(0.0f);
    public CurrentStatistic rightTailCooldown = new(0.0f);
    public CurrentStatistic flipTailCooldown = new(0.0f);
    public CurrentStatistic forwardSpeedAttack = new (100.0f);
}
