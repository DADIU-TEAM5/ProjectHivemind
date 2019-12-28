using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Enemy/Tracker")]
public class EnemyTracker : ScriptableObject
{
    [Header("Events")]
    public GameEvent NoEnemiesEvent;

    private Dictionary<int, Enemy> Enemies;

    public void Reset() {
        Enemies.Clear();
    }

    public void Add(Enemy enemy) {
        if (!ReferenceEquals(enemy, null) && !Enemies.ContainsKey(enemy.InstanceId)) {
            Enemies.Add(enemy.InstanceId, enemy);
        }
    }

    public void Remove(Enemy enemy) {
        if (!ReferenceEquals(enemy, null) && Enemies.ContainsKey(enemy.InstanceId)) {
            Enemies.Remove(enemy.InstanceId);

            if (Enemies.Count == 0) {
                NoEnemiesEvent?.Raise();
            }
        }
    }

    public int NumberOfEnemies() => Enemies.Count;

    private void OnEnable() {
        if (ReferenceEquals(Enemies, null)) {
            Enemies = new Dictionary<int, Enemy>();
        } 
    }
}
