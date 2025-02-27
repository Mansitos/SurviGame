using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxFood = 100f;
    [SerializeField] private float maxThirst = 100f;
    [SerializeField] private float maxEnergy = 100f;

    [SerializeField] private float foodConsumptionPerSecond = 0.25f;
    [SerializeField] private float thirstConsumptionPerSecond = 0.35f;
    [SerializeField] private float starvingDamagePerSecond = 0.25f;

    [SerializeField] private float runningEnergyConsumptionPerSecond = 0.03f;
    [SerializeField] private float walkingEnergyConsumptionPerSecond = 0.01f;

    private float health;
    private float food;
    private float thirst;
    private float energy;
    private bool didNotSleepPenalty;

    private GameManager gm;
    private InputHandler movement;

    void Start()
    {
        health = maxHealth;
        food = maxFood;
        thirst = maxThirst;
        energy = maxEnergy;

        gm = GameManager.Instance;
        movement = gm.GetInputHandler();

        StartCoroutine(UpdateFoodAndThirst());
        StartCoroutine(CheckEnergyConsumptionStatus());
    }

    private IEnumerator UpdateFoodAndThirst()
    {
        while (true)
        {
            // all is calibrated assuming 1 second update step.
            yield return new WaitForSeconds(1f);

            ConsumeFood(foodConsumptionPerSecond);
            ConsumeThirst(thirstConsumptionPerSecond);
            HandleStarving();
        }
    }
    private IEnumerator CheckEnergyConsumptionStatus()
    {
        while (true)
        {
            // all is calibrated assuming 1 second update step.
            yield return new WaitForSeconds(1f);
            if (movement.isWalking && !movement.isRunning)
            {
                ReduceEnergy(walkingEnergyConsumptionPerSecond);
            }else if (movement.isRunning)
            {
                ReduceEnergy(runningEnergyConsumptionPerSecond);
            }
        }
    }

    // ---- GET Player Status Methods ----
    public float GetHealth() => health;
    public float GetMaxHealth() => maxHealth;
    public float GetFood() => food;
    public float GetMaxFood() => maxFood;
    public float GetThirst() => thirst;
    public float GetMaxThirst() => maxThirst;
    public float GetEnergy() => energy;
    public float GetMaxEnergy() => maxEnergy;

    // ---- MODIFY Status Methods ----
    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0) Debug.Log("Player died!");
    }
    private void HandleStarving()
    {
        if (food <= 0 || thirst <= 0)
        {
            TakeDamage(starvingDamagePerSecond);
        }
    }
    public void Heal(float amount) { health = Mathf.Clamp(health + amount, 0, maxHealth); }
    public void ConsumeFood(float amount) { food = Mathf.Clamp(food - amount, 0, maxFood); }
    public void AddFood(float amount) { food = Mathf.Clamp(food + amount, 0, maxFood); }
    public void ConsumeThirst(float amount) { thirst = Mathf.Clamp(thirst - amount, 0, maxThirst); }
    public void AddThirst(float amount) { thirst = Mathf.Clamp(thirst + amount, 0, maxThirst); }
    public void ReduceEnergy(float amount) { energy = Mathf.Clamp(energy - amount, 0, maxEnergy); }
    public void AddEnergy(float amount) { energy = Mathf.Clamp(energy + amount, 0, maxEnergy); }
    public void SetDidNotSleepPenalty(bool flag)
    {
        if (didNotSleepPenalty == true && flag == false)
        {
            didNotSleepPenalty = true;
            maxEnergy = maxEnergy * 2; // reverting back
        }
        else if (didNotSleepPenalty == false && flag == true)
        {
            didNotSleepPenalty = true;
            maxEnergy = maxEnergy / 2; // applying penalty
        }
    }
    public void Sleep()
    {
        energy = maxEnergy;
    }
}
